using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataManagementServer.Sdk.Extensions;

namespace DataManagementServer.Sdk.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTest
    {
        [TestMethod]
        public void GetTypeByCode_Bool_GetBool()
        {
            // Arrange
            var code = TypeCode.Boolean;

            // Act
            var type = TypeExtensions.GetTypeByCode(code);

            // Assert
            Assert.AreEqual(type, typeof(bool));
        }

        [TestMethod]
        public void GetTypeByCode_Char_GetChar()
        {
            // Arrange
            var code = TypeCode.Char;

            // Act
            var type = TypeExtensions.GetTypeByCode(code);

            // Assert
            Assert.AreEqual(type, typeof(char));
        }

        [TestMethod]
        public void GetTypeByCode_Sbyte_GetSbyte()
        {
            // Arrange
            var code = TypeCode.SByte;

            // Act
            var type = TypeExtensions.GetTypeByCode(code);

            // Assert
            Assert.AreEqual(type, typeof(sbyte));
        }

        [TestMethod]
        public void GetTypeByCode_Uint64_GetUint64()
        {
            // Arrange
            var code = TypeCode.UInt64;

            // Act
            var type = TypeExtensions.GetTypeByCode(code);

            // Assert
            Assert.AreEqual(type, typeof(ulong));
        }

        [TestMethod]
        public void GetTypeByCode_Single_GetSingle()
        {
            // Arrange
            var code = TypeCode.Single;

            // Act
            var type = TypeExtensions.GetTypeByCode(code);

            // Assert
            Assert.AreEqual(type, typeof(float));
        }

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

        [TestMethod]
        public void GetDefaultValue_DateTime_GetDefault()
        {
            // Arrange
            var type = typeof(DateTime);

            // Act
            var defaultValue = type.GetDefaultValue();

            // Assert
            Assert.AreEqual(defaultValue, default(DateTime));
        }

        [TestMethod]
        public void GetDefaultValue_Int_GetDefault()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var defaultValue = type.GetDefaultValue();

            // Assert
            Assert.AreEqual(defaultValue, default(int));
        }

        [TestMethod]
        public void GetDefaultValue_DbNull_GetDefault()
        {
            // Arrange
            var type = typeof(DBNull);

            // Act
            var defaultValue = type.GetDefaultValue();

            // Assert
            Assert.AreEqual(defaultValue, default(DBNull));
        }

        [TestMethod]
        public void GetDefaultValue_Decimal_GetDefault()
        {
            // Arrange
            var type = typeof(decimal);

            // Act
            var defaultValue = type.GetDefaultValue();

            // Assert
            Assert.AreEqual(defaultValue, default(decimal));
        }
    }
}
