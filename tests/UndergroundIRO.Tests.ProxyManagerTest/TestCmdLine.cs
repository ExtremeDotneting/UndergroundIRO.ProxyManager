using System;
using System.Collections.Generic;
using System.Net.Http;
using IRO.CmdLine;
using IRO.XWebView.CefSharp.OffScreen.Providers;
using IRO.XWebView.Core;
using IRO.XWebView.Core.Consts;
using Newtonsoft.Json;
using UndergroundIRO.ProxyManager.Core;
using UndergroundIRO.ProxyManager.Core.Models;
using UndergroundIRO.ProxyManager.HidemyParser;

namespace UndergroundIRO.Tests.BestChangeParserKitTest
{
    public class TestCmdLine : CommandLineBase
    {
        int _callNum = 1;

        IXWebView _xwv;

        public TestCmdLine(CmdLineExtension cmdLineExtension = null) : base(cmdLineExtension)
        {
        }

        [CmdInfo]
        public void TestHidemy()
        {
            var settings = new HidemyProxyParseSettings
            {
                MaxTime = 1000,
                Limit = int.MaxValue
            };
            var parser = new HidemyProxyParser(GetXWV());
            var list = parser.Parse(settings).Result;
            Cmd.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
            Cmd.WriteLine("\n\nCall number: " + _callNum++);
             Cmd.WriteLine("Records count: " + list.Count);
            Cmd.WriteLine("---------------");
        }

        IXWebView GetXWV()
        {
            if (_xwv == null)
            {
                _xwv = new OffScreenCefSharpXWebViewProvider().Resolve().Result;
            }
            return _xwv;
        }
    }
}
