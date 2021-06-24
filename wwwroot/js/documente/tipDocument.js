var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // folosim ajax pt incarcarea datelor
        "ajax": {
            "url": "/Admin/TipDocuments/GetAll"
        },
        "columns": [
            { "data": "denumire", "width": "60%" },
            {
                "data": "tipDocumentId",
                // functie cu id-ul furnizorului pt returnare buton edit, details, delete
                "render": function(data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/TipDocuments/Edit/${data}" class="btn btn-success">
                                <i class="fa fa-pencil-square" aria-hidden="true"></i>
                            </a> 
                            <a href="/Admin/TipDocuments/Details/${data}" class="btn btn-info">
                                <i class="fa fa-info-circle" aria-hidden="true"></i>
                            </a>
                            <a onclick=Delete("/Admin/TipDocuments/DeleteAPI/${data}") class="btn btn-danger">
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
    $("#tipDocumentInput").modal("show");
}

function onCloseModal() {
    $("#tipDocumentInput").modal("hide");
}

function onSubmit() {
    var form = $("#tipDocumentForm");
    var actionUrl = form.attr('action');
    var sendData = form.serialize();
    console.log(sendData);
    sendData = JSON.stringify(sendData);
    console.log(sendData);

    $.ajax({
        url: actionUrl,
        method: "POST",
        data: sendData,
        dataType: "json",
        contentType: "application/json",
        success: function () {
            toastr.success("Tip Document creat cu success!");
            onCloseModal();
        },
        error: function (e) {
            toastr.error(e);
        }
    });

}