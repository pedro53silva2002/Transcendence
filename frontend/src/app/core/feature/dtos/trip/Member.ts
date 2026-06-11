export interface TripMemberDto {
  id: number;
  tripId: number;
  userId: number;
  displayName: string;
  profilePicture: string | null;
  role: 'Admin' | 'Member';
  joinedAt: string;
  updatedAt: string;
}

export interface CreateTripMemberDto {
  tripId: number;
  userIds: number[];
}

export interface UpdateTripMemberDto {
  userId: number;
  role: 'Admin' | 'Member';
}

export type TripMemberSearchFieldsDto = {
  tripId: number;
  role: 'Admin' | 'Member';
  displayName: string;
};

export type TripMemberOrderByDto = 'role' | 'userId' | 'displayName';
