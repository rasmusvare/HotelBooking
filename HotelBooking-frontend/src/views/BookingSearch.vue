<script lang="ts">
import BookingSearchList from "@/components/booking/BookingSearchList.vue";
import type { IBooking } from "@/domain/IBooking";
import { BookingService } from "@/services/BookingService";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useHotelStore } from "@/stores/Hotels";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { useSearchResultsStore } from "@/stores/SearchResults";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    hotelId: String,
  },
  components: {
    BookingSearchList,
  },
})
export default class BookingSearch extends Vue {
  searchStore = useSearchResultsStore();

  bookingService = new BookingService();

  hotelStore = useHotelStore();

  roomTypeStore = useRoomTypeStore();
  roomTypeService = new RoomTypeService();

  hotelId!: string;

  email: string = "";
  bookings: IBooking[] = [];

  async mounted() {
    const res = await this.roomTypeService.getAll(
      this.hotelStore.$state.current?.id
    );
    this.roomTypeStore.$state.data = res;
  }

  async handleSearch() {
    const res = await this.bookingService.searchByEmail(this.email);
    this.bookings = res.data as IBooking[];
  }
}
</script>

<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <div>
        <RouterLink to="/">Home</RouterLink>
        |
        <RouterLink to="/admin">Admin</RouterLink>
      </div>
      <p class="lead mb-4 mt-4">Search for bookings</p>

      <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
        <div class="form-floating">
          <input v-model="email" type="text" class="form-control" />
          <label for="floatingInput">Email</label>
        </div>
        <div class="d-flex justify-content-center">
          <button class="w-10 btn btn-lg btn-primary" @click="handleSearch()">
            Search bookings
          </button>
        </div>
      </div>
    </div>

    <hr v-if="bookings.length > 0" />
    <BookingSearchList v-if="bookings.length > 0" :bookings="bookings" />
  </div>
</template>
