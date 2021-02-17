var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // folosim ajax pt incarcarea datelor
        "ajax": {
            "url": "/Admin/ApplicationUsers/GetAll"
        },
        "columns": [
            { "data": "nume" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "pozitieFirma" },
            { "data": "rol" },
            { "data": "client.denumire" },
            {
                "data": "id",
                // functie cu id-ul user-ului pt returnare buton edit, details, delete
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Delete("/Admin/ApplicationUsers/DeleteAPI/${data}") class="btn btn-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }
            }
        ]
    });
}

// url e metoda apelata din Controller pentru stergere user
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
                        // Reload tabel dupa stergere si afisare mesaj de succes
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