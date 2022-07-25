import { BaseService } from "./BaseService";
import type { IRoomType } from "@/domain/IRoomType";
import type { IBooking } from "@/domain/IBooking";

const apiVersion = "v1";

export class BookingService extends BaseService<IBooking> {
  constructor() {
    super("booking", apiVersion);
  }
}
