import { ClippoStateValue } from '../clippoState';
import { ClippoAction } from './clippoAction';
import { ClippoOptions } from '../clippoOptions';
import { ActivateClipAction } from './activateClipAction';
import { SetValuesClipAction } from './setValuesClipAction';

export class DeleteClipAction implements ClippoAction {
  public static kind = 'webinex://Delete';

  public kind: string = DeleteClipAction.kind;
  public id: string;

  constructor(id: string) {
    if (id == null) throw new Error('`id` might not be null');

    this.id = id;
  }

  public apply<TClip>(
    value: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>
  ) {
    return value.items.filter(clip => options.model.getId(clip) !== this.id);
  }

  public shake<TClip>(
    value: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>
  ): ClippoStateValue<TClip> {
    const activate = this.findPreviouslyAdded(value);
    const setValues = this.findSetValues(value);
    return activate == null
      ? value
      : this.removePreviouslyAdded(value, options, activate, setValues);
  }

  private removePreviouslyAdded<TClip>(
    value: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>,
    activate: ActivateClipAction,
    setValues: SetValuesClipAction[]
  ) {
    const actions = this.removeActions(value, activate, setValues);
    const items = this.removeItem(value, options, activate);

    return {
      ...value,
      actions,
      items,
    };
  }

  private removeActions<TClip>(
    value: ClippoStateValue<TClip>,
    activate: ActivateClipAction,
    setValues: SetValuesClipAction[]
  ) {
    const { actions } = value;

    const result = [...actions];
    result.splice(result.indexOf(activate), 1);
    result.splice(result.indexOf(this), 1);
    setValues.forEach(a => result.splice(result.indexOf(a), 1));

    return result;
  }

  private removeItem<TClip>(
    value: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>,
    activate: ActivateClipAction
  ) {
    const { items } = value;
    const { getId } = options.model;
    return items.filter(x => getId(x) !== activate.id);
  }

  private findPreviouslyAdded<TClip>(value: ClippoStateValue<TClip>) {
    return value.actions.find(this.isActivateSame);
  }

  private isActivateSame = (
    action: ClippoAction
  ): action is ActivateClipAction & boolean => {
    return ActivateClipAction.is(action) && action.id === this.id;
  };

  private findSetValues<TClip>(value: ClippoStateValue<TClip>) {
    return value.actions.filter(this.isSetValuesSame);
  }

  private isSetValuesSame = (
    action: ClippoAction
  ): action is SetValuesClipAction & boolean => {
    const isSetValues = action.kind === SetValuesClipAction.KIND;
    if (!isSetValues) return false;

    const setValuesAction = action as SetValuesClipAction;
    return setValuesAction.id === this.id;
  };
}
