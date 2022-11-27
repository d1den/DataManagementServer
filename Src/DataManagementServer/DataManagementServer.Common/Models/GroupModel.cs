using DataManagementServer.Common.Schemes;
using System;
using System.Text.Json.Serialization;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Группа каналов ядра
    /// </summary>
    public class GroupModel : BaseModel
    {
        /// <summary>
        /// Id корневой группы
        /// </summary>
        [JsonIgnore]
        public static readonly Guid RootGroupId = Guid.Empty;

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
    }
}
