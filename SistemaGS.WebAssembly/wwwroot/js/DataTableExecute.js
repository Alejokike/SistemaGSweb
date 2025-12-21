function InitTable(ID) {
    var Table = $(ID);
    if (Table !== null && Table.length > 0 && !($.fn.dataTable.isDataTable(ID))) {
        new DataTable( ID, {
            paging: true,
            searching: true,
            ordering: true,
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
            }
        });
        /*
        $(ID).DataTable({
            paging: true,
            searching: true,
            ordering: true,
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
            }
        });
        */
    }
}
function DestroyTable(ID) {
    var Table = $.fn.DataTable.isDataTable(ID) ? $(ID) : null;
    if (Table !== null && Table.length > 0) {
        Table.DataTable().page(0).draw(false);
        Table.DataTable().destroy();
    }
    /*
    var Table = $(ID);
    if (Table !== null && Table.length > 0 && $.fn.DataTable.isDataTable(ID)) {
        Table.DataTable().page(0).draw(false);
        Table.DataTable().destroy();
    }
    */
}
function RefreshTable(ID) {
    var table = $.fn.DataTable.isDataTable(ID) ? $(ID).DataTable() : null;
    if (table !== null && table.length > 0) {
        table.page(0).draw(false);
        table.destroy();
    }
}