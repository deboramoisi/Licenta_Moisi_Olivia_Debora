var selectedClient = 1;
var selectedName = "Contsal";
var selectedYear = 2020;
var solduri = [[]];
var lineChart, barPrChart, stackedBar;

$(document).ready(function () {

    GetDenumire();
    GetProfitPierdereAJAX();
    GetSolduri();

}); 

function FilterByClient(client, year) {
    selectedYear = (year !== '') ? year : '2020';
    if (client !== '') {
        selectedClient = client;
    }
    console.log(selectedClient, selectedYear);

    GetDenumire();
    GetProfitPierdereAJAX();
    GetSolduri();
   
}

function GetProfitPierdereAJAX() {

    $.ajax({
        type: "GET",
        url: "/Admin/Dashboard/GetProfitPierdere?id=" + selectedClient + "&an=" + selectedYear,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (!isEmpty(data)) {
                // daca avem date pentru perioada selectata atunci afisam vizualizarile
                $("#myChart").css("display", "block");
                $("#lineChartProfit").css("display", "block");
                $("[name = 'profitPierdere']").html(selectedName + " " + selectedYear + " - profit/pierdere");
                if (barPrChart != null) {
                    RemoveDataset(barPrChart);
                }
                BarForProfitPierdere(data);
                if (lineChart != null) {
                    RemoveDataset(lineChart);
                }
                LineForProfitPierdere(data);
            } else {
                // daca nu atunci afisam mesaj coresp + display none pentru vizualizari
                $("[name = 'profitPierdere']").html("Nu exista date disponibile pentru aceasta perioada");
                $("#myChart").css("display", "none");
                $("#lineChartProfit").css("display", "none");
            }
        }
    });

}

function GetSolduri() {

    $.get("/Admin/Dashboard/GetSolduriCasa?id=" + selectedClient + "&an=" + selectedYear)
        .done(function (data) {
        if (data[0].length !== 0) {
            solduri = data;
            $("#chart2").css("display", "block");
            $("[name = 'sold']").html(selectedName + " " + selectedYear + "- solduri casa stacked bar");
            if (stackedBar != null) {
                RemoveDataset(stackedBar);
            }
            StackedBarHorizontal(solduri);
        } else {
            solduri = [[]];
            $("[name = 'sold']").html("Nu exista date disponibile pentru aceasta perioada");
            $("#chart2").css("display", "none");
        }
    });

}

function GetDenumire(tip) {

    $.get("/Admin/Dashboard/GetDenumireClient?id=" + selectedClient).done(function (data) {
        selectedName = data;
    });

}