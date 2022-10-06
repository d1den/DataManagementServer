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

var channel = new Channel(new ChannelModel()
{
    ValueType = TypeCode.String,
    Value = "Hello World!"
});
var sub = channel.ObservableUpdate.Subscribe(s => Console.WriteLine(s.EventArgs.UpdatedFields.FirstOrDefault().Value));
channel.UpdateValue("New string!");
sub.Dispose();

channel.UpdateValue("dawdawdw");
Console.ReadLine();