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
    public class GenericDelegateCommandTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void ConstructorTest()
        {
            DelegateCommand<int?> command = new DelegateCommand<int?>(param => { });
            Assert.IsNotNull(command);
            Assert.IsTrue(command.CanExecute(1));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Constructor_InvalidType()
        {
            new DelegateCommand<int>(param => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Execute_Null()
        {
            new DelegateCommand<object>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_CanExecute_Null()
        {
            new DelegateCommand<object>((o) => { }, null);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            bool executed = false;
            object commandParameter = null;
            DelegateCommand<object> command = new DelegateCommand<object>((object parameter) =>
            {
                executed = true;
                commandParameter = parameter;
            });

            object obj = new object();
            command.Execute(obj);
            Assert.IsTrue(executed);
            Assert.AreEqual(obj, commandParameter);
        }

        [TestMethod]
        public void ExecuteTest_CanExecute_True()
        {
            bool executed = false;
            DelegateCommand<object> command = new DelegateCommand<object>((o) =>
            {
                executed = true;
            }, (o) => true);

            object obj = new object();
            command.Execute(obj);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExecuteTest_CanExecute_False()
        {
            DelegateCommand<object> command = new DelegateCommand<object>((o) => { }, (o) => false);
            object obj = new object();
            command.Execute(obj);
        }

        [TestMethod]
        public void RaiseCanExecuteChangedTest()
        {
            bool executed = false;
            bool canExecute = false;
            DelegateCommand<object> command = new DelegateCommand<object>((o) => executed = true, (o) => canExecute);

            object obj = new object();
            Assert.IsFalse(command.CanExecute(obj));
            canExecute = true;
            Assert.IsTrue(command.CanExecute(obj));

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());

            Assert.IsFalse(executed);
        }
    }
}
