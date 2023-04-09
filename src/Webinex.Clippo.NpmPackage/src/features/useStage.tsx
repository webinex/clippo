import { useCallback } from 'react';
import { ClippoActionsCollection } from '../actions';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';

export function useStage<TClip>(
  options: ClippoOptions<TClip>,
  state: ClippoState<TClip>
) {
  const { update, value } = state;
  const { actions } = value;

  return useCallback(() => {
    update({
      actions: [],
      items: new ClippoActionsCollection(actions).apply(value, options),
    });
  }, [update, value, actions, options]);
}
