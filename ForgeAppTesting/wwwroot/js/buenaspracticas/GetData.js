$(document).ready(function () {
    startConnection();
});

var connection;
var connectionId;

function startConnection(onReady) {
    if (connection && connection.connectionState) { if (onReady) onReady(); return; }
    connection = new signalR.HubConnectionBuilder().withUrl("/api/signalr/designautomation").build();
    connection.start()
        .then(function () {
            connection.invoke('getConnectionId')
                .then(function (id) {
                    connectionId = id; // we'll need this...
                    if (onReady) onReady();
                });
        });

    connection.on("onComplete", function (message) {
        var obj = JSON.parse(message);
        var notificacion = `El trabajo ${obj.id} ha devuelto el estado: ${obj.status}`;
        notifySuccess(notificacion);
    });
}

function notifySuccess(text) {
    $.notify(text, "success");
}

function notifyInfo(text) {
    $.notify(text, "info");
}

function getRvtData() {
    var node = $('#userHubs').jstree('get_selected', true)[0];
    var data = node.parents
        .find(x => x.includes('folders'))
        .split('/');
    var carpeta = data[data.length - 1];
    data = node.parents
        .filter(x => x.includes('hubs'))
        .find(x => x.includes('projects'))
        .split('/');
    var proyecto = data[data.length - 1];
    data = node.parents
        .filter(x => x.includes('hubs'))[1]
        .split('/');
    var hub = data[data.length - 1];
    data = node.parents
        .find(x => x.includes('items'))
        .split('/');
    data = data[data.length - 1].split(':');
    var version = node.text.split(':')[0].replace('v', '');
    var _id = data[data.length - 1];
    jQuery.ajax({
        url: '/api/forge/oauth/token/files',
        success: function (res) {
            var url = `https://developer.api.autodesk.com/data/v1/projects/${proyecto}/folders/${carpeta}/contents`;
            $.ajax({
                type: 'GET',
                url: url,
                headers: { "Authorization": 'Bearer ' + res.access_token },
                success: (result) => {
                    data = result.included.find(x => x.id.includes(_id))
                        .relationships.storage.data.id.split('/');                    
                    var item = $('#userHubs').jstree(true).get_node(node.parent).text;
                    var rvt = {
                        guid: _id,//data[data.length - 1],
                        rvtName: item,
                        projectId: proyecto,
                        hubId: hub,
                        connectionId: connectionId,
                        version: version
                    };
                    console.log(rvt);
                    console.log(data[data.length - 1]);
                    $.ajax({
                        type: 'POST',
                        url: 'api/forge/designautomation/workitems',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(rvt),
                        success: (result) => {
                            var texto = `Se está procesando el trabajo ${result.workItemId}...`;
                            notifyInfo(texto);
                        }
                    });

                }
            });
        }
    });    
}