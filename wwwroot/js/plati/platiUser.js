var dataTable;

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
                    achitata: "achitata"
                },
                "render": function (data) {

                    // butonul de plateste se genereaza dinamic doar in cazul platilor de servicii care nu sunt achitate
                    if (data.tipPlata.denumire == "Servicii" && data.achitata == false) {
                        console.log(typeof (data.achitata));
                        console.log(data.achitata);
                        return `<button type="button" id="checkout-button">Plateste</button>`;
                    } else {
                        return ``;
                    }
                }
            }
        ]
    });
}