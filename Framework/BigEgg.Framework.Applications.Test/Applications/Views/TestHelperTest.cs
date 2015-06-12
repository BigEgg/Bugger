using BigEgg.Framework.Applications.Applications.ViewModels;
using BigEgg.Framework.Applications.Applications.Views;
using BigEgg.Framework.Applications.UnitTesting.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Windows.Threading;

namespace BigEgg.Framework.Applications.Test.Applications.Views
{
    [TestClass]
    public class ViewHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetViewModelTest_ViewNull()
        {
            ViewHelper.GetViewModel(null);
        }

        [TestMethod]
        public void GetViewModelTest()
        {
            IView view = new MockView();
            var viewModel = new MockViewModel(view);

            Assert.AreEqual(viewModel, view.GetViewModel<MockViewModel>());
        }

        [TestMethod]
        public void GetViewModelWithDispatcherTest()
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

            var view = new MockView();
            var viewModel = new MockViewModel(view);

            Assert.AreEqual(viewModel, view.GetViewModel<MockViewModel>());
        }


        private class MockViewModel : ViewModel
        {
            public MockViewModel(IView view)
                : base(view)
            { }
        }
    }
}