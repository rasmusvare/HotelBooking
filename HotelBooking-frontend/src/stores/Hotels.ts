import { defineStore } from 'pinia'
import type { IHotel } from "@/domain/IHotel";


export const useHotelStore = defineStore({
  id: 'hotelstore',

  state: () => ({
    data: [] as IHotel[],
    current: null as IHotel | null
  }),

  getters: {
    getHotelById: (state) => {
      return (hotelId: string) => state.data.find((hotel) => hotel.id === hotelId)
    },
  },

  actions: {
    addHotel(hotel: IHotel) {
      this.data.push(hotel);
    },

    updateHotel(id: string, hotel: IHotel) {
      const res = this.data[this.data.findIndex(e => e.id == id)];
      if (res != null) {
        res.name = hotel.name;
        res.description = hotel.description;
      }
    },

    deleteHotel(id: string) {
      this.data = this.data.filter(e => e.id !== id)
    }
  }
})
