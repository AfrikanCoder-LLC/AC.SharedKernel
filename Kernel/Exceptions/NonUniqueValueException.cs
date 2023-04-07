
namespace Kernel.Exceptions
{
    public class NonUniqueValueException : DomainException
    {
        public NonUniqueValueException(string value, string valueFieldName, string itemType) : base
            ($@"Another {itemType} entity was found with a matching value for field [{valueFieldName}] = [{value}]. Please ensure the value must be unique")
        {
        }
    }
}
