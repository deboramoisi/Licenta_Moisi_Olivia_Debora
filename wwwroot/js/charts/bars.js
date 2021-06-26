var incasari, plati;

function BarForProfitPierdere(prPierdere) {

    var ctx = document.getElementById('myChart').getContext('2d');
    barPrChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Jan', 'Febr', 'Mar', 'Apr', 'Mai', 'Iun', 'Iul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: '# of Profit',
                data: prPierdere,
                backgroundColor: [
                    "#FFECFF",
                    "#FFDFFF",
                    "#FFCEFF",
                    "#FFBBFF",
                    "#F4D2F4",
                    "#E8C6FF",
                    "#DFB0FF",
                    "#EAF1FB",
                    "#CD85FE",
                    "#CEDEF4",
                    "#DBF0F7",
                    "#A095EE"
                ],
                borderColor: [
                    "#F4D2F4",
                    "#F4D2F4",
                    "#F0C4F0",
                    "#EEBBEE",
                    "#BC2EBC",
                    "#E1CAF9",
                    "#892EE4",
                    "#6094DB",
                    "#9B4EE9",
                    "#6094DB",
                    "#A5DBEB",
                    "#6755E3"
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

function StackedBarHorizontal(data) {

    incasari = (data[0].length != 0 ) ? data[0] : '';
    plati = (data[1].length != 0) ? data[1] : '';
    months = ['Ian', 'Feb', 'Mar', 'Apr', 'Mai', 'Iun', 'Iul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var availableMonths = months.slice(0, Math.max(plati.length, incasari.length));

    var ctx2 = document.getElementById('chart2');

    stackedBar = new Chart(ctx2, {
        type: 'bar',
        data: {
            labels: availableMonths,
            datasets: [
                {
                    label: 'Incasari',
                    data: incasari,
                    backgroundColor: '#D6E9C6',
                },
                {
                    label: 'Plati',
                    data: plati,
                    backgroundColor: '#FAEBCC',
                }
            ]
        },
        options: {
            indexAxis: 'y',
            elements: {
                bar: {
                    borderWidth: 2,
                }
            },
            responsive: true,
            plugins: {
                legend: {
                    position: 'right',
                },
                title: {
                    display: true,
                    text: 'Solduri Casa Horizontal Bar'
                }
            }
        }
    })
}