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

            return (T)value;
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
