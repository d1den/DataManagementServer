namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Статусы работы устройства
    /// </summary>
    public enum DeviceStatus
    {
        /// <summary>
        /// Нет статуса
        /// </summary>
        None,
        
        /// <summary>
        /// Создано
        /// </summary>
        Created,

        /// <summary>
        /// Нормально работает
        /// </summary>
        Runnig,

        /// <summary>
        /// Остановлено
        /// </summary>
        Stoped,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error
    }
}
