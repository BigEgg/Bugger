using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bugger.Base.Models;
using Bugger.Proxy;

namespace Bugger.Presentation.DesignData
{
    /// <summary>
    /// This class is for test only.
    /// </summary>
    public class FakeProxy : TracingSystemProxyBase
    {
        #region Fields
        private readonly ObservableCollection<string> status;
        private List<Bug> bugs;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeProxy"/> class.
        /// </summary>
        public FakeProxy()
            : base("Fake")
        {
            this.status = new ObservableCollection<string>();

            this.bugs = new List<Bug>();
            this.CanQuery = true;
        }

        #region Properties
        public override ObservableCollection<string> StateValues { get { return this.status; } }
        #endregion


        #region Methods
        #region Protected Methods
        protected override void OnInitialize()
        {
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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

            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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

            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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

            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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
            this.bugs.Add(new Bug()
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

        protected override ReadOnlyCollection<Bug> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            List<Bug> queriedResult = new List<Bug>();

            foreach (string userName in userNames)
            {
                if (isFilterCreatedBy)
                    queriedResult.AddRange(this.bugs
                        .Where(x => x.AssignedTo.ToLower() == userName.ToLower()
                            || x.CreatedBy.ToLower() == userName.ToLower()));
                else
                    queriedResult.AddRange(this.bugs.Where(x => x.AssignedTo.ToLower() == userName.ToLower()));
            }

            return new ReadOnlyCollection<Bug>(queriedResult.Distinct().ToList());
        }
        #endregion
        #endregion
    }
}
