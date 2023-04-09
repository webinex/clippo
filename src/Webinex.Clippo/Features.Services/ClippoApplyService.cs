using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webinex.Clippo.Actions;
using Webinex.Coded;

namespace Webinex.Clippo.Features.Services
{
    internal interface IClippoApplyService<TClip>
    {
        Task<CodedResult> ApplyAsync(IEnumerable<IClippoAction> actions);
        Task<CodedResult> ApplyNewAsync(TClip clip, IEnumerable<IClippoAction> actions);
    }

    internal class ClippoApplyService<TClip> : IClippoApplyService<TClip>
    {
        private readonly IEnumerable<IClippoActionHandler<TClip>> _actionHandlers;
        private readonly IEnumerable<IClippoNewActionHandler<TClip>> _newActionHandlers;

        public ClippoApplyService(
            IEnumerable<IClippoActionHandler<TClip>> actionHandlers,
            IEnumerable<IClippoNewActionHandler<TClip>> newActionHandlers)
        {
            _actionHandlers = actionHandlers;
            _newActionHandlers = newActionHandlers;
        }

        public async Task<CodedResult> ApplyAsync(IEnumerable<IClippoAction> actions)
        {
            actions = actions?.ToArray() ?? throw new ArgumentNullException(nameof(actions));

            foreach (var action in actions)
            {
                foreach (var actionHandler in _actionHandlers)
                {
                    var result = await actionHandler.HandleAsync(action);
                    if (!result.Succeed) return result;
                }
            }
            
            return CodedResults.Success();
        }

        public async Task<CodedResult> ApplyNewAsync(TClip clip, IEnumerable<IClippoAction> actions)
        {
            actions = actions?.ToArray() ?? throw new ArgumentNullException(nameof(actions));
            clip = clip ?? throw new ArgumentNullException(nameof(clip));

            foreach (var action in actions)
            {
                foreach (var actionHandler in _newActionHandlers)
                {
                    var result = await actionHandler.HandleNewAsync(clip, action);
                    if (!result.Succeed) return result;
                }
            }
            
            return CodedResults.Success();
        }
    }
}