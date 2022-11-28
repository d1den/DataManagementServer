namespace DataManagementServer.Common.Schemes
{
    /// <summary>
    /// Схема модели устройства
    /// </summary>
    public class DeviceScheme
    {
        /// <summary>
        /// Название устройства
        /// </summary>
        public const string Name = nameof(Name);

        /// <summary>
        /// Статус устройства
        /// </summary>
        public const string Status = nameof(Status);

        /// <summary>
        /// Период опроса устройства, мс
        /// </summary>
        public const string PollingPeriod = nameof(PollingPeriod);
    }
}
