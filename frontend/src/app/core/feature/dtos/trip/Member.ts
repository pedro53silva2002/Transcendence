export interface TripMemberDto {
  id: number;
  trip_id: number;
  user_id: number;
  role: 'ADMIN' | 'MEMBER';
  joined_at: string;
}

export interface CreateTripMemberDto {
  trip_id: number;
  user_id: number;
  role: 'ADMIN' | 'MEMBER';
}

export interface UpdateTripMemberDto {
  role: 'ADMIN' | 'MEMBER';
}

export type TripMemberSearchFieldsDto = {
  tripId: number;
  role: 'ADMIN' | 'MEMBER';
};

export type TripMemberOrderByDto = 'role' | 'userId';
