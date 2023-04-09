import { useCallback } from 'react';
import { useFetch } from './useFetch';
import {
  ClippoAction,
  ClippoActionsCollection as Collection,
  shakeActions,
} from '../actions';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { useHttp } from '../http';

export interface ApplyArgs {
  actions: ClippoAction[];
  reload?: boolean;
}

function useApplyImmediate<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { update } = clippoState;
  const http = useHttp<TClip>(options);
  const fetch = useFetch(options, clippoState);
  const { reloadAfterAction } = options;

  return useCallback(
    async (args: ApplyArgs) => {
      const { actions, reload: reloadArg } = args;
      const reload = reloadArg ?? reloadAfterAction ?? false;

      function handleApplied() {
        update(prev => {
          const items = new Collection(actions).apply(prev, options);
          return { applyPending: [], applyFailed: undefined, items };
        });
      }

      function handleFailure(ex: any) {
        update({ applyPending: [], applyFailed: ex });
      }

      async function reloadIfNeeded() {
        if (reload) {
          return await fetch();
        }
      }

      async function apply() {
        update({ applyPending: actions });
        try {
          await http.apply(actions);
          handleApplied();
          return reloadIfNeeded();
        } catch (ex) {
          handleFailure(ex);
          throw ex;
        }
      }

      return await apply();
    },
    [update, http, fetch, reloadAfterAction, options]
  );
}

function useApplyDefer<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { update } = clippoState;

  return useCallback(
    (args: ApplyArgs) => {
      if (args.reload === true)
        console.warn(
          '`args.reload` might not be `true` when using `defer` mode. It would be ignored.'
        );

      update(prev => {
        const value = { ...prev, actions: [...prev.actions, ...args.actions] };
        return shakeActions(value, options);
      });

      return Promise.resolve();
    },
    [update, options]
  );
}

export function useApply<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const applyImmediate = useApplyImmediate(options, clippoState);
  const applyDefer = useApplyDefer(options, clippoState);
  const { defer } = options;

  return useCallback(
    (args: ApplyArgs) => {
      if (args == null) throw new Error('`args` might not be null');
      if (args.actions == null)
        throw new Error('`args.actions` might not be null');

      return defer ? applyDefer(args) : applyImmediate(args);
    },
    [applyDefer, applyImmediate, defer]
  );
}
