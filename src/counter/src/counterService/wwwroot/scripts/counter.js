function init() {
    initWebSocket();
}

var websocket;
var lastCounterValue = 0;

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

    websocket.addEventListener("message", function(event){
        try {
            var newCounterValue = JSON.parse(event.data).value;
            if (lastCounterValue > newCounterValue)
            {
                var message = `Unexpected : Counter value decreased.
                lastCounterValue : ${lastCounterValue} , newCounterValue : ${newCounterValue}`;
                document.getElementById("message").innerHTML = message;
            }

            lastCounterValue = newCounterValue;
        } catch(err)
        {
            document.getElementById("message").innerHTML = err + " event : " + JSON.stringify(event);
        }
    });
}


function onNewDataReceived(jsonString) {
    try {
        jsonData = JSON.parse(jsonString);
        document.getElementById("data").innerHTML = "" + jsonData.value;
    }
    catch (err) {
    }
}