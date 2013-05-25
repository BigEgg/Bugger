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
        private MultiThreadingObservableCollection<Bug> userRedBugs;
        private MultiThreadingObservableCollection<Bug> userYellowBugs;
        private MultiThreadingObservableCollection<Bug> teamBugs;
        private DateTime refreshTime;
        #endregion

        [ImportingConstructor]
        public DataService()
        {
            this.userRedBugs = new MultiThreadingObservableCollection<Bug>();
            this.userYellowBugs = new MultiThreadingObservableCollection<Bug>();
            this.teamBugs = new MultiThreadingObservableCollection<Bug>();
            this.refreshTime = DateTime.Now;
        }

        #region Properties
        public MultiThreadingObservableCollection<Bug> UserRedBugs 
        {
            get { return this.userRedBugs; }
        }

        public MultiThreadingObservableCollection<Bug> UserYellowBugs
        {
            get { return this.userYellowBugs; }
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
