namespace BookInfo.API.Services
{
    public class Mail : IMail
    {
        private string To = "Paula.mikhail@gmail.com";
        private string From = "noreplay@hotmail.com";

        public void send(string massage)
        {
            Console.WriteLine($"Mail from {From}");
            Console.WriteLine($"Mail To {To}");
            Console.WriteLine($"Mail Body {massage} A7aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }
}
