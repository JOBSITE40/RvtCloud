var certPanel;
var fasesPanel;

class ExtensionCertificacion extends Autodesk.Viewing.Extension {
    constructor(viewer, options) {
        super(viewer, options);
    }

    load() {
        $.notify('Se ha cargado el módulo de Certificación.', { className: "info" });
        return true;
    }

    unload() {
        return true;
    }

    onToolbarCreated() {
        certPanel = new DockingPanel(viewer, viewer.container, '_certPanel', 'Info Partida', null, 'certDiv');
        fasesPanel = new FasesPanel(viewer, viewer.container, '_fasesPanel', 'Fase y Porcentaje');
    }
}

Autodesk.Viewing.theExtensionManager.registerExtension('ExtensionCertificacion', ExtensionCertificacion);