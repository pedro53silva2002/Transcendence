import { UserDto } from "./UserDto";

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
}

export interface LoginDto {
  email: string;
  password: string;
}
