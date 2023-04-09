using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Webinex.Clippo.AspNetCore.Controllers.Json
{
    internal class ClippoAspNetCoreJsonConfiguration
        : IClippoAspNetCoreJsonConfiguration,
            IClippoAspNetCoreJsonSettings
    {
        private ClippoAspNetCoreJsonConfiguration(IMvcBuilder mvcBuilder)
        {
            mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));

            mvcBuilder.AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new ClippoActionJsonConverter(this));
            });
        }

        public static ClippoAspNetCoreJsonConfiguration GetOrCreate(IMvcBuilder mvcBuilder)
        {
            mvcBuilder = mvcBuilder ?? throw new ArgumentNullException(nameof(mvcBuilder));

            var instance = (ClippoAspNetCoreJsonConfiguration)mvcBuilder.Services.FirstOrDefault(x =>
                    x.ImplementationInstance?.GetType() == typeof(ClippoAspNetCoreJsonConfiguration))
                ?.ImplementationInstance;

            if (instance != null)
                return instance;

            instance = new ClippoAspNetCoreJsonConfiguration(mvcBuilder);
            mvcBuilder.Services.AddSingleton(instance);
            return instance;
        }

        public IClippoAspNetCoreJsonConfiguration AddAction(string kind, Type type)
        {
            kind = kind ?? throw new ArgumentNullException(nameof(kind));
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (!typeof(IClippoAction).IsAssignableFrom(type))
                throw new ArgumentException($"Might be assignable from {nameof(IClippoAction)}", nameof(type));

            Actions.Add(kind, type);
            return this;
        }

        public IDictionary<string, Type> Actions { get; } = new Dictionary<string, Type>
        {
            ["webinex://Activate"] = typeof(ActivateClippoAction),
            ["webinex://ActivateNew"] = typeof(ActivateNewClippoAction),
            ["webinex://Delete"] = typeof(DeleteClipAction),
            ["webinex://SetValues"] = typeof(SetValuesClippoAction)
        };
    }
}