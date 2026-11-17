// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;

namespace Microsoft.Agents.AI.Workflows;

/// <summary>
/// Provides a mechanism to return an executor to a 'reset' state, allowing a workflow containing
/// shared instances of it to be resued after a run is disposed.
/// </summary>
public interface IResettableExecutor
{
    /// <summary>
    /// Reset the executor
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the completion of the reset operation.</returns>
    /// <exception cref="InvalidOperationException">The executor does not implement this method.</exception>
    ValueTask ResetAsync()
#if NET
    {
        throw new InvalidOperationException("Executors implementing IResettableExecutor must implement ResetAsync explicitly or publicly.");
    }
#else
    ;
#endif
}
