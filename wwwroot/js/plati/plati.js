var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Plata/GetAll",
        },
        "columns": [
            { "data": "client.denumire" },
            { "data": "tipPlata.denumire" },
            { "data": "suma" },
            { "data": "data" },
            { "data": "dataScadenta" },
            {
                "data": "plataId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Plata/Edit/${data}" class="btn btn-outline-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a onclick=Delete("/Admin/Plata/DeleteAPI/${data}") class="btn btn-outline-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, "autoWidth": true

            }
        ]
    });
}