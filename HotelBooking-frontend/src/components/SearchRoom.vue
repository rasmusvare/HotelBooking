<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IRoomType } from "@/domain/IRoomType";
import type { ISearchParameters } from "@/domain/ISearchParameters";
import { RoomTypeService } from "@/services/RoomTypeService";
import { HotelService } from "@/services/HotelService";
import { useHotelStore } from "@/stores/Hotels";
import { useSearchResultsStore } from "@/stores/SearchResults";

@Options({
  props: {
    hotelId: String,
  },
})
export default class SearchRoom extends Vue {
  searchResultsStore = useSearchResultsStore();
  roomTypeService = new RoomTypeService();
  hotelStore = useHotelStore();
  hotelService = new HotelService();

  hotelId: string | undefined = "";
  searchResults!: IRoomType[] | undefined;

  errorMessage: Array<string> | undefined = [];

  searchData: ISearchParameters = {
    hotelId: "",
    startDate: "",
    endDate: "",
    numberOfGuests: 1,
  };

  roomTypes: IRoomType[] = [];

  maxGuests = 3;

  async handleSearch() {
    if (this.searchData.startDate === "" || this.searchData.endDate === "") {
      this.errorMessage = [];
      this.errorMessage?.push("Please select start and end date");
    } else {
      this.searchData.hotelId = this.hotelStore.$state.current?.id;
      const res = await this.roomTypeService.searchRooms(this.searchData);

      if (res.status >= 300) {
        this.errorMessage = res.errorMessage;
        console.log(res);
      } else {
        this.errorMessage = [];
        this.searchResultsStore.$state.data = res.data!;
        this.searchResultsStore.$state.startdate = this.searchData.startDate;
        this.searchResultsStore.$state.enddate = this.searchData.endDate;
        this.searchResultsStore.$state.numberOfGuests =
          this.searchData.numberOfGuests;
      }
    }
  }
}
</script>

<template>
  <div class="container">
    <p class="lead mb-4">Search for available rooms</p>
    <div
      class="text-danger validation-summary-errors"
      data-valmsg-summary="true"
      data-valmsg-replace="true"
    >
      <ul v-for="error of errorMessage">
        <li>{{ error }}</li>
      </ul>
    </div>
    <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
      <div class="form-floating">
        <input
          v-model="searchData.startDate"
          type="date"
          class="form-control"
        />
        <label for="floatingInput">Start date</label>
      </div>
      <div class="form-floating">
        <input v-model="searchData.endDate" type="date" class="form-control" />
        <label for="floatingInput">End date</label>
      </div>
      <div class="form-floating">
        <select v-model="searchData.numberOfGuests" class="form-select">
          <option disabled value="">Number of guests</option>
          <template v-bind:key="each" v-for="each in maxGuests">
            <option :value="each">{{ each }}</option>
          </template>
        </select>
        <label for="floatingInput">Guests</label>
      </div>
      <button class="btn btn-lg btn-primary" @click="handleSearch()">
        Search
      </button>
    </div>
  </div>
</template>
