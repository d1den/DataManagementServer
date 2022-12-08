using DataManagementServer.Common.Resources;
using DataManagementServer.Common.Schemes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
                if (TryGetFieldValue(GroupScheme.ParentId, out object parentId))
                {
                    if (parentId is Guid)
                    {
                        return (Guid)parentId;
                    }

                    if (parentId is string)
                    {
                        return Guid.Parse(parentId as string);
                    }

                    if (parentId is null)
                    {
                        return null;
                    }

                    throw new InvalidCastException(string.Format(ErrorMessages.ValueTypeError, typeof(Guid), parentId?.GetType()));
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
