
using System;
using System.Text.Json.Serialization;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Базовая модель
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Id сущности
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Поля канала
        /// </summary>
        public FieldValueCollection Fields { get; set; } = new();

        /// <summary>
        /// Свойство доступа к поля модели
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <returns>Значение поля</returns>
        public object this[string fieldName]
        {
            get { return Fields[fieldName]; }
            set { Fields.Add(fieldName, value); }
        }

        /// <summary>
        /// Конструктор модели
        /// </summary>
        public BaseModel() { }

        /// <summary>
        /// Конструктор модели
        /// </summary>
        /// <param name="id">Id сущности</param>
        public BaseModel(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Преобразовать модель к другой, унаследованной от базовой
        /// </summary>
        /// <typeparam name="T">Тип модели, унаследованной от базовой</typeparam>
        /// <returns>Преобразованная модель</returns>
        public T CastToModel<T>() where T : BaseModel
        {
            var newModel = Activator.CreateInstance(typeof(T)) as BaseModel;
            newModel.Id = Id;
            newModel.Fields = Fields.Clone() as FieldValueCollection;

            return newModel as T;
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
        /// <returns>Значение поля или узначение по умолчанию, если поля нет</returns>
        /// <exception cref="ArgumentNullException">Ошибка при передаче пустого имени поля</exception>
        /// <exception cref="InvalidCastException">Ошибка при невозможности приведения поля к заданному типу</exception>
        public T GetFieldValue<T>(string fieldName)
        {
            return Fields.GetFieldValue<T>(fieldName);
        }
        #endregion
    }
}
