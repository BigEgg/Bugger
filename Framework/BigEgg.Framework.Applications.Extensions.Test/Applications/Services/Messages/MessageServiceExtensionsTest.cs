using BigEgg.Framework.Applications.Extensions.Applications.Services.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Applications.Extensions.Test.Applications.Services.Messages
{
    [TestClass]
    public class MessageServiceExtensionsTest
    {
        private readonly string message = "Hello World";


        [TestMethod]
        public void ShowMessageTest()
        {
            var messageService = new MockMessageService();

            messageService.ShowMessage(message);
            Assert.AreEqual(MessageType.Message, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowMessageTest_ServiceNull()
        {
            MessageServiceExtensions.ShowMessage(null, message);
        }

        [TestMethod]
        public void ShowWarningTest()
        {
            var messageService = new MockMessageService();

            messageService.ShowWarning(message);
            Assert.AreEqual(MessageType.Warning, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowWarningTest_ServiceNull()
        {
            MessageServiceExtensions.ShowWarning(null, message);
        }

        [TestMethod]
        public void ShowErrorTest()
        {
            var messageService = new MockMessageService();

            messageService.ShowError(message);
            Assert.AreEqual(MessageType.Error, messageService.MessageType);
            Assert.AreEqual(message, messageService.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowErrorTest_ServiceNull()
        {
            MessageServiceExtensions.ShowError(null, message);
        }

        [TestMethod]
        public void ShowQuestionTest_ReturnTrue()
        {
            var messageService = new MockMessageService();

            var showQuestionCalled = false;
            messageService.ShowQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return true;
            };
            Assert.IsTrue(messageService.ShowQuestion(message) == true);
            Assert.IsTrue(showQuestionCalled);
        }

        [TestMethod]
        public void ShowQuestionTest_ReturnFalse()
        {
            var messageService = new MockMessageService();

            var showQuestionCalled = false;
            messageService.ShowQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return false;
            };
            Assert.IsTrue(messageService.ShowQuestion(message) == false);
            Assert.IsTrue(showQuestionCalled);
        }

        [TestMethod]
        public void ShowQuestionTest_ReturnNull()
        {
            var messageService = new MockMessageService();

            var showQuestionCalled = false;
            messageService.ShowQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return null;
            };
            Assert.IsFalse(messageService.ShowQuestion(message).HasValue);
            Assert.IsTrue(showQuestionCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowQuestionTest_ServiceNull()
        {
            MessageServiceExtensions.ShowQuestion(null, message);
        }

        [TestMethod]
        public void ShowYesNoQuestionTest_ReturnTrue()
        {
            var messageService = new MockMessageService();

            var showQuestionCalled = false;
            messageService.ShowYesNoQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return true;
            };
            Assert.IsTrue(messageService.ShowYesNoQuestion(message));
            Assert.IsTrue(showQuestionCalled);
        }

        [TestMethod]
        public void ShowYesNoQuestionTest_ReturnFalse()
        {
            var messageService = new MockMessageService();

            var showQuestionCalled = false;
            messageService.ShowYesNoQuestionAction = m =>
            {
                showQuestionCalled = true;
                Assert.AreEqual(message, m);
                return false;
            };
            Assert.IsFalse(messageService.ShowYesNoQuestion(message));
            Assert.IsTrue(showQuestionCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShowYesNoQuestionTest_ServiceNull()
        {
            MessageServiceExtensions.ShowYesNoQuestion(null, message);
        }
    }
}
