namespace Bugger.Applications.Models
{
    public enum SettingDialogStatus
    {
        NotWorking,
        InitiatingProxy,
        InitiatingProxyFailed,
        ValidatingProxySettings,
        ProxyUnvalid,
        ProxyBusy,
        ProxyCannotConnect,
        ProxyValid
    }
}
