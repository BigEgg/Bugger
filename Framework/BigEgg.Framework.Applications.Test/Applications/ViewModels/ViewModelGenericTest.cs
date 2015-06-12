using BigEgg.Framework.Applications.Applications.ViewModels;
using BigEgg.Framework.Applications.Applications.Views;
using BigEgg.Framework.Applications.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace BigEgg.Framework.Applications.Test.Applications.ViewModels
{
    [TestClass]
    public class ViewModelGenericTest
    {
        [TestMethod]
        public void PropertyChangedTest()
        {
            var school = new School() { Name = "MIT" };
            var view = new PeopleView();
            var viewModel = new SchoolViewModel(view, school);

            Assert.AreEqual(view, viewModel.View);
            Assert.AreEqual(viewModel, view.DataContext);

            Assert.IsFalse(viewModel.IsSchoolNameChanged);
            school.Name = "Yale";
            Assert.IsTrue(viewModel.IsSchoolNameChanged);
        }


        public class SchoolViewModel : ViewModel<IPeopleView>
        {
            private readonly School school;

            public SchoolViewModel(IPeopleView view, School school) : base(view)
            {
                this.school = school;
                PropertyChangedEventManager.AddHandler(school, schoolPropertyChanged, "");
            }

            public bool IsSchoolNameChanged { get; set; }

            private void schoolPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Name")
                {
                    IsSchoolNameChanged = true;
                }
            }
        }

        public class PeopleView : IPeopleView
        {
            public object DataContext { get; set; }
        }

        public interface IPeopleView : IView
        { }

        public class School : Model
        {
            private string name;

            public string Name
            {
                get { return name; }
                set { SetProperty(ref name, value); }
            }
        }
    }
}
