using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Webinex.Clippo.Ports.Model;

namespace Webinex.Clippo.Adapters.Model
{
    internal class DefinitionClippoModel<TClip> : IClippoModel<TClip>
    {
        private readonly IClippoModelDefinition<TClip> _model;

        public DefinitionClippoModel(IClippoModelDefinition<TClip> model)
        {
            _model = model;
        }

        public TClip New(IDictionary<string, object> values)
        {
            values = values ?? throw new ArgumentNullException(nameof(values));
            return _model.New(values) ?? throw new InvalidOperationException("Might not be null.");
        }

        public void SetValues(TClip clip, IDictionary<string, object> values)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            values = values ?? throw new ArgumentNullException(nameof(values));
            
            _model.SetValues(clip, values);
        }

        public string GetId(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.Id.Compile().Invoke(clip).ToString();
        }

        public string GetOwnerType(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.OwnerType.Compile().Invoke(clip);
        }

        public void SetOwnerType(TClip clip, string ownerType)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.OwnerType);
            propertyInfo.SetValue(clip, ownerType);
        }

        public string GetOwnerId(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.OwnerId.Compile().Invoke(clip);
        }

        public void SetOwnerId(TClip clip, string ownerId)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.OwnerId);
            propertyInfo.SetValue(clip, ownerId);
        }

        public string GetDirectory(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.Directory.Compile().Invoke(clip);
        }

        public void SetDirectory(TClip clip, string directory)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.Directory);
            propertyInfo.SetValue(clip, directory);
        }

        public bool GetActive(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.Active.Compile().Invoke(clip);
        }

        public void SetActive(TClip clip, bool active)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.Active);
            propertyInfo.SetValue(clip, active);
        }

        public int GetSizeBytes(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.SizeBytes.Compile().Invoke(clip);
        }

        public void SetSizeBytes(TClip clip, int sizeBytes)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.SizeBytes);
            propertyInfo.SetValue(clip, sizeBytes);
        }

        public string GetFileName(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.FileName.Compile().Invoke(clip);
        }

        public void SetFileName(TClip clip, string fileName)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.FileName);
            propertyInfo.SetValue(clip, fileName);
        }

        public string GetMimeType(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.MimeType.Compile().Invoke(clip);
        }

        public void SetMimeType(TClip clip, string mimeType)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.MimeType);
            propertyInfo.SetValue(clip, mimeType);
        }

        public string GetRef(TClip clip)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            return _model.Reference.Compile().Invoke(clip);
        }

        public void SetRef(TClip clip, string reference)
        {
            clip = clip ?? throw new ArgumentNullException(nameof(clip));
            var propertyInfo = Property(_model.Reference);
            propertyInfo.SetValue(clip, reference);
        }

        private static PropertyInfo Property<TResult>(Expression<Func<TClip, TResult>> expression)
        {
            return ExpressionUtil.Property(expression);
        }
    }
}