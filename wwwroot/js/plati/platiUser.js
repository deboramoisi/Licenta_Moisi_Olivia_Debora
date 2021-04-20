var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Clienti/Plata/GetAll",
        },
        "columns": [
            { "data": "tipPlata.denumire" },
            { "data": "suma" },
            { "data": "data" },
            { "data": "dataScadenta" }
        ]
    });
}