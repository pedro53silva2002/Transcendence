export interface TripMembership {
  tripId: number;
  role: 'MEMBER' | 'ADMIN';
}

export interface MeDto {
  username: string;
  displayName: string;
  email: string;
  profilePicture?: string;
  profilePhotoUrl?: string;
  trips?: TripMembership[];
}
