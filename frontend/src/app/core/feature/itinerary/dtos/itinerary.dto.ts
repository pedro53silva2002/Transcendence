export interface ItineraryDto {
  id: number;
  tripId: number;
  title: string;
  description: string | null;
  expectedPrice: number;
  day: number;
  createdBy: number;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateItineraryDto {
  tripId: number;
  title: string;
  description?: string;
  expectedPrice: number;
  day: number;
}

export type ItinerarySearchFieldsDto = {
  tripId: number;
  day: number;
  title: string;
  expectedPrice: number;
  createdBy: number;
};

export interface TripTotalPriceDto {
	totalPrice: number;
}

export type ItineraryOrderByDto = 'id' | 'tripId' | 'title' | 'expectedPrice' | 'day' | 'createdBy';
