using DataManagementServer.Core.Extentions;
using DataManagementServer.Core.Services.Abstract;
using DataManagementServer.Core.Services.Concrete;
using DataManagementServer.Sdk.PluginInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataManagementServer.Core.Tests.Extentions
{

    [TestClass]
    public class AssemblyExtentionsTest
    {
        [TestMethod]
        public void GetTypesByInterface_TypeExist()
        {
            //Arange
            var assembly = Assembly.LoadFrom(@"D:\GitRepositories\DataManagementServer\Src\DataManagementServer\DataManagementServer.Core\bin\Debug\net6.0\DataManagementServer.Core.dll");

            //Act
            var types = new List<Type>();
            types.AddRange(assembly.GetTypesByInterface(typeof(IPluginService)));

            //Assert
            Assert.AreEqual(1, types.Count);
            Assert.AreEqual(typeof(PluginService), types.FirstOrDefault());
        }

        [TestMethod]
        public void GetTypesByInterface_TypeNotExist()
        {
            //Arange
            var assembly = Assembly.LoadFrom(@"D:\GitRepositories\DataManagementServer\Src\DataManagementServer\DataManagementServer.Core\bin\Debug\net6.0\DataManagementServer.Core.dll");

            //Act
            var types = new List<Type>();
            types.AddRange(assembly.GetTypesByInterface(typeof(IPlugin)));

            //Assert
            Assert.AreEqual(0, types.Count);
        }
    }
}
