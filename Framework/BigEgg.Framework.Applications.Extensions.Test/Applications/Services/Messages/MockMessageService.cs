using BigEgg.Framework.Applications.Extensions.Applications.Services.Messages;
using System;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.Messages
{
    public class MockMessageService : IMessageService
    {
        public MessageType MessageType { get; private set; }

        public object Owner { get; private set; }

        public string Message { get; private set; }

        public Func<string, bool?> ShowQuestionAction { get; set; }

        public Func<string, bool> ShowYesNoQuestionAction { get; set; }


        public void ShowMessage(object owner, string message)
        {
            MessageType = MessageType.Message;
            Owner = owner;
            Message = message;
        }

        public void ShowWarning(object owner, string message)
        {
            MessageType = MessageType.Warning;
            Owner = owner;
            Message = message;
        }

        public void ShowError(object owner, string message)
        {
            MessageType = MessageType.Error;
            Owner = owner;
            Message = message;
        }

        public bool? ShowQuestion(object owner, string message)
        {
            Owner = owner;
            return ShowQuestionAction(message);
        }

        public bool ShowYesNoQuestion(object owner, string message)
        {
            Owner = owner;
            return ShowYesNoQuestionAction(message);
        }
    }


    public enum MessageType
    {
        None,
        Message,
        Warning,
        Error
    }
}
