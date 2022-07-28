<script lang="ts">
import { useHotelStore } from "@/stores/Hotels";
import { useAmenityStore } from "@/stores/Amenities";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { Options, Vue } from "vue-class-component";

import HotelAmenitiesEdit from "@/components/hotel/HotelAmenitiesEdit.vue";
import HotelRoomTypesEdit from "@/components/hotel/HotelRoomTypesEdit.vue";
import HotelEditForm from "@/components/hotel/HotelEditForm.vue";

import { HotelService } from "@/services/HotelService";
import { RoomTypeService } from "@/services/RoomTypeService";
import type { IHotel } from "@/domain/IHotel";
import { AmenityService } from "@/services/AmenityService";

@Options({
  components: {
    HotelEditForm,
    HotelAmenitiesEdit,
    HotelRoomTypesEdit,
  },
  props: {
    hotelId: String,
  },
})
export default class HotelEdit extends Vue {
  hotelStore = useHotelStore();
  hotelService = new HotelService();

  amenityStore = useAmenityStore();
  amenityService = new AmenityService();

  roomTypeStore = useRoomTypeStore();
  roomTypeService = new RoomTypeService();

  hotelId!: string;

  hotel: IHotel = {
    name: "",
    description: "",
    email: "",
    telephoneNumber: "",
    address: "",
  };

  async mounted() {
    const hotels = await this.hotelService.getAll();
    this.hotelStore.$state.data = hotels;

    if (hotels?.length != 0) {
      this.hotel = hotels[0];
    }

    const amenities = await this.amenityService.getAll(this.hotelId);
    this.amenityStore.$state.data = amenities;

    const roomTypes = await this.roomTypeService.getAll(this.hotelId);
    this.roomTypeStore.$state.data = roomTypes;
  }
}
</script>

<template>
  <div class="container" v-if="hotel != null">
    <div>
      <HotelEditForm :hotel="hotel" />
    </div>
    <br />
    <div>
      <HotelAmenitiesEdit :hotelId="hotelId" />
    </div>
    <br />
    <div>
      <HotelRoomTypesEdit :hotelId="hotelId" />
    </div>
  </div>
</template>
