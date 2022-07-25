import { defineStore } from 'pinia'
import type { IAmenity } from '@/domain/IAmenity';

export const useAmenityStore = defineStore({
    id: 'amenitystore',

    state: () => ({
        data: [] as IAmenity[]
    }),

    getters: {
        getAmenityById: (state) => {
            return (amenityId: string) => state.data.find((amenity) => amenity.id === amenityId)
        },
    },

    actions: {
        addAmenity(amenity: IAmenity) {
            this.data.push(amenity);
        },

        updateAmenity(id: string, amenity: IAmenity) { 
            let res = this.data[this.data.findIndex(e => e.id == id)];
            if (res != null) {
                res.hotelId = amenity.hotelId;
                res.name = amenity.name;
                res.description = amenity.description;
            }
        },

        deleteAmenity(id: string) {
            this.data = this.data.filter(e => e.id !== id)
        }
    }
})
