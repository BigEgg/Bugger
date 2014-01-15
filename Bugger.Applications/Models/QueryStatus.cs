namespace Bugger.Applications.Models
{
    /// <summary>
    /// The enumeration type that define the status when query the bugs.
    /// </summary>
    public enum QueryStatus
    {
        NotWorking,
        Qureying,
        QureyPause,
        FillingData,
        Success,
        Failed
    }
}
