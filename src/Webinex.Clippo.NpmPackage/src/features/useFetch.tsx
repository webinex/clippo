import { useCallback } from 'react';
import { clippoInitialState } from '..';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { useHttp } from '../http';

function useFetchByDir<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { directory, defer } = options;
  const { update, setValue } = clippoState;
  const http = useHttp<TClip>(options);

  return useCallback(async () => {
    update({ fetchPending: true });

    try {
      const items = await http.byDir(directory);
      setValue({ ...clippoInitialState(defer), fetchPending: false, items });
    } catch (ex) {
      update({ fetchPending: false, fetchFailed: ex });
      throw ex;
    }
  }, [directory, update, setValue, defer, http]);
}

function useFetchByOwner<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { ownerType, ownerId, directory, defer } = options;
  const { update, setValue } = clippoState;
  const http = useHttp<TClip>(options);

  return useCallback(async () => {
    update({ fetchPending: true });

    try {
      const items = await http.byOwner(ownerType, ownerId, directory);
      setValue({ ...clippoInitialState(defer), fetchPending: false, items });
    } catch (ex) {
      update({ fetchPending: false, fetchFailed: ex });
      throw ex;
    }
  }, [ownerId, ownerType, directory, defer, setValue, update, http]);
}
export function useFetch<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { ownerType, ownerId, directory } = options;

  const fetchByDir = useFetchByDir(options, clippoState);
  const fetchByOwner = useFetchByOwner(options, clippoState);

  return useCallback(() => {
    if (!ownerType && !ownerId && !directory) {
      throw new Error(
        'unable to fetch when nor `ownerType`, `ownerId`, `directory` defined'
      );
    }

    if (ownerType && ownerId) {
      return fetchByOwner();
    }

    return fetchByDir();
  }, [ownerType, ownerId, directory, fetchByDir, fetchByOwner]);
}
