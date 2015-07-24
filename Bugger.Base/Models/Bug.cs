using BigEgg.Framework;
using System;

namespace Bugger.Base.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class Bug : IEquatable<Bug>
    {
        #region Fields
        private readonly string id;
        private readonly string title;
        private readonly string description;
        private readonly string assignedTo;
        private readonly string state;
        private readonly DateTime lastChangedDate;
        private readonly string createdBy;
        private readonly string priority;
        private readonly string severity;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Bug"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="assignedTo">The assigned to.</param>
        /// <param name="state">The state.</param>
        /// <param name="lastChangedDate">The last changed date.</param>
        /// <param name="createdBy">The created by.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="severity">The severity.</param>
        /// <exception cref="ArgumentException">id</exception>
        /// <exception cref="ArgumentException">title</exception>
        /// <exception cref="ArgumentException">description</exception>
        /// <exception cref="ArgumentException">assignedTo</exception>
        /// <exception cref="ArgumentException">state</exception>
        public Bug(string id, string title, string description, string assignedTo, string state,
                   DateTime lastChangedDate, string createdBy, string priority, string severity)
        {
            Preconditions.NotNullOrWhiteSpace(id);
            Preconditions.NotNullOrWhiteSpace(title);
            Preconditions.NotNullOrWhiteSpace(description);
            Preconditions.NotNullOrWhiteSpace(assignedTo);
            Preconditions.NotNullOrWhiteSpace(state);

            this.id = id;
            this.title = title;
            this.description = description;
            this.assignedTo = assignedTo;
            this.state = state;
            this.lastChangedDate = lastChangedDate;
            this.createdBy = createdBy;
            this.priority = priority;
            this.severity = severity;
        }

        #region Implement IEquatable interface
        /// <summary>
        /// Indicates whether the current bug object is equal to another bug object.
        /// </summary>
        /// <param name="other">A bug object to compare with this object.</param>
        /// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Bug other)
        {
            if (other == null) return false;

            return ID == other.ID
                && Title == other.Title
                && Description == other.Description
                && AssignedTo == other.AssignedTo
                && State == other.State
                && LastChangedDate == other.LastChangedDate
                && CreatedBy == other.CreatedBy
                && Priority == other.Priority
                && Severity == other.Severity;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the ID of this bug.
        /// </summary>
        /// <value>
        /// The ID of this bug.
        /// </value>
        public string ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets a string that describes the title of this bug
        /// </summary>
        /// <value>
        /// A string that describes the title of this bug.
        /// </value>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// Gets a string that describes this bug.
        /// </summary>
        /// <value>
        /// A string that describes this bug.
        /// </value>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the string value of the user who this bug currently be assigned to.
        /// </summary>
        /// <value>
        /// The string value of the user who this bug currently be assigned to.
        /// </value>
        public string AssignedTo
        {
            get { return assignedTo; }
        }

        /// <summary>
        /// Gets a string that describes the state of this bug.
        /// </summary>
        /// <value>
        /// A string that describes the state of this bug.
        /// </value>
        public string State
        {
            get { return state; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> object that represents the date and time when this
        /// bug was last changed.
        /// </summary>
        /// <value>
        /// The <see cref="DateTime"/> object that represents the date and time that when bug 
        /// was last changed.
        /// </value>
        public DateTime LastChangedDate
        {
            get { return lastChangedDate; }
        }

        /// <summary>
        /// Gets the string value of the user who created this bug.
        /// </summary>
        /// <value>
        /// The string value of the user who created this bug.
        /// </value>
        public string CreatedBy
        {
            get { return createdBy; }
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
        }
        #endregion
    }
}
