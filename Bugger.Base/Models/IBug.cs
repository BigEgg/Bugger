using System;

namespace Bugger.Models
{
    public interface IBug
    {
        /// <summary>
        /// Gets the ID of this bug.
        /// </summary>
        /// <value>
        /// The ID of this bug.
        /// </value>
        string ID { get; }

        /// <summary>
        /// Gets the string that describes the title of this bug
        /// </summary>
        /// <value>
        /// The string that describes the title of this bug.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets the string that describes this bug.
        /// </summary>
        /// <value>
        /// The string that describes this bug.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the string value of the user who this bug currently be assigned to.
        /// </summary>
        /// <value>
        /// The string value of the user who this bug currently be assigned to.
        /// </value>
        string AssignedTo { get; }

        /// <summary>
        /// Gets the string that describes the state of this bug.
        /// </summary>
        /// <value>
        /// The string that describes the state of this bug.
        /// </value>
        string State { get; }

        /// <summary>
        /// Gets the System.DateTime object that represents the date and time that this
        /// bug was last changed.
        /// </summary>
        /// <value>
        /// The System.DateTime object that represents the date and time that this bug 
        /// was last changed.
        /// </value>
        DateTime ChangedDate { get; }

        /// <summary>
        /// Gets the string value of the user who created this bug.
        /// </summary>
        /// <value>
        /// The string value of the user who created this bug.
        /// </value>
        string CreatedBy { get; }

        /// <summary>
        /// Gets the string that describes the priority of this bug.
        /// </summary>
        /// <value>
        /// The string that describes the priority of this bug.
        /// </value>
        string Priority { get; }

        /// <summary>
        /// Gets the string that describes the severity of this bug.
        /// </summary>
        /// <value>
        /// The string that describes the severity of this bug.
        /// </value>
        string Severity { get; }


        /// <summary>
        /// Gets the type of this bug.
        /// </summary>
        /// <value>
        /// The type of this bug.
        /// </value>
        BugType Type { get; }

        /// <summary>
        /// Gets the value indicating whether the bug is update.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the bug is update; otherwise, <c>false</c>.
        /// </value>
        bool IsUpdate { get; }
    }
}
