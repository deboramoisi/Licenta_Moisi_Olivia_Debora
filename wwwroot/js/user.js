var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        // folosim ajax pt incarcarea datelor
        "ajax": {
            "url": "/Admin/ApplicationUsers/GetAll"
        },
        "columns": [
            { "data": "nume" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "pozitieFirma" },
            {
                "data": "client.denumire",
                "defaultContent": "<i> -- Client individual --</i>"
            },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                // functie cu id-ul user-ului pt returnare buton edit, details, delete
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        // user is currently locked
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger">
                                <i class="fa fa-lock-open" aria-hidden="true"></i>
                            </a>
                            <a href="/Admin/ApplicationUsers/Edit/${data.id}" class="btn btn-outline-success">
                                <i class="fas fa-user-edit"></i>
                            </a>
                            <a onclick=Delete("/Admin/ApplicationUsers/DeleteAPI/${data.id}") class="btn btn-outline-danger">
                                <i class="fas fa-user-times"></i>
                            </a>
                        </div>
                        `;
                    }
                    else {
                        // user-ul nu e blocat
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success">
                                <i class="fas fa-user-lock"></i>
                            </a>
                            <a href="/Admin/ApplicationUsers/Edit/${data.id}" class="btn btn-outline-success">
                                <i class="fas fa-user-edit"></i>
                            </a>
                            <a onclick=Delete("/Admin/ApplicationUsers/DeleteAPI/${data.id}") class="btn btn-outline-danger">
                                <i class="fas fa-user-times"></i>
                            </a>
                        </div>
                        `;
                    }
                }, width: "18%"
            }
        ]
    });
}

// Functie pentru lock/unlock user
function LockUnlock(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/ApplicationUsers/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                // Reload tabel dupa lock/unlock si afisare mesaj de succes
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
        
        
    });

}