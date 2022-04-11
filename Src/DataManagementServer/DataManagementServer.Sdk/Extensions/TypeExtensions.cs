using System;

namespace DataManagementServer.Sdk.Extensions
{
    /// <summary>
    /// Расширения для класса System.Type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Получить тип его коду
        /// </summary>
        /// <param name="typeCode">Код типа</param>
        /// <returns>Тип</returns>
        public static Type GetTypeByCode(TypeCode typeCode)
        {
            var type = Type.GetType($"System.{typeCode}");
            return type;
        }
        /// <summary>
        /// Получить значение типа по умолчанию
        /// </summary>
        /// <param name="type">Тип, к которому применяется метод</param>
        /// <returns>Значение по умолчанию, упакованное в object</returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type == typeof(float))
            {
                return default(float);
            }
            if (type == typeof(double))
            {
                return default(double);
            }
            if (type == typeof(decimal))
            {
                return default(decimal);
            }
            if (type == typeof(DateTime))
            {
                return default(DateTime);
            }
            if (type == typeof(bool))
            {
                return default(bool);
            }
            if (type == typeof(sbyte))
            {
                return default(sbyte);
            }
            if (type == typeof(byte))
            {
                return default(byte);
            }
            if (type == typeof(short))
            {
                return default(short);
            }
            if (type == typeof(ushort))
            {
                return default(ushort);
            }
            if (type == typeof(int))
            {
                return default(int);
            }
            if (type == typeof(uint))
            {
                return default(uint);
            }
            if (type == typeof(ulong))
            {
                return default(ulong);
            }
            if (type == typeof(long))
            {
                return default(long);
            }

            return null;
        }
    }
}
