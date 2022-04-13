using System;

namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Аргумент события обновления элемента
    /// </summary>
    public class UpdateEventArgs
    {
        /// <summary>
        /// Id обновлённого элемента
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Обновлённые поля
        /// </summary>
        public FieldValueCollection UpdatedFields { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id обновлённого элемента</param>
        /// <param name="fields">Обновлённые поля</param>
        /// <exception cref="ArgumentException">Ошибка при пустом id</exception>
        /// <exception cref="ArgumentNullException">Ошибка при null значении полей</exception>
        public UpdateEventArgs(Guid id, FieldValueCollection fields)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(nameof(id));
            }
            UpdatedFields = fields ?? throw new ArgumentNullException(nameof(fields));
            Id = id;
        }
    }
}
