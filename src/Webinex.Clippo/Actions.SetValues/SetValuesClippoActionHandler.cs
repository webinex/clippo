using System.Threading.Tasks;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Actions.SetValues
{
    internal class SetValuesClippoActionHandler<TClip> : ClippoActionHandler<TClip, SetValuesClippoAction>
    {
        private readonly IClippoStore<TClip> _clippoStore;
        private readonly IClippoModel<TClip> _clippoModel;

        public SetValuesClippoActionHandler(IClippoStore<TClip> clippoStore, IClippoModel<TClip> clippoModel)
        {
            _clippoStore = clippoStore;
            _clippoModel = clippoModel;
        }

        protected override async Task<CodedResult> HandleAsync(SetValuesClippoAction action)
        {
            var clip = await _clippoStore.ByIdAsync(action.Id, GetClipOptions.IncludeInactive);
            if (clip == null) return ClippoCodes.NOT_FOUND.Failed();
            
            _clippoModel.SetValues(clip, action.Values);
            await _clippoStore.UpdateAsync(clip);
            return CodedResults.Success();
        }
    }
}