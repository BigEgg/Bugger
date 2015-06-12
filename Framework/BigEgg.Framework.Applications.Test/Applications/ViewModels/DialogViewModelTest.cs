using BigEgg.Framework.Applications.Applications.ViewModels;
using BigEgg.Framework.Applications.Applications.Views;
using BigEgg.Framework.Applications.UnitTesting.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigEgg.Framework.Applications.Test.Applications.ViewModels
{
    [TestClass]
    public class DialogViewModelTest
    {
        [TestMethod]
        public void GeneralTest()
        {
            var view = new AboutDialogView();
            var viewModel = new AboutDialogViewModel(view);

            var owner = new object();
            view.ShowDialogAction = v =>
            {
                Assert.AreEqual(owner, v.Owner);
                Assert.IsTrue(v.IsVisible);
            };

            Assert.IsFalse(view.IsVisible);
            bool? dialogResult = viewModel.ShowDialog(owner);
            Assert.IsFalse(dialogResult.HasValue);
            Assert.IsFalse(view.IsVisible);
        }


        private class AboutDialogViewModel : DialogViewModel<IAboutDialogView>
        {
            public AboutDialogViewModel(IAboutDialogView view) : base(view)
            { }
        }

        private class AboutDialogView : MockDialogView, IAboutDialogView
        { }

        private interface IAboutDialogView : IDialogView
        { }
    }
}
