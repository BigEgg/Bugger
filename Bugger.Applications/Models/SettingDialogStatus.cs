namespace Bugger.Applications.Models
{
    /// <summary>
    /// The enumeration type that define the status of the setting dialog.
    /// </summary>
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
