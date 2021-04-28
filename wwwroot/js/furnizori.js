var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // folosim ajax pt incarcarea datelor
        "ajax": {
            "url": "/Clienti/Informatii/GetFurnizori"
        },
        "columns": [
            { "data": "denumire" },
            { "data": "cod_fiscal" },
            {
                "data": "furnizorID",
                // functie cu id-ul furnizorului pt returnare buton edit, details, delete
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Clienti/Furnizori/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }
            }
        ]
    });
}