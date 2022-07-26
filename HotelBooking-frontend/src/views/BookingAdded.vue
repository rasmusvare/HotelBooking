<script lang="ts">
import { useSearchResultsStore } from "@/stores/SearchResults";
import { Options, Vue } from "vue-class-component";

@Options({
  props: {
    roomType: String,
    price: Number,
    bookingHolderEmail: String,
    numberOfNights: Number,
  },
})
export default class BookingCreate extends Vue {
  searchStore = useSearchResultsStore();

  roomType!: string;
  price!: number;
  bookingHolderEmail!: string;
  numberOfNights!: number;
}
</script>

<template>
  <div class="container">
    <div class="px-4 py-5 my-5 text-center">
      <h3 class="display-7 fw-bold">New booking added for {{ roomType }}</h3>
      <p>Dates: {{ searchStore.startdate }} - {{ searchStore.enddate }}</p>
      <p>Total price {{ price }}€ (for {{ numberOfNights }} nights)</p>
      <p>
        You can search and edit your booking by using the email
        {{ bookingHolderEmail }}
      </p>
      <p>
        <RouterLink to="/"> Home </RouterLink>
      </p>
    </div>
  </div>
</template>
