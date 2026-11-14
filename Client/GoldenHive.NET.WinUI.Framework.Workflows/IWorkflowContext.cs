namespace GoldenHive.NET.WinUI.Framework.Workflows;

/// <summary>Application contexts that offer the workflow manager.</summary>
public interface IWorkflowContext
{
    WorkflowManager Workflows { get; }
}
