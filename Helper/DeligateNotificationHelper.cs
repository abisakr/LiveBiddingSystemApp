namespace Live_Bidding_System_App.Helper
{
    public class DeligateNotificationHelper
    {
        public static void SendNotification(string message)
        {
            // Here you can implement your notification logic (e.g., sending an email)
            Console.WriteLine($"Item edited: {message}");
        }
        public static void RenderInfo(string message)
        {
            // Here you can implement your notification logic (e.g., sending an email)
            Console.WriteLine($"Log : Item name is {message}");
        }
    }
}
