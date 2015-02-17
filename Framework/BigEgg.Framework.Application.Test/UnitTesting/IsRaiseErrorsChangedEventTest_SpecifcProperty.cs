using BigEgg.Framework.Application.Foundation;
using BigEgg.Framework.Application.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BigEgg.Framework.Application.Test.UnitTesting
{
    [TestClass]
    public class IsRaiseErrorsChangedEventTest_SpecificProperty
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void SuccessRiseErrorsChangedEvent()
        {
            Person person = new Person();
            person.Validate();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.Name, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Observable_Null()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent((Person)null, x => x.Name, () => person.Name = "Han");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_PropertySelector_Null()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent(person, null, () => person.Name = "Han");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Action_Null()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.Name, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void RiseWrongPropertyName()
        {
            WrongPerson wrongPerson = new WrongPerson();
            AssertHelper.IsRaiseErrorChangedEvent(wrongPerson, x => x.Name, () => wrongPerson.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void NotRisePropertyName()
        {
            WrongPerson wrongPerson = new WrongPerson();
            AssertHelper.IsRaiseErrorChangedEvent(wrongPerson, x => x.Age, () => wrongPerson.Age = 31);
        }

        [TestMethod]
        [ExpectedException(typeof(EventRaiseMoreThanOnceException))]
        public void PropertyNameRaise2Times()
        {
            WrongPerson wrongPerson = new WrongPerson();
            AssertHelper.IsRaiseErrorChangedEvent(wrongPerson, x => x.Weight, () => wrongPerson.Weight = 80);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckWrongProperty()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.Name.Length, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(SenderObservableNotSameException))]
        public void WrongEventSenderTest()
        {
            WrongPerson person = new WrongPerson();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.Name, () => person.RaiseWrongNameErrorsChanged());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongExpressionTest1()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongExpressionTest2()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => x.ToString(), () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongExpressionTest3()
        {
            Person person = new Person();
            AssertHelper.IsRaiseErrorChangedEvent(person, x => Math.Abs(1), () => person.Name = "Luke");
        }


        private class Person : ValidatableModel
        {
            private string name;

            [Required]
            public string Name
            {
                get { return name; }
                set { SetPropertyAndValidate(ref name, value); }
            }
        }

        private class WrongPerson : INotifyDataErrorInfo
        {
            private string name;
            private double weight;

            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            public string Name
            {
                get { return name; }
                set
                {
                    if (name != value)
                    {
                        name = value;
                        OnErrorsChanged(new DataErrorsChangedEventArgs("WrongName"));
                    }
                }
            }

            public int Age { get; set; }

            public double Weight
            {
                get { return weight; }
                set
                {
                    if (weight != value)
                    {
                        weight = value;
                        OnErrorsChanged(new DataErrorsChangedEventArgs("Weight"));
                        OnErrorsChanged(new DataErrorsChangedEventArgs("Weight"));
                    }
                }
            }

            public bool HasErrors { get; set; }

            IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
            {
                throw new NotImplementedException();
            }

            protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
            {
                EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            public void RaiseWrongNameErrorsChanged()
            {
                if (ErrorsChanged != null) { ErrorsChanged(null, new DataErrorsChangedEventArgs("Name")); }
            }
        }
    }
}
