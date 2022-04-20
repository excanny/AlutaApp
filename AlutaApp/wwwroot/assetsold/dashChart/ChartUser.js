
let getDataVariable = {};
let reportDate = [];
let reportValue = [];
$(document).ready(function () {
    debugger;
    getData();
});

function getData() {
    debugger;
    $.ajax({
        url: '/Charts/Charts',
        type: 'post',
        dataType: 'json',
        contentType: 'application/json',
        success: function (resp) {
            var rest = JSON.stringify(resp);
            let dater = JSON.parse(rest);

            console.log(dater.date[0]);
            console.log(dater.value[0]);
            getDataVariable = rest;
            let daters = JSON.parse(getDataVariable);
            console.log("getDataVariable" + getDataVariable);
            reportDate = daters.date[0];
            reportValue = daters.value[0];
            document.getElementById("dat").textContent = reportDate;
            document.getElementById("val").textContent = reportValue;
            console.log(reportDate);
            console.log(reportValue);
            return dater;
        },
        error: function (err) {
            console.log("Error" + err);
        }
    });


function filterData() {
    const date2 = [...dates];
    console.log(date2);
    const startdate = document.getElementById('startdate');
    const enddate = document.getElementById('enddate');
    const indexstartdate = date2.indexOf(startdate.value);
    const indexenddate = date2.indexOf(enddate.value);
    console.log(indexstartdate);
    console.log(indexenddate);
    const filterDate = date2.slice(indexstartdate, indexenddate + 1);
    console.log(filterDate);
    myChart.config.data.labels = filterDate;
    const datapoint2 = [...datapoints];
    const filterDatapoints = datapoint2.slice(indexstartdate, indexenddate + 1);
    myChart.config.data.datasets[0].data = filterDatapoints;
    myChart.update();
}
// setup 
const dates = reportDate;
console.log("dates" + document.getElementById("dat").innerHTML);
const datapoints = reportValue;
console.log("datapoints" + document.getElementById("val").innerHTML);
const data = {
    labels: dates,
    datasets: [{
        label: 'Weekly Sales',
        data: datapoints,
        backgroundColor: [
            'rgba(255, 26, 104, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)',
            'rgba(0, 0, 0, 0.2)'
        ],
        borderColor: [
            'rgba(255, 26, 104, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)',
            'rgba(0, 0, 0, 1)'
        ],
        borderWidth: 1
    }]
};

// config 
const config = {
    type: 'bar',
    data,
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
};

// render init block
const myChart = new Chart(
    document.getElementById('myChart'),
    config
);
