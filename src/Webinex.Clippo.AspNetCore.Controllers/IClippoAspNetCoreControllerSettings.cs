using System.Diagnostics.CodeAnalysis;

namespace Webinex.Clippo.AspNetCore.Controllers
{
    /// <summary>
    ///     Clippo MVC settings
    /// </summary>
    public interface IClippoAspNetCoreControllerSettings
    {
        /// <summary>
        ///     Authentication schema. If null - no authorization performed.
        /// </summary>
        [MaybeNull]
        public string Schema { get; }

        /// <summary>
        ///     Authorization policy. If null - no authorization performed.
        /// </summary>
        [MaybeNull]
        public string Policy { get; }
    }
}