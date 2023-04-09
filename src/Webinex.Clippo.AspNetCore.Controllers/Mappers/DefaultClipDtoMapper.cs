using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Webinex.Clippo.AspNetCore.Controllers.Mappers
{
    internal class DefaultClipDtoMapper<TClip, TDto> : IClipDtoMapper<TClip, TDto>
    {
        private readonly ILogger _logger;

        public DefaultClipDtoMapper(ILogger<DefaultClipDtoMapper<TClip, TDto>> logger)
        {
            _logger = logger;
        }

        public Task<TDto[]> MapAsync(IEnumerable<TClip> clipsEnumerable)
        {
            var clips = clipsEnumerable?.ToArray() ?? throw new ArgumentNullException(nameof(clipsEnumerable));
            return Task.FromResult(MapInternal(clips));
        }

        private TDto[] MapInternal(TClip[] clips)
        {
            if (IsSameType())
            {
                return MapSameType(clips);
            }

            if (TryGetMatchingConstructor(out var constructor))
            {
                return MapConstructor(clips, constructor);
            }

            throw new InvalidOperationException(
                $"Unable to convert {typeof(TClip).Name} to {typeof(TDto).Name}:" +
                $"you might use same type or {typeof(TDto).Name} might have public constructor with 1 parameter of type {typeof(TClip).Name}. " +
                $"Or you can create your own implementation of {nameof(IClipDtoMapper<TClip, TDto>)}.");
        }

        private bool IsSameType()
        {
            if (typeof(TClip) != typeof(TDto))
            {
                _logger.LogInformation(
                    $"Types of {typeof(TClip).Name} and {typeof(TDto).Name} different, continue mapping.");
                return false;
            }

            _logger.LogInformation(
                $"Types of {typeof(TClip).Name} and {typeof(TDto).Name} the same, return same value.");

            return true;
        }

        private TDto[] MapSameType(TClip[] clip)
        {
            return clip.Cast<TDto>().ToArray();
        }

        private TDto[] MapConstructor(TClip[] clips, ConstructorInfo constructorInfo)
        {
            return clips.Select(clip => (TDto)constructorInfo.Invoke(new Object[] { clip })).ToArray();
        }

        private bool TryGetMatchingConstructor(out ConstructorInfo constructorInfo)
        {
            constructorInfo = default;

            var constructors = typeof(TDto).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            var constructor = constructors.SingleOrDefault(IsMatchingConstructor);

            if (constructor == null)
            {
                _logger.LogInformation(
                    $"Unable to find public constructor with 1 parameter of type {typeof(TClip).Name} on type {typeof(TDto).Name}");
                return false;
            }

            return true;
        }

        private bool IsMatchingConstructor(ConstructorInfo constructorInfo)
        {
            var parameters = constructorInfo.GetParameters();
            return parameters.Length == 1 && parameters.Single().ParameterType == typeof(TClip);
        }
    }
}