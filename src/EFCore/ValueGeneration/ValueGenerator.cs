// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration
{
    /// <summary>
    ///     Generates values for properties when an entity is added to a context.
    /// </summary>
    public abstract class ValueGenerator
    {
        /// <summary>
        ///     Gets a value to be assigned to a property.
        /// </summary>
        /// <param name="entry"> The change tracking entry of the entity for which the value is being generated. </param>
        /// <returns> The value to be assigned to a property. </returns>
        public virtual object? Next(EntityEntry entry)
            => NextValue(entry);

        /// <summary>
        ///     Template method to be overridden by implementations to perform value generation.
        /// </summary>
        /// <param name="entry"> The change tracking entry of the entity for which the value is being generated. </param>
        /// <returns> The generated value. </returns>
        protected abstract object? NextValue(EntityEntry entry);

        /// <summary>
        ///     Gets a value to be assigned to a property.
        /// </summary>
        /// <param name="entry"> The change tracking entry of the entity for which the value is being generated. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns> The value to be assigned to a property. </returns>
        /// <exception cref="OperationCanceledException"> If the <see cref="CancellationToken"/> is canceled. </exception>
        public virtual ValueTask<object?> NextAsync(
            EntityEntry entry,
            CancellationToken cancellationToken = default)
            => NextValueAsync(entry, cancellationToken);

        /// <summary>
        ///     Template method to be overridden by implementations to perform value generation.
        /// </summary>
        /// <param name="entry"> The change tracking entry of the entity for which the value is being generated. </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns> The generated value. </returns>
        /// <exception cref="OperationCanceledException"> If the <see cref="CancellationToken"/> is canceled. </exception>
        protected virtual ValueTask<object?> NextValueAsync(
            EntityEntry entry,
            CancellationToken cancellationToken = default)
            => new(NextValue(entry));

        /// <summary>
        ///     <para>
        ///         Gets a value indicating whether the values generated are temporary (i.e they should be replaced
        ///         by database generated values when the entity is saved) or are permanent (i.e. the generated values
        ///         should be saved to the database).
        ///     </para>
        ///     <para>
        ///         An example of temporary value generation is generating negative numbers for an integer primary key
        ///         that are then replaced by positive numbers generated by the database when the entity is saved. An
        ///         example of permanent value generation are client-generated values for a <see cref="Guid" /> primary
        ///         key which are saved to the database.
        ///     </para>
        /// </summary>
        public abstract bool GeneratesTemporaryValues { get; }

        /// <summary>
        ///     Gets a value indicating whether the values generated are stable. That is, the value will always be the
        ///     same for a given property in a given entity, and does not depend on what other values may have been generated
        ///     previously. For example, discriminator values generated for a TPH hierarchy are stable. Stable values will never
        ///     be marked as unknown.
        /// </summary>
        public virtual bool GeneratesStableValues
            => false;
    }
}
