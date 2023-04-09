import { useMemo } from 'react';
import { ClippoOptions, useOptions } from './clippoOptions';
import { ClippoState, useClippoState } from './clippoState';
import {
  useFetch,
  useStore,
  useGetById,
  useContent,
  useApply,
  useRemove,
  useOpen,
  useItems,
  usePreload,
  useStage,
} from './features';

export type UseClippoResult<TClip> = ClippoState<TClip> & {
  /**
   * Resets actions, items and loads clips based on options (by owner or by directory)
   */
  fetch: ReturnType<typeof useFetch>;

  /**
   * Stores files and adds them to items.
   */
  store: ReturnType<typeof useStore>;

  /**
   * Gets clip by id
   */
  getById: (id: string) => Promise<TClip>;

  /**
   * Downloads clip content
   */
  content: ReturnType<typeof useContent>;

  /**
   * Applies actions
   */
  apply: ReturnType<typeof useApply>;

  /**
   * Download & opens clip in new tab
   */
  open: ReturnType<typeof useOpen>;

  /**
   * Removes clip by id
   */
  remove: ReturnType<typeof useRemove>;

  /**
   * Applies actions on items and removes actions.
   */
  stage: ReturnType<typeof useStage>;
};

export function useClippo<TClip>(
  optionsArg?: ClippoOptions<TClip>
): UseClippoResult<TClip> {
  const options = useOptions<TClip>(optionsArg);
  const clippoState = useClippoState<TClip>(options);
  const clippoStateValue = clippoState.value;

  const fetch = useFetch<TClip>(options, clippoState);
  const store = useStore<TClip>(options, clippoState);
  const getById = useGetById<TClip>(options, clippoState);
  const content = useContent<TClip>(options, clippoState);
  const apply = useApply<TClip>(options, clippoState);
  const open = useOpen<TClip>(options, clippoState);
  const remove = useRemove<TClip>(options, clippoState);
  const stage = useStage<TClip>(options, clippoState);
  const items = useItems<TClip>(options, clippoState);
  usePreload(options, clippoState);

  const value = useMemo(
    () => ({
      ...clippoStateValue,
      items,
      originalItems: clippoStateValue.items,
    }),
    [clippoStateValue, items]
  );

  return {
    ...clippoState,
    value,
    fetch,
    store,
    getById,
    content,
    apply,
    open,
    remove,
    stage,
  };
}
