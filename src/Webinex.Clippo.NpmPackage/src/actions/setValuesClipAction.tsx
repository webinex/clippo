import { ClippoAction } from './clippoAction';

export class SetValuesClipAction implements ClippoAction {
  public static KIND: string = 'webinex://SetValues';
  public kind: string = SetValuesClipAction.KIND;
  public id: string;
  public values: { [key: string]: any };

  constructor(id: string, values: { [key: string]: any }) {
    if (id == null) throw new Error('`id` might not be null');
    if (values == null) throw new Error('`values` might not be null');

    this.id = id;
    this.values = values;
  }
}
