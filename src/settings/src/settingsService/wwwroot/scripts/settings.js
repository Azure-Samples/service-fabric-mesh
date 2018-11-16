function init() {
    getData();
    updateCurrentTime();
}

function getData() {
    $.getJSON(window.location.origin + "/api/values/containerInfo")
        .done(function (jsonData) {
            onNewDataReceived(jsonData);
            setTimeout(getData, 5000);
        })
        .fail(function (jqxhr, textStatus, error) {
            setTimeout(getData, 5000);
        });
}

function updateCurrentTime() {
    document.getElementById("currentTime").innerHTML = "" + new Date();
    setTimeout(updateCurrentTime, 1000);
}

function onNewDataReceived(jsonData) {
    try {
        loadInfoTable('environment-table', 'environmentVariables', jsonData);
        loadInfoTable('settings-table', 'settings', jsonData);
        loadInfoTable('container-info-table', 'genericInfo', jsonData);
        document.getElementById("updatedAt").innerHTML = "" + new Date();
    }
    catch (err) {
    }
}

function loadInfoTable(tableId, propertyName, infoObject) {

    var header = '<tr><th width="35%">Name</th><th width="65%">Value</th></tr>';

    loadTable(
        tableId, 
        header,
        ['name', 'value'], 
        infoObject[propertyName]);
}

function loadTable(tableId, header, fields, data) {

    var rows = header;

    $.each(data, function(index, item) {

        var row = '<tr>';

        $.each(fields, function(index, field) {
            row += '<td>' + item[field+''] + '</td>';
        });

        rows += row + '</tr>';
    });

    $('#' + tableId).html(rows);
}
