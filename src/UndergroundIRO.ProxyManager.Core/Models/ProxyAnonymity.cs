using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UndergroundIRO.ProxyManager.Core.Models
{
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProxyAnonymity
    {
        No = 1,
        Low = 2,
        Medium = 4,
        High = 8
    }
}