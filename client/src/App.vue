<template>
  <NavigationButtons @setActiveTab="setActiveTab"/>
  <img alt="Vue logo" src="./assets/logo.png" />

  <div v-show="activeTab!=0">
    <EmployeeList v-show="activeTab == 1" @onClickEmployee="onClickEmployee" ref="listCurrent" @archivedEmployee="archivedEmployee()"/>
    <EmployeeList v-show="activeTab == 2" @onClickEmployee="onClickEmployee" ref="listPrevious" :previous=true />
    <PositionList v-show="activeTab == 3" @onClickNewPosition="onClickNewPosition" ref="listPositions"/>
  </div>

  <div class="employeeDetailContainer" v-if="showEmployeeDetail" >
    <EmployeeDetail ref="employeeDetail" @closeDetail="closeDetail" :employeeData="employeeData" :detailMode="detailMode" @addedEmployee="addedEmployee()" @updatedEmployee="updatedEmployee()"/>
  </div>

  <div class="newPositionPopupContainer" v-if="showNewPosPopup" >
    <NewPositionPopup ref="newPosPopup" @closeNewPositionPopup="closeNewPositionPopup" @addedPosition="addedPosition()"/>
  </div>
  
</template>

<script lang="ts">
  import EmployeeDetail from './components/EmployeeDetail.vue'
  import EmployeeList from './components/EmployeeList.vue'
  import NavigationButtons from './components/NavigationButtons.vue'
  import NewPositionPopup from './components/NewPositionPopup.vue'
  import PositionList from './components/PositionList.vue'

  export default {
  name: "App",
  components: {
    EmployeeList,
    NavigationButtons,
    PositionList,
    EmployeeDetail,
    NewPositionPopup
  },
  data() {
    return {
      activeTab: 1,
      showEmployeeDetail: false,
      employeeData: {},
      detailMode: "view",
      showNewPosPopup: false
    }
  },
  methods: {
    setActiveTab(num: number) {
      this.activeTab = num
      this.refreshLists()
      if (this.$refs.listPositions) {
        this.$refs.listPositions.getPositions()
      }
    },
    onClickEmployee(employee: JSON, detailMode: string) {
      this.detailMode = detailMode

      if (detailMode == 'add') {
        this.onClickEmployeeAdd()
        return
      }

      this.employeeData = employee
      this.showEmployeeDetail = true
    },
    getCurrentDate() {
      const date = new Date();
      let currentDay= String(date.getDate()).padStart(2, '0');
      let currentMonth = String(date.getMonth()+1).padStart(2,"0");
      let currentYear = date.getFullYear();
      // we will display the date as YYYY-MM-DD
      let currentDate = `${currentYear}-${currentMonth}-${currentDay}`;
      return currentDate
    },
    onClickEmployeeAdd() {
      this.employeeData = {
        firstName: "",
        lastName: "",
        address: "",
        birthDate: "",
        startDate: this.getCurrentDate(),
        position: {name:""},
        wage: null
      }
      this.showEmployeeDetail = true
    },
    refreshLists() {
      this.$refs.listCurrent.getEmployees()
      this.$refs.listPrevious.getEmployees()
    },
    addedEmployee() {
      this.refreshLists()
    },
    archivedEmployee() {
      this.refreshLists()
    },
    updatedEmployee() {
      this.refreshLists()
    },
    closeDetail() {
      this.refreshLists()
      this.showEmployeeDetail=false
    },
    addedPosition() {
      this.$refs.listPositions.getPositions()
      // this.showNewPosPopup
    },
    onClickNewPosition() {
      this.showNewPosPopup = true;
    },
    closeNewPositionPopup() {
      this.showNewPosPopup = false;
    }
  }
}

</script> 

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 10px;
}

.employeeDetailContainer {
  display: flex;
  /* border: 4px solid yellowgreen; */
  box-sizing: border-box;
  position: absolute;
  top: 0;
  left: 0;
  min-width: 100%;
  min-height: 100%;
  justify-content: center;
  align-items: center;
}

.newPositionPopupContainer {
  display: flex;
  /* border: 4px solid yellowgreen; */
  box-sizing: border-box;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  justify-content: center;
  align-items: center;
}
</style>
