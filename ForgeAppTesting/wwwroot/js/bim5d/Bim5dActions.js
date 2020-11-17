var graficoLm;
var graficoAv;
var dbIdsLm;

function certificar() {
    fasesPanel.setVisible(true);
    //var node = $('#bonareaPanel').jstree('get_selected', true)[0];
    //var capitulo = node.parent.split('/')[0];
    //var partida = node.id.split('/')[0];
    //var selection = viewer.getSelection();
    //viewer.model.getExternalIdMapping((mapping) => {
    //    const keysMapping = Object.keys(mapping);
    //    var _externalIds = selection
    //        .map(x => x - 1)
    //        .map(x => keysMapping[x]);
    //    var _data = {
    //        padreId: capitulo,
    //        hijoId: partida,
    //        externalIds: _externalIds
    //    };
    //    $.ajax({
    //        type: 'POST',
    //        url: 'api/bim5d/partidas/certificar',
    //        contentType: 'application/json; charset=utf-8',
    //        data: JSON.stringify(_data),
    //        success: (result) => {
    //            $.notify(`Se han certificado ${result} en la Partida ${partida}`);
    //        }
    //    });
    //});
}

function drawGraficoPartida(labels, cantidad, avance) {
    Chart.defaults.global.defaultFontColor = '#adb5bd';
    var ctx = document.getElementById('graficoPartida').getContext('2d');
    if (graficoLm !== undefined) {
        graficoLm.destroy();
    }
    graficoLm = new Chart(ctx, {
        // The type of chart we want to create
        type: 'horizontalBar',

        // The data for our dataset
        data: {
            labels: labels,
            datasets: [{
                label: 'Cantidad',
                backgroundColor: 'rgba(255, 196, 0, 0.5)',
                borderColor: 'rgba(255, 196, 0, 1.0)',
                data: cantidad
            },
            {
                label: 'Avance',
                backgroundColor: 'rgba(135, 206, 250, 0.5)',
                borderColor: 'rgba(135, 206, 250, 1.0)',
                data: avance
            }]
        },
        options: {
            'onClick': function (evento, item) {
                if (item[0] === undefined) {

                } else {
                    var label = item[0]._model.label;
                    var nombreParametro = getParameterAvance();
                    viewer.model.getBulkProperties(dbIdsLm, [nombreParametro], (result) => {
                        var dbIds = result
                            .filter(x => x.properties[0].displayValue === label)
                            .map(x => x.dbId);
                        //viewer.select(dbIds);
                        viewer.clearThemingColors(viewer.model);
                        let cantidad = new THREE.Vector4(1, 196 / 255, 0, 1);
                        dbIds.map(id => {
                            viewer.setThemingColor(id, cantidad);
                        });
                        viewer.fitToView(dbIds, viewer.model);
                    }, (error) => {
                        console.log(error);
                    });
                }
            }
        }
    });
}