var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Documents/GetAll",
        },
        "columns": [
            {
                "data": "documentPath",
                "render": function (data) {
                    var filePath = "/img/documente/" + data;
                    var icon = getExtensie(data);
                    return `<a href="${filePath}" target="_blank">
                               <i class="${icon}"></i>&nbsp; Vizualizare document
                            </a>`;
                }
            },
            { "data": "client.denumire" },
            { "data": "applicationUser.nume" },
            { "data": "tipDocument.denumire" },
            { "data": "data" },
            {
                "data": "documentId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Documents/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/Documents/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/Documents/DeleteAPI/${data}") class="btn btn-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, "autoWidth": true

            }
        ]
    });
}