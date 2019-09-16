using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UndergroundIRO.ProxyManager.Core.Models;
using UndergroundIRO.ProxyManager.Core.Models.Internal;

namespace UndergroundIRO.ProxyManager.Core
{
    public class ProxyManager
    {
        readonly IDictionary<Proxy, ProxyItemData> _dict = new ConcurrentDictionary<Proxy, ProxyItemData>();

        /// <summary>
        /// Timeout after which proxy can be restored from blacklist.
        /// </summary>
        public TimeSpan BrokenProxyTimeout { get; set; } = TimeSpan.MaxValue;

        public void SetProxyStatus(Proxy proxy, ProxyStatus proxyStatus)
        {
            var itemData = GetData(proxy);
            itemData.Status = proxyStatus;
            if (proxyStatus == ProxyStatus.Broken)
            {
                itemData.BrokenOn = DateTime.UtcNow;
            }
        }

        public ProxyStatus GetProxyStatus(Proxy proxy)
        {
            var itemData = GetData(proxy);
            return itemData.Status;
        }

        ProxyItemData GetData(Proxy proxy)
        {
            if (proxy == null) throw new ArgumentNullException(nameof(proxy));
            ProxyItemData itemData;
            if (_dict.TryGetValue(proxy, out var data))
            {
                itemData = data;
            }
            else
            {
                itemData = new ProxyItemData();
                _dict[proxy] = itemData;
            }
            
            if (itemData.Status==ProxyStatus.Broken && itemData.BrokenOn + BrokenProxyTimeout < DateTime.UtcNow)
            {
                itemData.Status = ProxyStatus.Free;
            }
            return itemData;


        }
    }
}
