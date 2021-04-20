var dataTable;

$(document).ready(function () {
    loadDataTable();

    var url = "/Admin/TipPlata/Create";
    var placeholderElement = $('#tipPlata-modal');
    $('button[data-toggle="ajax-tipPlata-modal"]').click(function (event) {
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal-tp"]', function (event) {
        event.preventDefault();

        var form = $('#formTip');
        console.log(form);
        var data = form.serialize();
        console.log(data);

        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            success: function () {
                    placeholderElement.find('.modal').modal('hide');
                    toastrAlert("success", "Tip plata adaugat cu succes!");
                    dataTable.ajax.reload();
            },
            error: function () {
                toastrAlert("error", "Eroare la adaugarea tipului!");
            }
        });
    })
    
});

function transmiteId(id) {
    var url = "/Admin/TipPlata/Edit";
    var placeholderElement = $('#tipPlata-modal');

    $.get(url + "/" + id).done(function (data) {
        placeholderElement.html(data);
        placeholderElement.find('.modal').modal('show');
    });

    placeholderElement.on('click', '[data-save="modal-tp-edit"]', function (event) {
        event.preventDefault();
        var form = $('#formTipEdit');
        var data = form.serialize();
        console.log(data);

        $.ajax({
            url: url,
            type: 'POST',
            data: data,
            success: function () {
                placeholderElement.find('.modal').modal('hide');
                toastrAlert("success", "Tip plata modificat cu succes!");
                dataTable.ajax.reload();
            },
            error: function (err) {
                toastrAlert("error", "Eroare la modificarea tipului!");
                console.error(err);
            }
        });
    })
}

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/TipPlata/GetAll"
        },
        "columns": [
            { "data": "denumire", "width": "60%" },
            {
                "data": "tipPlataId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <button onclick="transmiteId(${data})" type="button" data-target="#edit-tipPlata" data-toggle="ajax-tipPlata-edit-modal" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </button>

                            <a onclick=Delete("/Admin/TipPlata/DeleteAPI/${data}") class="btn btn-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, "width": "40%"
            }
        ]
    });
}