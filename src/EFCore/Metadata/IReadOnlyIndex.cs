// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata
{
    /// <summary>
    ///     Represents an index on a set of properties.
    /// </summary>
    public interface IReadOnlyIndex : IReadOnlyAnnotatable
    {
        /// <summary>
        ///     Gets the properties that this index is defined on.
        /// </summary>
        IReadOnlyList<IReadOnlyProperty> Properties { get; }

        /// <summary>
        ///     Gets the name of this index.
        /// </summary>
        string? Name { get; }

        /// <summary>
        ///     Gets a value indicating whether the values assigned to the indexed properties are unique.
        /// </summary>
        bool IsUnique { get; }

        /// <summary>
        ///     Gets the entity type the index is defined on. This may be different from the type that <see cref="Properties" />
        ///     are defined on when the index is defined a derived type in an inheritance hierarchy (since the properties
        ///     may be defined on a base type).
        /// </summary>
        IReadOnlyEntityType DeclaringEntityType { get; }

        /// <summary>
        ///     <para>
        ///         Creates a human-readable representation of the given metadata.
        ///     </para>
        ///     <para>
        ///         Warning: Do not rely on the format of the returned string.
        ///         It is designed for debugging only and may change arbitrarily between releases.
        ///     </para>
        /// </summary>
        /// <param name="options"> Options for generating the string. </param>
        /// <param name="indent"> The number of indent spaces to use before each new line. </param>
        /// <returns> A human-readable representation. </returns>
        string ToDebugString(MetadataDebugStringOptions options = MetadataDebugStringOptions.ShortDefault, int indent = 0)
        {
            var builder = new StringBuilder();
            var indentString = new string(' ', indent);

            builder.Append(indentString);

            var singleLine = (options & MetadataDebugStringOptions.SingleLine) != 0;
            if (singleLine)
            {
                builder.Append("Index: ");
            }

            builder
                .AppendJoin(
                    ", ",
                    Properties.Select(
                        p => singleLine
                            ? p.DeclaringEntityType.DisplayName() + "." + p.Name
                            : p.Name));

            builder.Append(" " + Name ?? "<unnamed>");

            if (IsUnique)
            {
                builder.Append(" Unique");
            }

            if (!singleLine
                && (options & MetadataDebugStringOptions.IncludeAnnotations) != 0)
            {
                builder.Append(AnnotationsToDebugString(indent + 2));
            }

            return builder.ToString();
        }
    }
}
