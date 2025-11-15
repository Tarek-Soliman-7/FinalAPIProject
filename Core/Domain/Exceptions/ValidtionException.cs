namespace Domain.Exceptions
{
    public sealed class ValidtionException:Exception
    {
        public IEnumerable<string> Errors { get; set; } = [];
        public ValidtionException(IEnumerable<string> errors):base("Validation Failed")
        {
            Errors = errors;
        }
    }
}
