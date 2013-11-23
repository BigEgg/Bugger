namespace Bugger.Proxy.TFS.Models
{
    public enum ProgressTypes
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
