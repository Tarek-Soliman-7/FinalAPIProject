namespace Domain.Exceptions
{
    public sealed class BasketNotFoundException:NotFoundException
    {
        public BasketNotFoundException(string Id):base ($"Basket With Id : {Id} Not Found")
        {
            
        }
    }
}
