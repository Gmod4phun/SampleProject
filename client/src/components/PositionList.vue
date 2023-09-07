<template>
  <div>
    <h2>List of positions</h2>

    <div v-if="!loaded">
      <h3>Loading</h3>
    </div>

    <div v-if="error && loaded">
      <h3>Error loading data</h3>
    </div>

    <div v-if="!error && loaded">

      <div>
        <button class="newPosButton" @click="$emit('onClickNewPosition')">Add new position</button>
      </div>

      <div v-if="positions.length == 0">
        <h2>There are no positions</h2>
      </div>

      <div v-if="positions.length > 0">
        <div class="entryContainer">
          <div class="entryItem empName" style="font-weight: bold;">Position name</div>
          <div class="entryItem btnDelContainer"></div>
        </div>

        <hr>

        <div v-for="pos in positions" :key="pos.name" data-test="pos">
          <div class="entryContainer row">
            <div class="entryItem positionEntry">{{ pos.name }}</div>
            <div class="entryItem btnDelContainer">
              <!-- <button @click="onClickPositionDelete(pos.name)">Delete</button> -->
              <button @click="showConfirm(pos.name)">Delete</button>
            </div>
          </div>
        </div>
      </div>

    </div>

  </div>
</template>
  
<script>

import PositionService from '../services/PositionService.ts';

const positionService = new PositionService();

export default {
  data() {
    return {
      positions: [],
      error: false,
      loaded: false
    }
  },
  created() {
    this.getPositions()
  },
  methods: {
    async getPositions() {
      this.loaded = false

      try {
        // const response = await axios.get(`${API_URL_POSITION}/all`)
        const response = await positionService.getPositions()

        this.positions = response.data
        this.eror = false
        this.loaded = true
      } catch {
        this.error = true
        this.loaded = true
      }
    },
    async onClickPositionDelete(name) {
      try {
        // await axios.delete(`${API_URL_POSITION}/${name}`)
        await positionService.deletePosition(name)

        this.getPositions()
      } catch {
        alert("Failed to delete position. Maybe it's referenced by contracts.")
      }
    },
    showConfirm(name) {
      const result = confirm(`Are you sure you want to delete the position ${name}?`);
      if (result) {
        this.onClickPositionDelete(name)
      }
    }
  }
}
</script>

<style lang="scss">

.positionEntry {
  /* margin: 5px; */
  flex: 1;
}

.newPosButton {
  margin-bottom: 16px;
  width: 20%;
}

.entryContainer {
  display: flex;
  /* border: solid 2px blue; */
  justify-content: center;
  gap: 20px;
  margin: 5px;

  &.row {
    /* border: solid 2px blue; */
    box-shadow: 0 0 2px black;
    background-color: rgba(0,0,0,0.02);
  }
}

.entryItem {
  /* border: solid 4px lime; */
  flex: 1;
}

</style>
