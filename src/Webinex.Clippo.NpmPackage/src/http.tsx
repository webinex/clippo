import { AxiosInstance } from 'axios';

export interface VFolderId {
  type: string;
  id: string;
}

export interface VFile<TMeta, TData> {
  id: string;
  folder: VFolderId;
  name: string;
  bytes: number;
  ref: string;
  mimeType: string;
  meta: TMeta;
  data: TData;
}

export interface VFolder<TMeta, TData> {
  id: string;
  type: string;
  version: string;
  path: string | null;
  files: VFile<TMeta, TData>[];
}

export interface Optional<TValue> {
  hasValue: boolean;
  value?: TValue | null;
}

export interface VFileAddPatch<TData> {
  name: string;
  mimeType: string;
  bytes: number;
  ref: string;
  data: TData;
}

export interface VFileDeletePatch {
  id: string;
}

export interface VFileSetPatch<TData> {
  id: string;
  name?: Optional<string> | null;
  mimeType?: Optional<string> | null;
  bytes?: Optional<number> | null;
  ref?: Optional<string> | null;
  data?: Optional<TData> | null;
}

export interface VFilePatch<TData> {
  add?: VFileAddPatch<TData> | null;
  delete?: VFileDeletePatch | null;
  set?: VFileSetPatch<TData> | null;
}

export interface VFolderPatch<TData> {
  id: VFolderId;
  version?: Optional<string> | null;
  files?: VFilePatch<TData>[] | null;
}

export interface VFileState<TData> {
  id?: string | null;
  name: string;
  bytes: number;
  ref: string;
  mimeType: string;
  data: TData;
}

export interface VFolderState<TData> {
  id: string;
  type: string;
  path?: Optional<string> | null;
  version?: Optional<string> | null;
  files: VFileState<TData>[];
}

export class ClippoHttp<TMeta, TData> {
  private _axios: AxiosInstance;

  constructor(axios: AxiosInstance) {
    if (axios == null) throw new Error('`axios` might not be null');

    this._axios = axios;
  }

  public getAll = async (
    folderId?: VFolderId | null,
    folderPath?: string | null,
  ): Promise<VFolder<TMeta, TData>[]> => {
    const params = new URLSearchParams();
    if (folderId != null) {
      params.append('type', folderId.type);
      params.append('id', folderId.id);
    }
    if (folderPath != null) params.append('path', folderPath);
    const urlParams = params.toString();
    const url = urlParams ? `/?${urlParams}` : '/';

    const { data } = await this._axios.get<VFolder<TMeta, TData>[]>(url);
    return data;
  };

  public patch = async (patch: VFolderPatch<TData>): Promise<VFolder<TMeta, TData>> => {
    const { data } = await this._axios.patch<VFolder<TMeta, TData>>('/', patch);
    return data;
  };

  public save = async (folder: VFolderState<TData>): Promise<VFolder<TMeta, TData>> => {
    const { data } = await this._axios.put<VFolder<TMeta, TData>>('/', folder);
    return data;
  };
}
