<template>
  <div id="map-container">
    <form id="sidebar">
      <div id="sidebar-title">Algorithm Set Up</div>
      <div class="sidebar-sub-title">Departure:</div>
      <div>
        <label for="departure-lon">Longitude:</label>
        <input id="departure-lon" v-model.number="departure.lon" required/>
        <label for="departure-lat">Latitude:</label>
        <input id="departure-lat" v-model.number="departure.lat" required/>
      </div>
      <div class="sidebar-sub-title">Arrival:</div>
      <div>
        <label for="arrival-lon">Longitude:</label>
        <input id="arrival-lon" v-model.number="arrival.lon" required/>
        <label for="arrival-lat">Latitude:</label>
        <input id="arrival-lat" v-model.number="arrival.lat" required/>
      </div>
      <div class="sidebar-sub-title">Step (hr):</div>
      <input
          id="hours"
          v-model.number="step"
          type="number"
          min="1"
          max="24"
          step="1"
          required
      />
      <div>
        <button @click="submitForm">Submit</button>
        <button @click="resetForm">Reset</button>
      </div>
    </form>
    <div id="map"></div>
  </div>
</template>

<script lang="ts">
import {defineComponent, onMounted, ref} from "vue";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import Waypoint from "./models/waypoint";
import PathRequest from "./models/pathRequest";
import common from "./services/common";

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
      step: 3 as number,
    };
    const departure = ref<Waypoint>(_defaultValues.departure);
    const arrival = ref<Waypoint>(_defaultValues.arrival);
    const step = ref(_defaultValues.step);
    const path = ref<Waypoint[] | null>(null);
    const markers = ref<L.LayerGroup | null>(null);
    const map = ref<L.Map | null>(null);
    const departureMarker = ref<L.Marker | null>();
    const arrivalMarker = ref<L.Marker | null>(null);
    const greatCircle = ref<L.Polyline | null>(null);
    const submitForm = async (event: Event) => {
      event.preventDefault();
      path.value = await common.getGreatCirclePath(
          <PathRequest>{
            departure: departure.value,
            arrival: arrival.value,
            step: step.value,
          }
      );
      //@ts-ignore
      if (greatCircle.value) map.value?.removeLayer(greatCircle.value);
      else greatCircle.value = L.polyline([], {color: "red"});
      path.value?.forEach((p) => {
        console.log(p.lat, p.lon);
        greatCircle.value?.addLatLng(L.latLng(p.lat, p.lon));
      });
      //@ts-ignore
      greatCircle.value.addTo(map.value);
    };
    onMounted(() => {
      const mapElement = document.getElementById("map");
      if (mapElement) {
        map.value = L.map(mapElement).setView([0, 0], 2);
        map.value.zoomControl.setPosition("topright");
        L.tileLayer(
            "https://a.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}@2x.png",
            {
              attribution:
                  'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors',
              maxZoom: 18,
            }//@ts-ignore
        ).addTo(map.value);
        departureMarker.value = L.marker(
            [departure.value.lat, departure.value.lon],
            {
              draggable: true,
            }//@ts-ignore
        ).addTo(map.value);
        arrivalMarker.value = L.marker([arrival.value.lat, arrival.value.lon], {
          draggable: true,
          //@ts-ignore
        }).addTo(map.value);
        markers.value = new L.LayerGroup();
        map.value.on("click", (event: any) => {
          if (!departureMarker.value) {
            departureMarker.value = L.marker(event.latlng, {
              draggable: true,
              //@ts-ignore
            }).addTo(map.value);
            departure.value = <Waypoint>{
              lat: event.latlng.lat,
              lon: event.latlng.lng,
              timestamp: new Date(),
            };
          } else if (!arrivalMarker.value) {
            arrival.value = <Waypoint>{
              lat: event.latlng.lat,
              lon: event.latlng.lng,
              timestamp: new Date(),
            };
          }
        });
      }
    });

    const resetForm = () => {
      if (departureMarker.value) {
        departureMarker.value.remove();
        departureMarker.value = null;
        departure.value = _defaultValues.departure;
      }
      if (arrivalMarker.value) {
        arrivalMarker.value.remove();
        arrivalMarker.value = null;
        arrival.value = _defaultValues.arrival;
      }
      step.value = _defaultValues.step;
    };

    return {departure, arrival, step, path, submitForm, resetForm};
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