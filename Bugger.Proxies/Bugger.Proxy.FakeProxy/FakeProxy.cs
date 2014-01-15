using Bugger.Domain.Models;
using Bugger.Proxy.FakeProxy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Bugger.Proxy.FakeProxy
{
    /// <summary>
    /// This class is for test only.
    /// </summary>
    [Export(typeof(ITracingSystemProxy)), Export]
    public class FakeProxy : TracingSystemProxyBase
    {
        #region Fields
        private readonly ObservableCollection<string> status;
        private List<FakeBug> bugs;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeProxy"/> class.
        /// </summary>
        [ImportingConstructor]
        public FakeProxy()
            : base("Fake")
        {
            this.status = new ObservableCollection<string>();

            this.bugs = new List<FakeBug>();
            this.CanQuery = true;
        }


        #region Properties
        /// <summary>
        /// Gets the status values.
        /// </summary>
        /// <value>
        /// The status values.
        /// </value>
        public override ObservableCollection<string> StateValues { get { return this.status; } }
        #endregion


        #region Methods
        #region Protected Methods
        /// <summary>
        /// The method which will execute when the Controller.Initialize() execute.
        /// </summary>
        protected override void OnInitialize()
        {
            this.bugs.Add(new FakeBug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 2,
                Title = "Bug2",
                Description = "Description for Bug2.",
                Type = BugType.Red,
                AssignedTo = "Pupil",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 3,
                Title = "Bug3",
                Description = "Description for Bug3.",
                Type = BugType.Red,
                AssignedTo = "User1",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 4,
                Title = "Bug4",
                Description = "Description for Bug4.",
                Type = BugType.Red,
                AssignedTo = "User2",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = "High"
            });

            this.bugs.Add(new FakeBug()
            {
                ID = 5,
                Title = "Bug5",
                Description = "Description for Bug5.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 11),
                CreatedBy = "Pupil",
                Priority = "High",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 6,
                Title = "Bug6",
                Description = "Description for Bug6.",
                AssignedTo = "Pupil",
                State = "Closed",
                ChangedDate = new DateTime(2013, 4, 11),
                CreatedBy = "Pupil",
                Priority = "High",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 7,
                Title = "Bug7",
                Description = "Description for Bug7.",
                AssignedTo = "User1",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 11),
                CreatedBy = "Pupil",
                Priority = "High",
                Severity = "Low"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 8,
                Title = "Bug8",
                Description = "Description for Bug8.",
                Type = BugType.Red,
                AssignedTo = "User2",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 11),
                CreatedBy = "Pupil",
                Priority = "High",
            });

            this.bugs.Add(new FakeBug()
            {
                ID = 9,
                Title = "Bug9",
                Description = "Description for Bug9.",
                AssignedTo = "BigEgg",
                State = "Design",
                ChangedDate = new DateTime(2013, 4, 12),
                CreatedBy = "User1",
                Priority = "Low",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 10,
                Title = "Bug10",
                Description = "Description for Bug10.",
                Type = BugType.Red,
                AssignedTo = "Pupil",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 12),
                CreatedBy = "User1",
                Priority = "Low",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 11,
                Title = "Bug11",
                Description = "Description for Bug11.",
                Type = BugType.Red,
                AssignedTo = "User1",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 12),
                CreatedBy = "User1",
                Priority = "Medium",
                Severity = "Low"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 12,
                Title = "Bug12",
                Description = "Description for Bug12.",
                Type = BugType.Red,
                AssignedTo = "User2",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 12),
                CreatedBy = "User1",
                Priority = "High",
                Severity = "High"
            });

            this.bugs.Add(new FakeBug()
            {
                ID = 13,
                Title = "Bug13",
                Description = "Description for Bug13.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 13),
                CreatedBy = "User2",
                Priority = "High",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 14,
                Title = "Bug14",
                Description = "Description for Bug14.",
                AssignedTo = "Pupil",
                State = "Closed",
                ChangedDate = new DateTime(2013, 4, 13),
                CreatedBy = "User2",
                Priority = "High",
                Severity = "Medium"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 15,
                Title = "Bug15",
                Description = "Description for Bug15.",
                AssignedTo = "User1",
                State = "Resolve",
                ChangedDate = new DateTime(2013, 4, 13),
                CreatedBy = "User2",
                Priority = "Medium",
                Severity = "High"
            });
            this.bugs.Add(new FakeBug()
            {
                ID = 16,
                Title = "Bug16",
                Description = "Description for Bug16.",
                AssignedTo = "User2",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 13),
                CreatedBy = "User2",
                Priority = "High",
                Severity = "High"
            });

            this.status.Add("Design");
            this.status.Add("Implement");
            this.status.Add("Resolve");
            this.status.Add("Closed");
        }

        /// <summary>
        /// Query the bugs with the specified user names list which the bug assign to.
        /// </summary>
        /// <param name="userNames">The user names list which the bug assign to.</param>
        /// <param name="isFilterCreatedBy">if set to <c>true</c> indicating whether filter the created by field.</param>
        /// <returns>
        /// The bugs.
        /// </returns>
        protected override ReadOnlyCollection<IBug> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            var queriedResult = new List<IBug>();

            foreach (string userName in userNames)
            {
                if (isFilterCreatedBy)
                {
                    queriedResult.AddRange(this.bugs.Where(x => x.AssignedTo.ToLower() == userName.ToLower() ||
                                                           x.CreatedBy.ToLower() == userName.ToLower()));
                }
                else
                {
                    queriedResult.AddRange(this.bugs.Where(x => x.AssignedTo.ToLower() == userName.ToLower()));
                }
            }

            return new ReadOnlyCollection<IBug>(queriedResult.Distinct().ToList());
        }
        #endregion
        #endregion
    }
}
