<template>
  <ul class="hotel-ul dropdown-menu text-small shadow" aria-labelledby="dropdownHotel">
    <template v-if="hotelStore.$state.data != null">
      <template v-bind:key="hotel.id" v-for="hotel in hotelStore.$state.data">
        <li>
          <div class="d-flex align-items-center justify-content-between">
            <RouterLink class="dropdown-item px-1" :to="'/admin/' + hotel.id" style="max-width: 70%">
              <p class="hotel-name nav-link link-dark py-0 my-0">{{hotel.name}}</p>
            </RouterLink>
            <RouterLink :to="'/admin/' + hotel.id + '/details'">
              <button class="btn btn-sm btn-primary me-3">Edit</button>
            </RouterLink>
          </div>
        </li>
      </template>
    </template>
    <li><hr class="dropdown-divider"></li>
    <li>
      
        <span class="dropdown-item disabled">Create New..</span>
 
    </li>
  </ul>
</template>

<script lang="ts">
import { useHotelStore } from "@/stores/Hotels";
import { Vue } from "vue-class-component";

export default class HotelDropdown extends Vue {
  hotelStore = useHotelStore();
}

</script>

<style>
.hotel-ul > li {
  max-width: 200px;
}

p.hotel-name {
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}
</style>