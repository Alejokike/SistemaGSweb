function InitTable(ID) {
    
    var Table = $(ID);
    if (Table.length && Table != null) {
        //if ($.fn.DataTable.isDataTable(ID)) {
        DestroyTable(ID)
        //}
        $(ID).DataTable({
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
    var Table = $(ID);
    if ((Table.length && Table != null) && $.fn.DataTable.isDataTable(Table)) {
        Table.DataTable().destroy();
    }
}