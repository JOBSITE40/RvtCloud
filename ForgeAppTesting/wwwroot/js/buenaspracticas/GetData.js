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
        .find(x => x.includes('items'))
        .split('/');
    data = data[data.length - 1].split(':');
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
                        guid: data[data.length - 1],
                        rvtName: item
                        };
                    console.log(item);
                    console.log(rvt);
                    $.ajax({
                        type: 'POST',
                        url: 'api/forge/designautomation/workitems',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(rvt),
                        success: (result) => {
                            console.log(result);
                        }
                    });

                }
            });
        }
    });    
}