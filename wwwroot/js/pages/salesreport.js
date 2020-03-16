function storeChart(labels, data) {
    var storeSeries = [];
    for (var i = 0; i < data.length; i++) {
        // add browser data
        storeSeries.push({
            name: labels[i],
            y: data[i]
        });
    }
    Highcharts.chart('storeChart', {
        chart: {
            type: 'pie',
            events: {
                load: function () {
                    document.getElementById('storeChart').style.background = 'none';
                }
            }
        },
        title: {
            text: 'By Store',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'Completed Orders'
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valuePrefix: 'R '
        },
        plotOptions: {
            pie: {
                innerSize: 100,
                depth: 45,
                dataLabels: {
                    useHTML: true,
                    enabled: true,
                    connectorColor: '#888',
                    format: '<div style="position: relative; top: -20px; text-align: center">{point.name}<br><b><span style="font-size: 29px">{point.percentage:.0f}%</span></b> (R{point.y:.2f})</div>'
                }
            }
        },
        series: [{
            name: 'Amount',
            data: storeSeries
        }]
    });
}

function userChart(labels, data) {
    var userSeries = [];
    for (var i = 0; i < data.length; i++) {
        // add browser data
        userSeries.push({
            name: labels[i],
            y: data[i]
        });
    }
    Highcharts.chart('userChart', {
        chart: {
            type: 'pie'
        },
        title: {
            text: 'By User',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'Completed Orders'
        },
        credits: {
            enabled: false
        },
        tooltip: {
            valuePrefix: 'R '
        },
        plotOptions: {
            pie: {
                innerSize: 100,
                depth: 45,
                dataLabels: {
                    useHTML: true,
                    enabled: true,
                    connectorColor: '#888',
                    format: '<div style="position: relative; top: -20px; text-align: center">{point.name}<br><b><span style="font-size: 29px">{point.percentage:.0f}%</span></b> (R{point.y:.2f})</div>'
                }
            }
        },
        series: [{
            name: 'Amount',
            data: userSeries
        }]
    });
}

function categoryChart(labels, data) {
    Highcharts.chart('categoryChart', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'By Category',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'Completed Orders'
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: labels,
            crosshair: true
        },
        yAxis: {
            min: 0,
            tickInterval: 20,
            lineWidth: 1,
            title: {
                text: 'Amounts'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td>{series.name}: </td>' +
                '<td style="padding:0"><b>R {point.y:.2f}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
                dataLabels: {
                    enabled: true,
                    format: 'R {y:.2f}'
                }
            }
        },
        series: [{
            name: 'Amount',
            data: data

        }]
    });
}

function salesChart(labels, data) {
    Highcharts.chart('salesChart', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: 'Sales Overview',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'Completed Orders'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            categories: labels
        },
        yAxis: {
            lineWidth: 1,
            allowDecimals: false,
            title: {
                text: 'Amounts'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td>{series.name}: </td>' +
                '<td style="padding:0"><b>R {point.y:.2f}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            },
            series: {
                dataLabels: {
                    enabled: true,
                    format: 'R {y:.2f}'
                }
            }
        },
        series: [{
            name: 'Amount',
            data: data
        }]
    });
}

function profitChart(labels, data) {
    Highcharts.chart('profitChart', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Profit',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'Completed Orders'
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: labels,
            crosshair: true
        },
        yAxis: {
            min: 0,
            tickInterval: 20,
            lineWidth: 1,
            title: {
                text: 'Amounts'
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td>{series.name}: </td>' +
                '<td style="padding:0"><b>R {point.y:.2f}</b></td></tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            series: {
                dataLabels: {
                    enabled: true,
                    format: 'R {y:.2f}'
                }
            }
        },
        series: [{
            name: 'Amount',
            data: data

        }]
    });
}

function ordersChart(labels, data) {
    Highcharts.chart('ordersChart', {
        chart: {
            type: 'line'
        },
        title: {
            text: 'Orders',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'Completed Orders'
        },
        credits: {
            enabled: false
        },
        xAxis: {
            categories: labels
        },
        yAxis: {
            allowDecimals: false,
            title: {
                text: 'Amounts'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            name: 'Amount',
            data: data
        }]
    });
}

function orderStatusChart(labels, data) {
    var orderStatusSeries = [];
    for (var i = 0; i < data.length; i++) {
        // add browser data
        orderStatusSeries.push({
            name: labels[i],
            y: data[i]
        });
    }
    Highcharts.chart('orderStatusChart', {
        chart: {
            type: 'pie'
        },
        title: {
            text: 'By Order Status',
            style: {
                fontWeight: 'bold',
                fontSize: '25px'
            }
        },
        subtitle: {
            text: 'All Orders'
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            pie: {
                innerSize: 100,
                depth: 45,
                dataLabels: {
                    useHTML: true,
                    enabled: true,
                    connectorColor: '#888',
                    format: '<div style="position: relative; top: -20px; text-align: center">{point.name}<br><b><span style="font-size: 29px">{point.percentage:.0f}%</span></b> ({point.y})</div>'
                }
            }
        },
        series: [{
            name: 'Amount',
            data: orderStatusSeries
        }]
    });
}