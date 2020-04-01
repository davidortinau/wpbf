using System;
using TinyMessenger;

namespace WhitePaperBible.ViewModels.Messages
{
    public class LoggedOutMessage : ITinyMessage
    {
        public LoggedOutMessage()
        {
        }

        public object Sender { get; private set; }
    }
}
