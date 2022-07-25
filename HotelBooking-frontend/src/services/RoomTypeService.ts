import { BaseService } from "./BaseService";
import type { ISearchParameters } from "@/domain/ISearchParameters";
import httpClient from "@/http-client";
import type { IRoomType } from "@/domain/IRoomType";
import type { IServiceResult } from "@/domain/IServiceResult";
// import { useIdentityStore } from "@/stores/identity";

const apiVersion = "v1";

export class RoomTypeService extends BaseService<IRoomType> {
  constructor() {
    super("roomtype", apiVersion);
  }

  async searchRooms(searchParameters: ISearchParameters): Promise<IServiceResult<IRoomType[] | void>>  {
    // const startDay = searchParameters.startDate.getDate();
    // const startMonth = searchParameters.startDate.getMonth();
    // const startYear = searchParameters.startDate.getFullYear();
    // const endDay = searchParameters.endDate.getDate();
    // const endMonth = searchParameters.endDate.getMonth();
    // const endYear = searchParameters.endDate.getFullYear();
let response;
    try {
       response = await httpClient.get(
        // `/${this.apiVersion}/${this.path}/${searchParameters.hotelId}&startday=${startDay}&startmonth=${startMonth}&startyear=${startYear}&endday=${endDay}&endmonth=${endMonth}&endyear=${endYear}&guests=${searchParameters.numberOfGuests}`,
        `/${this.apiVersion}/${this.path}/${searchParameters.hotelId}/search/startdate=${searchParameters.startDate}&enddate=${searchParameters.endDate}&guests=${searchParameters.numberOfGuests}`,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token
          }
        }
      );
    }
      catch (e) {
        return this.handleError(e);
      }
  
      return { status: response.status, data: response.data as IRoomType[] };
    
  }
}
