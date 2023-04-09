import { AxiosInstance } from 'axios';
import { useMemo } from 'react';
import { ClippoAction } from '../actions';
import { ClippoOptions } from '../clippoOptions';
import { ClipContentDto } from './clipContentDto';
import { StoreClipDto } from './storeClipDto';

export class ClippoHttp<TClip> {
  private _axios: AxiosInstance;

  constructor(axios: AxiosInstance) {
    if (axios == null) throw new Error('`axios` might not be null');

    this._axios = axios;
  }

  public async byId(id: string) {
    if (id == null) throw new Error('`id` might not be null');

    const response = await this._axios.get<TClip>(id);
    return response.data;
  }

  public async byOwner(ownerType: string, ownerId: string, directory?: string) {
    if (ownerType == null) throw new Error('`ownerType` might not be null');
    if (ownerId == null) throw new Error('`ownerId` might not be null');

    let query = `ownerType=${encodeURIComponent(ownerType)}`;
    query = query + `&ownerId=${encodeURIComponent(ownerId)}`;

    if (directory)
      query = query + `&directory=${encodeURIComponent(directory)}`;

    const uri = `by-owner?${query}`;
    const response = await this._axios.get<TClip[]>(uri);
    return response.data;
  }

  public async byDir(directory: string) {
    if (directory == null) throw new Error('`directory` might not be null');

    const uri = `by-directory?directory=${encodeURIComponent(directory)}`;
    const response = await this._axios.get<TClip[]>(uri);
    return response.data;
  }

  public async content(id: string): Promise<ClipContentDto> {
    if (id == null) throw new Error('`id` might not be null');

    const uri = `${id}/content`;
    const response = await this._axios({
      url: uri,
      method: 'GET',
      responseType: 'blob',
    });

    const data: Blob = response.data;
    let fileName = response.headers['content-disposition']
      .split('filename=')[1]
      .split(';')[0];

    if (fileName.startsWith('"')) {
      fileName = fileName.substring(1);
    }

    if (fileName.endsWith('"')) {
      fileName = fileName.substring(0, fileName.length - 1);
    }

    return {
      id,
      blob: data,
      fileName,
      mimeType: data.type,
      sizeBytes: data.size,
    };
  }

  public async store(args: StoreClipDto) {
    if (args == null) throw new Error('`args` might not be null');
    if (args.file == null) throw new Error('`args.file` might not be null');

    const payload = { actions: args.actions ?? [], values: args.values ?? {} };
    const payloadJson = JSON.stringify(payload);

    const formData = new FormData();
    formData.append('file', args.file);
    formData.append('payload', payloadJson);

    const response = await this._axios.post<TClip>('', formData);
    return response.data;
  }

  public async apply(actions: ClippoAction[]): Promise<void> {
    if (actions == null) throw new Error('`actions` might not be null');

    await this._axios.put('', actions);
  }
}

export function useHttp<TClip>(options: ClippoOptions<TClip>) {
  const { axios } = options;
  return useMemo(() => new ClippoHttp<TClip>(axios), [axios]);
}
