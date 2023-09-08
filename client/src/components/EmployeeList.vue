<template>
  <div>
    <h2>List of {{ previous ? 'previous' : 'current' }} employees</h2>

    <div v-if="!loaded">
      <h3>Loading</h3>
    </div>

    <div v-if="error && loaded">
      <h3>Error loading data</h3>
    </div>

    <div v-if="!error && loaded">

      <div v-if="!previous">
        <button class="newEmpButton" @click="clickEmployeeAdd()">Add new employee</button>
      </div>

      <div v-if="employees.length == 0">
        <h2>There are no {{ previous ? 'previous' : 'current' }} employees</h2>
      </div>

      <div v-if="employees.length > 0">
        <div class="employeeEntryContainer" v-if="!previous">
          <div class="employeeEntryItem empName" style="font-weight: bold;">Employee Name</div>
          <div class="employeeEntryItem empPos" style="font-weight: bold;">Current Position</div>
          <div class="employeeEntryItem btnEditContainer"></div>
          <div class="employeeEntryItem btnDelContainer"></div>
        </div>

        <div class="employeeEntryContainer" v-if="previous">
          <div class="employeeEntryItem empName" style="font-weight: bold;">Employee Name</div>
          <div class="employeeEntryItem empPos" style="font-weight: bold;">Last Position</div>
          <div class="employeeEntryItem empLeaveDate" style="font-weight: bold;">Leave Date</div>
          <div class="employeeEntryItem btnDelContainer"></div>
        </div>

        <hr>

        <div v-for="employee in employees" :key="employee.id" data-test="employee">
            <div class="employeeEntryContainer row" v-show="!previous">
              <div class="employeeEntryItem empName item" @click="clickEmployee(employee, 'view')">{{ employee.fullName }}</div>
              <div class="employeeEntryItem empPos">{{ employee.position.name }}</div>
              <div class="employeeEntryItem btnEditContainer">
                <button @click="clickEmployee(employee, 'edit')">Edit</button>
              </div>
              <div class="employeeEntryItem btnDelContainer">
                <button @click="showConfirmDelete(employee)">Delete</button>
              </div>
            </div>

            <div class="employeeEntryContainer row" v-show="previous">
              <div class="employeeEntryItem empName item" @click="clickEmployee(employee, 'view')">{{ employee.fullName }}</div>
              <div class="employeeEntryItem empPos">{{ employee.position.name }}</div>
              <div class="employeeEntryItem empLeaveDate">{{ employee.endDate }}</div>
              <div class="employeeEntryItem btnDelContainer">
                <button @click="showConfirmDeletePermanent(employee)">Delete</button>
              </div>
            </div>
        </div>

      </div>

    </div>

  </div>
</template>
  
<script lang="ts">

import EmployeeService from '../services/EmployeeService';
import {Employee, Position, Contract} from '../interfaces/CustomDataTypes';

const employeeService = new EmployeeService();

export default {
  data() {
    return {
      employees: Array<Employee>(),
      error: false,
      loaded: false
    }
  },
  props: ['previous'],
  created() {
    this.getEmployees()
    // console.log("employee list created ")
  },
  emits: ['onClickEmployee', 'archivedEmployee'],
  methods: {
    async getEmployees() {
      this.loaded = false
      try {
        const response = this.previous ? await employeeService.getPreviousEmployees() : await employeeService.getCurrentEmployees()

        this.employees = response.data
        this.error = false
        this.loaded = true
      }
      catch {
        this.error = true
        this.loaded = true
      }
    },
    async deleteEmployee(id: number) {
      try {
        await employeeService.deleteEmployee(id)
        this.getEmployees()
      } catch {}
    },
    async archiveEmployee(id: number) {
      try {
        await employeeService.archiveEmployee(id)
        this.getEmployees()
        this.$emit("archivedEmployee")
      } catch {}
    },
    onDeleteClick(id: number, forceDelete: boolean) {
      if (forceDelete) {
        this.deleteEmployee(id)
      }
      else {
        this.archiveEmployee(id)
      }
    },
    clickEmployee(employee: Employee, detailMode: string) {
      this.$emit('onClickEmployee', employee, detailMode)
    },
    clickEmployeeAdd() {
      this.$emit('onClickEmployee', null, 'add')
    },
    showConfirmDelete(employee: Employee) {
      const result = confirm(`Are you sure you want to delete the employee ${employee.fullName}?`);
      if (result) {
        this.showConfirmArchive(employee)
      }
    },
    showConfirmArchive(employee: Employee) {
      const result = confirm(`Do you want to archive the employee ${employee.fullName}?`);
      if (result) {
        this.archiveEmployee(employee.id!)
      }
      else {
        this.deleteEmployee(employee.id!)
      }
    },
    showConfirmDeletePermanent(employee: Employee) {
      const result = confirm(`Are you sure you want to permanently delete the employee ${employee.fullName}?`);
      if (result) {
        this.deleteEmployee(employee.id!)
      }
    },
  }
}
</script>

<style lang="scss">
.employeeEntryContainer {
  display: flex;
  /* border: solid 2px blue; */
  justify-content: center;
  gap: 20px;
  margin: 5px;

  &.row {
    /* border: solid 2px blue; */
    box-shadow: 0 0 2px black;
    background-color: rgba(0,0,0,0.02);

    &:hover {
      background-color: rgba(0,0,0,0.1);
    }
  }
}

.employeeEntryItem {
  /* border: solid 4px lime; */
  flex: 1;
}

.empName {
  flex: 2;
  text-align: center;
  text-justify: center;
  align-items: center;
  justify-content: center;

  &.item {
    &:hover {
      background-color: rgb(240,240,240);
      border-radius: 16px;
      box-shadow: 0 0 4px gray;
    }
  }

}

.empPos {
  flex: 2;
}

.newEmpButton {
  margin-bottom: 16px;
  width: 20%;
}

</style>
