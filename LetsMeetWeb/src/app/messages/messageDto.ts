export interface MessageDto {
  id?: string;
  roomId: string;
  content: string;
  date?: string;
  from?: string;
  isFromUser?: boolean;
}
