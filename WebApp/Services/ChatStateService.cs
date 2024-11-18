using System.Collections.Generic;
using WebApp.DTOs;

namespace WebApp.Services
{
    public class ChatStateService
    {
        public List<Message> Messages { get; private set; } = new()
        {
            new Message
            {
                IsUser = false,
                Content = "Welcome to Dine Master! I’m your virtual waiter. Feel free to ask me about our menu, specials, or anything else. For example, you can ask: 'Can you recommend a dessert?'"
            }
        };

        public bool IsBotTyping { get; set; } = false;

        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }
    }
}
