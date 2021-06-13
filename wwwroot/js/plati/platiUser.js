var dataTable;
var plataId;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Clienti/Informatii/GetPlati",
        },
        "columns": [
            { "data": "tipPlata.denumire" },
            { "data": "suma" },
            { "data": "data" },
            { "data": "dataScadenta" },
            {
                "data": {
                    tipPlata: "tipPlata.denumire",
                    plataId: "plataId",
                    achitata: "achitata"
                },
                "render": function (data) {

                    // butonul de plateste se genereaza dinamic doar in cazul platilor de servicii care nu sunt achitate
                    if (data.tipPlata.denumire == "Servicii" && data.achitata == false) {
                        plataId = data.plataId;
                        console.log(plataId);
                        return `<button type="button" id="checkout-button">Plateste</button>`;
                    } else if (data.achitata == true) {
                        return '<button type="button" class="btn btn-dark col-10" disabled>Plata achitata</button>';
                    } else {
                        return ``;
                    }
                }
            }
        ]
    });
}
