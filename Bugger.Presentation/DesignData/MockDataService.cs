using BigEgg.Framework.Applications.Collections;
using BigEgg.Framework.Applications.ViewModels;
using Bugger.Applications.Models;
using Bugger.Applications.Services;
using Bugger.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bugger.Presentation.DesignData
{
    public class MockDataService : DataModel, IDataService
    {
        #region Fields
        private MultiThreadingObservableCollection<Bug> userBugs;
        private MultiThreadingObservableCollection<Bug> teamBugs;
        private DateTime refreshTime;

        private QueryStatus userBugsQueryState;
        private QueryStatus teamBugsQueryState;
        private int userBugsProgressValue;
        private int teamBugsProgressValue;
        private InitializeStatus initializeStatus;
        #endregion

        public MockDataService()
        {
            this.userBugs = new MultiThreadingObservableCollection<Bug>();
            this.teamBugs = new MultiThreadingObservableCollection<Bug>();
            this.refreshTime = DateTime.Now;

            this.userBugsQueryState = QueryStatus.Qureying;
            this.userBugsProgressValue = 50;
            this.teamBugsQueryState = QueryStatus.QureyPause;
            this.teamBugsProgressValue = 100;
            this.initializeStatus = InitializeStatus.Initializing;

            InitializeBugs();
        }

        #region Properties
        public MultiThreadingObservableCollection<Bug> UserBugs
        {
            get { return this.userBugs; }
        }

        public MultiThreadingObservableCollection<Bug> TeamBugs
        {
            get { return this.teamBugs; }
        }

        public DateTime RefreshTime { get; set; }

        public QueryStatus UserBugsQueryState
        {
            get { return this.userBugsQueryState; }
            set
            {
                if (this.userBugsQueryState != value)
                {
                    this.userBugsQueryState = value;
                    RaisePropertyChanged("UserBugsQueryState");
                }
            }
        }

        public int UserBugsProgressValue
        {
            get { return this.userBugsProgressValue; }
            set
            {
                if (this.userBugsProgressValue != value)
                {
                    this.userBugsProgressValue = value;
                    RaisePropertyChanged("UserBugsProgressValue");
                }
            }
        }

        public QueryStatus TeamBugsQueryState
        {
            get { return this.teamBugsQueryState; }
            set
            {
                if (this.teamBugsQueryState != value)
                {
                    this.teamBugsQueryState = value;
                    RaisePropertyChanged("TeamBugsQueryState");
                }
            }
        }

        public int TeamBugsProgressValue
        {
            get { return this.teamBugsProgressValue; }
            set
            {
                if (this.teamBugsProgressValue != value)
                {
                    this.teamBugsProgressValue = value;
                    RaisePropertyChanged("TeamBugsProgressValue");
                }
            }
        }

        public InitializeStatus InitializeStatus
        {
            get { return this.initializeStatus; }
            set
            {
                if (this.initializeStatus != value)
                {
                    this.initializeStatus = value;
                    RaisePropertyChanged("InitializeStatus");
                }
            }
        }
        #endregion

        #region Methods
        private void InitializeBugs()
        {
            List<Bug> bugs = new List<Bug>();

            bugs.Add(new Bug()
            {
                ID = 1,
                Title = "Bug1",
                Description = "Description for Bug1.",
                Type = BugType.Red,
                AssignedTo = "BigEgg",
                State = "Implement",
                ChangedDate = new DateTime(2013, 4, 10),
                CreatedBy = "BigEgg",
                Priority = "High",
                Severity = ""
            });
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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

            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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

            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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

            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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
            bugs.Add(new Bug()
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

            foreach (var bug in bugs.Where(x => x.AssignedTo == "BigEgg" || x.CreatedBy == "BigEgg"))
            {
                this.userBugs.Add(bug);
            }

            foreach (var bug in bugs.Where(x => x.AssignedTo == "BigEgg"
                                             || x.AssignedTo == "Pupil"
                                             || x.AssignedTo == "User1"
                                             || x.CreatedBy == "BigEgg"
                                             || x.CreatedBy == "Pupil"
                                             || x.CreatedBy == "User1"))
            {
                this.teamBugs.Add(bug);
            }
        }
        #endregion
    }
}
