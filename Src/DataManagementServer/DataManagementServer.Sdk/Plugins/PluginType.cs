namespace DataManagementServer.Sdk.Plugins
{
    /// <summary>
    /// Типы плагинов
    /// </summary>
    public enum PluginType
    {
        /// <summary>
        /// Стандартный
        /// </summary>
        Default,

        /// <summary>
        /// Транспортировщик данных 
        /// </summary>
        DataTransporter,

        /// <summary>
        /// Обработчик данных
        /// </summary>
        DataProcessor,

        /// <summary>
        /// Отправитель оповещений
        /// </summary>
        NotificationSender
    }
}
