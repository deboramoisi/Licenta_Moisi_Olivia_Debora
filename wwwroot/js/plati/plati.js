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
                "data": {
                    achitata: "achitata",
                    tipPlata: "tipPlata.denumire"
                },
                "render": function (data) {
                    if (data.tipPlata.denumire == "Servicii") {
                        if (data.achitata == true) {
                            return '<button class="btn btn-outline-success" disabled><i class="far fa-check-circle"></i></button>';
                        } else {
                            return '<button class="btn btn-outline-danger text-center" disabled><i class="far fa-times-circle"></i></button>';
                        }
                    } else {
                        return ``;
                    }
                }
            },
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