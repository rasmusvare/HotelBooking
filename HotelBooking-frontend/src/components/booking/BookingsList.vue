<template>
  <table class="table">
    <thead>
    <tr>
      <th scope="col">Client</th>
      <th scope="col">Room Type</th>
      <th scope="col">Number of guests</th>
      <th scope="col">Start</th>
      <th scope="col">End</th>
      <th scope="col"></th>
    </tr>
    </thead>
    <tbody>
    <template v-bind:key="booking.id" v-for="booking in bookings">
      <tr>
        <td></td>
        <!-- <td>{{`${reservation.client.firstName} ${reservation.client.lastName}`}}</td> -->
        <td>{{ roomTypeStore.getRoomTypeById(booking.roomTypeId!)?.name}}</td>
        <td>{{booking.guests.length}}</td>
        <td>{{new Date(booking.dateFrom!).toDateString()}}</td>
        <td>{{new Date(booking.dateTo!).toDateString()}}</td>
        <td style="width: 10%">
        {{booking.id}}
        <RouterLink :to="{ name: 'bookingedit', params: { bookingId: booking.id }}">
                        <button class="btn btn-primary ms-3 btn-sm">Manage</button>
                    </RouterLink>
                    </td>
                    <!-- </tr> -->
        <!-- <td style="width:25%">
          <button class="btn btn-primary btn-sm" data-bs-toggle="modal" :data-bs-target="'#create-stay-modal'">
            Create Stay -->
          <!-- </button> -->
          <!-- <StayCreateModal :client="booking.client"/> -->
          <!-- <RouterLink :to="{ name: 'reservationedit', params: { reservationId: reservation.id }}"> -->
            <!-- <button class="btn btn-primary ms-3 btn-sm">Manage</button> -->
          <!-- </RouterLink> -->
        <!-- </td> -->
      </tr>
    </template>
    </tbody>
  </table>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
// import StayCreateModal from "./StayCreateModal.vue";
import type { IBooking } from "@/domain/IBooking";
import { BookingService } from "@/services/BookingService";
import { useHotelStore } from "@/stores/Hotels";
import { useRoomTypeStore } from "@/stores/RoomTypes";

@Options({
  props: {
    bookings: Object as unknown as IBooking[],
  },
  components: {
    // StayCreateModal
  }
})
export default class BookingsList extends Vue {
  bookings!: IBooking[]
  bookingService = new BookingService();

  hotelStore = useHotelStore();
  roomTypeStore = useRoomTypeStore()

  async mounted(){

    // const res = await this.bookingService.getAll(this.hotelStore.$state.current?.id)
    // this.bookings = res;
  }
}
</script>

<style>

</style>