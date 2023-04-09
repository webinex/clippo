using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Webinex.Clippo.Ports.Model
{
    /// <summary>
    ///     Allows you to configure <see cref="IClippoModelDefinition{TClip}"/> with more secure and fluent way.
    /// </summary>
    /// <typeparam name="TClip"></typeparam>
    public abstract class ClippoModelDefinitionConfiguration<TClip>
    {
        public ClippoModelDefinitionConfiguration()
        {
            var builder = new ClipModelBuilder<TClip>();
            // ReSharper disable once VirtualMemberCallInConstructor
            Configure(builder);

            Definition = builder.Build();
        }

        internal IClippoModelDefinition<TClip> Definition { get; }

        protected abstract void Configure([NotNull] ClipModelBuilder<TClip> model);
    }
    
    /// <summary>
    ///     Clippo model definition builder
    /// </summary>
    /// <typeparam name="TClip"></typeparam>
    public class ClipModelBuilder<TClip>
    {
        private readonly ModelDefinition _definition = new ModelDefinition();

        public IDictionary<string, object> Values { get; } = new Dictionary<string, object>();

        public ClipModelBuilder<TClip> HasId(Expression<Func<TClip, object>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            _definition.Id = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasOwnerType(Expression<Func<TClip, string>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.OwnerType = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasOwnerId(Expression<Func<TClip, string>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.OwnerId = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasDirectory(Expression<Func<TClip, string>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.Directory = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasActive(Expression<Func<TClip, bool>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.Active = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasSizeBytes(Expression<Func<TClip, int>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.SizeBytes = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasFileName(Expression<Func<TClip, string>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.FileName = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasMimeType(Expression<Func<TClip, string>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.MimeType = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasReference(Expression<Func<TClip, string>> navigator)
        {
            navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            AssertPublicSetter(navigator);

            _definition.Reference = navigator;
            return this;
        }

        public ClipModelBuilder<TClip> HasNew(Func<IDictionary<string, object>, TClip> factory)
        {
            factory = factory ?? throw new ArgumentNullException(nameof(factory));

            _definition.NewFunc = factory;
            return this;
        }

        public ClipModelBuilder<TClip> HasSetValues(Action<TClip, IDictionary<string, object>> setter)
        {
            setter = setter ?? throw new ArgumentNullException(nameof(setter));

            _definition.SetValuesFunc = setter;
            return this;
        }

        /// <summary>
        ///     Uses parameterless constructor and then calls SetValues or constructor with parameter IDictionary{string, object}.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when no matching constructor found</exception>
        public ClipModelBuilder<TClip> UseDefaultConstructor()
        {
            var type = typeof(TClip);
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if (!TryDictionaryConstructor(constructors) && !TryEmptyConstructor(constructors))
            {
                throw new InvalidOperationException(
                    $"Might have public constructor with only {nameof(IDictionary<string, object>)} parameter or without parameters.");
            }

            return this;
        }

        private void AssertPublicSetter<TResult>(Expression<Func<TClip, TResult>> navigator)
        {
            var property = ExpressionUtil.Property(navigator);

            if (property.SetMethod == null || !property.SetMethod.IsPublic)
                throw new ArgumentException("Might have public setter", nameof(navigator));
        }

        private bool TryEmptyConstructor(ConstructorInfo[] constructors)
        {
            var constructor = constructors.SingleOrDefault(x => x.GetParameters().Length == 0);
            if (constructor == null)
                return false;

            _definition.NewFunc = (values) =>
            {
                var instance = (TClip)constructor.Invoke(Array.Empty<object>());
                _definition.SetValues(instance, values);
                return instance;
            };

            return true;
        }

        private bool TryDictionaryConstructor(ConstructorInfo[] constructors)
        {
            var constructor = constructors.SingleOrDefault(IsDictionaryConstructor);

            if (constructor == null)
                return false;

            _definition.NewFunc = (values) => (TClip)constructor.Invoke(new object[] { values });
            return true;
        }

        private bool IsDictionaryConstructor(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            return parameters.Length == 1 &&
                   parameters.Single().ParameterType == typeof(IDictionary<string, object>);
        }

        internal IClippoModelDefinition<TClip> Build()
        {
            if (_definition.Id == null)
                throw new InvalidOperationException($"{nameof(HasId)} might be called");

            if (_definition.OwnerType == null)
                throw new InvalidOperationException($"{nameof(HasOwnerType)} might be called");

            if (_definition.OwnerId == null)
                throw new InvalidOperationException($"{nameof(HasOwnerId)} might be called");
            
            if (_definition.Directory == null)
                throw new InvalidOperationException($"{nameof(HasDirectory)} might be called");
            
            if (_definition.Active == null)
                throw new InvalidOperationException($"{nameof(HasActive)} might be called");
            
            if (_definition.SizeBytes == null)
                throw new InvalidOperationException($"{nameof(HasSizeBytes)} might be called");
            
            if (_definition.FileName == null)
                throw new InvalidOperationException($"{nameof(HasFileName)} might be called");
            
            if (_definition.MimeType == null)
                throw new InvalidOperationException($"{nameof(HasMimeType)} might be called");
            
            if (_definition.Reference == null)
                throw new InvalidOperationException($"{nameof(HasReference)} might be called");
            
            if (_definition.NewFunc == null)
                throw new InvalidOperationException($"{nameof(HasNew)} or {nameof(UseDefaultConstructor)} might be called");

            return _definition;
        }

        private class ModelDefinition : IClippoModelDefinition<TClip>
        {
            public Expression<Func<TClip, object>> Id { get; set; }
            public Expression<Func<TClip, string>> OwnerType { get; set; }
            public Expression<Func<TClip, string>> OwnerId { get; set; }
            public Expression<Func<TClip, string>> Directory { get; set; }
            public Expression<Func<TClip, bool>> Active { get; set; }
            public Expression<Func<TClip, int>> SizeBytes { get; set; }
            public Expression<Func<TClip, string>> FileName { get; set; }
            public Expression<Func<TClip, string>> MimeType { get; set; }
            public Expression<Func<TClip, string>> Reference { get; set; }
            public Action<TClip, IDictionary<string, object>> SetValuesFunc { get; set; } = (_, __) => { };
            public Func<IDictionary<string, object>, TClip> NewFunc { get; set; }

            public TClip New(IDictionary<string, object> values)
            {
                return NewFunc(values);
            }

            public void SetValues(TClip clip, IDictionary<string, object> values)
            {
                SetValuesFunc(clip, values);
            }
        }
    }
}