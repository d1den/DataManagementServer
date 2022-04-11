using System;
using Newtonsoft.Json;

namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Группа каналов ядра
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// Id группы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id родительской группы
        /// </summary>
        [JsonIgnore]
        public Guid? ParentId
        {
            get
            {
                if (TryGetFieldValue(GroupScheme.ParentId, out Guid parentId))
                {
                    return parentId;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Fields[GroupScheme.ParentId] = value;
            }
        }

        /// <summary>
        /// Название группы
        /// </summary>
        [JsonIgnore]
        public string Name
        {
            get
            {
                if (TryGetFieldValue(GroupScheme.Name, out string name))
                {
                    return name;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Fields[GroupScheme.Name] = value;
            }
        }

        /// <summary>
        /// Поля группы
        /// </summary>
        public FieldValueCollection Fields { get; } = new ();

        /// <summary>
        /// Свойство доступа к поля модели
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <returns>Значение поля</returns>
        [JsonIgnore]
        public object this[string fieldName]
        {
            get { return Fields[fieldName]; }
            set { Fields.Add(fieldName, value); }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public GroupModel()
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id группы</param>
        /// <exception cref="ArgumentNullException">Ошибка, если Id пустое</exception>
        public GroupModel(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            Id = id;
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
