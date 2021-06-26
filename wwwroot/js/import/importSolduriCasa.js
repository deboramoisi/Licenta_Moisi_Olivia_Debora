var dataTable;

$(function () {
    loadDataTable();
    deleteSolduri();
    var placeholderElement = $('#modal-solduri-import-ph');
    $('button[data-toggle="ajax-solduri-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal-casa"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');

        var formData = new FormData();

        var fileInput = $('#fileUpload').get(0);

        if (!returnAlertWrongExtension(fileInput)) {
            return false;
        } 

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
                toastrAlert("success","Solduri casa importate cu success!")
                dataTable.ajax.reload();               
            }, error: function () {
                toastrAlert("error", "Eroare la importarea soldurilor!")                
            }
           
        });

    })
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/SolduriCasa/GetAllSolduri"
        },
        "columns": [
            { "data": "client.denumire" },
            { "data": "data" },
            { "data": "sold_prec" },
            { "data": "incasari" },
            { "data": "plati" },
            { "data": "sold_zi" }, 
            {
                "data": "solduriCasaId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/SolduriCasa/Edit/${data}" class="btn btn-outline-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/SolduriCasa/Details/${data}" class="btn btn-outline-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/SolduriCasa/DeleteAPI/${data}") class="btn btn-outline-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, width: "20%"
            }
        ]
    });
}

function deleteSolduri() {
    var placeholderElement = $('#modal-solduri-import-ph');
    $('button[data-toggle="ajax-del-sold-modal"]').click(function (event) {
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
                placeholderElement.find('.modal').modal('hide');
                toastrAlert("success","Solduri sterse cu succes!")
                dataTable.ajax.reload();
            }, error: function () {
                toastrAlert("error", "Eroare la stergerea soldurilor!")
            }
        });

    })
}
