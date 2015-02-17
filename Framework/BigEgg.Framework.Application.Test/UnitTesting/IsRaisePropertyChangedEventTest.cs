using BigEgg.Framework.Application.Foundation;
using BigEgg.Framework.Application.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace BigEgg.Framework.Application.Test.UnitTesting
{
    [TestClass]
    public class IsRaisePropertyChangedEventTest
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void SuccessRisePropertyChange()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.Name, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Observable_Null()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent((Person)null, x => x.Name, () => person.Name = "Han");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_PropertySelector_Null()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, null, () => person.Name = "Han");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Precondition_Action_Null()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.Name, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void RiseWrongPropertyName()
        {
            WrongPerson wrongPerson = new WrongPerson();
            AssertHelper.IsRaisePropertyChangedEvent(wrongPerson, x => x.Name, () => wrongPerson.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(NoEventRaiseException))]
        public void NotRisePropertyName()
        {
            WrongPerson wrongPerson = new WrongPerson();
            AssertHelper.IsRaisePropertyChangedEvent(wrongPerson, x => x.Age, () => wrongPerson.Age = 31);
        }

        [TestMethod]
        [ExpectedException(typeof(EventRaiseMoreThanOnceException))]
        public void PropertyNameRaise2Times()
        {
            WrongPerson wrongPerson = new WrongPerson();
            AssertHelper.IsRaisePropertyChangedEvent(wrongPerson, x => x.Weight, () => wrongPerson.Weight = 80);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckWrongProperty()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.Name.Length, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(SenderObservableNotSameException))]
        public void WrongEventSenderTest()
        {
            WrongPerson person = new WrongPerson();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.Name, () => person.RaiseWrongNamePropertyChanged());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongExpressionTest1()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => x, () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongExpressionTest2()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => x.ToString(), () => person.Name = "Luke");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WrongExpressionTest3()
        {
            Person person = new Person();
            AssertHelper.IsRaisePropertyChangedEvent(person, x => Math.Abs(1), () => person.Name = "Luke");
        }


        private class Person : Model
        {
            private string name;

            public string Name
            {
                get { return name; }
                set { SetProperty(ref name, value); }
            }
        }

        private class WrongPerson : INotifyPropertyChanged
        {
            private string name;
            private double weight;

            public event PropertyChangedEventHandler PropertyChanged;

            public string Name
            {
                get { return name; }
                set
                {
                    if (name != value)
                    {
                        name = value;
                        OnPropertyChanged(new PropertyChangedEventArgs("WrongName"));
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
                        OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
                        OnPropertyChanged(new PropertyChangedEventArgs("Weight"));
                    }
                }
            }


            public void RaiseWrongNamePropertyChanged()
            {
                if (PropertyChanged != null) { PropertyChanged(null, new PropertyChangedEventArgs("Name")); }
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null) { PropertyChanged(this, e); }
            }
        }
    }
}
