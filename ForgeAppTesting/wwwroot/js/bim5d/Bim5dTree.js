$(document).ready(function () {
    $(document).on('DOMNodeInserted', function (e) {
        if ($(e.target).hasClass('orbit-gizmo')) {
            if (viewer === null || viewer === undefined) return;
            // Aquí vamos a dibujar el dashboard
            new Bim5d(viewer);
        }
    });
});

class Bim5d {
    constructor(viewer) {
        var _this = this;
        viewer.addEventListener(Autodesk.Viewing.GEOMETRY_LOADED_EVENT, (result) => {
            _this.adjustLayout();
        });
    }

    adjustLayout() {
        var row = $(".row").children();
        if (row.length > 1) {
            $(row[0]).removeClass('col-sm-4').addClass('col-sm-1 transition-width');
            $(row[1])
                .after('<div id="panelBim5d" class="col-sm-4 transition-width">' +
                    '<div class="panel panel-default fill">' +
                    '<div class="panel-heading" data-toggle="tooltip">BIM 5D<span id="closeBim5d" class="glyphicon glyphicon-remove" style="margin-top:5px;cursor: pointer; float: right;"> </span></div>' +
                    '<input class="form-control-plaintext" type="text" id="parametroAvance" />' +
                    '<div id="wrap"><div id="bonareaPanel"></div></div></div></div>');
            $(row[0]).hide();
            this.loadTree();
            $('#closeBim5d').click(function () {
                $(row[0]).removeClass('col-sm-1').addClass('col-sm-4 transition-width');
                $(row[0]).show();
                $('#panelBim5d').remove();
                viewer.uninitialize();
            });
        }
    }

    loadTree() {
        $('#bonareaPanel').jstree({
            'core': {
                'themes': { "icons": true },
                'multiple': false,
                'data': {
                    "url": '/api/bim5d/tree',
                    "dataType": "json",
                    'cache': false,
                    'data': function (node) {
                        $('#userHubs').jstree(true).toggle_node(node);
                        return { "id": node.id, "guid": guidProject, "urn": _urn };
                    }
                }
            },
            'types': {
                'default': { 'icon': 'glyphicon glyphicon-question-sign' },
                '#': { 'icon': 'glyphicon glyphicon-user' },
                'capitulo': { 'icon': 'glyphicon glyphicon-folder-open' },
                'partida': { 'icon': 'glyphicon glyphicon-file' }
            },
            "plugins": ["types"],
        }).bind("activate_node.jstree", function (evt, data) {
            if (data != null && data.node != null && data.node.type == 'partida') {
                certPanel.setVisible(true);
                $('#panel-overlay').css('display', 'block');
                var id = data.node.id.split('/')[0];
                $.ajax({
                    url: '/api/bim5d/partidas',
                    type: 'GET',
                    data: {
                        id: id
                    },
                    success: (result) => {
                        if (result !== null) {
                            viewer.model.getExternalIdMapping((mapping) => {
                                const keysMapping = Object.keys(mapping);
                                getExternalIds(result, mapping).then((_dbIds) => {
                                    dbIdsLm = _dbIds;
                                    viewer.isolate(dbIdsLm);
                                    viewer.fitToView(dbIdsLm, viewer.model);
                                    var unidad;
                                    $.ajax({
                                        url: '/api/bim5d/partidas/info',
                                        type: 'GET',
                                        data: {
                                            id: id
                                        },
                                        success: (info) => {
                                            console.log(info);
                                            unidad = info.unidad;
                                            $('#cantidad').html('Cantidad: ' + info.cantidad + ' ' + unidad);
                                            $('#precio').html('Precio: ' + info.precio);
                                            $('#importe').html('Importe: ' + info.importe);
                                        }
                                    });
                                    var nombreParametro = getParameterAvance();
                                    viewer.model.getBulkProperties(dbIdsLm, [nombreParametro], (propiedades) => {
                                        var _values = propiedades
                                            .map(function (x) {
                                                var y = x.dbId - 1;
                                                return {
                                                    dbId: x.dbId,
                                                    externalId: keysMapping[y],
                                                    value: x.properties[0].displayValue
                                                }
                                            });
                                        var _data = {
                                            partida: id,
                                            values: _values
                                        };
                                        $.ajax({
                                            type: 'POST',
                                            url: 'api/bim5d/partidas/cantidad',
                                            contentType: 'application/json; charset=utf-8',
                                            dataType: "json",
                                            data: JSON.stringify(_data),
                                            success: (result) => {
                                                var _cantidad = result.map(x => x.cantidad);
                                                var _avance = result.map(x => x.avance);
                                                var cantidad = _cantidad.reduce(function (a, b) {
                                                    return a + b;
                                                });
                                                var avance = _avance.reduce(function (a, b) {
                                                    return a + b;
                                                });
                                                avance = Math.round(avance * 100) / 100;
                                                var avancePercentage = Math.round((avance / cantidad) * 100);
                                                $('#avance').html('Avance: ' + avance + ' ' + unidad);
                                                $('#avanceGrafico').css('width', `${avancePercentage}%`)
                                                drawGraficoPartida(
                                                    result.map(x => x.grupo),
                                                    _cantidad,
                                                    _avance
                                                );
                                            },
                                            error: (error) => {
                                                console.log(error);
                                                //alert('No está cargado el modelo');
                                            }
                                        });
                                    }, (error) => {
                                        console.log(error);
                                    });
                                });
                            });
                        } else {
                            confirm('No está cargado el modelo. ¿Desea cargarlo?');
                        }
                    }
                });
            }
        });
    }
}

function getParameterAvance() {
    var nombreParametro = document.getElementById('parametroAvance').value;
    if (nombreParametro === undefined || nombreParametro === '') {
        nombreParametro = 'SACYR_UBICACION';
    }
    return nombreParametro;
}

async function getExternalIds(data, mapping) {
    return data.map(x => mapping[x.externalId]);
}

function distinct(value, index, self) {
    return self.indexOf(value) === index;
}
