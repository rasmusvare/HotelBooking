<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IGuest } from "@/domain/IGuest";
import { Modal } from "bootstrap";

@Options({
  props: {
    guests: Object as unknown as IGuest[],
    guest: Object as unknown as IGuest,
  },
})
export default class GuestEditModal extends Vue {
  guests!: IGuest[];
  guest!: IGuest;

  guestFormData: IGuest = {
    id: "",
    firstName: "",
    lastName: "",
    idCode: "",
    email: "",
    phoneNumber: "",
    isBookingOwner: false,
  };

  errorMessage: Array<string> = [];

  editGuestModal!: Modal;

  async mounted() {
    this.editGuestModal = new Modal(
      document.getElementById("editGuestModal-" + this.guest!.id)!
    );
    if (this.guest != undefined) {
      this.guestFormData.id = this.guest.id;
      this.guestFormData.firstName = this.guest.firstName;
      this.guestFormData.lastName = this.guest.lastName;
      this.guestFormData.idCode = this.guest.idCode;
      this.guestFormData.email = this.guest.email;
      this.guestFormData.phoneNumber = this.guest.phoneNumber;
      this.guestFormData.isBookingOwner = this.guest.isBookingOwner;
    }
  }

  handleEditGuest() {
    this.validateForm();
    if (this.errorMessage?.length == 0) {
      var guest = this.guests.find((g) => g.id == this.guestFormData.id)!;
      guest.firstName = this.guestFormData.firstName;
      guest.lastName = this.guestFormData.lastName;
      guest.idCode = this.guestFormData.idCode;
      guest.email = this.guestFormData.email;
      guest.phoneNumber = this.guestFormData.phoneNumber;
      guest.isBookingOwner = this.guestFormData.isBookingOwner;
      this.editGuestModal.hide();
    }
  }

  handleDeleteGuest() {
    var guestIndex = this.guests.indexOf(this.guest);
    this.guests.splice(guestIndex, 1);
    this.editGuestModal.hide();
  }

  validateForm() {
    this.errorMessage = [];
    if (this.guestFormData.firstName.length == 0) {
      this.errorMessage?.push("Please enter first name");
    }
    if (this.guestFormData.lastName.length == 0) {
      this.errorMessage?.push("Please enter last name");
    }
    if (this.guestFormData.idCode.length == 0) {
      this.errorMessage?.push("Please enter ID number");
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
    return re.test(this.guestFormData.email);
  }

  validatePhone() {
    var re = /^\d{6,}$/;
    return re.test(this.guestFormData.phoneNumber);
  }
}
</script>

<template>
  <div
    class="modal fade"
    :id="'editGuestModal-' + guest!.id"
    tabindex="-1"
    aria-labelledby="ModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="ModalLabel">Add guest</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="container">
            <div
              class="text-danger validation-summary-errors"
              data-valmsg-summary="true"
              data-valmsg-replace="true"
            >
              <ul v-for="error of errorMessage">
                <li>{{ error }}</li>
              </ul>
            </div>
            <div class="form-floating">
              <input
                v-model="guestFormData.firstName"
                type="text"
                class="form-control form-control-top"
              />
              <label for="floatingInput">First name</label>
            </div>
            <div class="form-floating">
              <input
                v-model="guestFormData.lastName"
                type="text"
                class="form-control form-control-middle"
              />
              <label for="floatingInput">Last name</label>
            </div>
            <div class="form-floating">
              <input
                v-model="guestFormData.idCode"
                type="text"
                class="form-control form-control-middle"
              />
              <label for="floatingInput">National identification number</label>
            </div>
            <div class="form-floating">
              <input
                v-model="guestFormData.phoneNumber"
                type="text"
                class="form-control form-control-middle"
              />
              <label for="floatingInput">Phone number</label>
            </div>
            <div class="form-floating">
              <input
                v-model="guestFormData.email"
                type="email"
                class="form-control form-control-bottom"
                @blur="validateEmail"
              />
              <label for="floatingInput">Email</label>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <div class="d-flex">
            <button class="btn btn-danger w-50" @click="handleDeleteGuest()">
              Delete
            </button>
            <button
              class="btn btn-primary w-50 ms-2"
              @click="handleEditGuest()"
            >
              Edit
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
