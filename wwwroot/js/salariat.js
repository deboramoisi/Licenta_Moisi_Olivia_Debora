var dataTable;

$(document).ready(function () {
    loadDataTable();
    DatePicker();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Admin/Salariats/GetAll",
        },
        "columns": [
            { "data": "client.denumire" },
            { "data": "nume" },
            { "data": "prenume" },
            { "data": "functie" },
            {
                "data": "salariatId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Salariats/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/Salariats/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/Salariats/DeleteAPI/${data}") class="btn btn-danger">
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

function DatePicker() {
    $('input[class=date_year]').datepicker({
        dateFormat: "dd/M/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "-60:+0"
    }); 
}