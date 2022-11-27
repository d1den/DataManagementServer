using System;

namespace DataManagementServer.Common.Models
{
    /// <summary>
    /// Модель плагина
    /// </summary>
    public class PluginModel
    {
        /// <summary>
        /// Id плагина
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название плагина
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип плагина
        /// </summary>
        public PluginType Type { get; set; }
    }
}
