import { Component, ViewChild } from '@angular/core';
import * as L from 'leaflet';
import { TPO, PuntsInteres, ParcNou } from '../../assets/js/sample-geojson.js';
import '../../assets/js/leaflet-sidebar.js'
import { SafeResourceUrl } from '@angular/platform-browser';
import * as angular from "angular";
import * as $ from "jquery";
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

    var geoJson;

    $.ajax({
      async: false,
      type: "GET",
      url: "http://localhost:29164/estacions/PintarUbicacio",
      contentType: "application/json; charset=utf-8",
      dataType: "json"
    }).done(function (data) {
      geoJson = data;
    })

    L.geoJSON([geoJson], {

      style: function (feature) {
        return feature.properties && feature.properties.style;
      },


      pointToLayer: function (geoJson, latlng) {
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

  onClick(data) {
    const sidebar = this.sidebar;
    sidebar.removePanel('text');

    var idestacio = data.sourceTarget.feature.idEstacio
    var geoJson;

    //fer crida ajax al metode findID del controller passant id estació del data
    $.ajax({
      async: false,
      data: { "idEstacio": idestacio },
      type: "GET",
      url: "http://localhost:29164/estacions/FindId",
      dataType: "json"
    }).done(function (data) {
      geoJson = data;
      })

    let panelHtml = '</br>' + `<h4> CURRENT CONDITIONS </h4>` + '</br>' + `<p> Outside Temperature ${geoJson.outTemp}ºC </p>` +
      `<p> Heat Index ${geoJson.heatindex}ºC </p>` + `<p> Wind Chill ${geoJson.windchill}ºC </p>` +
      `<p> Dew Point ${geoJson.dewpoint}ºC </p>` + `<p> Humidity ${geoJson.outHumidity}% </p>` +
      `<p> Barometer ${geoJson.barometer} mbar </p>` + `<p> Wind ${geoJson.windSpeed}km/h </p>` +
      `<p> Rain Rate ${geoJson.rainRate} cm/h </p>` + `<p> Rain Today ${geoJson.rain}cm </p>` +
      `<p> Inside Temperature ${geoJson.inTemp}ºC </p>` + `<p> Inside Humidity ${geoJson.inHumidity}% </p>`
    this.panelContent.pane = panelHtml;
    sidebar.addPanel(this.panelContent);
    sidebar.open('text');

  }


};


