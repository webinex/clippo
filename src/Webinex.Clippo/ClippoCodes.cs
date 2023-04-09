using Webinex.Coded;

namespace Webinex.Clippo
{
    /// <summary>
    ///     Clippo failure codes
    /// </summary>
    public static class ClippoCodes
    {
        /// <summary>
        ///     Attempt to apply activate action on already activated clip
        /// </summary>
        public static readonly Code ALREADY_ACTIVE = Code.INVALID.Child("CLIPPO.ALREADY_ACTIVE");

        /// <summary>
        ///     Attempt to get content of clip without file reference
        /// </summary>
        public static readonly Code NO_REF = Code.INVALID.Child("CLIPPO.NO_REF");

        /// <summary>
        ///     Attempt to store empty file when .Interceptors.NoEmpty() called
        /// </summary>
        public static readonly Code NO_EMPTY = Code.INVALID.Child("CLIPPO.NO_EMPTY");

        /// <summary>
        ///     Attempt to store file with size greater than provided in .Interceptors.MaxSize(size)
        /// </summary>
        public static readonly Code MAX = Code.INVALID.Child("CLIPPO.MAX");
        
        /// <summary>
        ///     Clip not found
        /// </summary>
        public static readonly Code NOT_FOUND = Code.NOT_FOUND.Child("CLIPPO");
    }
}