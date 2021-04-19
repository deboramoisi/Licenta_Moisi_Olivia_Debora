var dataTable;

$(document).ready(function () {
    loadDataTable();
});

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
                            <a href="/Admin/TipPlata/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
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

function ShowModal(event) {
    onShowModal(event, null);
}

function onShowModal(obj, isEventDetails) {
    $("#tipPlataInput").modal("show");
}

function onCloseModal() {
    $("#tipPlataInput").modal("hide");
}

function onSubmit() {
    var url = "/Admin/TipPlata/Create";
    var sendData = $('form').serialize();
    sendData = JSON.stringify(sendData);

    $.ajax({
        url: url,
        method: "POST",
        data: sendData,
        dataType: "json",
        contentType: "application/json",
        success: function () {
            toastr.success("Tip Plata creat cu success!");
            onCloseModal();
        },
        error: function (e) {
            toastr.error(e);
        }
    });
}