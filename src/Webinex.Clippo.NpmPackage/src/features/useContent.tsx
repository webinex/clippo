import { useCallback } from 'react';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { useHttp } from '../http';

export function useContent<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { update, value } = clippoState;
  const { items } = value;
  const http = useHttp<TClip>(options);
  const { getId } = options.model;

  return useCallback(
    (id: string) => {
      if (id == null) throw new Error('`id` might not be null');

      const clip = items.find(item => getId(item) === id);

      update({ contentPending: clip });
      return http
        .content(id)
        .then(value => {
          update({ contentPending: undefined, contentFailed: undefined });
          return value;
        })
        .catch(error => {
          update({ contentPending: undefined, contentFailed: error });
          throw error;
        });
    },
    [update, items, http, getId]
  );
}
