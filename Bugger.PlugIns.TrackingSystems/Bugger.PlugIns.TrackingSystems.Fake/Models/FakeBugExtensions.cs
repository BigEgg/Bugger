using BigEgg.Framework;
using Bugger.Models;

namespace Bugger.PlugIns.TrackingSystems.Fake.Models
{
    public static class FakeBugExtensions
    {
        public static Bug ToBug(this FakeBug fakeBug)
        {
            Preconditions.NotNull(fakeBug);

            return new Bug(
                fakeBug.Id.ToString(),
                fakeBug.Title,
                fakeBug.Description,
                fakeBug.AssignedTo,
                fakeBug.State.ToString(),
                fakeBug.LastChangedDate,
                fakeBug.CreatedBy,
                fakeBug.Priority.ToString(),
                fakeBug.Severity.ToString());
        }
    }
}
