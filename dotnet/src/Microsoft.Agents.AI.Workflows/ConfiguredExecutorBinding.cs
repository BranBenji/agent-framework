// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.Shared.Diagnostics;

namespace Microsoft.Agents.AI.Workflows;

// TODO: Unwrap the Configured object, just like for SubworkflowBinding
internal record ConfiguredExecutorBinding(Configured<Executor> ConfiguredExecutor, Type ExecutorType, object? RawValueOverride = null)
    : ExecutorBinding(Throw.IfNull(ConfiguredExecutor).Id,
                           ConfiguredExecutor.BoundFactoryAsync,
                           ExecutorType,
                           RawValueOverride ?? ConfiguredExecutor.Raw)
{
    /// <inheritdoc/>
    public override bool IsSharedInstance { get; } = ConfiguredExecutor.Raw is Executor;

    protected override async ValueTask<bool> ResetCoreAsync()
    {
        if (this.ConfiguredExecutor.Raw is IResettableExecutor resettable)
        {
            await resettable.ResetAsync().ConfigureAwait(false);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public override bool SupportsConcurrentSharedExecution =>
        this.ConfiguredExecutor.Raw is not Executor executor || executor.IsCrossRunShareable;

    /// <inheritdoc/>
    public override bool SupportsResetting => this.ConfiguredExecutor.Raw is IResettableExecutor;
}
