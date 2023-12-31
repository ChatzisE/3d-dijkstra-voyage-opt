<template>
  <div id="map-container">
    <form id="sidebar">
      <div id="sidebar-title">Algorithm Set Up</div>
      <div style="color: #ee9265" class="sidebar-sub-title">Departure:</div>
      <div>
        <label for="departure-lon">Longitude:</label>
        <input id="departure-lon" v-model.number="departure.lon" required/>
        <label for="departure-lat">Latitude:</label>
        <input id="departure-lat" v-model.number="departure.lat" required/>
      </div>
      <div style="color: #84BD72FF" class="sidebar-sub-title">Arrival:</div>UI
      <div>
        <label for="arrival-lon">Longitude:</label>
        <input id="arrival-lon" v-model.number="arrival.lon" required/>
        <label for="arrival-lat">Latitude:</label>
        <input id="arrival-lat" v-model.number="arrival.lat" required/>
      </div>
      <div class="sidebar-sub-title">Step (hr):</div>
      <input
          v-model.number="step"
          type="number"
          min="1"
          max="24"
          step="1"
          required
      />
      <div class="sidebar-sub-title">Speed (kn):</div>
      <input
          v-model.number="speedOverGround"
          type="number"
          min="1"
          max="24"
          step="1"
          required
      />
      <div style="text-align: center">
        <button @click="submitForm">Run</button>
        <button @click="resetForm">Reset</button>
      </div>
    </form>
    <div id="map"></div>
  </div>
</template>

<script lang="ts">
import {defineComponent, onMounted, ref} from "vue";
import Waypoint from "./models/waypoint";
import PathRequest from "./models/pathRequest";
import common from "./services/common";
import MapHelper from "./services/mapHelper";

export default defineComponent({
  name: "App",
  setup: function () {
    const _defaultValues = {
      departure: {
        lat: 46.44,
        lon: -6.06,
        timestamp: new Date(),
      } as Waypoint,
      arrival: {
        lat: 39.91,
        lon: -64.78,
        timestamp: new Date(),
      } as Waypoint,
      step: 12 as number,
      speedOverGround: 12 as number,
    };
    const departure = ref<Waypoint>(_defaultValues.departure);
    const arrival = ref<Waypoint>(_defaultValues.arrival);
    const step = ref(_defaultValues.step);
    const speedOverGround = ref(_defaultValues.speedOverGround);
    const path = ref<Waypoint[] | null>(null);
    let mapHelper: MapHelper;
    const submitForm = async (event: Event) => {
      event.preventDefault();
      path.value = await common.getGreatCirclePath(
          <PathRequest>{
            departure: departure.value,
            arrival: arrival.value,
            step: step.value,
            speedOverGround: speedOverGround.value
          }
      );
      if (path.value) {
        mapHelper.drawPath(path.value);
      }

    };
    onMounted(() => {
      mapHelper = new MapHelper('map');
      mapHelper.addMarker(departure.value.lat, departure.value.lon, 'departure');
      mapHelper.addMarker(arrival.value.lat, arrival.value.lon, 'arrival');
    });

    const resetForm = () => {
      departure.value = _defaultValues.departure;
      arrival.value = _defaultValues.arrival;
      step.value = _defaultValues.step;
      speedOverGround.value = _defaultValues.speedOverGround;
    };
    return {departure, arrival, step, speedOverGround, path, submitForm, resetForm};
  },
});
</script>

<style>
#map-container {
  height: 100%;
  width: 100%;
  background: transparent;
}

#sidebar {
  width: 300px;
  padding: 10px;
  box-sizing: border-box;
  border-radius: 20px;
  margin: 5px;
  margin-bottom: 20px;
  height: 98%;
  position: absolute;
  top: 10px;
  left: 0;
  z-index: 1000;
  background-color: #f0f8ffa3;
  display: flex;
  flex-direction: column;
  font-family: "Open Sans", sans-serif;
  font-size: 16px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
  color: #0e2954;
}

#sidebar > div {
  margin-bottom: 10px;
}

#sidebar > div > label {
  display: inline-block;
  width: 100px;
}

#sidebar #sidebar-title {
  text-align: center;
  font-size: 20px;
  font-weight: bold;
}

#sidebar .sidebar-sub-title {
  font-size: 16px;
  font-weight: bold;
}

#map {
  width: 100%;
  height: 100%;
}
</style>