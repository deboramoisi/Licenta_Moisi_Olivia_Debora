var dataTable;
var idCategorie = 58;
var tabel = "";

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    $.ajax({
        type: "GET",
        url: "/Clienti/Documente/FilterByCategorie/" + idCategorie,
        success: function (data) {
            if (data.documente.length != 0) {
                console.log(data.documente);
                $.each(data.documente, function (i, element) {
                    var filePath = "/img/documente/" + element.documentPath;
                    var icon = getExtensie(element.documentPath);
                    tabel += `<tr>
                                <th><a href="${filePath}" target="_blank"><i class="${icon}"></i> Vizualizare document </a></th>
                                <th>${element.tipDocument.denumire}</th>
                                <th>${element.data}</th>
                            </tr>`;
                });
                $("#tblData").html(tabel);
            } else {
                $("#tblData").html("<tr><th></th><th>Nu exista documente din aceasta categorie</th><th></th></tr>");
            }
        },
        error: function (data) {
            console.log("error");
        }
    })
}

function FilterByCategorie(element) {
    idCategorie = element;
    tabel = "";
    loadDataTable();
}