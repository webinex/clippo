using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Webinex.Clippo.AspNetCore.Controllers.Mappers
{
    /// <summary>
    ///     Allows override Clip to Clip DTO mapping
    /// </summary>
    /// <typeparam name="TClip">Clip type</typeparam>
    /// <typeparam name="TClipDto">Clip DTO type</typeparam>
    public interface IClipDtoMapper<TClip, TClipDto>
    {
        /// <summary>
        ///     Maps <paramref name="clips"/> to Clip DTO types
        /// </summary>
        /// <param name="clips">Clips</param>
        /// <returns>Mapped Clip DTOs</returns>
        Task<TClipDto[]> MapAsync([NotNull] IEnumerable<TClip> clips);
    }
    
    public static class ClipDtoMapperExtensions
    {
        /// <summary>
        ///     Map single clip to clip DTO
        /// </summary>
        /// <param name="mapper"><see cref="IClipDtoMapper{TClip,TClipDto}"/></param>
        /// <param name="clip">Clip to map</param>
        /// <typeparam name="TClip">Clip type</typeparam>
        /// <typeparam name="TClipDto">Clip DTO type</typeparam>
        /// <returns>Mapped Clip DTO</returns>
        public static async Task<TClipDto> MapAsync<TClip, TClipDto>(
            [NotNull] this IClipDtoMapper<TClip, TClipDto> mapper,
            [NotNull] TClip clip)
        {
            mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            clip = clip ?? throw new ArgumentNullException(nameof(clip));

            var result = await mapper.MapAsync(new[] { clip });
            return result.Single();
        }
    }
}