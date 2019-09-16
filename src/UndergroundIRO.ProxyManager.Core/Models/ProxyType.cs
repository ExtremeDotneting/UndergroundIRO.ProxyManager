using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UndergroundIRO.ProxyManager.Core.Models
{
     [Flags]
     [JsonConverter(typeof(StringEnumConverter))]
    public enum ProxyType
    {
        Https = 1,
        Http = 2,
        Socks5 = 4,
        Socks4 = 8
    }
}
