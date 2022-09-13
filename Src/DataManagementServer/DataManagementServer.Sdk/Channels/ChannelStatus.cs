namespace DataManagementServer.Sdk.Channels
{
    /// <summary>
    /// Статус Канала
    /// </summary>
    public enum ChannelStatus
    {
        /// <summary>
        /// Хорошее качество, данные достоверные
        /// </summary>
        Good,

        /// <summary>
        /// Плохое качество, данные не верны
        /// </summary>
        Bad,

        /// <summary>
        /// Соединение прервано
        /// </summary>
        NoConnection
    }
}
