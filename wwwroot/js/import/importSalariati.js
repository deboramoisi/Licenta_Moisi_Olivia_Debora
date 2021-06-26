var dataTable;

$(function () {
    loadDataTable();
    
    var placeholderElement = $('#modal-salariati-import-ph');
    $('button[data-toggle="ajax-salariati-modal"]').click(function (event) {
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
            success: function () {

                placeholderElement.find('.modal').modal('hide');
                toastrAlert("success", "Salariati importati cu succes!")
                dataTable.ajax.reload();

            }
            
        });

    })

    // Delete salariati
    var placeholderElement = $('#modal-salariati-import-ph');
    $('button[data-toggle="ajax-del-sal-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            // cautam elementul cu clasa modal
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal-del"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var data = form.serialize();

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: data,
            success: function (message) {
                placeholderElement.find('.modal').modal('hide');
                toastrAlert("success", "Salariati stersi cu succes!")
                dataTable.ajax.reload();
            }, error: function (err) {
                toastrAlert("error", "Eroare la stergerea salariatilor!")
            }
        });

    })

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
