using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using GoldenHive.NET.WinUI.Framework.Contracts.Api;
using GoldenHive.NET.WinUI.Framework.Monads;
using GoldenHive.NET.WinUI.Framework.Mvvm;
using GoldenHive.NET.WinUI.Framework.Security;
using GoldenHive.NET.WinUI.Framework.WorkflowContracts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace GoldenHive.NET.WinUI.Framework.ModuleLoader.Sdk;

/// <summary>
/// A form generated from an InputSchema (workflow input steps, report parameters, module dialogs).
/// Validation uses the shared InputValidator - the same rules as the workflow engine and the AI guardrails.
/// Half-width fields are placed side by side in the wider size classes.
/// </summary>
public sealed partial class InputForm : UserControl
{
    private readonly InputSchema _schema;
    private readonly LanguageManager _languages;
    private readonly Dictionary<string, (FrameworkElement Control, TextBlock Error)> _fields = new(StringComparer.OrdinalIgnoreCase);

    public InputForm(InputSchema schema, LanguageManager languages, Func<string, Task<Result<IReadOnlyList<LookupItem>>>>? lookup = null, JsonObject? values = null)
    {
        _schema = schema;
        _languages = languages;
        var grid = new Grid { ColumnSpacing = 12, RowSpacing = 12 };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var row = 0;
        var column = 0;
        foreach (var field in schema.Fields)
        {
            var control = CreateControl(field, lookup);
            var error = new TextBlock { Foreground = (Brush)Application.Current.Resources["SystemFillColorCriticalBrush"], FontSize = 12, Visibility = Visibility.Collapsed, TextWrapping = TextWrapping.Wrap };
            var label = Text(field.Label is { Length: > 0 } l ? l : field.Name) + (field.Required ? " *" : string.Empty);
            var cell = UiKit.Field(label, control);
            cell.Children.Add(error);
            if (field.Help is { } help)
            {
                ToolTipService.SetToolTip(control, Text(help));
            }

            var half = field.Span == FieldSpan.Half;
            if (!half && column == 1)
            {
                row++;
                column = 0;
            }

            while (grid.RowDefinitions.Count <= row)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            Grid.SetRow(cell, row);
            Grid.SetColumn(cell, column);
            Grid.SetColumnSpan(cell, half ? 1 : 2);
            grid.Children.Add(cell);
            if (half && column == 0)
            {
                column = 1;
            }
            else
            {
                row++;
                column = 0;
            }

            _fields[field.Name] = (control, error);
        }

        Content = grid;
        if (values is not null)
        {
            SetValues(values);
        }
    }

    public bool IsReadOnly
    {
        set
        {
            foreach (var (control, _) in _fields.Values)
            {
                if (control is Control c)
                {
                    c.IsEnabled = !value;
                }
            }
        }
    }

    /// <summary>The current values (raw; not yet validated).</summary>
    public JsonObject Values
    {
        get
        {
            var values = new JsonObject();
            foreach (var input in _schema.Fields)
            {
                values[input.Name] = Read(input, _fields[input.Name].Control);
            }

            return values;
        }
    }

    public void SetValues(JsonObject values)
    {
        foreach (var field in _schema.Fields)
        {
            var value = values.FirstOrDefault(kv => kv.Key.Equals(field.Name, StringComparison.OrdinalIgnoreCase)).Value;
            if (value is not null)
            {
                Write(_fields[field.Name].Control, value);
            }
        }
    }

    /// <summary>Validates with the shared rules and shows the errors next to the fields.</summary>
    public Validation<JsonObject> Validate()
    {
        var result = InputValidator.Validate(_schema, Values);
        ShowErrors(result.IsValid ? null : Error.Many(result.Errors));
        return result;
    }

    /// <summary>Shows field errors of a (server) error next to the fields; null clears them.</summary>
    public void ShowErrors(Error? error)
    {
        var messages = error is null ? new Dictionary<string, string>() : ErrorMessages.Fields(_languages, error);
        foreach (var (name, (_, text)) in _fields)
        {
            var has = messages.TryGetValue(name, out var message);
            text.Text = message ?? string.Empty;
            text.Visibility = has ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private string Text(string keyOrText) => ErrorMessages.Text(_languages, keyOrText);

    private FrameworkElement CreateControl(InputField field, Func<string, Task<Result<IReadOnlyList<LookupItem>>>>? lookup)
    {
        switch (field.Type)
        {
            case InputFieldType.Number:
                return new NumberBox
                {
                    SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact,
                    Minimum = field.Min is { } min ? (double)min : double.MinValue,
                    Maximum = field.Max is { } max ? (double)max : double.MaxValue,
                    Value = double.TryParse(field.Default, NumberStyles.Float, CultureInfo.InvariantCulture, out var number) ? number : double.NaN,
                };
            case InputFieldType.Boolean:
                return new ToggleSwitch { IsOn = field.Default is "true" };
            case InputFieldType.Date:
                return new CalendarDatePicker
                {
                    Date = DateTimeOffset.TryParse(field.Default, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date) ? date : null,
                };
            case InputFieldType.Choice:
            {
                var combo = new ComboBox();
                foreach (var option in field.Options)
                {
                    combo.Items.Add(new ComboBoxItem { Content = Text(option.Label is { Length: > 0 } l ? l : option.Value), Tag = option.Value });
                }

                SelectByTag(combo, field.Default);
                return combo;
            }

            case InputFieldType.EntityRef:
            {
                var combo = new ComboBox { PlaceholderText = "…" };
                if (lookup is not null && field.Lookup is { } url)
                {
                    _ = LoadLookupAsync(combo, url, lookup, field.Default);
                }

                return combo;
            }

            case InputFieldType.MultiLine:
                return new TextBox { AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, MinHeight = 80, MaxLength = field.MaxLength ?? 0, Text = field.Default ?? string.Empty };
            default:
                return new TextBox { MaxLength = field.MaxLength ?? 0, Text = field.Default ?? string.Empty };
        }
    }

    private static async Task LoadLookupAsync(ComboBox combo, string url, Func<string, Task<Result<IReadOnlyList<LookupItem>>>> lookup, string? selected)
    {
        var items = await lookup(url);
        var current = (combo.SelectedItem as ComboBoxItem)?.Tag as string ?? combo.Tag as string ?? selected;
        combo.Items.Clear();
        foreach (var item in items.GetValueOrDefault([]))
        {
            combo.Items.Add(new ComboBoxItem { Content = item.Label, Tag = item.Value });
        }

        SelectByTag(combo, current);
    }

    private static void SelectByTag(ComboBox combo, string? value)
    {
        combo.Tag = value;
        combo.SelectedItem = combo.Items.OfType<ComboBoxItem>().FirstOrDefault(i => string.Equals(i.Tag as string, value, StringComparison.OrdinalIgnoreCase));
    }

    private static JsonNode? Read(InputField field, FrameworkElement control) => control switch
    {
        NumberBox n => double.IsNaN(n.Value) ? null : JsonValue.Create(Math.Round((decimal)n.Value, 4)),
        ToggleSwitch t => JsonValue.Create(t.IsOn),
        CalendarDatePicker d => d.Date is { } date ? JsonValue.Create(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)) : null,
        ComboBox c => (c.SelectedItem as ComboBoxItem)?.Tag is string tag ? JsonValue.Create(tag) : c.Tag is string pending ? JsonValue.Create(pending) : null,
        TextBox t => string.IsNullOrWhiteSpace(t.Text) ? null : JsonValue.Create(t.Text),
        _ => null,
    };

    private static void Write(FrameworkElement control, JsonNode value)
    {
        var text = value is JsonValue v && v.GetValueKind() == JsonValueKind.String ? v.GetValue<string>() : value.ToJsonString();
        switch (control)
        {
            case NumberBox n:
                n.Value = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number) ? number : double.NaN;
                break;
            case ToggleSwitch t:
                t.IsOn = text is "true" or "True";
                break;
            case CalendarDatePicker d:
                d.Date = DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date) ? date : null;
                break;
            case ComboBox c:
                SelectByTag(c, text);
                break;
            case TextBox t:
                t.Text = text;
                break;
        }
    }
}
