﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Clienti/IncarcariClient/GetAll",
        },
        "columns": [
            {
                "data": "documentPath",
                "render": function (data) {
                    var filePath = "/img/documente/" + data;
                    var icon = getExtensie(data);
                    return `<a href="${filePath}" target="_blank"><i class="${icon}"></i> Vizualizare document </a>`;
                }
            },
            { "data": "tipDocument.denumire" },
            { "data": "data" },
            {
                "data": "documentId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Delete("/Clienti/IncarcariClient/DeleteAPI/${data}") class="btn btn-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, "autoWidth": true

            }
        ]
    });
}