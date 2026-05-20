export interface CreateUserDto {
  username: string;
  email: string;
  password: string;
}

export interface MeDto {
  username: string;
  displayName: string;
  email: string;
  profilePhotoUrl: string | null;
  trips: TripMembershipDto[];
}

export interface TripMembershipDto {
  tripId: number;
  role: 'ADMIN' | 'MEMBER';
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

export interface AuthResponseDto {
  user: UserDto;
  token: string;
  expiresAt: string;
}

export interface LoginDto {
  email: string;
  password: string;
}
