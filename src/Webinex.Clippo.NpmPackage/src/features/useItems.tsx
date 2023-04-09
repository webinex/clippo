import { useMemo } from 'react';
import { ClippoActionsCollection } from '../actions';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';

export function useItems<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { value } = clippoState;

  return useMemo(() => {
    return new ClippoActionsCollection(value.actions).apply(value, options);
  }, [value, options]);
}
