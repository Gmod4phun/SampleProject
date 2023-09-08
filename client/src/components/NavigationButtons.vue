<template>
<div class="buttonsContainer">
    <button class="buttonItem" @click="$emit('setActiveTab', 1)">List of Current Employees</button>
    <button class="buttonItem" @click="$emit('setActiveTab', 2)">Previous employees</button>
    <button class="buttonItem" @click="$emit('setActiveTab', 3)">List of Positions</button>
</div>
</template>

<script lang="ts">
export default {
    methods: {
        btnCurEmployees() {},
        btnPrevEmployees() {},
        btnPositions() {}
    },
    emits: ['setActiveTab']
}
</script>

<style lang="scss">
.buttonsContainer {
    display:flex;
    justify-content: space-evenly;
    gap: 20px;
}

.buttonItem {
    flex: 1;
    text-align: center;
}
</style>
