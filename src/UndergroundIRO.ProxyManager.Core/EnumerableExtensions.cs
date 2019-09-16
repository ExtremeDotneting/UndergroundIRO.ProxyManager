using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UndergroundIRO.ProxyManager.Core.Models;

namespace UndergroundIRO.ProxyManager.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Proxy> OrderByAnonymity(this IEnumerable<Proxy> @this)
        {
            return @this.OrderBy(r=>r.Anonymity);
        }
    }
}
