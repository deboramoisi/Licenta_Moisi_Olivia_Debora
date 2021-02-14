var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Admin/IstoricSalars/GetAll",
        },
        "columns": [
            { "data": "salariat.numePrenume" },
            { "data": "salariat.pozitie" },
            { "data": "dataInceput" },
            {
                "data": "dataSfarsit",
                "defaultContent": "<i>Not set</i>"
            },
            { "data": "salariu" },
            {
                "data": "istoricSalarId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/IstoricSalars/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/IstoricSalars/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/IstoricSalars/DeleteAPI/${data}") class="btn btn-danger">
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

$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    // On Click pe butonul de adaugare
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        // url e metoda care se apeleaza din controller
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            // afisare fereastra modala
            PlaceHolderElement.find('.modal').modal('show');
        })
    })

    // Cand se da click pe "Save" button
    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            // Dupa adaugare date in tabel, inchidem fereastra modala
            PlaceHolderElement.find('.modal').modal('hide');
            dataTable.ajax.reload();
        })

    })

})