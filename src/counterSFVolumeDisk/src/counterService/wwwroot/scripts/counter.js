function init() {
    initWebSocket();
}

var websocket;
function initWebSocket() {
    websocket = new WebSocket("ws://" + window.location.host + "/data");

    websocket.onopen = function () { };

    websocket.onmessage = function (args) {
        onNewDataReceived(args.data);
    };

    websocket.onclose = function (args) {
        setTimeout(initWebSocket, 100);
    };

    websocket.onerror = function (error) {
        websocket.close();
    }
}


function onNewDataReceived(jsonString) {
    try {
        jsonData = JSON.parse(jsonString);
        document.getElementById("data").innerHTML = "" + jsonData.value;
    }
    catch (err) {
    }
}