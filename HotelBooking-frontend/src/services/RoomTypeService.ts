import { BaseService } from "./BaseService";
import type { ISearchParameters } from "@/domain/ISearchParameters";
import httpClient from "@/http-client";
import type { IRoomType } from "@/domain/IRoomType";
import type { IServiceResult } from "@/domain/IServiceResult";

const apiVersion = "v1";

export class RoomTypeService extends BaseService<IRoomType> {
  constructor() {
    super("roomtype", apiVersion);
  }

  async searchRooms(
    searchParameters: ISearchParameters
  ): Promise<IServiceResult<IRoomType[] | void>> {
    let response;
    try {
      response = await httpClient.get(
        `/${this.apiVersion}/${this.path}/${searchParameters.hotelId}/search/startdate=${searchParameters.startDate}&enddate=${searchParameters.endDate}&guests=${searchParameters.numberOfGuests}`,
        {
          headers: {
            Authorization: "bearer " + this.identityStore.$state.jwt?.token,
          },
        }
      );
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status, data: response.data as IRoomType[] };
  }
}
