/* eslint-disable @typescript-eslint/no-unused-vars */
import EsriConfig from "@arcgis/core/config";
import ArcGISMap from "@arcgis/core/Map";
import MapView from "@arcgis/core/views/MapView";
import SceneView from "@arcgis/core/views/SceneView";
import WMSLayer from "@arcgis/core/layers/WMSLayer";
import WMTSLayer from "@arcgis/core/layers/WMTSLayer";
import BaseElevationLayer from "@arcgis/core/layers/BaseElevationLayer";
import ElevationLayer from "@arcgis/core/layers/ElevationLayer";
import Viewpoint from "@arcgis/core/Viewpoint";
import Camera from "@arcgis/core/Camera";
import Point from "@arcgis/core/geometry/Point";

EsriConfig.assetsPath = "/assets";
EsriConfig.apiKey = "AAPK2ecc83e252294018a265438653dd9cc25DUG27XZzbpWZyhsUauz-x4e3zl8LFZEqy5iP-NBNQRRYSOlkT77xDFsYi2bZY1N";

let view: MapView;
let sceneView: SceneView;

const ExaggeratedElevationLayer = new BaseElevationLayer({
    
    
});
export class EsriScene {
    view: SceneView;
    map: ArcGISMap;

    constructor(container) {

        const layer = new WMSLayer({
            title: "test",
            
            url: "https://klyk.app/geoserver/mountainmap/wms?service=WMS",
            sublayers: [
                {
                    name: "mountainmap:biking_trails"                    
                },
                {
                    name: "mountainmap:resorts"
                },
                {
                    name: "mountainmap:ski_trails"
                },
                {
                    name: "mountainmap:trailheads"
                },
                {
                    name: "mountainmap:hearts"
                },
                {
                    name: "mountainmap:skate_parks"
                }  
            ]
        });

        const hikingLayer = new WMSLayer({
            url: "https://klyk.app/geoserver/mountainmap/wms?service=WMS",
            sublayers: [
                {
                    name: "mountainmap:hiking_trails"
                }
            ],
            minScale: 200000
        });


        const wmtsLayer = new WMTSLayer({
            url: "https://klyk.app/geoserver/gwc/service/wmts",
            version: "1.1.1",
            
            activeLayer: {
                id : "mountainmap:hiking_trails"
            }
        });

        
        var bob = new ElevationLayer({

        });

        this.map = new ArcGISMap({
            basemap: "satellite",
            ground: "world-elevation",            
            layers: [layer, hikingLayer]
        });

        this.view = new SceneView({
            // An instance of Map or WebScene
            map: this.map,
            container,
            viewpoint: new Viewpoint({
                scale: 20000000,
                targetGeometry: new Point({
                    x:-100,y:38
                })
            })
            // The id of a DOM element (may also be an actual DOM element)
            //container: "viewDiv"
        });

        //view.when(function () {
        //    // SceneView is now ready for display and can be used. Here we will
        //    // use goTo to view a particular location at a given zoom level, camera
        //    // heading and tilt.
        //    view.goTo({
        //        center: [-112, 38],
        //        zoom: 13,
        //        heading: 30,
        //        tilt: 60
        //    })
        //})
        //    .catch(function (err) {
        //        // A rejected view indicates a fatal error making it unable to display,
        //        // this usually means that WebGL is not available, or too old.
        //        console.error("SceneView rejected:", err);
        //    });

        //this.view.when(() => {
        //    console.log("Map is loaded");
        //})

        console.log(this.view);

    }

}