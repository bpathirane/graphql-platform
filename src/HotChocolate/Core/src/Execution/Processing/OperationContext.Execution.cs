using System;
using System.Collections.Immutable;
using HotChocolate.Execution.Processing.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Execution.Processing;

internal sealed partial class OperationContext
{
    /// <summary>
    /// The work scheduler organizes the processing of request tasks.
    /// </summary>
    public IWorkScheduler Scheduler
    {
        get
        {
            AssertInitialized();
            return _workScheduler;
        }
    }

    /// <summary>
    /// Gets the backlog of the task that shall be processed after
    /// all the main tasks have been executed.
    /// </summary>
    public IDeferredWorkScheduler DeferredScheduler
    {
        get
        {
            AssertInitialized();
            return _deferredWorkScheduler;
        }
    }

    /// <summary>
    /// The result helper which provides utilities to build up the result.
    /// </summary>
    public ResultBuilder Result
    {
        get
        {
            AssertInitialized();
            return _resultHelper;
        }
    }

    /// <summary>
    /// Register cleanup tasks that will be executed after resolver execution is finished.
    /// </summary>
    /// <param name="action">
    /// Cleanup action.
    /// </param>
    public void RegisterForCleanup(Action action)
    {
        AssertInitialized();
        _cleanupActions.Add(action);
    }

    public ResolverTask CreateResolverTask(
        ISelection selection,
        object? parent,
        ObjectResult parentResult,
        int responseIndex,
        Path path,
        IImmutableDictionary<string, object?> scopedContextData)
    {
        AssertInitialized();

        var resolverTask = _resolverTaskFactory.Create();

        resolverTask.Initialize(
            this,
            selection,
            parentResult,
            responseIndex,
            parent,
            path,
            scopedContextData);

        return resolverTask;
    }

}
