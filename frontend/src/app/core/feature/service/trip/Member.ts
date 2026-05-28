import { CreateTripMemberDto, TripMemberDto } from './../../dtos/trip/Member';
import { ApiResponse, BaseApiService } from '../../../logic/services/base-api.service';
import { Observable } from 'rxjs';

export class TripMemberService extends BaseApiService {
  public create(dto: CreateTripMemberDto): Observable<ApiResponse<TripMemberDto>> {}
}
