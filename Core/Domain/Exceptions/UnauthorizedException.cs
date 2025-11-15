namespace Domain.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message= $"Invalid email or password"):base(message)
        {
            
        }
    }
}
