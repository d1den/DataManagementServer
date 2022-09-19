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