import { useCallback } from 'react';
import { useContent } from './useContent';
import { ClippoOptions } from '../clippoOptions';
import { ClippoState } from '../clippoState';
import { ClipContentDto } from '../http';

function download(content: ClipContentDto) {
  const { blob, fileName } = content;
  const url = URL.createObjectURL(blob);
  const anchor = document.createElement('a');
  anchor.href = url;
  anchor.target = '_blank';
  anchor.download = fileName;

  anchor.click();

  setTimeout(() => {
    anchor.remove();
  }, 0);
}

export function useOpen<TClip>(
  options: ClippoOptions<TClip>,
  clippoState: ClippoState<TClip>
) {
  const content = useContent(options, clippoState);

  return useCallback(
    (id: string) => {
      if (id == null) throw new Error('`id` might not be null');

      return content(id)
        .then(download)
        .catch(error => {
          throw error;
        });
    },
    [content]
  );
}
