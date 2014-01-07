using BigEgg.Framework.Applications.Collections;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Models;
using Bugger.Domain.Models;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    [Export(typeof(IDataService)), Export]
    internal class DataService : DataModel, IDataService
    {
        #region Fields
        private readonly MultiThreadingObservableCollection<Bug> userBugs;
        private readonly MultiThreadingObservableCollection<Bug> teamBugs;
        private DateTime refreshTime;
        private QueryStatus userBugsQueryState;
        private QueryStatus teamBugsQueryState;
        private int userBugsProgressValue;
        private int teamBugsProgressValue;
        private InitializeStatus initializeStatus;
        #endregion

        [ImportingConstructor]
        public DataService()
        {
            this.userBugs = new MultiThreadingObservableCollection<Bug>();
            this.teamBugs = new MultiThreadingObservableCollection<Bug>();
            this.refreshTime = DateTime.Now;
            this.userBugsQueryState = QueryStatus.QureyPause;
            this.userBugsProgressValue = 0;
            this.teamBugsQueryState = QueryStatus.QureyPause;
            this.teamBugsProgressValue = 0;
            this.initializeStatus = InitializeStatus.Initializing;
        }

        #region Properties
        public MultiThreadingObservableCollection<Bug> UserBugs { get { return this.userBugs; } }

        public MultiThreadingObservableCollection<Bug> TeamBugs { get { return this.teamBugs; } }

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

        public QueryStatus UserBugsQueryState
        {
            get { return this.userBugsQueryState; }
            set
            {
                if (this.userBugsQueryState != value)
                {
                    this.userBugsQueryState = value;
                    RaisePropertyChanged("UserBugsQueryState");
                }
            }
        }

        public int UserBugsProgressValue
        {
            get { return this.userBugsProgressValue; }
            set
            {
                if (this.userBugsProgressValue != value)
                {
                    this.userBugsProgressValue = value;
                    RaisePropertyChanged("UserBugsProgressValue");
                }
            }
        }

        public QueryStatus TeamBugsQueryState
        {
            get { return this.teamBugsQueryState; }
            set
            {
                if (this.teamBugsQueryState != value)
                {
                    this.teamBugsQueryState = value;
                    RaisePropertyChanged("TeamBugsQueryState");
                }
            }
        }

        public int TeamBugsProgressValue
        {
            get { return this.teamBugsProgressValue; }
            set
            {
                if (this.teamBugsProgressValue != value)
                {
                    this.teamBugsProgressValue = value;
                    RaisePropertyChanged("TeamBugsProgressValue");
                }
            }
        }

        public InitializeStatus InitializeStatus
        {
            get { return this.initializeStatus; }
            set
            {
                if (this.initializeStatus != value)
                {
                    this.initializeStatus = value;
                    RaisePropertyChanged("InitializeStatus");
                }
            }
        }
        #endregion
    }
}
