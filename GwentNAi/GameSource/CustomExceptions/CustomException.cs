namespace GwentNAi.GameSource.CustomExceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
            Console.WriteLine(message);
        }
    }
}
