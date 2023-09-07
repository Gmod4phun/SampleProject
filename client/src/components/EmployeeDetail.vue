<template>
    <div class="detailContainer">
        Employee Detail
        <hr class="entrySeparator">
        <div class="row">
            <label for="firstName">First Name</label>
            <input type="text" id="firstName" :disabled="!canEdit()" placeholder="Enter first name" v-model.trim.lazy="employeeData.firstName">
        </div>
        <div class="errorMessage" v-if="v$.employeeData.firstName.$error">
            {{ v$.employeeData.firstName.$errors[0].$message }}
        </div>
        <hr class="entrySeparator">
        
        <div class="row">
            <label for="lastName">Last Name</label>
            <input type="text" id="lastName" :disabled="!canEdit()" placeholder="Enter last name" v-model.trim.lazy="employeeData.lastName">
        </div>
        <div class="errorMessage" v-if="v$.employeeData.lastName.$error">
            {{ v$.employeeData.lastName.$errors[0].$message }}
        </div>
        <hr class="entrySeparator">

        <div class="row">
            <label for="address">Address</label>
            <input type="text" id="address" :disabled="!canEdit()" :placeholder="!canEdit() ? '' : 'Enter full address'" v-model.trim.lazy="employeeData.address">
        </div>
        <hr class="entrySeparator">

        <div class="row">
            <label for="birthDate">Birth Date</label>
            <input type="date" id="birthDate" :disabled="!canEdit()" v-model.trim.lazy="employeeData.birthDate">
        </div>
        <div class="errorMessage" v-if="v$.employeeData.birthDate.$error">
            {{ v$.employeeData.birthDate.$errors[0].$message }}
        </div>
        <hr class="entrySeparator">

        <div v-if="!employeeData.isArchived">

            <div class="row">
                <label for="startDate">Start Date</label>
                <input type="date" id="startDate" :disabled="!canEdit()" v-model.trim.lazy="employeeData.startDate">
            </div>
            <div class="errorMessage" v-if="v$.employeeData.startDate.$error">
                {{ v$.employeeData.startDate.$errors[0].$message }}
            </div>
            <hr class="entrySeparator">

            <div class="row">
                <label for="position">Position</label>
                <select id="position" v-if="canEdit()" v-model="employeeData.position.name">
                    <option value="" style="color: gray">Select a position</option>
                    <option v-for="position in positions" :key="position.name" data-test="position">
                        <option value="position.name">{{ position.name }}</option>
                    </option>
                </select>

                <input type="text" id="position" v-if="!canEdit()" :disabled="true" v-model="employeeData.position.name">
            </div>
            <div class="errorMessage" v-if="v$.employeeData.position.name.$error">
                {{ v$.employeeData.position.name.$errors[0].$message }}
            </div>
            <hr class="entrySeparator">

            <div class="row">
                <label for="wage">Wage</label>
                <input type="text" id="wage" :disabled="!canEdit()" placeholder="Enter a monthly wage" v-model.number.lazy="employeeData.wage">
            </div>
            <div class="errorMessage" v-if="v$.employeeData.wage.$error">
                {{ v$.employeeData.wage.$errors[0].$message }}
            </div>
            <hr class="entrySeparator">

        </div>


        <button class="bottomButton" v-if="this.detailMode=='edit'" @click="saveChanges">Save changes</button>
        <button class="bottomButton" v-if="this.detailMode=='add'" @click="confirmAdd">Confirm</button>
        <button class="bottomButton" @click="$emit('closeDetail')">{{ !canEdit() ? "Close" : "Cancel" }}</button>

        <div v-if="detailMode!='add'">
            <h2>Previous positions:</h2>
            <div v-if="getPreviousContractData().length">
                <div v-for="contract in getPreviousContractData()" :key="contract.id" data-test="contract">
                    <div class="contractDataContainer">
                        <div class="contractDataItem contractDuration">{{ contract.start }} -- {{ contract.end }}</div>
                        <div class="contractDataItem contractPosition">{{ contract.position }}</div>
                    </div>
                </div>
            </div>
            <div v-else>
                <h3>No previous positions</h3>
            </div>
        </div>
    </div>
    
</template>

<script lang="ts">

import useValidate from '@vuelidate/core'
import {required, decimal, minValue, maxValue, helpers} from '@vuelidate/validators'

import EmployeeService from '../services/EmployeeService.ts';
import PositionService from '../services/PositionService.ts';
import ContractService from '../services/ContractService.ts';

const employeeService = new EmployeeService();
const positionService = new PositionService();
const contractService = new ContractService();

export default {
    data() {
        return {
            v$: useValidate(),
            positions: [],
            employeeContracts: []
        }
    },
    validations() {
        return {
            employeeData: {
                firstName: {required},
                lastName: {required},
                birthDate: {required,
                    maxValue: helpers.withMessage('Birth date must be a past date', value => {
                        return this.getDateOnly(value as Date) < this.getCurrentDateOnly()
                    }),
                },
                startDate: {
                    required,
                    minValue: helpers.withMessage('Start date must be a present or future date', value => {
                        return this.getDateOnly(value as Date) >= this.getCurrentDateOnly()
                    }),
                },
                position: {
                    name: {required},
                },
                wage: {required, decimal, minValue: minValue(0)}
            }
        }
    },
    props: { employeeData: Object, detailMode: String},
    emits: ['closeDetail', 'addedEmployee', 'updatedEmployee'],
    created() {
        this.getPositions()

        if (this.detailMode!='add') {
            this.getContractsForEmployee(this.employeeData.id)
        }
    },
    methods: {
        canEdit() {
            return this.detailMode != "view"
        },
        async saveChanges() {
            this.v$.$validate()
            if (this.v$.$error) {
                return;
            }

            try {
                await employeeService.updateEmployee(this.employeeData)

                this.getContractsForEmployee(this.employeeData.id)
                this.$emit('updatedEmployee')
            }
            catch {
                console.log("Error updating employee")
            }
        },
        confirmAdd() {
            console.log("Add employee")
            // console.log(this.employeeData)
            this.addEmployee()
        },
        async addEmployee() {
            this.v$.$validate()
            if (this.v$.$error) {
                return;
            }

            try {
                // await axios.post(`${API_URL_EMPLOYEE}/add`, this.employeeData)
                await employeeService.addEmployee(this.employeeData);
                this.$emit('addedEmployee')
            }
            catch {
                alert("Wrong data for employee")
            }
        },
        async getPositions() {
            try {
                // const response = await axios.get(`${API_URL_POSITION}/all`)
                const response = await positionService.getPositions()
                this.positions = response.data
            }
            catch {
                this.positions = []
            }
        },
        async getContractsForEmployee(id: number) {
        try {
            // const response = await axios.get(`${API_URL_CONTRACT}/foremployee/${id}`)
            const response = await contractService.getContractsForEmployee(id)
            this.employeeContracts = response.data
        }
        catch {
            this.employeeContracts = []
        }

        },
        getPreviousContractData() {
            // Sort the employeeContracts array by start dates
            // lets sort by contract id instead, if we have same dates for some contracts
            this.employeeContracts.sort((a, b) => a.id - b.id);

            // Initialize an array to store the date ranges
            const dateRanges = [];

            // Iterate through the sorted contracts array to calculate date ranges
            for (let i = 0; i < this.employeeContracts.length - 1; i++) {
                const currentContract = this.employeeContracts[i];
                const nextContract = this.employeeContracts[i + 1];

                // Calculate the date range between the current and next contract start dates
                const startDate = currentContract.startDate;
                const endDate = nextContract.startDate;

                // Push the date range to the dateRanges array
                dateRanges.push({ id: currentContract.id, start: startDate, end: endDate, position: currentContract.position.name });
            }

            if (this.employeeData.isArchived) {
                const lastContract = this.employeeContracts[this.employeeContracts.length - 1]
                if (lastContract) {
                    // console.log(lastContract)
                    dateRanges.push({ id: lastContract.id, start: lastContract.startDate, end: this.employeeData.endDate, position: lastContract.position.name });
                }
            }

            return dateRanges
        },
        getCurrentDateOnly() {
            return this.getDateOnly(new Date())
        },
        getDateOnly(val: Date) {
            var newDate = new Date(val)
            newDate.setHours(0,0,0,0)
            return newDate
        }
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
  /* border: 4px solid orange; */
  /* background-color: white; */
  background-color: rgb(200,200,200);
  /* width: 50%; */
  /* height: 50%; */
  /* align-items: center; */
  /* justify-content: center; */
  box-shadow: 0 0 4px black;
  padding: 8px;
  position: fixed;
}

.bottomButton {
    flex: 1;
    /* position: relative; */
    /* margin-top: 25%; */
    /* bottom: ; */
    min-height: 40px;
    margin-top: 4px;
    margin-bottom: 4px;
}

.contractDataContainer {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 80%;
    align-self: center;
    margin: 8px auto;
}

.contractPosition {
    text-align: start;
    flex: 1;
}

.contractDuration {
    flex: 2;
}

.entrySeparator {
    margin-top: 2px;
}
</style>
