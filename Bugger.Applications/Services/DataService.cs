using BigEgg.Framework.Applications.Collections;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Models;
using Bugger.Domain.Models;
using System;
using System.ComponentModel.Composition;

namespace Bugger.Applications.Services
{
    /// <summary>
    /// The service class that contains all the data that related with the bugs and query progress.
    /// </summary>
    [Export(typeof(IDataService)), Export]
    internal class DataService : DataModel, IDataService
    {
        #region Fields
        private readonly MultiThreadingObservableCollection<IBug> userBugs;
        private readonly MultiThreadingObservableCollection<IBug> teamBugs;
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
            this.userBugs = new MultiThreadingObservableCollection<IBug>();
            this.teamBugs = new MultiThreadingObservableCollection<IBug>();
            this.refreshTime = DateTime.Now;
            this.userBugsQueryState = QueryStatus.QureyPause;
            this.userBugsProgressValue = 0;
            this.teamBugsQueryState = QueryStatus.QureyPause;
            this.teamBugsProgressValue = 0;
            this.initializeStatus = InitializeStatus.Initializing;
        }

        #region Properties
        /// <summary>
        /// Gets the user's bugs.
        /// </summary>
        /// <value>
        /// The user bugs.
        /// </value>
        public MultiThreadingObservableCollection<IBug> UserBugs { get { return this.userBugs; } }

        /// <summary>
        /// Gets the team's bugs.
        /// </summary>
        /// <value>
        /// The team bugs.
        /// </value>
        public MultiThreadingObservableCollection<IBug> TeamBugs { get { return this.teamBugs; } }

        /// <summary>
        /// Gets or sets the refresh time.
        /// </summary>
        /// <value>
        /// The refresh time.
        /// </value>
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

        /// <summary>
        /// Gets or sets the state of query user bugs.
        /// </summary>
        /// <value>
        /// The state of query user bugs.
        /// </value>
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

        /// <summary>
        /// Gets or sets the progress value of query user bugs.
        /// </summary>
        /// <value>
        /// The progress value of query user bugs.
        /// </value>
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

        /// <summary>
        /// Gets or sets the state of query team bugs.
        /// </summary>
        /// <value>
        /// The state of query team bugs.
        /// </value>
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

        /// <summary>
        /// Gets or sets the progress value of query team bugs.
        /// </summary>
        /// <value>
        /// The progress value of query team bugs.
        /// </value>
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

        /// <summary>
        /// Gets or sets the initialize status of the data and proxy.
        /// </summary>
        /// <value>
        /// The initialize status.
        /// </value>
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
