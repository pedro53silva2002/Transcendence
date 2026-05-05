export interface TripMembership {
  tripId: number;
  role: 'MEMBER' | 'ADMIN';
}

export interface MeDto {
  username: string;
  profilePicture?: string;
  trips?: TripMembership[];
}
