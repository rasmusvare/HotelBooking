import { defineStore } from "pinia";
import type { IRoomType } from "@/domain/IRoomType";

export const useSearchResultsStore = defineStore({
  id: "searchresults",
  state: () => ({
    data: [] as IRoomType[] | undefined,
    startdate: "",
    enddate: "",
    numberOfGuests: 1,
  }),
  getters: {},
  actions: {},
});
