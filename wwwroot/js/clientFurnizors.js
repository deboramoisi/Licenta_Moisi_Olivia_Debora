var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // folosim ajax pt incarcarea datelor
        "ajax": {
            "url": "/Admin/ClientFurnizors/GetAll"
        },
        "columns": [
            { "data": "client.denumire", "width": "30%" },
            { "data": "furnizor.denumire", "width": "30%" },
            {
                "data": "clientFurnizorId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/ClientFurnizors/Edit/${data}" class="btn btn-success">
                                    <i class="fa fa-pencil-square" aria-hidden="true"></i>
                                </a> 
                                <a href="/Admin/ClientFurnizors/Details/${data}" class="btn btn-info">
                                    <i class="fa fa-info-circle" aria-hidden="true"></i>
                                </a>
                                <a onclick=Delete("/Admin/ClientFurnizors/DeleteAPI/${data}") class="btn btn-danger">
                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                </a>
                            </div>
                            `;
                }, "width": "40%"
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