using BigEgg.Framework.Applications.Applications.Commands;
using BigEgg.Framework.Applications.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigEgg.Framework.Applications.Test.Applications.Commands
{
    [TestClass]
    public class DelegateCommandTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void ConstructorTest()
        {
            DelegateCommand command = new DelegateCommand(() => { });
            Assert.IsNotNull(command);
            Assert.IsTrue(command.CanExecute());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Execute_Null()
        {
            new DelegateCommand(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_CanExecute_Null()
        {
            new DelegateCommand(() => { }, null);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            bool executed = false;
            DelegateCommand command = new DelegateCommand(() => executed = true);

            command.Execute();
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void ExecuteTest_CanExecute_True()
        {
            bool executed = false;
            bool canExecute = true;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => true);

            command.Execute();
            Assert.IsTrue(executed);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExecuteTest_CanExecute_False()
        {
            DelegateCommand command = new DelegateCommand(() => { }, () => false);
            command.Execute();
        }

        [TestMethod]
        public void RaiseCanExecuteChangedTest()
        {
            bool executed = false;
            bool canExecute = false;
            DelegateCommand command = new DelegateCommand(() => executed = true, () => canExecute);

            Assert.IsFalse(command.CanExecute());
            canExecute = true;
            Assert.IsTrue(command.CanExecute());

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());

            Assert.IsFalse(executed);
        }
    }
}
