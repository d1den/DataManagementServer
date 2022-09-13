using System;

namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Аргумент события изменения коллекции элементов
    /// </summary>
    public class CollectionChangeEventArgs
    {
        /// <summary>
        /// Id Элемента, с которомы связаны изменения
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Тип изменения
        /// </summary>
        public CollectionChangeType Type { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id Элемента, с которомы связаны изменения</param>
        /// <param name="type">Тип изменения</param>
        public CollectionChangeEventArgs(Guid id, CollectionChangeType type)
        {
            Id = id;
            Type = type;
        }
    }
}
