export interface ClipContentDto {
  id: string;
  fileName: string;
  mimeType: string;
  sizeBytes: number;
  blob: Blob;
}
