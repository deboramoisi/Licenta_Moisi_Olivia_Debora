var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Clienti/Informatii/GetSalariati",
        },
        "columns": [
            { "data": "nume" },
            { "data": "prenume" },
            { "data": "functie" },
            { "data": "salar_brut" },
            {
                "data": "salariatId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Salariats/Edit/${data}" class="btn btn-outline-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/Salariats/Details/${data}" class="btn btn-outline-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }

            }
        ]
    });
}
