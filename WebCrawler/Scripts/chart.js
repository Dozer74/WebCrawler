$(document).ready(function() {
    function drawBarChart() {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "/Home/BuildChart",
            dataType: "json",
            success: function(chartData) {
                var data = new google.visualization.DataTable();
                data.addColumn("string", "Время");
                data.addColumn("number", "Подписчиков");
                $.each(chartData.Records, function(i) {
                    data.addRow([chartData.Records[i].Date, chartData.Records[i].MembersCount]);
                });

                var options = {
                    legend: {position: "none"},
                    height: Math.max(chartData.Records.length * 50, 150),
                    bars: "horizontal", // Required for Material Bar Charts.
                    hAxis: {
                        format: "decimal",
                        viewWindow: {
                            min: chartData.Min,
                            max: chartData.Max
                        }
                    }
                };
                var chart = new google.charts.Bar(document.getElementById("chartdiv"));
                chart.draw(data, google.charts.Bar.convertOptions(options));
            }
            /*error: function(data) {
                alert("Error In getting Records");
            }*/
        });
    }

    drawBarChart();
});