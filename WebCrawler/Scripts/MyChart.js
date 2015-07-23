$(document).ready(function () {
    function drawBarChart() {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: "/Home/BuildChart",
            dataType: "json",
            success: function (chartData) {
                var data = new google.visualization.DataTable();
                data.addColumn("string", "Время");
                data.addColumn("number", "Подписчиков");
                $.each(chartData.Records, function (i) {
                    data.addRow([chartData.Records[i].Date, chartData.Records[i].MembersCount]);
                });
                var options = {
                    chart: {
                        title: "Статистика"
                    },
                    legend: { position: "none" },
                    axes: {
                        x: {
                            all: {
                                range: {
                                    min: chartData.Min,
                                    max: chartData.Max
                                }
                            }
                        }
                    },
                    bars: "horizontal"
                };
                var chart = new google.charts.Bar(document.getElementById("chartdiv"));
                chart.draw(data, options);
            },
            error: function (data) {
                alert("Error In getting Records");
            }
        });
    }

    drawBarChart();
});