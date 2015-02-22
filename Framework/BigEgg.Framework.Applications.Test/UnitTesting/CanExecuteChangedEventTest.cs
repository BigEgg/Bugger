using BigEgg.Framework.Applications.Applications.Commands;
using BigEgg.Framework.Applications.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigEgg.Framework.Applications.Test.UnitTesting
{
    [TestClass]
    public class CanExecuteChangedEventTest
    {
        [TestMethod]
        public void CommandCanExecuteChangedTest()
        {
            DelegateCommand command = new DelegateCommand(() => { });

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Command_Null()
        {
            DelegateCommand command = new DelegateCommand(() => { });
            AssertHelper.CanExecuteChangedEvent(null, () => command.RaiseCanExecuteChanged());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Action_Null()
        {
            DelegateCommand command = new DelegateCommand(() => { });
            AssertHelper.CanExecuteChangedEvent(command, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void NotRaiseExecuteChanged()
        {
            DelegateCommand command = new DelegateCommand(() => { });
            AssertHelper.CanExecuteChangedEvent(command, () => { });
        }

        [TestMethod]
        [ExpectedException(typeof(EventRaiseMoreThanOnceException))]
        public void Raise2TimesExecuteChanged()
        {
            DelegateCommand command = new DelegateCommand(() => { });
            AssertHelper.CanExecuteChangedEvent(command, () =>
            {
                command.RaiseCanExecuteChanged();
                command.RaiseCanExecuteChanged();
            });
        }


        [TestMethod]
        [ExpectedException(typeof(SenderCommandNotSameException))]
        public void WrongEventSenderTest()
        {
            WrongCommand command = new WrongCommand();

            AssertHelper.CanExecuteChangedEvent(command, () => command.RaiseCanExecuteChanged());
        }


        private class WrongCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;


            public bool CanExecute(object parameter)
            {
                throw new NotImplementedException();
            }

            public void Execute(object parameter)
            {
                throw new NotImplementedException();
            }

            public void RaiseCanExecuteChanged()
            {
                if (CanExecuteChanged != null) { CanExecuteChanged(null, EventArgs.Empty); }
            }
        }
    }
}
