using BigEgg.Framework.Applications.Collections;
using Bugger.Applications.Models;
using Bugger.Domain.Models;
using System;
using System.ComponentModel;

namespace Bugger.Applications.Services
{
    /// <summary>
    /// The interface that define all the data that related with the bugs and query progress 
    /// which the service should contains.
    /// </summary>
    public interface IDataService : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the user's bugs.
        /// </summary>
        /// <value>
        /// The user bugs.
        /// </value>
        MultiThreadingObservableCollection<IBug> UserBugs { get; }

        /// <summary>
        /// Gets the team's bugs.
        /// </summary>
        /// <value>
        /// The team bugs.
        /// </value>
        MultiThreadingObservableCollection<IBug> TeamBugs { get; }

        /// <summary>
        /// Gets or sets the refresh time.
        /// </summary>
        /// <value>
        /// The refresh time.
        /// </value>
        DateTime RefreshTime { get; set; }

        /// <summary>
        /// Gets or sets the state of query user bugs.
        /// </summary>
        /// <value>
        /// The state of query user bugs.
        /// </value>
        QueryStatus UserBugsQueryState { get; set; }

        /// <summary>
        /// Gets or sets the state of query team bugs.
        /// </summary>
        /// <value>
        /// The state of query team bugs.
        /// </value>
        QueryStatus TeamBugsQueryState { get; set; }

        /// <summary>
        /// Gets or sets the progress value of query user bugs.
        /// </summary>
        /// <value>
        /// The progress value of query user bugs.
        /// </value>
        int UserBugsProgressValue { get; set; }

        /// <summary>
        /// Gets or sets the progress value of query team bugs.
        /// </summary>
        /// <value>
        /// The progress value of query team bugs.
        /// </value>
        int TeamBugsProgressValue { get; set; }

        /// <summary>
        /// Gets or sets the initialize status of the data and proxy.
        /// </summary>
        /// <value>
        /// The initialize status.
        /// </value>
        InitializeStatus InitializeStatus { get; set; }
    }
}
