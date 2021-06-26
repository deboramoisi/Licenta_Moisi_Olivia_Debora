var selectedYear, selectedClient;
var solduri;
var lineChart, barPrChart, stackedBar;

$(document).ready(function () {

    GetDenumire();
    FilterByYear();

}); 

function FilterByYear(year) {
    selectedYear = (year != null) ? year : '2020';
    GetSolduri();
    GetProfitPierdereAJAX();
}

function GetProfitPierdereAJAX() {
    $.ajax({
        type: "GET",
        url: "/Clienti/DashboardClient/GetProfitPierdere?an=" + selectedYear,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data.length !== 0) {
                // daca avem date pentru perioada selectata atunci afisam vizualizarile
                $("#myChart").css("display", "block");
                $("#lineChartProfit").css("display", "block");
                $("[name = 'profitPierdere']").html(selectedClient + " " + selectedYear + " - profit/pierdere");
                LineForProfitPierdere(data);
                BarForProfitPierdere(data);
            } else { 
                // daca nu atunci afisam mesaj coresp + display none pentru vizualizari
                $("[name = 'profitPierdere']").html("Nu exista date disponibile pentru aceasta perioada"); 
                $("#myChart").css("display", "none");
                $("#lineChartProfit").css("display", "none");
            }
        }
    });
}

function GetDenumire() {
    $.get("/Clienti/DashboardClient/GetDenumireClient").done(function (data) {
        selectedClient = data;
        $("[name = 'denumire']").html(data); 
    });
}

function GetSolduri() {
    $.get("/Clienti/DashboardClient/GetSolduriCasa?an=" + selectedYear).done(function (data) {
        if (data[0].length !== 0 || data[1].length !== 0) {
            solduri = data;
            $("#chart2").css("display", "block");
            $("[name = 'sold']").html(selectedClient + " " + selectedYear + " - Solduri casa");
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