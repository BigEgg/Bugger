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
        private MultiThreadingObservableCollection<Bug> userBugs;
        private MultiThreadingObservableCollection<Bug> teamBugs;
        private DateTime refreshTime;
        #endregion

        [ImportingConstructor]
        public DataService()
        {
            this.userBugs = new MultiThreadingObservableCollection<Bug>();
            this.teamBugs = new MultiThreadingObservableCollection<Bug>();
            this.refreshTime = DateTime.Now;
        }

        #region Properties
        public MultiThreadingObservableCollection<Bug> UserBugs
        {
            get { return this.userBugs; }
        }
        
        public MultiThreadingObservableCollection<Bug> TeamBugs
        {
            get { return this.teamBugs; }
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
