import { ClippoOptions } from '../clippoOptions';
import { ClippoState, StoreError } from '../clippoState';
import { ClippoHttp, StoreClipDto, useHttp } from '../http';
import {
  ActivateClipAction,
  ActivateNewClipAction,
  ClippoAction,
} from '../actions';
import { useCallback } from 'react';

class StoreAction<TClip> {
  private errors: any[] = [];

  constructor(
    private update: ClippoState<TClip>['update'],
    private options: ClippoOptions<TClip>,
    private http: ClippoHttp<TClip>,
    private args: StoreArg[]
  ) {}

  public store = async () => {
    this.pushFilesToPending();
    const stored: Array<TClip | null> = [];

    for (const arg of this.args) {
      const result = await this.storeOne(arg);
      stored.push(result);
    }

    this.processErrors();
    return stored;
  };

  private storeOne = async (arg: StoreArg): Promise<TClip | null> => {
    const { file, values } = arg;
    const actions = this.requestActions(arg);
    const args: StoreClipDto = { file, actions, values };

    return await this.http
      .store(args)
      .then(clip => {
        this.handleStored(arg, clip);
        return clip;
      })
      .catch(error => {
        this.handleFailed(arg, error);
        return null;
      });
  };

  private handleStored = (arg: StoreArg, clip: TClip) => {
    this.pushClip(clip);
    this.pushActions(arg, clip);
    this.removeFileFromPending(arg);
  };

  private handleFailed = (arg: StoreArg, ex: any) => {
    this.errors.push(ex);
    this.removeFileFromPending(arg);
  };

  private processErrors = () => {
    if (!this.storeError) {
      this.resetError();
      return;
    }

    this.pushError(this.storeError);
    throw this.storeError;
  };

  private pushError(error: any) {
    this.update({ storeFailed: error });
  }

  private resetError() {
    this.update({ storeFailed: undefined });
  }

  private requestActions(arg: StoreArg) {
    if (this.defer) {
      return [];
    }

    return [...this.activateRequestActions, ...arg.actions];
  }

  private pushClip(clip: TClip) {
    this.update(prev => ({ items: [...prev.items, clip] }));
  }

  private pushActions(arg: StoreArg, clip: TClip) {
    const userActions = arg.actions.map(x =>
      x.withClip ? x.withClip(clip) : x
    );
    const actions = [...this.activateResultActions(clip), ...userActions];
    this.update(prev => ({ actions: [...prev.actions, ...actions] }));
  }

  private removeFileFromPending(arg: StoreArg) {
    this.update(prev => ({
      storePending: prev.storePending.filter(x => x !== arg.file),
    }));
  }

  private pushFilesToPending() {
    this.update({ storePending: this.args.map(x => x.file) });
  }

  private get defer() {
    return !!this.options.defer;
  }

  private get activateRequestActions() {
    const { ownerType, ownerId, directory } = this.options;

    return this.defer
      ? []
      : [new ActivateNewClipAction(ownerType, ownerId, directory)];
  }

  private activateResultActions(clip: TClip) {
    const { ownerType, ownerId, directory } = this.options;
    if (!this.defer) return [];

    const activateAction = new ActivateClipAction(
      this.getId(clip),
      ownerType,
      ownerId,
      directory
    );

    return [activateAction];
  }

  private getId(clip: TClip) {
    return this.options.model.getId(clip);
  }

  private get storeError() {
    if (this.errors.length === 0) return undefined;

    return new StoreError(this.errors);
  }
}

type StoreArg = {
  file: File;
  actions?: ClippoAction[];
  values?: any;
};

function isStoreArgs(args: any[]): args is StoreArg[] {
  return args.some(arg => arg.hasOwnProperty('file'));
}

export function useStore<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const { update } = clippoState;
  const http = useHttp<TClip>(options);

  return useCallback(
    async (filesOrArgs: File[] | StoreArg[]) => {
      if (filesOrArgs == null)
        throw new Error('`filesOrArgs` might not be null');

      const args: StoreArg[] = isStoreArgs(filesOrArgs)
        ? filesOrArgs.map(arg => ({ actions: [], values: null, ...arg }))
        : filesOrArgs.map(file => ({ file, actions: [], values: null }));

      return await new StoreAction(update, options, http, args).store();
    },
    [update, http, options]
  );
}
