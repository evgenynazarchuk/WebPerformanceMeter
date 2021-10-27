using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace WebPerformanceMeter.Reports
{
    public class GrpcReportHtmlBuilder : HtmlBuilder<GrpcLogMessage>
    {
        public GrpcReportHtmlBuilder(string sourceJsonFilePath, string destinationJsonFilePath)
            : base(sourceJsonFilePath, destinationJsonFilePath) { }

        protected override string GenerateHtml()
        {
            if (this.logs is null)
            {
                return "";
            }

            var startedRequest = this.logs
                .GroupBy(x => new
                {
                    x.UserName,
                    x.Method,
                    x.Label,
                    StartRequestTime = (long)(x.StartTime / 10000000)
                })
                .Select(x => new GrpcLogByStartTime(
                    x.Key.UserName,
                    x.Key.Method,
                    x.Key.Label,
                    x.Key.StartRequestTime,
                    x.LongCount()))
                .ToList();

            var completedRequest = this.logs
                .GroupBy(x => new
                {
                    x.UserName,
                    x.Method,
                    x.Label,
                    EndRequestTime = (long)(x.EndTime / 10000000)
                })
                .Select(x => new GrpcLogByEndTime(
                    x.Key.UserName,
                    x.Key.Method,
                    x.Key.Label,
                    x.Key.EndRequestTime,
                    x.LongCount()))
                .ToList();

            var startedRequestTimeJsonString = new StringBuilder();
            var completedRequestTimeJsonString = new StringBuilder();

            foreach (var item in startedRequest)
            {
                startedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            foreach (var item in completedRequest)
            {
                completedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            //
            string sourceData = @$"

<script>
const startedRequestRawLog = [{startedRequestTimeJsonString}]
const completedRequestRawLog = [{completedRequestTimeJsonString}]
</script>
";

            var plotlyJsLineDraw = @"
<script>
function PlotlyJsLineDraw(chartName, yaxisLabel, plotlyIdent, plotlyData, rawData=true) {
    let chartPlotData = []
	if(rawData)
	{
		for(let key in plotlyData) {
		chartPlotData.push({
			x: plotlyData[key].map(item => item.x),
			y: plotlyData[key].map(item => item.y),
			type: 'scatter',
			name: key,
			})
		}
	}
	else {
		chartPlotData = plotlyData
	}
	

	let chartLayout ={
		showlegend: true,
		legend: {
			bgcolor: '#1A1A1A',
			font: {
				color: '#7C7C7C',
				family: 'Open Sans',
				size: 14
			},
			orientation: 'h',
			y: -0.4
		},
		title: {
			text: chartName,
			font: {
				color: '#828282',
				family: 'Open Sans',
				size: 21
			},
		},
		xaxis: {
			title: {
				text: '',
			},
			gridcolor: '#3C3C3C',
			gridwidth: 1,
			tickfont : {
				size : 11,
				color : '#7C7C7C'
			}
		},
		
		yaxis: {
			title: {
				text: yaxisLabel,
				font: {
					color: '#7C7C7C',
					family: 'Open Sans',
					size: 14
				},
			},
			gridcolor: '#3C3C3C',
			gridwidth: 1,
		},
		plot_bgcolor:'#1A1A1A',
		paper_bgcolor:'#1A1A1A',
	}
	
	
	Plotly.newPlot(plotlyIdent, chartPlotData, chartLayout);
}
</script>
";

            var charts = @"
<script>

/*
**
*/
let startedRequestData = { };
for (let item of startedRequestRawLog)
{
    if (startedRequestData[item.UserName + ' ' + item.Method + ' ' + item.Label] == undefined)
    {
        startedRequestData[item.UserName + ' ' + item.Method + ' ' + item.Label] = []
    }

	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    startedRequestData[item.UserName + ' ' + item.Method + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

PlotlyJsLineDraw('Started Requests', 'Count', 'StartedRequestsChart', startedRequestData)

/*
**
*/
let completedRequestData = { };
for (let item of startedRequestRawLog)
{
    if (completedRequestData[item.UserName + ' ' + item.Method + ' ' + item.Label] == undefined)
    {
        completedRequestData[item.UserName + ' ' + item.Method + ' ' + item.Label] = []
    }

	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    completedRequestData[item.UserName + ' ' + item.Method + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

PlotlyJsLineDraw('Completed Requests', 'Count', 'CompletedRequestsChart', completedRequestData)

</script>
";

            var bodyStyle = @"
<style>
body {
    background-color: #1A1A1A;
}
</style>
";

            //
            string totalHtml = $@"
<html>
<head>
<script src='https://cdn.plot.ly/plotly-2.3.0.min.js'></script>
{bodyStyle}
</head>
<body>
<div id='StartedRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedRequestsChart' style='width:99%;height:400px;'></div>
{sourceData}
{plotlyJsLineDraw}
{charts}
</body>
</html>
";

            //
            return totalHtml;
        }
    }
}
