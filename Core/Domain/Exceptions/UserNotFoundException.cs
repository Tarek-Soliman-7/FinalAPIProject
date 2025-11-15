namespace Domain.Exceptions
{
    public sealed class UserNotFoundException:NotFoundException
    {
        public UserNotFoundException(string userEmail):base($"User with email: {userEmail} not found")
        {
            
        }
    }
}
