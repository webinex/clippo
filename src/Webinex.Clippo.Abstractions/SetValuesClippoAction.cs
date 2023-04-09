using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Calls SetValues against clip entity with supplied values
    /// </summary>
    public class SetValuesClippoAction : IClippoAction
    {
        public SetValuesClippoAction([NotNull] string id, [NotNull] IDictionary<string, object> values)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        /// <summary>
        ///     Clip identifier
        /// </summary>
        [NotNull]
        public string Id { get; }
        
        /// <summary>
        ///     Values to pass in SetValues
        /// </summary>
        [NotNull]
        public IDictionary<string, object> Values { get; }
    }
}