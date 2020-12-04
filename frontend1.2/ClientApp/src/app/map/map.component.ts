import { Component, ViewChild } from '@angular/core';
import * as L from 'leaflet';
import { TPO, PuntsInteres, ParcNou } from '../../assets/js/sample-geojson.js';
import '../../assets/js/leaflet-sidebar.js'
import { SafeResourceUrl } from '@angular/platform-browser';
import * as angular from "angular";

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
/** map component*/
export class MapComponent {

  public map;
  public sidebar;
  public sidebarPointList;

  mbAttr = 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, ' +
    '<a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
    'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>';
  mbUrl = 'https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw';

  constructor() { }

  ngAfterViewInit(): void {
    this.initMap();
  }

  //sidebar variables
  public showLegend: boolean;
  public legendUrl: SafeResourceUrl;
  private panelContent: L.Control.PanelOptions = {
    id: 'text',
    tab: '<i class="material-icons" title="text">description</i>',
    position: 'top',
    title: 'Caracetrístiques meteorològiques',
    pane: ''
  }

  public sidebarOptions: L.SidebarOptions = {
    position: 'right',
    autopan: true,
    closeButton: true,
    container: 'sidebar',
  }

  public sidebarPointListOptions: L.SidebarOptions = {
    position: 'left',
    autopan: true,
    closeButton: true,
    container: 'sidebarPointList',
  }


  onEachFeature(feature, layer) {
    var popupContent = "<p>He començat com a GeoJSON " +
      feature.geometry.type + ", Però ara sóc un vector de leaflet!</p>";

    if (feature.properties && feature.properties.popupContent) {
      popupContent += feature.properties.popupContent;
    }

    layer.bindPopup(popupContent);
  }

  MGris = L.tileLayer(this.mbUrl, { id: 'mapbox/light-v9', tileSize: 512, zoomOffset: -1, attribution: this.mbAttr });
  Carrers = L.tileLayer(this.mbUrl, { id: 'mapbox/streets-v11', tileSize: 512, zoomOffset: -1, attribution: this.mbAttr });

  private initMap(): void {
    this.map = L.map('map', {
      center: [42.180835, 2.486769],
      layers: [this.MGris],
      zoom: 16
    });

    var Mapes = {
      "Mapa Gris": this.MGris,
      "Mapa Carrers": this.Carrers
    };

    L.control.layers(Mapes).addTo(this.map);

    L.geoJson([
      $.ajax({
          method: "GET",
          url: "../../../../Controllers/EstacionsController/PintarUbicacio",
          contentType: "application/json; charset=utf-8",
          dataType: "json"
      })]);
    
    L.geoJSON([PuntsInteres, ParcNou], {

      style: function (feature) {
        return feature.properties && feature.properties.style;
      },


      pointToLayer: function (feature, latlng) {
        return L.circleMarker(latlng, {
          radius: 8,
          fillColor: "#ff7800",
          color: "#000",
          weight: 1,
          opacity: 1,
          fillOpacity: 0.8
        });
      }
    }).addTo(this.map).on('click', this.onClick, this);

    L.geoJSON(TPO, {

      filter: function (feature, layer) {
        if (feature.properties) {
          // If the property "underConstruction" exists and is true, return false (don't render features under construction)
          return feature.properties.underConstruction !== undefined ? !feature.properties.underConstruction : true;
        }
        return false;
      },

      onEachFeature: this.onEachFeature
    }).addTo(this.map);


    this.sidebarPointList = L.control.sidebar('sidebarPointList', {
      closeButton: true,
      position: 'left',
      container: 'sidebarPointList',
      autopan: true
    });

    this.sidebar = L.control.sidebar('sidebar', {
      closeButton: true,
      position: 'right',
      container: 'sidebar',
      autopan: true
    });

    this.map.addControl(this.sidebarPointList);
    this.map.addControl(this.sidebar);
  }

  //Metode on rep les coordenades i centra el mapa
  centerMap(n1, n2, z) {
    this.map.setView([n1, n2], z)
  }

  onClick(data, feature) {
    var app = angular.module('leaflet-app', [])
    const sidebar = this.sidebar;
    sidebar.removePanel('text');
    let content = data.sourceTarget.feature.properties.popupContent;
    let panelHtml = `<h1>${content} </h1>`
    this.panelContent.pane = panelHtml;
    sidebar.addPanel(this.panelContent);
    sidebar.open('text');
  }
};

