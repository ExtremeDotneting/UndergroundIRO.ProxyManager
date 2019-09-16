using System.Collections.Generic;
using UndergroundIRO.ProxyManager.Core;
using UndergroundIRO.ProxyManager.Core.Models;

namespace UndergroundIRO.ProxyManager.HidemyParser
{
    public struct HidemyProxyParseSettings
    {
        public ProxyAnonymity? AnonymityFlags { get; set; }

        public ProxyType? TypeFlags { get; set; }

        /// <summary>
        /// Ping.
        /// </summary>
        public int? MaxTime { get; set; }

        /// <summary>
        /// If null - all from one page.
        /// </summary>
        public int? Limit { get; set; }
    }
}