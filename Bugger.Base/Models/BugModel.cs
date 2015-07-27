using BigEgg.Framework;
using BigEgg.Framework.Applications.Foundation;
using System;

namespace Bugger.Models
{
    /// <summary>
    /// The bug model class used to show in UI
    /// </summary>
    public class BugModel : Model, IComparable<BugModel>
    {
        private readonly Bug bug;
        private readonly BugPriorityLevel priorityLevel;
        private readonly Func<string, string, int> priorityCompareFunc;
        private BugModifyState modifiedState;


        /// <summary>
        /// Initializes a new instance of the <see cref="BugModel"/> class.
        /// </summary>
        /// <param name="bug">The bug object.</param>
        /// <param name="priorityLevel">Level of the bug's priority.</param>
        /// <exception cref="ArgumentNullException">bug</exception>
        public BugModel(Bug bug, BugPriorityLevel priorityLevel)
            : this(bug, priorityLevel, (a, b) => { return 0; })
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BugModel"/> class.
        /// </summary>
        /// <param name="bug">The bug object.</param>
        /// <param name="priorityLevel">Level of the bug's priority.</param>
        /// <param name="priorityCompareFunc">The priority compare function.</param>
        /// <exception cref="ArgumentNullException">bug</exception>
        /// <exception cref="ArgumentNullException">priorityCompareFunc</exception>
        public BugModel(Bug bug, BugPriorityLevel priorityLevel, Func<string, string, int> priorityCompareFunc)
        {
            Preconditions.NotNull(bug);
            Preconditions.NotNull(priorityCompareFunc);

            this.bug = bug;
            this.priorityLevel = priorityLevel;
            this.priorityCompareFunc = priorityCompareFunc;

            modifiedState = BugModifyState.Normal;
        }



        #region Implement IComparable interface
        /// <summary>
        ///  Compares the current bug model object with another bug model object.
        /// </summary>
        /// <param name="other">An bug model object to compare with this.</param>
        /// <returns>A value that indicates the relative order of the bug models being compared.</returns>
        public int CompareTo(BugModel other)
        {
            Preconditions.NotNull(other);

            if (PriorityLevel == BugPriorityLevel.Red && other.PriorityLevel == BugPriorityLevel.Yellow) return 1;
            if (PriorityLevel == BugPriorityLevel.Yellow && other.PriorityLevel == BugPriorityLevel.Red) return -1;
            if (Bug.LastChangedDate > other.Bug.LastChangedDate) return 1;
            if (Bug.LastChangedDate < other.Bug.LastChangedDate) return -1;

            if (priorityCompareFunc != null)
            {
                return priorityCompareFunc.Invoke(Bug.Priority, other.Bug.Priority);
            }
            else
            {
                return 0;
            }
        }
        #endregion


        /// <summary>
        /// Gets the bug object.
        /// </summary>
        /// <value>
        /// The bug object.
        /// </value>
        public Bug Bug { get { return bug; } }

        /// <summary>
        /// Gets the level of the bug's priority.
        /// </summary>
        /// <value>
        /// The level of the bug's priority.
        /// </value>
        public BugPriorityLevel PriorityLevel { get { return priorityLevel; } }

        /// <summary>
        /// Gets or sets a value indicating whether the bug is new created or modified.
        /// </summary>
        /// <value>
        /// The bug state.
        /// </value>
        public BugModifyState ModifiedState
        {
            get { return modifiedState; }
            set { SetProperty(ref modifiedState, value); }
        }
    }
}
