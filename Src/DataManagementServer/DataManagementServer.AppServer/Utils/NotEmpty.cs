using System;
using System.ComponentModel.DataAnnotations;

namespace DataManagementServer.AppServer.Utils
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class NotEmpty : ValidationAttribute
    {
        public const string DefaultErrorMessage = "Is not valid Guid";
        public NotEmpty() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)       {
            if (value is null)
            {
                return false;
            }

            return value switch
            {
                Guid guid => guid != Guid.Empty,
                _ => true,
            };
        }
    }
}
