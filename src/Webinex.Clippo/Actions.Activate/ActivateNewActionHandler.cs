using System.Threading.Tasks;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Actions.Activate
{
    internal class ActivateNewActionHandler<TClip> : ClippoNewActionHandler<TClip, ActivateNewClippoAction>
    {
        private readonly IClippoModel<TClip> _clippoModel;
        private readonly IClippoStore<TClip> _clippoStore;

        public ActivateNewActionHandler(IClippoModel<TClip> clippoModel, IClippoStore<TClip> clippoStore)
        {
            _clippoModel = clippoModel;
            _clippoStore = clippoStore;
        }

        public override async Task<CodedResult> HandleNewAsync(TClip clip, ActivateNewClippoAction action)
        {
            if (_clippoModel.GetActive(clip))
                return ClippoCodes.ALREADY_ACTIVE.Failed(_clippoModel.GetId(clip));
            
            SetValues(clip, action);
            await _clippoStore.UpdateAsync(clip);
            return CodedResults.Success();
        }

        private void SetValues(TClip clip, ActivateNewClippoAction action)
        {
            _clippoModel.SetActive(clip, true);
            _clippoModel.SetOwnerId(clip, action.OwnerId);
            _clippoModel.SetOwnerType(clip, action.OwnerType);
            _clippoModel.SetDirectory(clip, action.Directory);
        }
    }
}