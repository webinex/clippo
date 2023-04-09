import { ClippoOptions } from '../clippoOptions';
import { ClippoStateValue } from '../clippoState';

export function shakeActions<TClip>(
  value: ClippoStateValue<TClip>,
  options: ClippoOptions<TClip>
): ClippoStateValue<TClip> {
  if (value.actions.length === 0) {
    return value;
  }

  let result = value;
  const visited = [];

  while (true) {
    const action = result.actions.find(action => visited.indexOf(action) < 0);
    if (action == null) break;

    visited.push(action);

    if (action.shake) {
      result = action.shake(result, options);
    }
  }

  return result;
}
