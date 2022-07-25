import type { IAmenity } from '@/domain/IAmenity';
import { BaseService } from './BaseService';

const apiVersion: string = 'v1';

export class AmenityService extends BaseService<IAmenity> {
    constructor() {
        super("amenity", apiVersion)
    }

    // async getAllHotelAmenities(hotelId: string) : Promise<IServiceResult<IAmenity[]| null>> {
    //     let response;
    //     try {
    //         response = await httpClient.get(`${apiVersion}/hotel/amenities/${hotelId}`);
    //     } catch (e) {
    //         return this.handleErrorResponse((e as AxiosError));
    //     }

    //     return this.handleSuccessfulResponseArr(response);
    // }

}
