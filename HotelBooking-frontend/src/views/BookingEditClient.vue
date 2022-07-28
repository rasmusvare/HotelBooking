<script lang="ts">
import BookingEditFormClient from "@/components/booking/BookingEditFormClient.vue";
import type { IBooking } from "@/domain/IBooking";
import type { IRoomType } from "@/domain/IRoomType";
import { BookingService } from "@/services/BookingService";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useRoomTypeStore } from "@/stores/RoomTypes";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    bookingId: String,
  },
  components: {
    BookingEditFormClient,
  },
})
export default class BookingEdit extends Vue {
  roomTypeStore = useRoomTypeStore();
  roomTypeService = new RoomTypeService();

  bookingService = new BookingService();

  bookingId!: string;

  numberOfNights = 0;
  editDisabled = false;

  bookingData: IBooking = {
    hotelId: "",
    roomTypeId: "",
    dateFrom: "",
    dateTo: "",
    noOfGuests: 0,
    guests: [],
  };

  roomType: IRoomType = {
    hotelId: "",
    name: "",
    description: "",
    pricePerNight: 0,
    totalPrice: 0,
    numberOfBeds: 0,
    amenities: [],
  };

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    await this.bookingService
      .get(this.bookingId)
      .then((res) => (this.bookingData = res!));

    this.numberOfNights = this.calculateDuration(
      this.bookingData.dateFrom.toString(),
      this.bookingData.dateTo.toString()
    );

    if (this.roomTypeStore.$state.data.length == 0) {
      const res = await this.roomTypeService.getAll(this.bookingData.hotelId);
      this.roomTypeStore.$state.data = res;
    }

    this.roomType = this.roomTypeStore.getRoomTypeById(
      this.bookingData.roomTypeId
    )!;


  }

  async handleEditBooking() {
    const res = await this.bookingService.update(this.bookingData);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.$router.push({ name: "home" });
    }
  }

  async handleDeleteBooking() {
    const res = await this.bookingService.remove(this.bookingData.id!);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.$router.push({ name: "home" });
    }
  }

  calculateDuration(from: string, to: string) {
    const millisecondsPerDay = 1000 * 60 * 60 * 24;

    const dateFrom = new Date(from);
    const dateTo = new Date(to);

    const dateFromUTC = Date.UTC(
      dateFrom.getFullYear(),
      dateFrom.getMonth(),
      dateFrom.getDate()
    );
    const dateToUTC = Date.UTC(
      dateTo.getFullYear(),
      dateTo.getMonth(),
      dateTo.getDate()
    );

    if (this.addDays(new Date(), 3) >= dateFrom){

console.log("KUUPÄEV SUUREM KUI 3");
   this.editDisabled = true;

    } else{
        console.log("VÄIKSEM");
        
    }

    return Math.floor(
      (parseInt(dateToUTC.toString()) - parseInt(dateFromUTC.toString())) /
        millisecondsPerDay
    );
  }

addDays(date: Date, days: number) {
  var result = new Date(date);
  result.setDate(result.getDate() + days);
  console.log(result);
  
  return result;
}
}
</script>

<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <div>
        <h3 class="mt-2">Edit booking for {{ roomType.name }}</h3>
        <p>
          Total price {{ bookingData.totalPrice }}€ (for
          {{ numberOfNights }} nights)
        </p>
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
    </div>

    <div class="d-flex">
      <BookingEditFormClient :bookingData="bookingData" :editDisabled="editDisabled"/>
    </div>
    <div class="d-flex">
      <button 
      :disabled="editDisabled"
      class="btn btn-primary w-50" @click="handleEditBooking()">
        Save changes
      </button>
      <button 
            :disabled="editDisabled"
      class="btn btn-danger ms-3 w-50" @click="handleDeleteBooking()">
        Cancel booking
      </button>
    </div>
  </div>
</template>
