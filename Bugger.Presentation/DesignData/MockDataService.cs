﻿using BigEgg.Framework.Applications.Collections;
using BigEgg.Framework.Applications.ViewModels;
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
        private MultiThreadingObservableCollection<Bug> userRedBugs;
        private MultiThreadingObservableCollection<Bug> userYellowBugs;
        private MultiThreadingObservableCollection<Bug> teamBugs;
        private DateTime refreshTime;
        #endregion

        public MockDataService()
        {
            this.userRedBugs = new MultiThreadingObservableCollection<Bug>();
            this.userYellowBugs = new MultiThreadingObservableCollection<Bug>();
            this.teamBugs = new MultiThreadingObservableCollection<Bug>();
            this.refreshTime = DateTime.Now;

            InitializeBugs();
        }

        #region Properties
        public MultiThreadingObservableCollection<Bug> UserRedBugs 
        {
            get { return this.userRedBugs; }
        }

        public MultiThreadingObservableCollection<Bug> UserYellowBugs
        {
            get { return this.userYellowBugs; }
        }

        public MultiThreadingObservableCollection<Bug> TeamBugs
        {
            get { return this.teamBugs; }
        }

        public DateTime RefreshTime { get; set; }
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

            foreach (var bug in bugs.Where(x=> x.AssignedTo == "BigEgg" || x.CreatedBy == "BigEgg"))
            {
                if (bug.Type == BugType.Red)
                    this.userRedBugs.Add(bug);
                else
                    this.userYellowBugs.Add(bug);
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
