namespace Webinex.Clippo
{
    public enum GetClipOptions
    {
        /// <summary>
        ///     No options. Result would contain only active clips.
        /// </summary>
        None = 0,
        
        /// <summary>
        ///     Result should include inactive clips
        /// </summary>
        IncludeInactive = 1,
    }
}