<script lang="ts">
import type { IBooking } from "@/domain/IBooking";
import type { IGuest } from "@/domain/IGuest";
import type { IRoomType } from "@/domain/IRoomType";
import { BookingService } from "@/services/BookingService";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useHotelStore } from "@/stores/Hotels";
import { useSearchResultsStore } from "@/stores/SearchResults";

import { Options, Vue } from "vue-class-component";
import GuestsList from "../components/GuestsList.vue";

@Options({
  // props: (route: any) => ({
  //   hotelId: String,
  //   ...route.params,
  // }),
  props: {
    // hotelId: String,
    roomTypez: String,
    // dateFrom: Date,
    // DateTo: Date,
    // roomType2: ["room"]
  },

  components: {
    GuestsList,
  },
})
export default class BookingCreate extends Vue {
  searchStore = useSearchResultsStore();
  hotelStore = useHotelStore();
  roomTypez!: string;

  hotelId!: string;
  roomTypeId!: string;
  roomTypeService = new RoomTypeService();
  roomType: IRoomType | null = null;

  dateFrom!: Date;
  dateTo!: Date;

  nightCount = 0;

  errorMessage: Array<string> | undefined = [];

  guests: IGuest[] = [];

  disabled: boolean = true;

  bookingData: IBooking = {
    hotelId: "",
    roomTypeId: "",
    dateFrom: new Date(),
    dateTo: new Date(),
    noOfGuests: 1,
    guests: [],
  };

  bookingHolder: IGuest = {
    firstName: "",
    lastName: "",
    idCode: "",
    email: "",
    phoneNumber: "",
    isBookingOwner: true,
  };

  bookingService = new BookingService();

  mounted() {
    console.log(this.roomTypez);
    console.log(JSON.parse(this.roomTypez));
    this.roomType = JSON.parse(this.roomTypez) as IRoomType;

    this.bookingData.dateFrom = this.searchStore.startdate;
    this.bookingData.dateTo = this.searchStore.enddate;

    this.nightCount = this.calculateDuration(
      this.bookingData.dateFrom,
      this.bookingData.dateTo
    );
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

    return Math.floor(
      (parseInt(dateToUTC.toString()) - parseInt(dateFromUTC.toString())) /
        millisecondsPerDay
    );
  }

  async handleSaveBooking() {
    this.validateForm();
    if (this.errorMessage?.length == 0) {
      this.guests.push(this.bookingHolder);
      this.bookingData.hotelId = this.roomType!.hotelId;
      this.bookingData.roomTypeId = this.roomType!.id!;
      this.bookingData.dateFrom = new Date(
        this.searchStore.$state.startdate
      ).toDateString();
      this.bookingData.dateTo = new Date(
        this.searchStore.$state.enddate
      ).toDateString();
      this.bookingData.guests = this.guests;

      const res = await this.bookingService.add(this.bookingData);
      if (res.status >= 300) {
        this.errorMessage = res.errorMessage;
        console.log(res);
      } else {
        this.$router.push({
          name: "bookingadded",
          params: {
            roomType: this.roomType?.name,
            price: this.roomType?.totalPrice,
            bookingHolderEmail: this.bookingHolder.email,
            numberOfNights: this.nightCount,
          },
        });
      }
    }
  }

  validateForm() {
    this.errorMessage = [];
    if (this.searchStore.numberOfGuests - 1 != this.guests.length) {
      this.errorMessage.push(
        `Please add ${
          this.searchStore.numberOfGuests - 1
        } more guest(s) to the booking`
      );
    }
    if (this.bookingHolder.firstName.length == 0) {
      this.errorMessage.push("Please enter first name");
    }
    if (this.bookingHolder.lastName.length == 0) {
      this.errorMessage.push("Please enter last name");
    }
    if (this.bookingHolder.idCode.length == 0) {
      this.errorMessage.push("Please enter ID number");
    }
    if (!this.validatePhone()) {
      this.errorMessage.push(
        "Please enter a valid phone number (min 6 numbers)"
      );
    }
    if (!this.validateEmail()) {
      this.errorMessage?.push("Please enter a valid email");
    }
  }

  validateEmail() {
    var re =
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(this.bookingHolder.email);
  }

  validatePhone() {
    var re = /^\d{6,}$/;
    return re.test(this.bookingHolder.phoneNumber);
  }
}
</script>
<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <h3 class="display-7 fw-bold">
        Create a new booking for {{ roomType?.name }}
      </h3>
      <p>Dates: {{ searchStore.startdate }} - {{ searchStore.enddate }}</p>
      <p>
        Total price {{ roomType?.totalPrice }}€ (for {{ nightCount }} nights)
      </p>
      <div
        class="text-danger validation-summary-errors"
        data-valmsg-summary="true"
        data-valmsg-replace="true"
      >
        <ul v-for="error of errorMessage">
          <li>{{ error }}</li>
        </ul>
      </div>
      <div class="col-lg-6 mx-auto mt-5">
        <h4>Contact Information</h4>
        <div class="mt-4">
          <div class="form-floating" v-if="bookingHolder != undefined">
            <input
              v-model="bookingHolder.firstName"
              type="text"
              class="form-control form-control-top"
            />
            <label for="floatingInput">First Name</label>
          </div>
          <div class="form-floating">
            <input
              v-model="bookingHolder.lastName"
              type="text"
              class="form-control form-control-middle"
            />
            <label for="floatingInput">Last Name</label>
          </div>
          <div class="form-floating">
            <input
              v-model="bookingHolder.idCode"
              type="text"
              class="form-control form-control-middle"
            />
            <label for="floatingInput">National identification number</label>
          </div>
          <div class="form-floating">
            <input
              v-model="bookingHolder.email"
              type="text"
              class="form-control form-control-middle"
            />
            <label for="floatingInput">Email</label>
          </div>
          <div class="form-floating">
            <input
              v-model="bookingHolder.phoneNumber"
              type="text"
              class="form-control form-control-bottom"
            />
            <label for="floatingInput">Phone Number</label>
          </div>
        </div>
      </div>
      <div class="col-lg-10 mx-auto mt-5">
        <GuestsList :guests="guests" />
      </div>
      <div class="d-flex justify-content-center">
        <button
          class="w-10 btn btn-lg btn-primary"
          @click="handleSaveBooking()"
        >
          Confirm booking
        </button>
      </div>
    </div>
  </div>
</template>
