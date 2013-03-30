using System;

namespace Bugger.Proxys.Models
{
    /// <summary>
    /// The model class use to shows in the UI.
    /// </summary>
    public class Bug : IEquatable<Bug>
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

        #region Implement IEquatable interface
        public bool Equals(Bug other)
        {
            return this.ID == other.ID
                && this.Title == other.Title
                && this.Description == other.Description
                && this.AssignedTo == other.AssignedTo
                && this.State == other.State
                && this.ChangedDate == other.ChangedDate
                && this.CreatedBy == other.CreatedBy
                && this.Priority == other.Priority
                && this.Severity == other.Severity;
        }
        #endregion

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
            internal set { this.id = value; }
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
            internal set { this.title = value; }
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
            internal set { this.description = value; }
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
            internal set { this.assignedTo = value; }
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
            internal set { this.state = value; }
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
            internal set { this.changedDate = value; }
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
            internal set { this.createdBy = value; }
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
            internal set { priority = value; }
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
            internal set { severity = value; }
        }
        #endregion
    }
}
