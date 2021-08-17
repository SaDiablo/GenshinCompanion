using GenshinCompanion.Services.Interfaces;

namespace GenshinCompanion.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
