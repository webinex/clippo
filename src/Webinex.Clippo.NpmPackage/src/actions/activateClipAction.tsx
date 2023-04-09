import { ClippoAction } from './clippoAction';

export class ActivateClipAction implements ClippoAction {
  public static kind = 'webinex://Activate';

  public static is(action: ClippoAction): action is ActivateClipAction {
    return !!action && action.kind === ActivateClipAction.kind;
  }

  public kind: string = ActivateClipAction.kind;
  public id: string;
  public ownerType: string;
  public ownerId: string;
  public directory: string;

  constructor(
    id: string,
    ownerType: string,
    ownerId: string,
    directory?: string
  ) {
    if (id == null) throw new Error('`id` might not be null');

    this.id = id;
    this.ownerType = ownerType;
    this.ownerId = ownerId;
    this.directory = directory;
  }
}
