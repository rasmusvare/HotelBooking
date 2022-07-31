<script lang="ts">
import { useSearchResultsStore } from "@/stores/SearchResults";
import { Options, Vue } from "vue-class-component";
import { Tooltip } from "bootstrap";

@Options({
  props: {},
})
export default class SearchResults extends Vue {
  searchResultsStore = useSearchResultsStore();

  tooltipTriggerList!: NodeListOf<Element>;

  updated() {
    this.tooltipTriggerList = document.querySelectorAll(
      '[data-bs-toggle="tooltip"]'
    );
    const tooltipList = [...this.tooltipTriggerList].map(
      (tooltipTriggerEl) => new Tooltip(tooltipTriggerEl)
    );
  }
}
</script>

<template>
  <div v-if="searchResultsStore.data != undefined" class="container mt-5">
    <div class="d-flex justify-content-center">
      <h3>Search results</h3>
    </div>
    <div v-if="searchResultsStore.data.length == 0">
      <h5 class="mt-4">
        Sorry, no available rooms found for the search criteria.
      </h5>
    </div>
    <table v-if="searchResultsStore.data.length > 0" class="table">
      <thead>
        <tr>
          <th scope="col">Room type</th>
          <th scope="col">Amenities</th>
          <th scope="col">Price</th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="each in searchResultsStore.data">
          <tr>
            <td>
              <span>{{ each.name }}</span>
            </td>
            <td>
              <template v-for="(amenity, index) in each.amenities" :key="index">
                <span
                  ><a
                    href="#"
                    data-bs-toggle="tooltip"
                    :data-bs-title="amenity.description"
                    >{{ amenity.name }}</a
                  ></span
                ><span v-if="index < each.amenities!.length - 1">, </span>
              </template>
            </td>
            <td>
              <span> {{ each.totalPrice }}€</span>
            </td>
            <td style="width: 10%">
              <RouterLink
                :to="{
                  name: 'bookingcreate',
                  params: {
                    hotelId: each.hotelId,
                    roomTypeJSON: JSON.stringify(each),
                  },
                }"
              >
                <button class="btn btn-primary btn-sm ms-4">Book</button>
              </RouterLink>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>
