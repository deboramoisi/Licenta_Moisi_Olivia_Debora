var dataTable;

$(function () {
    loadDataTable();
    deleteFurnizori();
    var placeholderElement = $('#modal-furnizori-import-ph');
    $('button[data-toggle="ajax-furnizori-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            // cautam elementul cu clasa modal
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
        console.log(files[0].name, files[0]);
        formData.append(files[0].name, files[0]);

        var clientIdInput = $('#clientId').val();
        console.log(clientIdInput);

        actionUrl += '/' + clientIdInput;

        $.ajax({
            type: 'POST',
            url: actionUrl,
            contentType: false,
            processData: false,
            data: formData,
            success: function (message) {
                //var newBody = $('.modal-body', data);
                //placeholderElement.find('.modal-body').replaceWith(newBody);

                //var isValid = newBody.find('[name="IsValid"]').val() == 'True';
                //if (isValid) {
                    placeholderElement.find('.modal').modal('hide');

                    // toastr pentru succes
                    toastr["success"]("Furnizori importati cu succes!")

                    toastr.options = {
                        "closeButton": false,
                        "debug": false,
                        "newestOnTop": false,
                        "progressBar": false,
                        "positionClass": "toast-top-right",
                        "preventDuplicates": false,
                        "onclick": null,
                        "showDuration": "300",
                        "hideDuration": "1000",
                        "timeOut": "5000",
                        "extendedTimeOut": "1000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    }

                dataTable.ajax.reload();
                //}
            },
            error: function (err) {
                alert(err.message);
            }
        });

    })
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // folosim ajax pt incarcarea datelor
        "ajax": {
            "url": "/Admin/Furnizori/GetAllFurnizori"
        },
        "columns": [
            { "data": "denumire" },
            { "data": "cod_fiscal" },
            { "data": "tara" },
            { "data": "client.denumire" }, 
            {
                "data": "furnizorID",
                // functie cu id-ul furnizorului pt returnare buton edit, details, delete
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
            // cautam elementul cu clasa modal
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
                //var newBody = $('.modal-body', data);
                //placeholderElement.find('.modal-body').replaceWith(newBody);

                //var isValid = newBody.find('[name="IsValid"]').val() == 'True';
                //if (isValid) {
                placeholderElement.find('.modal').modal('hide');

                // toastr pentru succes
                toastr["success"]("Furnizori stersi cu succes!")

                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": false,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false,
                    "onclick": null,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                }

                dataTable.ajax.reload();
                //}
            },
            error: function (err) {
                alert(err.message);
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

function ReloadPage(timeout) {
    setTimeout(function () { window.location.reload(true); }, timeout);
}