using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models;
using Bugger.Proxy.ViewModels;
using System;

namespace Bugger.Proxy.TFS.ViewModels
{
    public class TFSBug : BugBase, ITFSBug
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TFSBug"/> class.
        /// </summary>
        public TFSBug()
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
        /// Checks is the bug had been the updated.
        /// If true, set the IsUpdate property to <c>true</c>.
        /// </summary>
        /// <param name="oldBug"></param>
        /// <exception cref="System.ArgumentNullException">oldBug cannot be null.</exception>
        /// <exception cref="System.ArgumentException">
        /// oldBug must be TFSBug type.
        /// or
        /// Two models' ID are not same, cannot compare.
        /// </exception>
        public override void CheckIsUpdate(IBug oldBug)
        {
            if (oldBug == null) { throw new ArgumentNullException("oldBug cannot be null."); }
            if (!(oldBug is TFSBug)) { throw new ArgumentException("oldBug must be TFSBug type."); }

            var other = oldBug as TFSBug;
            if (other.ID != this.ID) { throw new ArgumentException("Two models' ID are not same, cannot compare."); }

            this.IsUpdate = this.Title != other.Title ||
                            this.Description != other.Description ||
                            this.AssignedTo != other.AssignedTo ||
                            this.State != other.State ||
                            this.ChangedDate != other.ChangedDate ||
                            this.CreatedBy != other.CreatedBy ||
                            this.Priority != other.Priority ||
                            this.Severity != other.Severity;

        }
    }
}
