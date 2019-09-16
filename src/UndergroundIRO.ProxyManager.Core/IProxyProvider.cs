using System.Collections.Generic;
using System.Threading.Tasks;
using UndergroundIRO.ProxyManager.Core.Models;

namespace UndergroundIRO.ProxyManager.Core
{
    public interface IProxyProvider
    {
        Task<IList<Proxy>> Resolve(ProxyType proxyType, ProxyAnonymity anonymity);

        Task<Proxy> ResolveOne(ProxyType proxyType, ProxyAnonymity anonymity);
    }
}