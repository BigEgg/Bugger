﻿using Bugger.Domain.Models;
using Bugger.Domain.ViewModels;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.TFS.Views;
using Bugger.Proxy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugger.Proxy.TFS.ViewModels
{
    public class TFSBugViewModel : BugViewModelBase<ITFSBugView>, ITFSBugModel
    {
        #region Fields
        private int id;
        private string title;
        private string description;
        private string assignedTo;
        private string state;
        private DateTime changedDate;
        private string createdBy;
        private string priority;
        private string severity;
        #endregion

        public TFSBugViewModel(ITFSBugView view)
            : base(view)
        {
        }

        #region Properties
        /// <summary>
        /// Gets the ID of this bug.
        /// </summary>
        /// <value>
        /// The ID of this bug.
        /// </value>
        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets a string that describes the title of this bug
        /// </summary>
        /// <value>
        /// A string that describes the title of this bug.
        /// </value>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Gets a string that describes this bug.
        /// </summary>
        /// <value>
        /// A string that describes this bug.
        /// </value>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// Gets the string value of the user who this bug currently be assigned to.
        /// </summary>
        /// <value>
        /// The string value of the user who this bug currently be assigned to.
        /// </value>
        public string AssignedTo
        {
            get { return this.assignedTo; }
            set { this.assignedTo = value; }
        }

        /// <summary>
        /// Gets a string that describes the state of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the state of this bug.
        /// </value>
        public string State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        /// <summary>
        /// Gets the System.DateTime object that represents the date and time that this
        /// bug was last changed.
        /// </summary>
        /// <value>
        /// The System.DateTime object that represents the date and time that this bug 
        /// was last changed.
        /// </value>
        public DateTime ChangedDate
        {
            get { return this.changedDate; }
            set { this.changedDate = value; }
        }

        /// <summary>
        /// Gets the string value of the user who created this bug.
        /// </summary>
        /// <value>
        /// The string value of the user who created this bug.
        /// </value>
        public string CreatedBy
        {
            get { return this.createdBy; }
            set { this.createdBy = value; }
        }

        /// <summary>
        /// Gets a string that describes the priority of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the priority of this bug.
        /// </value>
        public string Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// Gets a string that describes the severity of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the severity of this bug.
        /// </value>
        public string Severity
        {
            get { return severity; }
            set { severity = value; }
        }
        #endregion


        /// <summary>
        /// Checks the update.
        /// </summary>
        /// <param name="oldModel">The old model.</param>
        /// <exception cref="System.ArgumentNullException">old Model cannot be null.</exception>
        /// <exception cref="System.ArgumentException">
        /// oldModel must be BugViewModel type.
        /// or
        /// Two models' ID are not same, cannot compare.
        /// </exception>
        public override void CheckUpdate(IBugViewModel oldModel)
        {
            if (oldModel == null) { throw new ArgumentNullException("old Model cannot be null."); }
            if (!(oldModel is TFSBugViewModel)) { throw new ArgumentException("oldModel must be BugViewModel type."); }
            
            var other = oldModel as TFSBugViewModel;
            if (other.ID != this.ID) {throw new ArgumentException("Two models' ID are not same, cannot compare.");}

            this.IsUpdate = this.Title != other.Title              ||
                            this.Description != other.Description  ||
                            this.AssignedTo != other.AssignedTo    ||
                            this.State != other.State              ||
                            this.ChangedDate != other.ChangedDate  ||
                            this.CreatedBy != other.CreatedBy      ||
                            this.Priority != other.Priority        ||
                            this.Severity != other.Severity;              

        }
    }
}
