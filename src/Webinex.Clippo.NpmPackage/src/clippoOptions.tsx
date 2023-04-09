import axios, { AxiosInstance } from 'axios';

export interface ClipModel<TClip> {
  getId: (clip: TClip) => string;
}

export interface ClippoOptions<TClip> {
  /**
   * Clip owner type
   */
  ownerType?: string;

  /**
   * Clip owner id
   */
  ownerId?: string;

  /**
   * Clip directory
   */
  directory?: string;

  /**
   * Should apply changes immediately or save it as actions.
   * Default: true.
   */
  defer?: boolean;

  /**
   * Should preload clips on mount.
   * Default: false.
   */
  noPreload?: boolean;

  /**
   * Axios instance to use for HTTP requests.
   * Default: default axios instance with baseURL = '/api/clippo'
   */
  axios?: AxiosInstance;

  /**
   * Clippo model data access service
   */
  model?: ClipModel<TClip>;

  /**
   * Should reload after actions applied.
   * Default: false.
   */
  reloadAfterAction?: boolean;
}

export const DEFAULT_CLIPPO_OPTIONS: ClippoOptions<any> = {
  defer: true,
  noPreload: false,
  axios: axios.create({ baseURL: '/api/clippo' }),
  model: {
    getId: clip => clip.id,
  },
};

export function useOptions<TClip>(options?: ClippoOptions<TClip>) {
  return Object.assign({}, DEFAULT_CLIPPO_OPTIONS, options ?? {});
}
