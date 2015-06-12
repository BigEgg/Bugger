using BigEgg.Framework.Applications.Applications.ViewModels;
using BigEgg.Framework.Applications.Applications.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BigEgg.Framework.Applications.Test.Applications.ViewModels
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest_ViewNull()
        {
            new MockViewModel(null);
        }

        [TestMethod]
        public void GetViewTest()
        {
            IView view = new MockView();
            MockViewModel viewModel = new MockViewModel(view);
            Assert.AreEqual(view, viewModel.View);
        }


        private class MockViewModel : ViewModel
        {
            public MockViewModel(IView view) : base(view)
            { }
        }

        private class MockView : IView
        {
            public object DataContext { get; set; }
        }
    }
}
