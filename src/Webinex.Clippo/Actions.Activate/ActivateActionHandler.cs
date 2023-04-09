using System;
using System.Threading.Tasks;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Actions.Activate
{
    internal class ActivateActionHandler<TClip> : ClippoActionHandler<TClip, ActivateClippoAction>
    {
        private readonly IClippoModel<TClip> _clippoModel;
        private readonly IClippoStore<TClip> _clippoStore;

        public ActivateActionHandler(IClippoModel<TClip> clippoModel, IClippoStore<TClip> clippoStore)
        {
            _clippoModel = clippoModel;
            _clippoStore = clippoStore;
        }

        protected override async Task<CodedResult> HandleAsync(ActivateClippoAction action)
        {
            ValidateAction(action);

            var clip = await _clippoStore.ByIdAsync(action.Id, GetClipOptions.IncludeInactive);
            if (clip == null) return ClippoCodes.NOT_FOUND.Failed();

            if (IsActive(clip))
                return ClippoCodes.ALREADY_ACTIVE.Failed(action.Id);
            
            SetActionValues(clip, action);
            await _clippoStore.UpdateAsync(clip);

            return CodedResults.Success();
        }

        private void SetActionValues(TClip clip, ActivateClippoAction action)
        {
            _clippoModel.SetActive(clip, true);
            _clippoModel.SetOwnerId(clip, action.OwnerId);
            _clippoModel.SetOwnerType(clip, action.OwnerType);
            _clippoModel.SetDirectory(clip, action.Directory);
        }

        private void ValidateAction(ActivateClippoAction action)
        {
            if (!action.Valid)
                throw new InvalidOperationException($"Malformed {nameof(ActivateClippoAction)}");
        }

        private bool IsActive(TClip clip)
        {
            return _clippoModel.GetActive(clip);
        }
    }
}