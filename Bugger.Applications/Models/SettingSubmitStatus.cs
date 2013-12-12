namespace Bugger.Applications.Models
{
    public enum SettingSubmitStatus
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
