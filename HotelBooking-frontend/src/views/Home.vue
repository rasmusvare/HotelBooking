<script lang="ts">
import { useIdentityStore } from "@/stores/identity";
import { Options, Vue } from "vue-class-component";
import SearchRoom from "@/components/SearchRoom.vue";
import { RoomTypeService } from "@/services/RoomTypeService";
import { useHotelStore } from "@/stores/Hotels";
import { HotelService } from "@/services/HotelService";
import type { IRoomType } from "@/domain/IRoomType";
import SearchResults from "@/components/SearchResults.vue";
import { useSearchResultsStore } from "@/stores/SearchResults";

@Options({
  props: {},
  components: {
    SearchRoom,
    SearchResults,
  },
})
export default class Home extends Vue {
  searchResultsStore = useSearchResultsStore();

  identityStore = useIdentityStore();
  roomtype = new RoomTypeService();
  hotelStore = useHotelStore();
  hotelService = new HotelService();

  hotelId: string | undefined = "";
  searchResults: IRoomType[] | undefined = undefined;

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    const hotels = await this.hotelService.getAll();
    this.hotelStore.$state.data = hotels;
    this.hotelStore.$state.current = hotels[0];
    this.hotelId = hotels[0].id;

    this.searchResultsStore.$state.data = undefined;
  }
}
</script>

<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <div>
        <RouterLink to="/bookingsearch">Search bookings</RouterLink>
        |
        <RouterLink to="/admin">Admin</RouterLink>
      </div>
      <h1 class="display-5 fw-bold">
        Welcome to {{ hotelStore.current?.name }}!
      </h1>
      <p>{{ hotelStore.current?.description }}</p>
      <p>
        {{ hotelStore.current?.address }} | {{ hotelStore.current?.email }} |
        {{ hotelStore.current?.telephoneNumber }}
      </p>
      <div class="col-lg-10 mx-auto mt-5">
        <SearchRoom :hotelId="hotelId"/>
      </div>
      <div class="col-lg-10 mx-auto">
        <SearchResults />
      </div>
    </div>
  </div>
</template>

<style>
.form-identity {
  width: 100%;
  max-width: 330px;
  padding: 15px;
  margin: auto;
}
</style>
