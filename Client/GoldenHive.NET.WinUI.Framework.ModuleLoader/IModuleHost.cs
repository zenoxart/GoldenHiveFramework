using GoldenHive.NET.WinUI.Framework.Commands;
using GoldenHive.NET.WinUI.Framework.Docking;
using GoldenHive.NET.WinUI.Framework.Navigation;
using GoldenHive.NET.WinUI.Framework.NotificationClient;
using GoldenHive.NET.WinUI.Framework.Security;
using GoldenHive.NET.WinUI.Framework.WorkflowContracts;
using GoldenHive.NET.WinUI.Framework.XamlMetadata;

namespace GoldenHive.NET.WinUI.Framework.ModuleLoader;

/// <summary>Receives the workflow registrations of modules (implemented by the WorkflowManager).</summary>
public interface IWorkflowRegistrar
{
    void RegisterStepType(string moduleId, IWorkflowStepHandler handler);

    void RegisterTemplate(string moduleId, WorkflowDefinition template);

    void UnregisterModule(string moduleId);
}

/// <summary>Everything the host offers to modules (implemented by the host's application context).</summary>
public interface IModuleHost : ICommandContext, IIconContext, INavigationContext, IDockContext, ISecurityContext, INotificationContext
{
    ModuleLoader Modules { get; }

    IWorkflowRegistrar? WorkflowRegistrar { get; }
}
