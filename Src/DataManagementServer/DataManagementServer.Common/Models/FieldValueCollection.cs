using System;
using System.Collections.Generic;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Коллекция Значений полей
    /// </summary>
    public class FieldValueCollection : Dictionary<string, object>, ICloneable
    {
        /// <summary>
        /// Попытка получения типизированного значения поля
        /// </summary>
        /// <typeparam name="T">Тип значения поля</typeparam>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="fieldValue">Значение поля</param>
        /// <returns>Результат получения значения</returns>
        /// <exception cref="ArgumentNullException">Ошибка при передаче пустого имени поля</exception>
        public bool TryGetFieldValue<T>(string fieldName, out T fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            fieldValue = default;
            try
            {
                if (TryGetValue(fieldName, out object value)
                    && (value is T
                        || Nullable.GetUnderlyingType(typeof(T)) != null))
                {
                    fieldValue = (T)value;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Получение типизированного значения поля
        /// </summary>
        /// <typeparam name="T">Тип значения поля</typeparam>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение поля или узначение по умолчанию, если поля нет</returns>
        /// <exception cref="ArgumentNullException">Ошибка при передаче пустого имени поля</exception>
        /// <exception cref="InvalidCastException">Ошибка при невозможности приведения поля к заданному типу</exception>
        public T GetFieldValue<T>(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            if (TryGetValue(fieldName, out object value))
            {
                return (T)value;
            }

            return default;
        }

        /// <summary>
        /// Создать копию коллекции
        /// </summary>
        /// <returns>Копия коллекции</returns>
        /// <exception cref="Exception">Ошибка при невозможности клонировать поле</exception>
        public object Clone()
        {
            var copyFields = new FieldValueCollection();
            foreach(var field in this)
            {
                if (field.Value.GetType().IsClass 
                    && field.Value is not string 
                    && field.Value is not DBNull)
                {
                    if (field.Value is not ICloneable)
                    {
                        throw new Exception($"Can1t clone field with type {field.Value.GetType().Name}");
                    }
                    copyFields.Add(field.Key, (field.Value as ICloneable)?.Clone());
                    continue;
                }
                copyFields.Add(field.Key, field.Value);
            }
            return copyFields;
        }
    }
}
