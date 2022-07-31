import { createRouter, createWebHistory } from "vue-router";
import Identity from "../views/Identity.vue";
import Login from "@/components/identity/Login.vue";
import Register from "@/components/identity/Register.vue";
import Home from "@/views/Home.vue";
import AdminIndex from "@/views/admin/AdminIndex.vue";
import RoomIndex from "@/views/admin/RoomIndex.vue";
import RoomEdit from "@/views/admin/RoomEdit.vue";

import BookingIndex from "@/views/admin/BookingIndex.vue";
import SearchRoom from "@/components/SearchRoom.vue";
import HotelEdit from "@/views/admin/HotelEdit.vue";
import BookingCreate from "@/views/BookingCreate.vue";
import BookingAdded from "@/views/BookingAdded.vue";

import BookingEdit from "@/views/admin/BookingEdit.vue";
import BookingSearch from "@/views/BookingSearch.vue";
import BookingEditClient from "@/views/BookingEditClient.vue";


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/", name: "home", component: Home},
        { path: "/identity", name: "identity", component: Identity,
      children: [
        { path: "/register", name: "register", component: Register },
        { path: "/login", name: "login", component: Login },
      ]},
        { path: "/search", name: "searchrooms", component: SearchRoom, props: true},
        { path: "/booking", name: "bookingcreate", component: BookingCreate, props: true},
        { path: "/bookingadded", name: "bookingadded", component: BookingAdded, props: true},
        { path: "/bookingsearch", name: "bookingsearch", component: BookingSearch, props: true},
        { path: "/bookingedit/:bookingId", name: "bookingeditclient", component: BookingEditClient, props: true},
    {
      path: "/admin", name: "admin", component: AdminIndex,
      children: [
        { path: ":hotelId/rooms", name: "roomindex", component: RoomIndex, props: true },
        { path: ':hotelId/details', name: 'hoteledit', component: HotelEdit, props: true },
        { path: ':hotelId/room/:roomId', name: 'roomedit', component: RoomEdit, props: true },
        { path: ":hotelId/bookings", name: "hotelbookings", component: BookingIndex, props: true },
        { path: ':hotelId/bookings/:bookingId', name: 'bookingedit', component: BookingEdit, props: true },
      ]
    }
  ]
});

export default router;
