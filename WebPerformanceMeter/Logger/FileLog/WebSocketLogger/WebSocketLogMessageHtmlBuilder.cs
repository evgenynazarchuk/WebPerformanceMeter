using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace WebPerformanceMeter.Report
{
    public class WebSocketLogMessageHtmlBuilder
    {
        private readonly string _sourceFilePath;

        private readonly string _destinationFilePath;

        private readonly StreamReader _reader;

        private readonly StreamWriter _writer;

        private readonly List<WebSocketLogMessage> _logMessages;

        public WebSocketLogMessageHtmlBuilder(string sourceFilePath, string destinationFilePath)
        { 
            this._sourceFilePath = sourceFilePath;
            this._destinationFilePath = destinationFilePath;
            this._reader = new(this._sourceFilePath, Encoding.UTF8, false, 65535);
            this._writer = new(destinationFilePath, false, Encoding.UTF8, 65355);
            this._logMessages = new List<WebSocketLogMessage>();
        }

        private void ReadLogMessages()
        {
            string? line;
            WebSocketLogMessage? httpLogMessage;

            while ((line = this._reader.ReadLine()) != null)
            {
                httpLogMessage = JsonSerializer.Deserialize<WebSocketLogMessage>(line);

                if (httpLogMessage is null)
                {
                    throw new ApplicationException("Error convertation");
                }

                this._logMessages.Add(httpLogMessage);
            }

            this._reader.Close();
        }

        public void BuildHtml()
        {
            this.ReadLogMessages();

            if (this._logMessages is null)
            {
                return;
            }

            var startedRequest = this._logMessages
                .GroupBy(x => new
                {
                    x.UserName,
                    x.ActionType,
                    x.Label,
                    StartRequestTime = (long)(x.StartTime / 10000000)
                })
                .Select(x => new WebSocketLogByStartTime(
                    x.Key.UserName,
                    x.Key.ActionType,
                    x.Key.Label,
                    x.Key.StartRequestTime,
                    x.LongCount()))
                .ToList();

            var completedRequest = this._logMessages
                .GroupBy(x => new
                {
                    x.UserName,
                    x.ActionType,
                    x.Label,
                    EndRequestTime = (long)(x.EndTime / 10000000)
                })
                .Select(x => new WebSocketLogByEndTime(
                    x.Key.UserName,
                    x.Key.ActionType,
                    x.Key.Label,
                    x.Key.EndRequestTime,
                    x.LongCount()))
                .ToList();

            StringBuilder startedRequestTimeJsonString = new();
            StringBuilder completedRequestTimeJsonString = new();

            foreach (var item in startedRequest)
            {
                startedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            foreach (var item in completedRequest)
            {
                completedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            //
            //
            //

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
    if (startedRequestData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        startedRequestData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    startedRequestData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

PlotlyJsLineDraw('Started Requests', 'Count', 'StartedRequestsChart', startedRequestData)

/*
**
*/
let completedRequestData = { };
for (let item of startedRequestRawLog)
{
    if (completedRequestData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        completedRequestData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    completedRequestData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
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
            _writer.WriteLine(totalHtml);
            _writer.Flush();
            _writer.Close();
        }
    }
}
