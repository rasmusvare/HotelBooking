import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IRoom extends IBaseEntity{
  hotelId?: string,
  roomNumber: string,
  roomTypeId: string,
  roomStatus: string,
}