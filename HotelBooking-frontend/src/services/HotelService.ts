import { BaseService } from "./BaseService";
import type { IHotel } from "@/domain/IHotel";

const apiVersion = "v1";

export class HotelService extends BaseService<IHotel> {
  constructor() {
    super("hotel", apiVersion);
  }
}
