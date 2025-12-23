function InitTable(ID) {
    var Table = $('#' + ID);
    if (Table !== null && Table.length > 0 && !($.fn.dataTable.isDataTable('#' + ID))) {
        if ($.fn.dataTable.isDataTable('#' + ID)) DestroyTable()
        new DataTable('#' + ID, {
            paging: true,
            searching: true,
            ordering: true,
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
            }
        });
    }
}
function DestroyTable(ID) {
    var Table = $.fn.DataTable.isDataTable('#' + ID) ? $('#' + ID) : null;
    if (Table !== null && Table.length > 0 && $.fn.DataTable.isDataTable('#' + ID)) {
        Table.DataTable().page(0).draw(false);
        Table.DataTable().destroy();
    }
}