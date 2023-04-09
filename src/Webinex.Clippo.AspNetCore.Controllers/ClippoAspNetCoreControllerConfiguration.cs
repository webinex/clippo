using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Webinex.Clippo.AspNetCore.Controllers.Json;
using Webinex.Clippo.AspNetCore.Controllers.Mappers;

namespace Webinex.Clippo.AspNetCore.Controllers
{
    internal class ClippoAspNetCoreControllerConfiguration<TClip, TDto> : IClippoAspNetCoreControllerConfiguration,
        IClippoAspNetCoreControllerSettings
    {
        private ClippoAspNetCoreControllerConfiguration(IMvcBuilder mvcBuilder)
        {
            MvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));

            MvcBuilder.Services.AddSingleton<IClippoAspNetCoreControllerSettings>(this);
            MvcBuilder.AddController(typeof(ClippoControllerBase<TClip, TDto>));
            MvcBuilder.Services.TryAddSingleton<IClipDtoMapper<TClip, TDto>, DefaultClipDtoMapper<TClip, TDto>>();
        }

        public static ClippoAspNetCoreControllerConfiguration<TClip, TDto> GetOrCreate(IMvcBuilder mvcBuilder)
        {
            mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));

            var instance = (ClippoAspNetCoreControllerConfiguration<TClip, TDto>)mvcBuilder.Services.FirstOrDefault(x =>
                    x.ImplementationInstance?.GetType() == typeof(ClippoAspNetCoreControllerConfiguration<TClip, TDto>))
                ?.ImplementationInstance;

            if (instance != null)
                return instance;

            instance = new ClippoAspNetCoreControllerConfiguration<TClip, TDto>(mvcBuilder);
            mvcBuilder.Services.AddSingleton(instance);
            return instance;
        }

        public IClippoAspNetCoreControllerConfiguration UsePolicy(string schema, string policy)
        {
            Schema = schema ?? throw new ArgumentNullException(nameof(schema));
            Policy = policy ?? throw new ArgumentNullException(nameof(policy));
            return this;
        }

        public Type ClipType { get; } = typeof(TClip);

        public Type DtoType { get; } = typeof(TDto);

        public IMvcBuilder MvcBuilder { get; }

        public string Schema { get; private set; }

        public string Policy { get; private set; }
    }
}