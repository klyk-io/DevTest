import "./scss/style.scss";
//import 'kendo-ui-core';
import { mapViewModel } from "./map.viewModel";

import { App } from './app';

//import Transform from 'ol/transform/proj/Tr';

let app;

//window.onload = function (e) {
//    getMyApp();
//}
export { mapViewModel }





//function getBaseUrl() {
//    const element = self.window.document.getElementById("base");
//    const base = element.getAttribute("href");
//    return base;
//}

//function getRootUrl() {
//    const element = self.window.document.getElementById("root");
//    const base = element.getAttribute("href");
//    return base;
//}

//export const api = getRootUrl() + "api";
//export const local = getBaseUrl();

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] === variable) { return pair[1]; }
    }
    return false;
}

function getLocalUrl() {
    const element = self.window.document.getElementById("local");

    const base = element.getAttribute("href");
    return base;
}

function getFormData(object) {
    const formData = new FormData();
    Object.keys(object).forEach(key => formData.append(key, object[key]));
    return formData;
}

var selectedAccountId = null;
var selectedCustomerId = null;

var accountsWidget = null;
var customers = null;

var autoBindCustomer = false;
//var viewModel = null;

$(document).ready(function () {

    if (selectedCustomerId > 0) {
        autoBindCustomer = true;
    }


})


export function toMiles(meters) {
    const factor = 0.000621371;
    return meters * factor;
}

export function toFeet(meters) {
    const factor = 3.280399;
    return meters * factor;
}

export function getCookie(cname) {
    const name = cname + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}


export const setAccessToken = function (accessToken) {
    sessionStorage.setItem("accessToken", accessToken);
};

let getAccessToken = function () {

    //get from session
    //alert(getCookie("AccessToken"));

    return sessionStorage.getItem("accessToken");
};


function parseQueryString(queryString) {
    var data = {},
        pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

    if (queryString === null) {
        return data;
    }

    pairs = queryString.split("&");

    for (var i = 0; i < pairs.length; i++) {
        pair = pairs[i];
        separatorIndex = pair.indexOf("=");

        if (separatorIndex === -1) {
            escapedKey = pair;
            escapedValue = null;
        } else {
            escapedKey = pair.substr(0, separatorIndex);
            escapedValue = pair.substr(separatorIndex + 1);
        }

        key = decodeURIComponent(escapedKey);
        value = decodeURIComponent(escapedValue);

        data[key] = value;
    }

    return data;
}


function getFragment() {
    if (window.location.hash.indexOf("#") === 0) {
        return parseQueryString(window.location.hash.substr(1));
    } else {
        return {};
    }
}

//$(document).ready(function () {

//    var token = getAccessToken();
//    if (token !== null) {

//        return getAccessToken();

//    } else {

//        var fragment = getFragment();

//    }

//});

function GmapToPostGis(obj) {

    var array = obj.getPath().getArray();

    var converted = [];
    for (var i in array) {
        var latLng = array[i];
        var coord = { x: latLng.lng(), y: latLng.lat() };

        converted.push(coord);
    }

    return converted;
}


//function addInteraction(LINE_STRING: GeometryType) {
//    throw new Error('Function not implemented.');
//}

//const featureStyles = {
//    'WayPoint': new Style({
//        image: new Circle({
//            radius: 5,
//            fill: new Fill({ color: '#ff00ff' }),
//            stroke: new Stroke({ color: '#bada55', width: 1 }),
//        }),
//    }),
//    'Jib': new Style({
//        image: new Circle({
//            radius: 5,
//            fill: new Fill({ color: '#0000ff' }),
//            stroke: new Stroke({ color: '#bada55', width: 1 }),
//        }),
//    }),
//    'Jump': new Style({
//        image: new Circle({
//            radius: 5,
//            fill: new Fill({ color: '#666666' }),
//            stroke: new Stroke({ color: '#bada55', width: 1 }),
//        }),
//    }),
//    'SecretStash': new Style({
//        image: new Circle({
//            radius: 10,
//            fill: new Fill({ color: '#666666' }),
//            stroke: new Stroke({ color: '#bada55', width: 1 }),
//        }),
//    }),
//};

//function getBikingTrails(map, resortId) {
//    var view = new View();
//    trailsDataSource.fetch(function () {
//        trailsDataSource.filter([{ field: "resortId", operator: "eq", value: parseInt(resortId) }, { field: "trailType", operator: "eq", value: "BIKE" }]);
//        var results = trailsDataSource.view();
//        //$.each(results, function (key, item) {
//        //	var wkt = new Wkt.Wkt();
//        //	wkt.read(item.path.wellKnownText);
//        //	var trail = wkt.toObject({
//        //		//icon: {
//        //		//    path: google.maps.SymbolPath.CIRCLE,
//        //		//    strokeColor: "black",
//        //		//    strokeWeight: 1,
//        //		//    fillColor: "green",
//        //		//    fillOpacity: 50,
//        //		//    scale: 5
//        //		//}
//        //	});
//        //	if ($.isArray(trail)) {
//        //		trail[0].setMap(map);
//        //	}
//        //	else {
//        //		trail.setMap(map);
//        //	}
//        //});
//    });
//}

