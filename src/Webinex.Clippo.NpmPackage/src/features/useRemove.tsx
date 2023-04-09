import { useApply } from './useApply';
import { DeleteClipAction } from '../actions';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { useCallback } from 'react';

export interface RemoveArgs {
  id: string;
  reload?: boolean;
}

export function useRemove<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const apply = useApply(options, clippoState);

  return useCallback(
    (args: RemoveArgs) => {
      if (args == null) throw new Error('`args` might not be null');
      if (args.id == null) throw new Error('`args.id` might not be null');

      const { id, reload } = args;

      return apply({
        actions: [new DeleteClipAction(id)],
        reload,
      });
    },
    [apply]
  );
}
