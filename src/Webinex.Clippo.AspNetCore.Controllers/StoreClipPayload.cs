using System;
using System.Collections.Generic;

namespace Webinex.Clippo.AspNetCore.Controllers
{
    public class StoreClipPayload
    {
        public StoreClipPayload(IEnumerable<IClippoAction> actions, IDictionary<string, object> values)
        {
            Actions = actions ?? Array.Empty<IClippoAction>();
            Values = values ?? new Dictionary<string, object>();
        }
        
        public IEnumerable<IClippoAction> Actions { get; }
        public IDictionary<string, object> Values { get; }

        public static StoreClipPayload Empty => new StoreClipPayload(null, null);
    }
}