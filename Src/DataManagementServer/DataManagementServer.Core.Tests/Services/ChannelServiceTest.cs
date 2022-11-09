using DataManagementServer.Common.Models;
using DataManagementServer.Common.Schemes;
using DataManagementServer.Core.Services.Concrete;
using DataManagementServer.Sdk.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManagementServer.Core.Tests.Services
{
    [TestClass]
    public class ChannelServiceTest
    {
        [TestMethod]
        public void Create()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;

            //Act
            var channelId = channelService.Create();

            //Assert
            Assert.AreEqual(1, channelService.Count);
            Assert.AreNotEqual(Guid.Empty, channelId);
        }



        [TestMethod]
        public void CreateByGroupId_Retrieve()
        {
            //Arange
            var orgService = new OrganizationService();
            var groupService = orgService as IGroupService;
            var channelService = orgService as IChannelService;
            var groupId = groupService.Create();

            //Act
            var channelId = channelService.Create(groupId);
            var channelModel = channelService.Retrieve(channelId, true);

            //Assert
            Assert.AreEqual(1, channelService.Count);
            Assert.AreEqual(groupId, channelModel.GroupId);
        }

        [TestMethod]
        public void CreateByModel_Retrieve_AllFields()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var model = new ChannelModel()
            {
                Name = "My channel",
                ValueType = TypeCode.Int64,
                Value = (long)12
            };

            //Act
            var channelId = channelService.Create(model);
            var channelModel = channelService.Retrieve(channelId, true);

            //Assert
            Assert.IsTrue(channelModel.Fields.Count > model.Fields.Count);
            Assert.AreEqual(model.Name, channelModel.Name);
            Assert.AreEqual(model.ValueType, channelModel.ValueType);
            Assert.AreEqual(model.Value, channelModel.Value);
        }

        [TestMethod]
        public void CreateByModel_Retrieve_NotAllFields()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var model = new ChannelModel()
            {
                Name = "My channel",
                ValueType = TypeCode.Int64,
                Value = (long)12
            };

            //Act
            var channelId = channelService.Create(model);
            var channelModel = channelService.Retrieve(channelId);

            //Assert
            Assert.AreEqual(0, channelModel.Fields.Count);
            Assert.AreEqual(channelId, channelModel.Id);
        }

        [TestMethod]
        public void CreateByModel_Retrieve_FieldNames()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var model = new ChannelModel()
            {
                Name = "My channel",
                ValueType = TypeCode.Int64,
                Value = (long)12
            };
            var fields = new[] { ChannelScheme.Name, ChannelScheme.ValueType };
            //Act
            var channelId = channelService.Create(model);
            var channelModel = channelService.Retrieve(channelId, fields);

            //Assert
            Assert.AreEqual(fields.Length, channelModel.Fields.Count);
            Assert.AreEqual(channelId, channelModel.Id);
            Assert.AreEqual(model.Name, channelModel.Name);
            Assert.AreEqual(model.ValueType, channelModel.ValueType);
            Assert.IsFalse(channelModel.TryGetFieldValue<object>(ChannelScheme.Value, out _));
        }


        [TestMethod]
        public void CreateByNullModel_ArgumentNullException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;

            //Act
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => channelService.Create(null));
        }

        [TestMethod]
        public void RetrieveAll_AllFields()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
            };
            channelService.Create(channelModel);
            channelService.Create(channelModel);
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            //Act
            var channelModels = channelService.RetrieveAll(true);

            //Assert
            Assert.AreEqual(4, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.IsTrue(model.Fields.Count > channelModel.Fields.Count);
            });
        }

        [TestMethod]
        public void RetrieveAll_NotAllFields()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
            };
            channelService.Create(channelModel);
            channelService.Create(channelModel);
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            //Act
            var channelModels = channelService.RetrieveAll();

            //Assert
            Assert.AreEqual(4, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(0, model.Fields.Count);
            });
        }

        [TestMethod]
        public void RetrieveAll_FieldNames()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
            };
            channelService.Create(channelModel);
            channelService.Create(channelModel);
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            //Act
            var channelModels = channelService.RetrieveAll(ChannelScheme.Name);

            //Assert
            Assert.AreEqual(4, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(1, model.Fields.Count);
                Assert.AreEqual(channelModel.Name, model.Name);
            });
        }

        [TestMethod]
        public void RetrieveByGroup_FieldNames()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
            };
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            channelModel.GroupId = Guid.NewGuid();
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            //Act
            var channelModels = channelService.RetrieveByGroup(channelModel.GroupId.Value, ChannelScheme.Name);

            //Assert
            Assert.AreEqual(2, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(1, model.Fields.Count);
                Assert.AreEqual(channelModel.Name, model.Name);
            });
        }

        [TestMethod]
        public void RetrieveByGroup_AllFields()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
            };
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            channelModel.GroupId = Guid.NewGuid();
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            //Act
            var channelModels = channelService.RetrieveByGroup(channelModel.GroupId.Value, true);

            //Assert
            Assert.AreEqual(2, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.IsTrue(model.Fields.Count > channelModel.Fields.Count);
            });
        }

        [TestMethod]
        public void RetrieveByGroup_NotAllFields()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
            };
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            channelModel.GroupId = Guid.NewGuid();
            channelService.Create(channelModel);
            channelService.Create(channelModel);

            //Act
            var channelModels = channelService.RetrieveByGroup(channelModel.GroupId.Value);

            //Assert
            Assert.AreEqual(2, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(0, model.Fields.Count);
            });
        }

        [TestMethod]
        public void RetrieveValue()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            var value = channelService.RetrieveValue(channelId);

            //Assert
            Assert.AreEqual(channelModel.Value, value);
        }

        [TestMethod]
        public void RetrieveValue_KeyNotFoundException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => channelService.RetrieveValue(Guid.Empty));
        }

        [TestMethod]
        public void RetrieveValue_ByType()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            var value = channelService.RetrieveValue<string>(channelId);

            //Assert
            Assert.AreEqual(channelModel.Value, value);
        }

        [TestMethod]
        public void RetrieveValue_ByType_KeyNotFoundException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => channelService.RetrieveValue<string>(Guid.Empty));
        }

        [TestMethod]
        public void RetrieveValue_ByType_ArgumentException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            //Assert
            Assert.ThrowsException<InvalidCastException>(() => channelService.RetrieveValue<int>(channelId));
        }

        [TestMethod]
        public void TryRetrieveValue_ByType()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            var result = channelService.TryRetrieveValue<string>(channelId, out var value);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(channelModel.Value, value);
        }

        [TestMethod]
        public void TryRetrieveValue_ByType_Nullable()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.Int32,
                Value = 12
            };
            var channelId = channelService.Create(channelModel);

            //Act
            var result = channelService.TryRetrieveValue<int?>(channelId, out var value);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(channelModel.Value, value);
        }

        [TestMethod]
        public void TryRetrieveValue_ByType_WrongId()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);

            //Act
            var result = channelService.TryRetrieveValue<string>(Guid.Empty, out var value);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryRetrieveValue_ByType_WrongType()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };
            var channelId = channelService.Create(channelModel);
            //Act
            var result = channelService.TryRetrieveValue<int>(Guid.Empty, out var value);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Update()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelId = channelService.Create();

            var channelModel = new ChannelModel(channelId)
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };

            //Act
            channelService.Update(channelModel);
            var model = channelService.Retrieve(channelId, channelModel.Fields.Keys.ToArray());

            //Assert
            Assert.AreEqual(channelModel.Name, model.Name);
            Assert.AreEqual(channelModel.ValueType, model.ValueType);
            Assert.AreEqual(channelModel.Value, model.Value);
        }

        [TestMethod]
        public void Update_ArgumentNullException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelId = channelService.Create();

            //Act
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => channelService.Update(null));
        }

        [TestMethod]
        public void Update_KeyNotFoundException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelId = channelService.Create();

            var channelModel = new ChannelModel()
            {
                Name = "Test",
                ValueType = TypeCode.String,
                Value = "New value"
            };

            //Act
            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => channelService.Update(channelModel));
        }

        [TestMethod]
        public void UpdateValue()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelId = channelService.Create();

            var arrangeValue = "New value";

            //Act
            channelService.UpdateValue(channelId, arrangeValue);
            var value = channelService.RetrieveValue(channelId);

            //Assert
            Assert.AreEqual(arrangeValue, value);
        }

        [TestMethod]
        public void UpdateValue_KeyNotFoundException()
        {
            //Arange
            var channelService = new OrganizationService() as IChannelService;
            var channelId = channelService.Create();

            var arrangeValue = "New value";

            //Act
            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => channelService.UpdateValue(Guid.Empty, arrangeValue));
        }
    }
}
