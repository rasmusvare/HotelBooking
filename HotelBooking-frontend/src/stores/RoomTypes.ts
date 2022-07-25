import type { IRoomType } from "./../domain/IRoomType";
import { defineStore } from "pinia";

export const useRoomTypeStore = defineStore({
  id: "roomtypestore",

  state: () => ({
    data: [] as IRoomType[],
  }),

  getters: {
    getRoomTypeById: (state) => {
      return (roomTypeId: string) =>
        state.data.find((roomType) => roomType.id === roomTypeId);
    },
  },

  actions: {
    addRoomType(roomType: IRoomType) {
      this.data.push(roomType);
    },

    updateRoomType(id: string, roomType: IRoomType) {
      const res = this.data[this.data.findIndex((e) => e.id == id)];
      if (res != null) {
        res.hotelId = roomType.hotelId;
        res.name = roomType.name;
        res.description = roomType.description;
        res.pricePerNight = roomType.pricePerNight
        res.numberOfBeds = roomType.numberOfBeds
        res.count = roomType.count;
      }
    },

    deleteRoomType(id: string) {
      this.data = this.data.filter((e) => e.id !== id);
    },
  },
});
