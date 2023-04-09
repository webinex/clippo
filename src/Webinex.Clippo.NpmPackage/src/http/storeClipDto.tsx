import { ClippoAction } from '../actions';

export interface StoreClipDto {
  file: File;
  actions?: ClippoAction[];
  values?: { [key: string]: any };
}
