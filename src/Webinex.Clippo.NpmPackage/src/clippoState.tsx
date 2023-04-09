import { useState } from 'react';
import { ClippoAction } from './actions';
import { ClippoOptions } from './clippoOptions';

export class StoreError extends Error {
  public innerErrors: any[];

  constructor(innerErrors: any[]) {
    super();
    this.innerErrors = innerErrors;
  }
}

export interface ClippoStateValue<TClip> {
  items: TClip[];
  actions: ClippoAction[];

  byIdPending: boolean;
  byIdFailed: any;

  fetchPending: boolean;
  fetchFailed: any;

  storePending: File[];
  storeFailed: StoreError;

  contentPending: TClip;
  contentFailed: any;

  applyPending: ClippoAction[];
  applyFailed: any;
}

export function clippoInitialState<TClip>(
  preload: boolean
): ClippoStateValue<TClip> {
  return {
    items: [],
    actions: [],
    byIdPending: false,
    byIdFailed: undefined,
    fetchPending: preload,
    fetchFailed: undefined,
    contentPending: undefined,
    contentFailed: undefined,
    storePending: [],
    storeFailed: undefined,
    applyPending: [],
    applyFailed: undefined,
  };
}

export interface ClippoState<TClip> {
  value: ClippoStateValue<TClip>;
  setValue: (
    valueOrSetter:
      | ClippoStateValue<TClip>
      | ((prev: ClippoStateValue<TClip>) => ClippoStateValue<TClip>)
  ) => any;
  update: (
    valueOrSetter:
      | Partial<ClippoStateValue<TClip>>
      | ((prev: ClippoStateValue<TClip>) => Partial<ClippoStateValue<TClip>>)
  ) => any;
}

export function useClippoState<TClip>(
  options: ClippoOptions<TClip>
): ClippoState<TClip> {
  const [state, setState] = useState(
    clippoInitialState<TClip>(!options.noPreload)
  );

  function update(
    valueOrSetter:
      | Partial<ClippoStateValue<TClip>>
      | ((prev: ClippoStateValue<TClip>) => Partial<ClippoStateValue<TClip>>)
  ) {
    setState(prev => {
      const value =
        typeof valueOrSetter === 'function'
          ? valueOrSetter(prev)
          : valueOrSetter;

      return { ...prev, ...value };
    });
  }

  return { value: state, setValue: setState, update };
}
