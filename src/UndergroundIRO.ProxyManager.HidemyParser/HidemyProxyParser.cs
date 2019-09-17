using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using UndergroundIRO.ParseKit.Network;
using UndergroundIRO.ProxyManager.Core.Models;
using UndergroundIRO.ProxyManager.HidemyParser.Exceptions;

namespace UndergroundIRO.ProxyManager.HidemyParser
{
    public class HidemyProxyParser
    {
        readonly IHttpService _httpService;

        /// <summary>
        /// Please use <see cref="XWebViewHttpService"/> .
        /// </summary>
        /// <param name="httpService"></param>
        public HidemyProxyParser(IHttpService httpService)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        }

        public static string GetUrl(HidemyProxyParseSettings s)
        {
            var url = "https://hidemy.name/en/proxy-list/?a=0";
            if (s.MaxTime != null)
            {
                var addStr = $"&maxtime={s.MaxTime}";
                url += addStr;
            }
            if (s.TypeFlags != null)
            {
                var addStr = "&type=";
                var prType = s.TypeFlags.Value;
                if (prType.HasFlag(ProxyType.Http))
                {
                    addStr += "h";
                }
                if (prType.HasFlag(ProxyType.Https))
                {
                    addStr += "s";
                }
                if (prType.HasFlag(ProxyType.Socks4))
                {
                    addStr += "4";
                }
                if (prType.HasFlag(ProxyType.Socks5))
                {
                    addStr += "5";
                }
                url += addStr;
            }
            if (s.AnonymityFlags != null)
            {
                var addStr = "&anon=";
                var prAnon = s.AnonymityFlags.Value;
                if (prAnon.HasFlag(ProxyAnonymity.No))
                {
                    addStr += "1";
                }
                if (prAnon.HasFlag(ProxyAnonymity.Low))
                {
                    addStr += "2";
                }
                if (prAnon.HasFlag(ProxyAnonymity.Medium))
                {
                    addStr += "3";
                }
                if (prAnon.HasFlag(ProxyAnonymity.High))
                {
                    addStr += "4";
                }
                url += addStr;
            }
            return url;

        }

        public async Task<IList<Proxy>> Parse(HidemyProxyParseSettings settings)
        {
            var hidemyUrl = GetUrl(settings);
            return await Parse(hidemyUrl, settings.Limit);
        }

        public async Task<IList<Proxy>> Parse(string hidemyUrl, int? limit = null)
        {
            var list = await ParseOnly(hidemyUrl, limit);
            var fitHashSet = FitCollection(list);
            return fitHashSet.ToList();
        }

        async Task<IList<Proxy>> ParseOnly(string hidemyUrl, int? limit)
        {
            try
            {
                int pageNum = 0;
                var list = new List<Proxy>();
                while (true)
                {
                    var pageUrl = GetUrlWithPageNum(hidemyUrl, pageNum);
                    var html = await _httpService.GetAsync(pageUrl);
                    var hw = new HtmlDocument();
                    hw.LoadHtml(html);
                    hw.OptionUseIdAttribute = true;
                    var tableRecords = hw
                        .GetElementbyId("global-wrapper")
                        .Descendants()
                        .First(x => x.GetClasses().Contains("proxy__t"))
                        .ChildNodes
                        .First(x => x.OriginalName == "tbody")
                        .ChildNodes
                        .Where(x => x.OriginalName == "tr");

                    int recordsInPage = 0;
                    foreach (var tr in tableRecords)
                    {
                        if (limit != null && list.Count >= limit)
                        {
                            return list;
                        }

                        var nodes = tr.ChildNodes.ToArray();
                        var ip = ResolveIP(nodes);
                        var port = ResolvePort(nodes);
                        var type = ResolveType(nodes);
                        var anon = ResolveAnon(nodes);

                        var newProxy = new Proxy()
                        {
                            IP = ip,
                            Port = port,
                            Type = type,
                            Anonymity = anon
                        };
                        list.Add(newProxy);
                        recordsInPage++;
                    }
                    //Break if limit null (one page parsed) or if there no records.
                    if (limit == null || recordsInPage == 0)
                    {
                        return list;
                    }
                    pageNum++;
                }

            }
            catch (Exception ex)
            {
                throw new HidemyParseException("Exception while parsing.", ex);
            }
        }

        string GetUrlWithPageNum(string baseUrl, int pageNum = 0)
        {
            if (pageNum == 0)
                return baseUrl;
            var addToUrl = $"&start={pageNum * 64}";
            return baseUrl + addToUrl;
        }

        /// <summary>
        /// Remove copies.
        /// </summary>
        HashSet<Proxy> FitCollection(IEnumerable<Proxy> proxies)
        {
            var res = new HashSet<Proxy>();
            foreach (var item in proxies)
            {
                if (res.Contains(item))
                    continue;
                res.Add(item);
            }
            return res;
        }

        string ResolveIP(HtmlNode[] children)
        {
            var str = children[0]
                   .InnerText.Trim();
            return str;
        }

        int ResolvePort(HtmlNode[] children)
        {
            var str = children[1]
                .InnerText.Trim();
            var num = JsonConvert.DeserializeObject<int>(str);
            return num;

        }

        ProxyType ResolveType(HtmlNode[] children)
        {
            var str = children[4]
                .InnerText.Trim().ToUpper();
            if (str == "SOCKS4")
            {
                return ProxyType.Socks4;
            }
            if (str == "SOCKS5")
            {
                return ProxyType.Socks5;
            }
            if (str == "HTTPS")
            {
                return ProxyType.Https;
            }
            return ProxyType.Http;
        }

        ProxyAnonymity ResolveAnon(HtmlNode[] children)
        {
            try
            {
                var str = children[5]
                    .InnerText.Trim().ToLower();
                if (str == "high")
                {
                    return ProxyAnonymity.High;
                }

                if (str == "low")
                {
                    return ProxyAnonymity.Low;
                }

                if (str == "medium")
                {
                    return ProxyAnonymity.Medium;
                }
                return ProxyAnonymity.No;
            }
            catch
            {
                return ProxyAnonymity.No;
            }
        }
    }
}
