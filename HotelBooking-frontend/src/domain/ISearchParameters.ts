import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface ISearchParameters extends IBaseEntity {
  hotelId?: string;
  startDate: string;
  endDate: string;
  numberOfGuests: number;
}
