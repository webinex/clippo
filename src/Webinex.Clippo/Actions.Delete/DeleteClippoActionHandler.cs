using System.Threading.Tasks;
using Webinex.Clippo.Ports.Model;
using Webinex.Coded;

namespace Webinex.Clippo.Actions.Delete
{
    internal class DeleteClippoActionHandler<TClip> : ClippoActionHandler<TClip, DeleteClipAction>
    {
        private readonly IClippoStore<TClip> _clippoStore;

        public DeleteClippoActionHandler(IClippoStore<TClip> clippoStore)
        {
            _clippoStore = clippoStore;
        }

        protected override async Task<CodedResult> HandleAsync(DeleteClipAction action)
        {
            var clip = await _clippoStore.ByIdAsync(action.Id, GetClipOptions.IncludeInactive);
            if (clip == null) return Code.NOT_FOUND.Failed();

            await _clippoStore.DeleteAsync(clip);
            return CodedResults.Success();
        }
    }
}