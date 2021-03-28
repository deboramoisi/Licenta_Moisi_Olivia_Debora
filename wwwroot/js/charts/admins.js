var selectedClient = 1;
var selectedName = "Contsal";
var selectedYear = 2020;
var solduri = [[]];

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

$(document).ready(function () {

    GetDenumire();
    GetProfitPierdereAJAX();
    GetSolduri();
        
}); 

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
                BarForProfitPierdere(data);
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
        } else {
            solduri = [[]];
        }
    });

    if (!isEmpty(solduri[0])) {
        $("#chart2").css("display", "block");
        $("[name = 'sold']").html(selectedName + " " + selectedYear + "- solduri casa stacked bar");
        StackedBar(solduri);
    } else {
        $("[name = 'sold']").html("Nu exista date disponibile pentru aceasta perioada");
        $("#chart2").css("display", "none");
    }

}

function GetDenumire(tip) {

    $.get("/Admin/Dashboard/GetDenumireClient?id=" + selectedClient).done(function (data) {
        selectedName = data;
    });

}


function LineForProfitPierdere(prPierdere) {

    var ctx = document.getElementById('lineChartProfit').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Febr', 'Mar', 'Apr', 'Mai', 'Iun', 'Iul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: '# of Profit',
                data: prPierdere,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)',
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}