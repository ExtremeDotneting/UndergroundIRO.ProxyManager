using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UndergroundIRO.ProxyManager.Core.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProxyStatus
    {
        Free,
        Used,
        Broken
    }
}