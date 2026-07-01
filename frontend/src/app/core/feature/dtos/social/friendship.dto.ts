import { UserDto } from '../../auth/dtos/user.dto';

export interface FriendshipDto {
  id: number;
  userId1: number;
  user1: UserDto;
  userId2: number;
  user2: UserDto;
  createdAt: string;
}

export interface CreateFriendshipDto {
  userId1: number;
  userId2: number;
}

export interface FriendDto {
  id: number;
  friendId: number;
  username: string;
  profilePhotoUrl: string | null;
}
