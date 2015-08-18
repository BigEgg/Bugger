using System;

namespace Bugger.PlugIns.TrackingSystems.Fake.Models
{
    public class FakeBug : IEquatable<FakeBug>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public FakeBugState State { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string CreatedBy { get; set; }
        public FakeBugPriority Priority { get; set; }
        public FakeBugSeverity Severity { get; set; }

        public FakeBug Clone()
        {
            return new FakeBug()
            {
                Id = Id,
                Title = Title,
                Description = Description,
                AssignedTo = AssignedTo,
                State = State,
                LastChangedDate = LastChangedDate,
                CreatedBy = CreatedBy,
                Priority = Priority,
                Severity = Severity
            };
        }

        public bool Equals(FakeBug other)
        {
            return this.Id == other.Id
                 && this.Title == other.Title
                 && this.Description == other.Description
                 && this.AssignedTo == other.AssignedTo
                 && this.State == other.State
                 && this.LastChangedDate == other.LastChangedDate
                 && this.CreatedBy == other.CreatedBy
                 && this.Priority == other.Priority
                 && this.Severity == other.Severity;
        }
    }

    public enum FakeBugState
    {
        Design,
        Implement,
        Resolve,
        Closed
    }

    public enum FakeBugPriority
    {
        High,
        Medium,
        Low
    }

    public enum FakeBugSeverity
    {
        High,
        Medium,
        Low
    }
}
