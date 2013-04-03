using BigEgg.Framework.Applications.Collections;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Domain.Models;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    [Export(typeof(IDataService)), Export]
    internal class DataService : DataModel, IDataService
    {
        #region Fields
        private MultiThreadingObservableCollection<Bug> redBugs;
        private MultiThreadingObservableCollection<Bug> yellowBugs;
        private DateTime refreshTime;
        #endregion

        [ImportingConstructor]
        public DataService()
        {
            this.redBugs = new MultiThreadingObservableCollection<Bug>();
            this.yellowBugs = new MultiThreadingObservableCollection<Bug>();
        }

        #region Properties
        public MultiThreadingObservableCollection<Bug> RedBugs 
        {
            get { return this.redBugs; }
        }

        public MultiThreadingObservableCollection<Bug> YellowBugs
        {
            get { return this.yellowBugs; }
        }

        public DateTime RefreshTime
        {
            get { return this.refreshTime; }
            set
            {
                if (this.refreshTime != value)
                {
                    this.refreshTime = value;
                    RaisePropertyChanged("RefreshTime");
                }
            }
        }
        #endregion
    } 
}
