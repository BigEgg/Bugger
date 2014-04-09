using BigEgg.Framework.Foundation;
using Bugger.Models;
using Bugger.Proxy.Models.Attributes;
using System;

namespace Bugger.Proxy.Models
{
    public class Bug : Model, IBug, IEquatable<IBug>
    {
        #region Fields
        private readonly string id;
        private readonly string title;
        private readonly string description;
        private readonly string assignedTo;
        private readonly string state;
        private readonly DateTime changedDate;
        private readonly string createdBy;
        private readonly string priority;
        private readonly string severity;

        private BugType type;
        private bool isUpdate;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Bug" /> class.
        /// </summary>
        /// <param name="id">The ID of this bug.</param>
        /// <param name="title">The string that describes the title of this bug.</param>
        /// <param name="description">The string that describes this bug.</param>
        /// <param name="assignedTo">The string value of the user who this bug currently be assigned to.</param>
        /// <param name="state">The string that describes the state of this bug.</param>
        /// <param name="changedDate">The System.DateTime object that represents the date and time that this bug was last changed.</param>
        /// <param name="createdBy">The string value of the user who created this bug.</param>
        /// <param name="priority">The string that describes the priority of this bug.</param>
        /// <param name="severity">The string that describes the severity of this bug.</param>
        /// <exception cref="System.ArgumentNullException">
        /// id cannot be null or empty.
        /// or
        /// title cannot be null or empty.
        /// or
        /// assignedTo cannot be null or empty.
        /// or
        /// state cannot be null or empty.
        /// </exception>
        public Bug(string id, string title, string description, string assignedTo,
                   string state, DateTime changedDate, string createdBy, string priority, string severity)
        {
            if (string.IsNullOrWhiteSpace(id)) { throw new ArgumentNullException("id cannot be null or empty."); }
            if (string.IsNullOrWhiteSpace(title)) { throw new ArgumentNullException("title cannot be null or empty."); }
            if (description == null) { throw new ArgumentNullException("description cannot be null"); }
            if (string.IsNullOrWhiteSpace(assignedTo)) { throw new ArgumentNullException("assignedTo cannot be null or empty."); }
            if (string.IsNullOrWhiteSpace(state)) { throw new ArgumentNullException("state cannot be null or empty."); }
            if (changedDate == null) { throw new ArgumentNullException("changeDate cannot be null."); }
            if (string.IsNullOrWhiteSpace(createdBy)) { throw new ArgumentNullException("createdBy cannot be null or empty."); }
            if (string.IsNullOrWhiteSpace(priority)) { throw new ArgumentNullException("priority cannot be null or empty."); }
            if (severity == null) { throw new ArgumentNullException("severity cannot be null"); }

            this.id = id;
            this.title = title;
            this.description = description;
            this.assignedTo = assignedTo;
            this.state = state;
            this.changedDate = changedDate;
            this.createdBy = createdBy;
            this.priority = priority;
            this.severity = severity;
        }

        #region Implement IEquatable Interface
        public bool Equals(IBug other)
        {
            if (other == null) { throw new ArgumentNullException("other cannot be null."); }

            return other.ID == this.id;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the ID of this bug.
        /// </summary>
        /// <value>
        /// The ID of this bug.
        /// </value>
        public string ID { get { return this.id; } }

        /// <summary>
        /// Gets the string that describes the title of this bug
        /// </summary>
        /// <value>
        /// The string that describes the title of this bug.
        /// </value>
        public string Title { get { return this.title; } }

        /// <summary>
        /// Gets the string that describes this bug.
        /// </summary>
        /// <value>
        /// The string that describes this bug.
        /// </value>
        public string Description { get { return this.description; } }

        /// <summary>
        /// Gets the string value of the user who this bug currently be assigned to.
        /// </summary>
        /// <value>
        /// The string value of the user who this bug currently be assigned to.
        /// </value>
        public string AssignedTo { get { return this.assignedTo; } }

        /// <summary>
        /// Gets the string that describes the state of this bug.
        /// </summary>
        /// <value>
        /// The string that describes the state of this bug.
        /// </value>
        public string State { get { return this.state; } }

        /// <summary>
        /// Gets the System.DateTime object that represents the date and time that this
        /// bug was last changed.
        /// </summary>
        /// <value>
        /// The System.DateTime object that represents the date and time that this bug 
        /// was last changed.
        /// </value>
        public DateTime ChangedDate { get { return this.changedDate; } }

        /// <summary>
        /// Gets the string value of the user who created this bug.
        /// </summary>
        /// <value>
        /// The string value of the user who created this bug.
        /// </value>
        public string CreatedBy { get { return this.createdBy; } }

        /// <summary>
        /// Gets the string that describes the priority of this bug.
        /// </summary>
        /// <value>
        /// The string that describes the priority of this bug.
        /// </value>
        public string Priority { get { return this.priority; } }

        /// <summary>
        /// Gets the string that describes the severity of this bug.
        /// </summary>
        /// <value>
        /// The string that describes the severity of this bug.
        /// </value>
        public string Severity { get { return this.severity; } }


        /// <summary>
        /// Gets or set the type of this bug.
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        [IgnoreMappingAttribute(Ignore = true)]
        public BugType Type
        {
            get { return this.type; }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the bug is update.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the bug is update; otherwise, <c>false</c>.
        /// </value>
        [IgnoreMappingAttribute(Ignore = true)]
        public bool IsUpdate
        {
            get { return this.isUpdate; }
            set
            {
                if (this.isUpdate != value)
                {
                    this.isUpdate = value;
                    RaisePropertyChanged("IsUpdate");
                }
            }
        }
        #endregion


    }
}
