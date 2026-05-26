import { UserDto } from './user.dto';

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

export interface AuthResponseDto {
  user: UserDto;
  token: string;
  expiresAt: string;
  refreshToken: string;
  refreshTokenExpires: string;
}

export interface LoginDto {
  username: string;
  password: string;
}
