var dataTable;

$(document).ready(function () {
    loadDataTable();
    deleteSolduriBalanta();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // functia ce returneaza toate elementele tabelulului
        "ajax": {
            "url": "/Admin/ProfitPierdere/GetAll",
        },
        "columns": [
            { "data": "client.denumire" },
            { "data": "rulaj_d" },
            { "data": "rulaj_c" },
            { "data": "profit_luna" },
            { "data": "pierdere_luna" },
            { "data": "month" },
            { "data": "year" },
            {
                "data": "profitPierdereId",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/ProfitPierdere/Edit/${data}" class="btn btn-outline-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/ProfitPierdere/Details/${data}" class="btn btn-outline-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/ProfitPierdere/DeleteAPI/${data}") class="btn btn-outline-danger">
                                <i class="fa fa-trash" aria-hidden="true"></i>
                            </a>
                        </div>
                            `;
                }, "autoWidth": true

            }
        ]
    });
}

function deleteSolduriBalanta() {
    var placeholderElement = $('#modal-solduri-delete-ph');
    $('button[data-toggle="ajax-del-profitP-modal"]').click(function (event) {
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
            success: function (data) {
                if (data.success) {
                    placeholderElement.find('.modal').modal('hide');
                    toastrAlert("success", data.message);
                    dataTable.ajax.reload();
                } else {
                    toastrAlert("error", data.message);
                }
                
            }, error: function (err) {
                console.log(err);
            }
        });

    })
}