using Newtonsoft.Json;
using System;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Базовая модель
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Поля канала
        /// </summary>
        public FieldValueCollection Fields { get; } = new();

        /// <summary>
        /// Свойство доступа к поля модели
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <returns>Значение поля</returns>
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public object this[string fieldName]
        {
            get { return Fields[fieldName]; }
            set { Fields.Add(fieldName, value); }
        }

        #region Методы доступа к полям
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
            return Fields.TryGetFieldValue(fieldName, out fieldValue);
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
            return Fields.GetFieldValue<T>(fieldName);
        }
        #endregion
    }
}
