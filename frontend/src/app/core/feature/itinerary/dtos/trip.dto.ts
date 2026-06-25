export interface TripDto {
  id: number;
  name: string;
  createdAt: string;
}

export interface CreateTripDto {
  name: string;
  description: string;
  startDate: string;
  endDate: string;
  destination: string;
  budget: number;
  visibility: 'PRIVATE' | 'FRIENDS' | 'PUBLIC';
  members: { userId: number; role: 'ADMIN' | 'MEMBER' }[];
}
