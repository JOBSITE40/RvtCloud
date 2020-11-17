class FasesPanel extends Autodesk.Viewing.UI.DockingPanel {
    constructor(viewer, container, id, title, options) {
        super(container, id, title, options);
        this.viewer = viewer;

        this.container.classList.add('docking-panel-container-solid-color-a');
        this.container.style.top = "10px";
        this.container.style.left = "10px";
        this.container.style.width = "auto";
        this.container.style.height = "auto";
        this.container.style.resize = "auto";

        this.createTitleBar(title);

        var div = document.createElement('div');
        div.setAttribute('id', 'fasesPanel');
        //var html = [
        //    '<div id="panel-overlay">',
        //        '<div class="progress">',
        //            '<div id="avanceGrafico" class="progress-bar bg-success" role="progressbar" style="width: 0%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>',
        //        '</div>',
        //        '<ul>',
        //            '<li id="cantidad">Cantidad:</li>',
        //            '<li id="avance">Avance:</li>',
        //            '<li id="precio">Precio:</li>',
        //            '<li id="importe">Importe:</li>',
        //        '</ul>',
        //        '<hr />',
        //        '<canvas id="graficoPartida" width="400" height="400"></canvas>',
        //        '<hr />',
        //        '<button type="button" class="btn btn-success btn-sm" onclick="certificar()">Certificar Selección</button>',
        //    '</div>'
        //].join('\n');
        //div.innerHTML += html;
        this.container.appendChild(div);        
    }
}