using BigEgg.Framework.Applications.Applications.ViewModels;
using BigEgg.Framework.Applications.Presentation.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BigEgg.Framework.Applications.Applications.Views;
using System.Windows.Data;

namespace BigEgg.Framework.Applications.Test.Presentation.Validations
{
    [TestClass]
    public class ValidationHelperTest
    {
        [TestMethod]
        public void IsEnabledTest()
        {
            var view = new PersonView();
            Assert.IsFalse(ValidationHelper.GetIsEnabled(view));

            ValidationHelper.SetIsEnabled(view, true);
            Assert.IsTrue(ValidationHelper.GetIsEnabled(view));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsEnabledTest_CannotTurnOffAfterStart()
        {
            var view = new PersonView();
            ValidationHelper.SetIsEnabled(view, true);
            ValidationHelper.SetIsEnabled(view, false);
        }

        [TestMethod]
        public void IsValidTest()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            Assert.IsTrue(ValidationHelper.GetIsValid(view));

            ValidationHelper.SetIsEnabled(view, true);

            ValidationHelper.InternalSetIsValid(view, false);
            Assert.IsFalse(ValidationHelper.GetIsValid(view));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidTest_ReadOnly()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            ValidationHelper.SetIsValid(view, true);
        }
        
        [TestMethod]
        public void IsValidTest_Binding()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            ValidationHelper.SetIsEnabled(view, true);
            Binding binding = new Binding("IsValid");
            binding.Mode = BindingMode.OneWayToSource;

            BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidTest_Binding_NotEnabled()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            Binding binding = new Binding("IsValid");
            BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidTest_Binding_BindingMode()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            ValidationHelper.SetIsEnabled(view, true);

            Binding binding = new Binding("IsValid");
            BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidTest_MultiBinding_BindingMode()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            ValidationHelper.SetIsEnabled(view, true);

            MultiBinding binding = new MultiBinding();
            binding.Bindings.Add(new Binding("IsValid"));
            BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidTest_PriorityBinding()
        {
            var view = new PersonView();
            var viewModel = new PersonViewModel(view);

            ValidationHelper.SetIsEnabled(view, true);

            PriorityBinding binding = new PriorityBinding();
            binding.Bindings.Add(new Binding("IsValid"));
            BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding);
        }

        private class PersonView : UserControl, IPersonView
        { }

        private class PersonViewModel : ViewModel<IPersonView>
        {
            public PersonViewModel(IPersonView view)
                : base(view)
            {
            }

            public bool IsValid { get; set; }
        }

        private interface IPersonView : IView { }
    }
}
