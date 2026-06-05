export interface CreateUserDto {
  username: string;
  email: string;
  password: string;
}

export interface UserDto {
  id: number;
  email: string;
  username: string;
  displayName: string;
  bio: string | null;
  profilePhotoUrl: string | null;
  oAuthProvider: string | null;
  oAuthId: string | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface UpdateUserDto {
  id: number;
  email: string;
  username: string;
  displayName: string;
  bio: string;
  profilePhotoUrl: string;
}

export interface AuthResponseDto {
  user: UserDto;
  token: string;
  expiresAt: string;
  refreshToken: string;
  refreshTokenExpires: string;
}

export type UserSearchFieldsDto = {
  username: string;
  displayName: string;
  email: string;
  id: number;
  createdAt: number;
};

export type UserOrderByFieldsDto = 'username' | 'displayName' | 'createdAt' | 'id';
