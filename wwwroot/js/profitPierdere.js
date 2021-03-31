var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Admin/ProfitPierdere/GetAll",
        },
        "columns": [
            { "data": "clientId" },
            { "data": "rulaj_d" },
            { "data": "rulaj_c" },
            { "data": "profit_luna" },
            { "data": "pierdere_luna" },
            { "data": "month" },
            { "data": "year" },
            {
                "data": "profitPierdereId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/ProfitPierdere/Edit/${data}" class="btn btn-outline-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/ProfitPierdere/Details/${data}" class="btn btn-outline-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/ProfitPierdere/DeleteAPI/${data}") class="btn btn-outline-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, "autoWidth": true

            }
        ]
    });
}
