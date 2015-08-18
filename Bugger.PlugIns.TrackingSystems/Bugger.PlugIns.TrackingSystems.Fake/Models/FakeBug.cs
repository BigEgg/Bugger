using System;

namespace Bugger.PlugIns.TrackingSystems.Fake.Models
{
    public class FakeBug
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
