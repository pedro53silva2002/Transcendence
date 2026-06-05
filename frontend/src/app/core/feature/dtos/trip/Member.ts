export interface TripMemberDto {
  id: number;
  tripId: number;
  userId: number;
  displayName: string;
  profilePicture: string | null;
  role: 'ADMIN' | 'MEMBER';
  joinedAt: string;
}

export interface CreateTripMemberDto {
  tripId: number;
  userId: number;
  role: 'ADMIN' | 'MEMBER';
}

export interface UpdateTripMemberDto {
  role: 'ADMIN' | 'MEMBER';
}

export type TripMemberSearchFieldsDto = {
  tripId: number;
  role: 'ADMIN' | 'MEMBER';
  displayName: string;
};

export type TripMemberOrderByDto = 'role' | 'userId' | 'displayName';
