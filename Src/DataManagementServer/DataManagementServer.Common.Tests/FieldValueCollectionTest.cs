using DataManagementServer.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace DataManagementServer.Common.Tests
{
    [TestClass]
    public class FieldValueCollectionTest
    {
        [TestMethod]
        public void GetFieldValue_ExistingValue_CorrectType()
        {
            //Arrange
            var fieldName = "isGoodDay";
            var fieldValue = true;

            var fields = new FieldValueCollection();
            fields[fieldName] = fieldValue;

            //Act
            var result = fields.GetFieldValue<bool>(fieldName);

            //Assert
            Assert.AreEqual(fieldValue, result);
        }

        [TestMethod]
        public void GetFieldValue_ExistingValue_NullableType()
        {
            //Arrange
            var fieldName = "isGoodDay";
            var fieldValue = true;

            var fields = new FieldValueCollection();
            fields[fieldName] = fieldValue;

            //Act
            var result = fields.GetFieldValue<bool?>(fieldName);

            //Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(fieldValue, result.Value);
        }

        [TestMethod]
        public void GetFieldValue_ExistingValue_IncorrectType_InvalidCastException()
        {
            //Arrange
            var fieldName = "isGoodDay";
            var fieldValue = true;

            var fields = new FieldValueCollection();
            fields[fieldName] = fieldValue;

            //Act
            //Assert
            Assert.ThrowsException<InvalidCastException>(() => fields.GetFieldValue<int>(fieldName));
        }

        [TestMethod]
        public void GetFieldValue_NotExistingValue_CorrectType()
        {
            //Arrange
            var fieldName = "isGoodDay";

            var fields = new FieldValueCollection();

            //Act
            var result = fields.GetFieldValue<bool>(fieldName);

            //Assert
            Assert.AreEqual(default, result);
        }

        [TestMethod]
        public void GetFieldValue_NotExistingValue_NullableType()
        {
            //Arrange
            var fieldName = "isGoodDay";

            var fields = new FieldValueCollection();

            //Act
            var result = fields.GetFieldValue<bool?>(fieldName);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetFieldValue_NotExistingValue_IncorrectType()
        {
            //Arrange
            var fieldName = "isGoodDay";

            var fields = new FieldValueCollection();

            //Act
            var result = fields.GetFieldValue<int>(fieldName);

            //Assert
            Assert.AreEqual(default, result);
        }

        [TestMethod]
        public void TryGetFieldValue_ExistingValue_CorrectType()
        {
            //Arrange
            var fieldName = "isGoodDay";
            var fieldValue = true;

            var fields = new FieldValueCollection();
            fields[fieldName] = fieldValue;

            //Act
            var result = fields.TryGetFieldValue<bool>(fieldName, out var actualValue);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(fieldValue, actualValue);
        }

        [TestMethod]
        public void TryGetFieldValue_ExistingValue_NullableType()
        {
            //Arrange
            var fieldName = "isGoodDay";
            var fieldValue = true;

            var fields = new FieldValueCollection();
            fields[fieldName] = fieldValue;

            //Act
            var result = fields.TryGetFieldValue<bool?>(fieldName, out var actualValue);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(actualValue.HasValue);
            Assert.AreEqual(fieldValue, actualValue.Value);
        }

        [TestMethod]
        public void TryGetFieldValue_ExistingValue_IncorrectType()
        {
            //Arrange
            var fieldName = "isGoodDay";
            var fieldValue = true;

            var fields = new FieldValueCollection();
            fields[fieldName] = fieldValue;

            //Act
            var result = fields.TryGetFieldValue<int>(fieldName, out var actualValue);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(default, actualValue);
        }

        [TestMethod]
        public void TryGetFieldValue_NotExistingValue()
        {
            //Arrange
            var fieldName = "isGoodDay";

            var fields = new FieldValueCollection();

            //Act
            var result = fields.TryGetFieldValue<int>(fieldName, out var actualValue);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(default, actualValue);
        }

        [TestMethod]
        public void Clone_DotNetBaseFields()
        {
            //Arrange
            var fields = new FieldValueCollection()
            {
                ["field1"] = 12,
                ["field2"] = DateTime.Now,
                ["field3"] = Guid.NewGuid(),
                ["field4"] = "Hello, World!"
            };

            //Act
            var result = fields.Clone() as FieldValueCollection;

            //Assert
            Assert.AreNotEqual(fields, result);
        }

        [TestMethod]
        public void Clone_StructFields()
        {
            //Arrange
            var fields = new FieldValueCollection()
            {
                ["field1"] = new Point(2, 4)
            };

            //Act
            var result = fields.Clone() as FieldValueCollection;

            //Assert
            Assert.AreNotEqual(fields, result);
        }

        [TestMethod]
        public void Clone_ICloneableFields()
        {
            //Arrange
            //—транный пример конечно, но надо было придумать какое-то поле, реализующее ICloneable
            var fields = new FieldValueCollection()
            {
                ["field1"] = new Point(2, 4),
                ["field2"] = new FieldValueCollection()
                {
                    ["field"] = "Hello, World!"
                }
            };

            //Act
            var result = fields.Clone() as FieldValueCollection;

            //Assert
            Assert.AreNotEqual(fields, result);
        }

        [TestMethod]
        public void Clone_ClassFieldsWithoutICloneable_Exception()
        {
            //Arrange
            //—транный пример конечно, но надо было придумать какое-то поле, реализующее ICloneable
            var fields = new FieldValueCollection()
            {
                ["field1"] = new Point(2, 4),
                ["field2"] = new PluginModel()
            };

            //Act
            //Assert
            Assert.ThrowsException<Exception>(() => fields.Clone());
        }
    }
}