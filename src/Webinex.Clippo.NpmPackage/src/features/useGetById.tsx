import { useCallback } from 'react';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { useHttp } from '../http';

export function useGetById<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { update } = clippoState;
  const http = useHttp<TClip>(options);

  return useCallback(
    async (id: string) => {
      if (id == null) throw new Error('`id` might not be null');

      update({ byIdPending: true });

      try {
        const clip = await http.byId(id);
        update({ byIdPending: false, byIdFailed: undefined });
        return clip;
      } catch (ex) {
        update({ byIdFailed: ex, byIdPending: false });
        throw ex;
      }
    },
    [update, http]
  );
}
