
export class App {
    //map: OpenLayersMap;

    constructor() {
        
    }
}

var timeout = 100000;
var tid = null;

function abortTimer() { // to be called when you want to stop the timer
    clearTimeout(tid);
}

function errorCallback(error) {

    console.log(error);
    alert(error.message);
}
