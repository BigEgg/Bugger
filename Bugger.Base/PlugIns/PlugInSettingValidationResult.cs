namespace Bugger.PlugIns
{
    /// <summary>
    /// The validation result for Plug-in's setting
    /// </summary>
    public enum PlugInSettingValidationResult
    {
        /// <summary>
        /// The Plug-in's settings is valid
        /// </summary>
        Valid,
        /// <summary>
        /// Is busying on some thing, cannot get the validation result now
        /// </summary>
        Busy,
        /// <summary>
        /// The Plug-in's settings have some invalid data
        /// </summary>
        UnValid
    }
}
