import { ClippoOptions } from '../clippoOptions';
import { ClippoStateValue } from '../clippoState';

export interface ClippoAction {
  /**
   * Clippo action kind
   */
  kind: string;

  /**
   * Apply useful in defer mode. When new action added, value.items would be a value with all actions applied
   */
  apply?: <TClip>(
    clippoState: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>
  ) => TClip[];

  /**
   * Shake is needed to adjust ClippoStateValue when new action added.
   * For example, when DeleteClipAction added, ActivateClipAction for with same id would be
   * removed and newly created items would be excluded from value.items.
   */
  shake?: <TClip>(
    clippoState: ClippoStateValue<TClip>,
    options: ClippoOptions<TClip>
  ) => ClippoStateValue<TClip>;

  /**
   * When this method defined. In defer mode it would be called after clip stored.
   */
  withClip?: <TClip>(clip: TClip) => ClippoAction;
}
