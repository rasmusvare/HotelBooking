<script lang="ts">
import type { IGuest } from "@/domain/IGuest";
import { useSearchResultsStore } from "@/stores/SearchResults";
import { Options, Vue } from "vue-class-component";
import GuestAddModal from "./GuestAddModal.vue";
import GuestEditModal from "./GuestEditModal.vue";

@Options({
  props: {
    guests: Object as unknown as IGuest[],
  },
  components: {
    GuestAddModal,
    GuestEditModal,
  },
})
export default class GuestsList extends Vue {
  guests!: IGuest[];

  searchResultStore = useSearchResultsStore();

  async handleOpenGuestModal() {}
}
</script>

<template>
  <div class="container mt-5">
    <div
      class="d-flex justify-content-between"
      v-if="searchResultStore.numberOfGuests > 1"
    >
      <h3>Guests</h3>
      <div>
        <button
          v-if="guests.length < searchResultStore.numberOfGuests - 1"
          class="btn btn-primary btn-sm"
          data-bs-toggle="modal"
          :data-bs-target="'#addGuestModal'"
          @click="handleOpenGuestModal()"
        >
          Create New
        </button>
        <GuestAddModal :guests="guests" />
      </div>
    </div>
    <div v-if="guests.length == 0 && searchResultStore.numberOfGuests > 1">
      <h5 class="mt-4">
        Please add {{ searchResultStore.numberOfGuests - 1 }} guest(s) to the
        booking
      </h5>
    </div>
    <table v-if="guests.length > 0" class="table">
      <thead>
        <tr>
          <th scope="col">First name</th>
          <th scope="col">Last name</th>
          <th scope="col">Id Code</th>
          <th scope="col">Email</th>
          <th scope="col">Telephone</th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="each in guests">
          <tr>
            <td>
              <span>{{ each.firstName }}</span>
            </td>
            <td>
              <span>{{ each.lastName }}</span>
            </td>
            <td>
              <span> {{ each.idCode }}</span>
            </td>
            <td>
              <span> {{ each.email }}</span>
            </td>
            <td>
              <span> {{ each.phoneNumber }}</span>
            </td>
            <td>
              <button
                class="btn btn-primary btn-sm"
                data-bs-toggle="modal"
                :data-bs-target="'#editGuestModal-' + each.id"
              >
                Edit
              </button>
              <GuestEditModal :guests="guests" :guest="each" />
            </td>
            <!-- <td style="width: 10%">
              <RouterLink 
              :to="{ name: 'bookingcreate', params: { test: 'asdasdasdsasad', roomTypeId:each.id } }"
>
              <button class="btn btn-primary btn-sm ms-4">Book</button>
              </RouterLink>
            </td> -->
          </tr>
        </template>
      </tbody>
    </table>
  </div>
  <!-- </div> -->
</template>
