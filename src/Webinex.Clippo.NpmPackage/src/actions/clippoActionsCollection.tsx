import { ClippoAction } from '.';
import { ClippoOptions } from '../clippoOptions';
import { ClippoStateValue } from '../clippoState';

export class ClippoActionsCollection {
  private _value: ClippoAction[];

  constructor(value: ClippoAction[]) {
    if (value == null) throw new Error('`value` might not be null');

    this._value = value;
  }

  public apply<TClip>(
    value: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>
  ): TClip[] {
    if (this._value.length === 0) {
      return value.items;
    }

    let result = value.items;

    for (const action of this._value) {
      if (!action.apply) continue;

      result = action.apply({ ...value, items: result }, options);
    }

    return result;
  }
}
