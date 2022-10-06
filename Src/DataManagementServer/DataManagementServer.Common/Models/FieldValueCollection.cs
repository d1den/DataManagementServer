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
        /// Ошибка неправильного типа значения поля
        /// </summary>
        private const string _FieldValueTypeError = "Field value has another type";

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
                if (TryGetValue(fieldName, out object value))
                {
                    if (value is T 
                        || Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        fieldValue = (T)value;
                        return true;
                    }
                    else if (value is null
                        && Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Получение типизированного значения поля
        /// </summary>
        /// <typeparam name="T">Тип значения поля</typeparam>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение поля</returns>
        /// <exception cref="ArgumentNullException">Ошибка при передаче пустого имени поля</exception>
        /// <exception cref="Exception">Ошибка при несоответвии типа поля</exception>
        public T GetFieldValue<T>(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            var value = this[fieldName];
            if (value is T 
                || Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                return (T)value;
            }
            else if (value is null
                && Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                return default(T);
            }
            else
            {
                throw new Exception(_FieldValueTypeError);
            }
        }

        /// <summary>
        /// Создать копию коллекции
        /// </summary>
        /// <returns>Копия коллекции</returns>
        public object Clone()
        {
            var copyFields = new FieldValueCollection();
            foreach(var field in this)
            {
                copyFields.Add(field.Key, field.Value);
            }
            return copyFields;
        }
    }
}
