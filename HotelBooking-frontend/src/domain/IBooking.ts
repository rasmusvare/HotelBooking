import type { IBaseEntity } from "@/domain/IBaseEntity";
import type { IGuest } from "@/domain/IGuest";

export interface IBooking extends IBaseEntity {
  hotelId: string;
  roomTypeId: string;
  dateFrom: Date | string;
  dateTo: Date | string;
  noOfGuests: number;
  totalPrice?: number,
  guests: IGuest[];
  bookingHolder?: IGuest;
}
