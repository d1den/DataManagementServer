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

var channelModel = new ChannelModel()
{
    Id = Guid.NewGuid(),
    Name = "test"
};

var model = channelModel as BaseModel;
var json = JsonConvert.SerializeObject(model);

var baseModels = JsonConvert.DeserializeObject<ChannelModel>(json);


Console.ReadLine();
/*
var service = new OrganizationService() as IGroupService;
service.SubscribeOnCollectionChange(e =>
{
    Console.WriteLine($"{e.Type} - Group {e.Id}");
});
var cts = new CancellationTokenSource();
var group1 = service.Create();
service.SubscribeOnUpdate(group1, e =>
{
    Console.WriteLine("------");
    Console.WriteLine("Group update");
    Console.WriteLine(e.Id);
    foreach (var field in e.UpdatedFields)
    {
        Console.WriteLine($"Fields '{field.Key}' - '{field.Value}'");
    }
    Console.WriteLine("------");
    Console.WriteLine();
},
cts.Token);
var group2 = service.Create(new GroupModel() { Name = "My group" });
var group3 = service.Create(new GroupModel() { Name = "Device 1", ParentId = group1 });

service.Update(new GroupModel(group1) { Name = "First group" });
var groups = service.RetrieveByParent(group1, true);
foreach(var group in groups)
{
    PrintGroup(group);
}
cts.Cancel();
// !поправить!
// у групп можно обновлять только имя - id родителя устанавливается при создании
service.Update(new GroupModel(group1) { Name = "New name" });
service.Delete(group2, true);

Console.ReadLine();

void PrintGroup(GroupModel group)
{
    Console.WriteLine("------");
    Console.WriteLine($"Group {group.Id}");
    Console.WriteLine($"Parent {group.ParentId}");
    Console.WriteLine($"Name {group.Name}");
    Console.WriteLine("------");
    Console.WriteLine();
}
*/