<template>
    <div class="detailContainer">
        New Position
        <hr class="entrySeparator">
        <div class="row">
            <label for="positionName">Position name</label>
            <input type="text" id="positionName" v-model="positionName">
        </div>
        <div class="errorMessage" v-if="v$.positionName.$error">
            {{ v$.positionName.$errors[0].$message }}
        </div>
        <hr class="entrySeparator">
        <button class="bottomButton" @click="confirmAdd">Confirm</button>
        <button class="bottomButton" @click="$emit('closeNewPositionPopup')">Close</button>
    </div>
</template>

<script lang="ts">

import useValidate from '@vuelidate/core'
import {required, minLength} from '@vuelidate/validators'

import PositionService from '../services/PositionService';

const positionService = new PositionService();

export default {
    data() {
        return {
            v$: useValidate(),
            positionName: ""
        }
    },
    validations() {
        return {
            positionName: {required, minLength: minLength(1)}
        }
    },
    methods: {
        confirmAdd() {
            // console.log(this.employeeData)
            this.addPosition()
        },
        async addPosition() {
            this.v$.$validate()
            if (this.v$.$error) {
                return;
            }

            try {
                // await axios.post(`${API_URL_POSITION}/add/${this.positionName}`)
                await positionService.addPosition(this.positionName)

                this.$emit('addedPosition')
            } catch {
                alert("Failed to add new position. Maybe it already exists?")
            }
        },
    }
}
</script>

<style lang="scss">
.row {
    margin: 4px;
    /* border: solid 1px magenta; */
    display: flex;
    align-items: start;
}

label {
    flex: 1;
}

input {
    flex: 3;
}

select {
    flex: 3;
}

.detailContainer {
  background-color: white;
  background-color: rgba(128,128,128, 0.5);
  backdrop-filter: blur(8px);
  width: 50%;
  box-shadow: 0 0 4px black;
  padding: 8px;
  position: fixed;
}

.bottomButton {
    flex: 1;
    /* position: relative; */
    /* margin-top: 25%; */
    /* bottom: ; */
    width: 100%;
    height: 8%;
    align-self: center;
    /* margin: 4px; */
}

.errorMessage {
    color:red;
    margin-left: 2px;
}
</style>
