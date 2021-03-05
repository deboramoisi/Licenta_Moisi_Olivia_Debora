var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Admin/Documents/GetAllUser",
        },
        "columns": [
            {
                "data": "documentPath",
                "render": function (data) {
                    var filePath = "/img/documente/" + data;
                    return `<a href="${filePath}" target="_blank"> Vizualizare document </a>`;
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

function ShowModal(event) {
    onShowModal(event, null);
}

function onShowModal(obj, isEventDetails) {
    $("#tipDocumentInput").modal("show");
}