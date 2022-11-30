using DataManagementServer.Core.Channels;
using DataManagementServer.Sdk.Channels;
using DataManagementServer.Common.Models;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataManagementServer.Core.Services;
using Serilog;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Linq;

IList<String> strings = new List<String>();
strings.Add("1");
strings.Add("2");
strings.Add("3");
strings.Add("5");
strings.Add("4");
strings.Add("6");


Console.WriteLine(String.Join(",",strings.Skip(7).Take(10).ToList()));