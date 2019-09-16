using System;

namespace UndergroundIRO.ProxyManager.Core.Models
{
    public class Proxy
    {
        public ProxyAnonymity Anonymity { get; set; }

        public ProxyType Type { get; set; }

        public int Port { get; set; }

        public string IP { get; set; }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return ToString() == obj.ToString();
        }

        public override string ToString()
        {
            return $"{Type.ToString().ToLower()}://{IP}:{Port}";
        }
    }
}
