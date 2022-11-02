namespace PetTracker.Server.Exceptions
{
    public class DeserializationFailedException : Exception
    {
        public DeserializationFailedException()
        {
        }

        public DeserializationFailedException(string message) : base(message)
        {
        }
    }
}
