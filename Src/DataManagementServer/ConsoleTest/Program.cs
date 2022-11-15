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

PrintTypeName<BaseModel>();
Console.ReadLine();

static void PrintTypeName<T>()
{
    var type = typeof(T);
    Console.WriteLine(type?.FullName);
}