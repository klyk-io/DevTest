import { EsriScene } from "./arcgis";

export const mapViewModel = new kendo.data.ObservableObject({
    init: function () {
        new EsriScene("viewDiv");
    }
});