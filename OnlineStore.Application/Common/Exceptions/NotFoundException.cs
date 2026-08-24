namespace OnlineStore.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string resourceType, string resourceIdentifier)
            : base($"{resourceType} with id: {resourceIdentifier} was not found.")
        {

        }
        public NotFoundException(string message)
            : base(message)
        {

        }

    }
}
