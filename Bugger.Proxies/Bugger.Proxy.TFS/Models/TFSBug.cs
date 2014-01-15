using Bugger.Domain.Models;
using Bugger.Proxy.TFS.Models.Attributes;
using System;

namespace Bugger.Proxy.TFS.Models
{
    public class TFSBug : IBug, IEquatable<TFSBug>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TFSBug"/> class.
        /// </summary>
        public TFSBug()
        {
            this.Type = BugType.Yellow;
        }


        #region Implement IBug Interface
        /// <summary>
        /// Gets or sets the type of this bug.
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        [IgnoreMapping(Ignore = true)]
        public BugType Type { get; internal set; }

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
        public void CheckUpdate(IBug oldModel)
        {
            if (oldModel == null) { throw new ArgumentNullException("old Model cannot be null."); }
            if (!(oldModel is TFSBug)) { throw new NotSupportedException("oldModel must be BugViewModel type."); }

            var other = oldModel as TFSBug;
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
        #endregion

        #region Implement IEquatable interface
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(TFSBug other)
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
        public int ID { get; internal set; }

        /// <summary>
        /// Gets a string that describes the title of this bug
        /// </summary>
        /// <value>
        /// A string that describes the title of this bug.
        /// </value>
        public string Title { get; internal set; }

        /// <summary>
        /// Gets a string that describes this bug.
        /// </summary>
        /// <value>
        /// A string that describes this bug.
        /// </value>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the string value of the user who this bug currently be assigned to.
        /// </summary>
        /// <value>
        /// The string value of the user who this bug currently be assigned to.
        /// </value>
        public string AssignedTo { get; internal set; }

        /// <summary>
        /// Gets a string that describes the state of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the state of this bug.
        /// </value>
        public string State { get; internal set; }

        /// <summary>
        /// Gets the System.DateTime object that represents the date and time that this
        /// bug was last changed.
        /// </summary>
        /// <value>
        /// The System.DateTime object that represents the date and time that this bug 
        /// was last changed.
        /// </value>
        public DateTime ChangedDate { get; internal set; }

        /// <summary>
        /// Gets the string value of the user who created this bug.
        /// </summary>
        /// <value>
        /// The string value of the user who created this bug.
        /// </value>
        public string CreatedBy { get; internal set; }

        /// <summary>
        /// Gets a string that describes the priority of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the priority of this bug.
        /// </value>
        public string Priority { get; internal set; }

        /// <summary>
        /// Gets a string that describes the severity of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the severity of this bug.
        /// </value>
        public string Severity { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the bug is update.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the bug is update; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpdate { get; private set; }
        #endregion
    }
}
