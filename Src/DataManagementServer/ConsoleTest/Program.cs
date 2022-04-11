using DataManagementServer.Core.Channels;
using DataManagementServer.Sdk.Channels;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

var channel = new Channel();
var model = new ChannelModel()
{
    Id = channel.Id,
    ValueType = TypeCode.Int32,
    Value = 12
};

channel.ObservableUpdate.Select(data => data.EventArgs).Subscribe(e =>
{
    Console.WriteLine("-------");
    Console.WriteLine($"Channel {e.Id} updated");
    foreach(var field in e.UpdatedFields)
    {
        Console.WriteLine($"Field '{field.Key}' new value = {field.Value}");
    }
    Console.WriteLine("-------");
    Console.WriteLine();
});

channel.Update(model);

model = new ChannelModel(channel.Id)
{
    Description = "New descr",
    Name = "Very good name"
};
Thread.Sleep(TimeSpan.FromSeconds(1));
channel.Update(model);

Thread.Sleep(TimeSpan.FromSeconds(1));
channel.UpdateValue(500);

Console.ReadLine();