using System;
using TinyMessenger;

namespace WhitePaperBible.ViewModels.Messages
{
    public class LoggedInMessage : ITinyMessage
    {
        public LoggedInMessage()
        {
        }

        public object Sender { get; private set; }
    }
}
