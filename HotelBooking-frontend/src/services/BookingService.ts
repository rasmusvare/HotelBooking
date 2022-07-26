import { BaseService } from "./BaseService";
import type { IBooking } from "@/domain/IBooking";
import type { IServiceResult } from "@/domain/IServiceResult";
import httpClient from "@/http-client";

const apiVersion = "v1";

export class BookingService extends BaseService<IBooking> {
  constructor() {
    super("booking", apiVersion);
  }

  async searchByEmail(
    email: string
  ): Promise<IServiceResult<IBooking[] | void>> {
    let response;
    try {
      response = await httpClient.get(
        `/${this.apiVersion}/${this.path}/search/${email}`
      );
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status, data: response.data as IBooking[] };
  }
}
