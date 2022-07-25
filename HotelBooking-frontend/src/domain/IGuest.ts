import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IGuest extends IBaseEntity{
  firstName: string,
  lastName: string,
  idCode: string,
  email: string,
  phoneNumber: string
  isBookingOwner: boolean
}