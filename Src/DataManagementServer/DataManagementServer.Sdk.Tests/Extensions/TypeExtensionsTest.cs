using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataManagementServer.Sdk.Extensions;

namespace DataManagementServer.Sdk.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTest
    {

        [TestMethod]
        public void GetTypeByCode_String_GetString()
        {
            // Arrange
            var code = TypeCode.String;

            // Act
            var type = TypeExtensions.GetTypeByCode(code);

            // Assert
            Assert.AreEqual(type, typeof(string));
        }

        [TestMethod]
        public void GetDefaultValue_String_GetDefault()
        {
            // Arrange
            var type = typeof(string);

            // Act
            var defaultValue = type.GetDefaultValue();

            // Assert
            Assert.AreEqual(defaultValue, default(string));
        }
    }
}
