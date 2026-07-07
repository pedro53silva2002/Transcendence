export type FriendRequestStatus = 'Pending' | 'Accepted' | 'Rejected' | 'Canceled';

export interface CreateFriendRequestDto {
  senderId: number;
  receiverId: number;
}

export interface FriendRequestDto {
  id: number;
  senderId: number;
  senderUsername: string;
  senderProfilePhotoUrl: string | null;
  receiverId: number;
  status: FriendRequestStatus;
  createdAt: string;
  updatedAt: string | null;
}

export interface FriendRequestExistsDto {
  senderId: number;
  receiverId: number;
}
