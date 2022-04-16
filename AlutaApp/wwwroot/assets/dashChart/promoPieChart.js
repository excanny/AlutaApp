google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);

// Draw the chart and set the chart values
function drawChart() {
    $.getJSON("/Administrators/CommentChart", function (result) {
        var data = google.visualization.arrayToDataTable([
            ['Month', 'Total'],
            ['January', result.january],
            ['February', result.february],
            ['March', result.march],
            ['April', result.april],
            ['May', result.may],
            ['June', result.june],
            ['July', result.july],
            ['August', result.august],
            ['September', result.september],
            ['October', result.october],
            ['November', result.november],
            ['December', result.december]
        ]);


        // Optional; add a title and set the width and height of the chart
        var options = { 'title': 'Comments', 'width': 180, 'height': 220, is3D: true, legend: 'none', fontName: 'Poppins', backgroundColor: { stroke: null, fill: null, strokeSize: 0 } };


        // Display the chart inside the <div> element with id="piechart"
        var chart = new google.visualization.PieChart(document.getElementById('commentPieChart'));
        chart.draw(data, options);
    }
    )
}


