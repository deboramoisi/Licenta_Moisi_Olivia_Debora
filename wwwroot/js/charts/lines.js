var selectedClient = 1;

function FilterByClient(client) {
    selectedClient = client;
    $.ajax({
        type: "GET",
        url: "/Admin/Dashboard/GetProfitPierdere?id=" + selectedClient + "&an=2020",
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            BarForProfitPierdere(data);
            LineForProfitPierdere(data);
            GetDenumire();
        }
    });
}

$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: "/Admin/Dashboard/GetProfitPierdere?id=" + selectedClient + "&an=2020",
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            BarForProfitPierdere(data);
            LineForProfitPierdere(data);
            GetDenumire();
        }
    });
        
}); 

function GetDenumire() {
    $.get("/Admin/Dashboard/GetDenumireClient?id=" + selectedClient).done(function (data) {
        $("[name = 'denumire']").html(data); 
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