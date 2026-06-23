export type FriendRequestStatus = 'Pending' | 'Accepted' | 'Rejected' | 'Canceled';

export interface CreateFriendRequestDto {
  senderId: number;
  receiverId: number;
}

export interface FriendRequestDto {
  id: number;
  senderId: number;
  receiverId: number;
  status: FriendRequestStatus;
  createdAt: string;
  updatedAt: string | null;
}
