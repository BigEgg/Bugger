namespace Bugger.Proxy.TFS.Models
{
    public enum ProgressType
    {
        NotWorking,
        OnConnectProgress,
        OnGetFiledsProgress,
        OnAutoFillMapSettings,
        Success,
        FailedOnConnect,
        FailedOnGetFileds,
        SuccessWithError
    }
}
