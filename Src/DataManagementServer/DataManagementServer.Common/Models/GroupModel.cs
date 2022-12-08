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
        public GroupModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id группы</param>
        public GroupModel(Guid id) : base(id) { }
    }
}
