<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IBooking } from "@/domain/IBooking";
import { BookingService } from "@/services/BookingService";
import { useRoomTypeStore } from "@/stores/RoomTypes";

@Options({
  props: {
    bookings: Object as unknown as IBooking[],
  },
  components: {},
})
export default class BookingsList extends Vue {
  bookings!: IBooking[];
  bookingService = new BookingService();

  roomTypeStore = useRoomTypeStore();
}
</script>

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
          <td>
            {{
              `${booking.bookingHolder?.firstName} ${booking.bookingHolder?.lastName}`
            }}
          </td>
          <td>
            {{ roomTypeStore.getRoomTypeById(booking.roomTypeId!)?.name }}
          </td>
          <td>{{ booking.guests.length }}</td>
          <td>{{ new Date(booking.dateFrom!).toDateString() }}</td>
          <td>{{ new Date(booking.dateTo!).toDateString() }}</td>
          <td style="width: 10%">
            <RouterLink
              :to="{ name: 'bookingedit', params: { bookingId: booking.id } }"
            >
              <button class="btn btn-primary ms-3 btn-sm">Manage</button>
            </RouterLink>
          </td>
        </tr>
      </template>
    </tbody>
  </table>
</template>
