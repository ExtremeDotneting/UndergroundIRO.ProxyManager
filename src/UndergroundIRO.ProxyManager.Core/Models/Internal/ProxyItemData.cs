using System;

namespace UndergroundIRO.ProxyManager.Core.Models.Internal
{
    internal class ProxyItemData
    {
        public ProxyStatus Status { get; set; } = ProxyStatus.Free;

        public DateTime BrokenOn { get; set; }
    }
}