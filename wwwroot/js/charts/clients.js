var selectedYear = "2020";
var solduri;

function FilterByYear(year) {
    selectedYear = (year !== '') ? year : '2020';
    GetSolduri();
    GetProfitPierdereAJAX();
}

$(document).ready(function () {

    GetSolduri();
    GetDenumire();
    GetProfitPierdereAJAX();
        
}); 

function GetProfitPierdereAJAX() {
    $.ajax({
        type: "GET",
        url: "/Clienti/DashboardClient/GetProfitPierdere?an=" + selectedYear,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (!isEmpty(data)) {
                // daca avem date pentru perioada selectata atunci afisam vizualizarile
                $("#myChart").css("display", "block");
                $("#lineChartProfit").css("display", "block");
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

function GetDenumire() {
    $.get("/Clienti/DashboardClient/GetDenumireClient").done(function (data) {
        $("[name = 'denumire']").html(data); 
    });
}

function GetSolduri() {
    $.get("/Clienti/DashboardClient/GetSolduriCasa?an=" + selectedYear).done(function (data) {
        if (data[0].length !== 0) {
            solduri = data;
        } else {
            solduri = [[]];
        }
    });

    if (!isEmpty(solduri)) {
        $("#chart2").css("display", "block");
        StackedBar(solduri);
    } else {
        $("[name = 'sold']").html("Nu exista date disponibile pentru aceasta perioada");
        $("#chart2").css("display", "none");
    }
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