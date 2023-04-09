import { ClippoAction } from './clippoAction';

export class ActivateNewClipAction implements ClippoAction {
  public static kind = 'webinex://ActivateNew';

  public kind: string = ActivateNewClipAction.kind;
  public ownerType: string;
  public ownerId: string;
  public directory: string;

  constructor(ownerType: string, ownerId: string, directory?: string) {
    if (ownerType == null) throw new Error('`ownerType` might not be null');
    if (ownerId == null) throw new Error('`ownerId` might not be null');

    this.ownerType = ownerType;
    this.ownerId = ownerId;
    this.directory = directory;
  }
}
