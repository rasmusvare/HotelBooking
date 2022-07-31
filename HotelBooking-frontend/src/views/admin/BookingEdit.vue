<script lang="ts">
import BookingEditForm from "@/components/booking/BookingEditForm.vue";
import type { IBooking } from "@/domain/IBooking";
import type { IRoomType } from "@/domain/IRoomType";
import { BookingService } from "@/services/BookingService";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    bookingId: String,
  },
  components: {
    BookingEditForm,
  },
})
export default class BookingEdit extends Vue {
  bookingService = new BookingService();

  bookingId!: string;

  bookingData: IBooking = {
    hotelId: "",
    roomTypeId: "",
    dateFrom: "",
    dateTo: "",
    noOfGuests: 0,
    guests: [],
  };

  roomTypes: IRoomType[] = [];

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    await this.bookingService
      .get(this.bookingId)
      .then((res) => (this.bookingData = res!));
    console.log(this.bookingData.guests.length);
  }

  handleSelectRoomType(roomType: IRoomType) {
    this.bookingData!.roomTypeId = roomType.id!;
  }

  async handleEditBooking() {
    const res = await this.bookingService.update(this.bookingData);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.$router.push({
        name: "hotelbookings",
        params: { hotelId: this.bookingData.hotelId },
      });
    }
  }

  async handleDeleteBooking() {
    const res = await this.bookingService.remove(this.bookingData.id!);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.$router.push({
        name: "hotelbookings",
        params: { hotelId: this.bookingData.hotelId },
      });
    }
  }
}
</script>

<template>
  <div class="container">
    <div>
      <BookingEditForm :bookingData="bookingData" />
    </div>
    <div
      class="text-danger validation-summary-errors"
      data-valmsg-summary="true"
      data-valmsg-replace="true"
    >
      <ul v-for="error of errorMessage">
        <li>{{ error }}</li>
      </ul>
    </div>
    <div class="d-flex">
      <button class="btn btn-primary w-50" @click="handleEditBooking()">
        Save changes
      </button>
      <button class="btn btn-danger ms-3 w-50" @click="handleDeleteBooking()">
        Delete booking
      </button>
    </div>
  </div>
</template>
