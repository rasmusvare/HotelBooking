import type { IAmenity } from './IAmenity';
import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IRoomType extends IBaseEntity{
  hotelId: string;
  name: string;
  description: string;
  count?: number;
  pricePerNight: number;
  totalPrice?: number;
  numberOfBeds: number;
  amenities?: IAmenity[];
}