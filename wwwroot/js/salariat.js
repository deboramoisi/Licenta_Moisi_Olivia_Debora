var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Clienti/Salariati/GetAll",
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

function Delete(url) {
    swal({
        title: "Sunteti sigur?",
        text: "In urma stergerii, datele nu vor putea fi recuperate!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((sterge) => {
        if (sterge) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
