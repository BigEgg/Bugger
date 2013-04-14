using Bugger.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bugger.Proxys.FakeProxy
{
    /// <summary>
    /// This class is for test only.
    /// </summary>
    [Export(typeof(ISourceControlProxy)), Export]
    public class FakeProxy : SourceControlProxy
    {
        #region Fields
        private List<Bug> bugs;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeProxy"/> class.
        /// </summary>
        [ImportingConstructor]
        public FakeProxy()
            : base("Fake")
        {
            this.bugs = new List<Bug>();
        }

        #region Methods
        #region Public Methods
        public override bool CanQuery() { return true; }
        #endregion


        #region Protected Methods
        protected override void OnInitialize()
        {
            this.bugs.Add(new Bug()
                {
                    ID = 1,
                    Title = "Bug1",
                    Description = "Description for Bug1.",
                    AssignedTo = "BigEgg",
                    State = "Implement",
                    ChangedDate = new DateTime(2013,4,10),
                    CreatedBy = "BigEgg",
                    Priority = "High",
                    Severity = ""
                });
            this.bugs.Add(new Bug()
            {
                ID = 2,
                Title = "Bug2",
                Description = "Description for Bug2.",
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
                State = "Implement",
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
                State = "Implement",
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
        }

        protected override ReadOnlyCollection<Bug> QueryCore(List<string> userNames, bool isFilterCreatedBy)
        {
            List<Bug> queriedBugs = new List<Bug>();

            foreach (string userName in userNames)
            {
                if (isFilterCreatedBy)
                    queriedBugs.AddRange(this.bugs.Where(x => x.AssignedTo == userName || x.CreatedBy == userName));
                else
                    queriedBugs.AddRange(this.bugs.Where(x => x.AssignedTo == userName));
            }

            return new ReadOnlyCollection<Bug>(queriedBugs);
        }
        #endregion
        #endregion
    }
}
