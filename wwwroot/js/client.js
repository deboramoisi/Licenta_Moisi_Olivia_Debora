var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Admin/Clients/GetAll",
        },
        "columns": [
            { "data": "denumire" },
            { "data": "nrRegComertului" },
            { "data": "codCAEN" },
            {
                "data": "tipFirma",
                "render": function (data) {
                    if (data == "Persoana Fizica") {
                        return `PF`;
                    } else if (data == "Persoana juridica") {
                        return `PJ`;
                    } else {
                        return 'S.Coop';
                    }
                }
            },
            { "data": "capitalSocial" },
            { "data": "tva" },
            {
                "data": "sediuSocial.telefon",
                "defaultContent": "<i>Not set</i>"
            },
            {
                "data": "sediuSocial.email",
                "defaultContent": "<i>Not set</i>"
            },
            //{
            //    "data": "clientFurnizori",
            //    "defaultContent": "<i>Not set yet</i>",
            //    "render": function (data) {
            //        var item = "";
            //        for (var i = 0; i < data.length; i++) {
            //            if (i == data.length - 1) {
            //                item += data[i].furnizor.denumire;
            //            } else {
            //                item += data[i].furnizor.denumire + ",";
            //            }
            //        }
            //        return item;
            //    }
            //},
            {
                "data": "clientId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Clients/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/Clients/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/Clients/DeleteAPI/${data}") class="btn btn-danger">
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
