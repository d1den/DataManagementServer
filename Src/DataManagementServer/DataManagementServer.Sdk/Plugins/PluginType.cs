namespace DataManagementServer.Sdk.Plugins
{
    /// <summary>
    /// Типы плагинов
    /// </summary>
    public enum PluginType
    {
        /// <summary>
        /// Стандартный тип
        /// </summary>
        Default,

        /// <summary>
        /// Источник данных
        /// </summary>
        DataSource,

        /// <summary>
        /// Обработка данных
        /// </summary>
        DataProccess,

        /// <summary>
        /// Плагин оповещения
        /// </summary>
        Notification
    }
}
