﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace WebPerformanceMeter.Logger
{
    public class HtmlGenerator
    {
        private string _rawLogPath;

        private string _outputHtmlPath;

        private StreamReader? _rawLogReader = null;

        private List<LogMessage>? _rawLogMessages = null;

        public HtmlGenerator(
            string rawLogPath,
            string outputHtmlPath)
        {
            _rawLogPath = rawLogPath;
            _outputHtmlPath = outputHtmlPath;
        }

        public void ReadRawLogMessages()
        {
            _rawLogReader = new(_rawLogPath, Encoding.UTF8, false, 65535);
            _rawLogMessages = new();

            string? rawLogAsString;
            LogMessage? rawLogMessage;

            while ((rawLogAsString = _rawLogReader.ReadLine()) != null)
            {
                rawLogMessage = JsonSerializer.Deserialize<LogMessage>(rawLogAsString);
                if (rawLogMessage is null) break;
                _rawLogMessages.Add(rawLogMessage);
            }
            _rawLogReader.Close();
        }

        public void GenerateReport()
        {
            if (_rawLogMessages is null)
            {
                return;
            }

            StreamWriter reportWriter = new(_outputHtmlPath, false, Encoding.UTF8, 65355);

            var groupByRequestStatusCodeEndResponse = _rawLogMessages
                .GroupBy(x => new { x.User, x.Request, x.RequestLabel, x.StatusCode, EndResponseTime = (long)(x.EndResponseTime / 10000000) })
                .Select(x => new
                {
                    x.Key,
                    CompletedRequest = x.LongCount(),
                    SentBytes = x.Sum(y => y.SendBytes),
                    ReceivedBytes = x.Sum(y => y.ReceiveBytes),
                    ResponseTime = x.Average(y => y.EndResponseTime - y.StartSendRequestTime),
                    SentTime = x.Average(y => y.StartWaitResponseTime - y.StartSendRequestTime),
                    WaitTime = x.Average(y => y.StartResponseTime - y.StartWaitResponseTime),
                    ReceivedTime = x.Average(y => y.EndResponseTime - y.StartResponseTime)
                }).ToList();

            var groupByEndResponse = _rawLogMessages
                .GroupBy(x => new { EndResponseTime = x.EndResponseTime / 10000000 })
                .Select(x => new
                {
                    EndResponseTime = x.Key.EndResponseTime,
                    SentBytes = x.Sum(y => y.SendBytes),
                    ReceivedBytes = x.Sum(y => y.ReceiveBytes)
                }).ToList();


            StringBuilder groupedStringLog = new();
            StringBuilder sentStringLog = new();
            StringBuilder receivedStringLog = new();


            foreach (var item in groupByRequestStatusCodeEndResponse)
            {
                LogMessageTimeAnalytic totalLog = new(
                    item.Key.User,
                    item.Key.Request,
                    item.Key.RequestLabel,
                    item.Key.StatusCode,
                    item.Key.EndResponseTime,
                    item.CompletedRequest,
                    item.ResponseTime,
                    item.SentTime,
                    item.WaitTime,
                    item.ReceivedTime);

                groupedStringLog.Append(JsonSerializer.Serialize(totalLog) + ",\n");
            }

            foreach (var item in groupByEndResponse)
            {
                sentStringLog.Append(JsonSerializer.Serialize(new LogMessageByteAnalytic(item.EndResponseTime, item.SentBytes)) + ",\n");
                receivedStringLog.Append(JsonSerializer.Serialize(new LogMessageByteAnalytic(item.EndResponseTime, item.ReceivedBytes)) + ",\n");
            }

            //
            string sourceData = @$"

<script>
const groupedRawLog = [{groupedStringLog}]
const sentBytesLog = [{sentStringLog}]
const receivedBytesLog = [{receivedStringLog}]
</script>
";

            var plotlyJsLineDraw = @"
<script>
function PlotlyJsLineDraw(chartName, plotlyIdent, plotlyData, rawData=true) {
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
				text: 'Milliseconds',
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
let responseTimeData = {}
for(let item of groupedRawLog) {
	if (responseTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        responseTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    responseTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ResponseTime / 10000 })
}

PlotlyJsLineDraw('Response Time', 'ResponseTimeChart', responseTimeData)

/*
**
*/
let completedRequestsData = { };
for (let item of groupedRawLog)
{
    if (completedRequestsData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        completedRequestsData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    completedRequestsData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CompletedRequests })
}

PlotlyJsLineDraw('Completed Requests','CompletedRequestsChart', completedRequestsData)

/*
**
*/
let sentTimeData = { };
for (let item of groupedRawLog)
{
    if (sentTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        sentTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    sentTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.SentTime / 10000 })
}

PlotlyJsLineDraw('Data Timed Sending','SentTimeChart', sentTimeData)

/*
**
*/
let waitTimeData = { };
for (let item of groupedRawLog)
{
    if (waitTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        waitTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    waitTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.WaitTime / 10000 })
}

PlotlyJsLineDraw('Data Wait Times','WaitTimeChart', waitTimeData)

/*
**
*/
let receivedTimeData = { };
for (let item of groupedRawLog)
{
    if (receivedTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        receivedTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    receivedTimeData[item.User + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ReceivedTime / 10000 })
}

PlotlyJsLineDraw('Data Timed Receiving','ReceivedTimeChart', receivedTimeData)

/*
**
*/
let sentBytesChartDataset = []
sentBytesChartDataset.push({
	x: sentBytesLog.map(item => {
		let date = new Date(0);
		date.setSeconds(item.EndResponseTime);
		return date.toISOString().substr(11, 8);
	}),
	y: sentBytesLog.map(item => item.Count),
	type: 'scatter'
})

PlotlyJsLineDraw('Sent Bytes','SentBytesChart', sentBytesChartDataset, false)

/*
**
*/
let receivedBytesChartDataset = []
receivedBytesChartDataset.push({
	x: receivedBytesLog.map(item => {
		let date = new Date(0);
		date.setSeconds(item.EndResponseTime);
		return date.toISOString().substr(11, 8);
	}),
	y: receivedBytesLog.map(item => item.Count),
	type: 'scatter'
})

PlotlyJsLineDraw('Received Bytes','ReceivedBytesChart', receivedBytesChartDataset, false)
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
            string htmlReport = $@"
<html>
<head>
<script src='https://cdn.plot.ly/plotly-2.3.0.min.js'></script>
{bodyStyle}
</head>
<body>
<div id='ResponseTimeChart' style='width:99%;height:400px;'></div>
<div id='CompletedRequestsChart' style='width:99%;height:400px;'></div>
<div id='SentTimeChart' style='width:99%;height:400px;'></div>
<div id='WaitTimeChart' style='width:99%;height:400px;'></div>
<div id='ReceivedTimeChart' style='width:99%;height:400px;'></div>
<div id='SentBytesChart' style='width:99%;height:400px;'></div>
<div id='ReceivedBytesChart' style='width:99%;height:400px;'></div>
{sourceData}
{plotlyJsLineDraw}
{charts}
</body>
</html>
";



            //
            reportWriter.WriteLine(htmlReport);
            reportWriter.Close();
        }
    }
}
