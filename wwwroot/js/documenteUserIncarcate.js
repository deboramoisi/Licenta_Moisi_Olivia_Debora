var dataTable;

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