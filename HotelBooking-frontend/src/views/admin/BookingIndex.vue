

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import BookingsList from "@/components/booking/BookingsList.vue";
import type { IBooking } from "@/domain/IBooking";
import { BookingService } from "@/services/BookingService";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { RoomTypeService } from "@/services/RoomTypeService";

@Options({
  props: {
    hotelId: String,
  },
  components: {
    BookingsList,
  },
  provide() {
    return {hotelId: this.hotelId}
  }
})
export default class BookingIndex extends Vue{
  hotelId!: string;
  bookings: IBooking[] = [];

  roomTypeStore = useRoomTypeStore();
  roomTypeService = new RoomTypeService();

  bookingService = new BookingService();

  async mounted() {
    const res = await this.bookingService.getAll(this.hotelId)
    this.bookings = res;

    const roomTypes = await this.roomTypeService.getAll(this.hotelId)
    this.roomTypeStore.$state.data = roomTypes;
  }
}
</script>

<template>
  <div class="container">
    <div class="d-flex justify-content-between">
      <span class="fs-4 link-dark">Bookings</span>
      <button type="button" class="btn btn-primary btn-sm">
        Create New TODO!!
      </button>
    </div>
    <hr/>
    <BookingsList :bookings="bookings"/>
  </div>
</template>
