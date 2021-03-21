var dataTable;

$(function () {
    loadDataTable();
    deleteFurnizori();
    var placeholderElement = $('#modal-furnizori-import-ph');
    $('button[data-toggle="ajax-furnizori-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');

        var formData = new FormData();

        var fileInput = $('#fileUpload').get(0);
        var files = fileInput.files;
        formData.append(files[0].name, files[0]);

        var clientIdInput = $('#clientId').val();

        actionUrl += '/' + clientIdInput;

        $.ajax({
            type: 'POST',
            url: actionUrl,
            contentType: false,
            processData: false,
            data: formData,
            success: function (message) {
                placeholderElement.find('.modal').modal('hide')
                toastrAlert("success", "Furnizori importati cu succes!")
                dataTable.ajax.reload()
            }, error: function () {
                toastrAlert("error", "Eroare la importarea furnizorilor!")
            }
           
        });

    })
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Furnizori/GetAllFurnizori"
        },
        "columns": [
            { "data": "denumire" },
            { "data": "cod_fiscal" },
            { "data": "client.denumire" }, 
            {
                "data": "furnizorID",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Furnizori/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/Furnizori/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/Furnizori/DeleteAPI/${data}") class="btn btn-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }
            }
        ]
    });
}

function deleteFurnizori() {
    var placeholderElement = $('#modal-furnizori-import-ph');
    $('button[data-toggle="ajax-del-furn-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var data = form.serialize();

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: data,
            success: function (message) {
                placeholderElement.find('.modal').modal('hide')
                toastrAlert("success", "Furnizori stersi cu succes!")
                dataTable.ajax.reload();
            }, error: function () {
                toastrAlert("error", "Eroare la stergerea furnizorilor!")
            }
        });

    })
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