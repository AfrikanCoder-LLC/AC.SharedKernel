
namespace Kernel.Exceptions
{
    public class ExistingRecordNotFoundException : DomainException
    {
        public ExistingRecordNotFoundException(string fieldName, string valueFieldName) 
            : base($"An existing record was not found for {fieldName} that has value [{valueFieldName}] . Please ensure it exists ")
        {

        }
    }
}
