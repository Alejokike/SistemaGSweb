function InitTable(ID) {
    var Table = $(ID);
    if (Table !== null && Table.length > 0 && !($.fn.dataTable.isDataTable(ID))) {
        if ($.fn.dataTable.isDataTable(ID)) DestroyTable()
        new DataTable(ID, {
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
function InitTableInventarioAjax(ID) {
    var Table = $(ID);
    if (Table !== null && Table.length > 0 && !($.fn.dataTable.isDataTable(ID))) {
        $(ID).DataTable({
            paging: true,
            searching: true,
            ordering: true,
            pageLength: 10,
            stateSave: true,
            lengthMenu: [
                [10, 25, 50, 100, -1],
                [10, 25, 50, 100, "Todos"]
            ],
            language: {
                url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
            },
            deferRender: true,
            serverSide: true,
            processing: true,
            ajax: {
                url: 'http://localhost:5006/api/Inventario/ItemsAjax',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: function (data) {
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: 'IdItem'
                },
                {
                    data: 'Nombre'
                },
                {
                    data: 'Categoria'
                },
                {
                    data: 'Unidad'
                },
                {
                    data: 'Cantidad',
                    render: function (data, type, row) {
                        if (row.Unidad === "VE") {
                            return parseFloat(data).toLocaleString('es-VE', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        } else {
                            return parseFloat(data).toLocaleString('es-VE', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
                        }
                    }
                },
                {
                    data: 'Dummy',
                    orderable: false,
                    searchable: false,
                    render: function (data, type, row) {
                        return `<button type="button" class="btn btn-outline-light text-center" onclick="verDetalleItem(${row.IdItem})"><i class="bi bi-box-arrow-up-right text-primary fs-5"></i></button>`;
                    }
                }
            ]
        });
    }    
}
function verDetalleItem(id) {
    DotNet.invokeMethodAsync('SistemaGS.WebAssembly', 'MostrarDetalleItem', id);
}
function DestroyTable(ID) {
    var Table = $.fn.DataTable.isDataTable(ID) ? $(ID) : null;
    if (Table !== null && Table.length > 0 && $.fn.DataTable.isDataTable(ID)) {
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