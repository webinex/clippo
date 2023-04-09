import { useEffect } from 'react';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { useFetch } from './useFetch';

export function usePreload<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const fetch = useFetch(options, clippoState);
  const { noPreload } = options;

  useEffect(() => {
    if (!noPreload) {
      fetch();
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [fetch, noPreload]);
}
