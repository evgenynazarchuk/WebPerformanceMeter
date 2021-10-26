using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace WebPerformanceMeter.Logger
{
    public class HttpHtmlReportGenerator
    {
        public HttpHtmlReportGenerator(
            string httpClientToolLogFileName,
            string httpClientReport)
        {
            this.httpLogMessageList = new();
            this.reader = new(httpClientToolLogFileName, Encoding.UTF8, false, 65535);
            this.writer = new(httpClientReport, false, Encoding.UTF8, 65355);
        }

        private readonly StreamReader reader;

        private readonly StreamWriter writer;

        private readonly List<HttpLogMessage> httpLogMessageList;

        public void ReadHttpLogMessage()
        {
            string? line;
            HttpLogMessage? httpLogMessage;

            while ((line = this.reader.ReadLine()) != null)
            {
                httpLogMessage = JsonSerializer.Deserialize<HttpLogMessage>(line);

                if (httpLogMessage is null)
                {
                    throw new ApplicationException("Error convertation");
                }

                this.httpLogMessageList.Add(httpLogMessage);
            }

            this.reader.Close();
        }

        public void GenerateReport()
        {
            this.ReadHttpLogMessage();

            if (this.httpLogMessageList is null)
            {
                return;
            }

            var completedRequestTime = this.httpLogMessageList
                .GroupBy(x => new 
                { 
                    x.User, 
                    x.RequestMethod, 
                    x.Request, 
                    x.RequestLabel, 
                    x.StatusCode,
                    EndResponseTime = (long)(x.EndResponseTime / 10000000) })
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

            var startedRequestTime = this.httpLogMessageList
                .GroupBy(x => new 
                { 
                    x.User, 
                    x.RequestMethod, 
                    x.Request, 
                    x.RequestLabel, 
                    x.StatusCode, 
                    StartRequestTime = (long)(x.StartSendRequestTime / 10000000) })
                .Select(x => new HttpLogMessageByStartedRequest(
                    x.Key.User,
                    x.Key.RequestMethod,
                    x.Key.Request,
                    x.Key.RequestLabel,
                    x.Key.StatusCode,
                    x.Key.StartRequestTime,
                    x.LongCount()))
                .ToList();

            var totalTraffic = this.httpLogMessageList
                .GroupBy(x => new { EndResponseTime = x.EndResponseTime / 10000000 })
                .Select(x => new
                {
                    EndResponseTime = x.Key.EndResponseTime,
                    SentBytes = x.Sum(y => y.SendBytes),
                    ReceivedBytes = x.Sum(y => y.ReceiveBytes)
                }).ToList();

            var userTraffic = this.httpLogMessageList
                .GroupBy(x => new 
                { 
                    x.User, 
                    EndResponseTime = x.EndResponseTime / 10000000 }
                ).Select(x => new
                {
                    x.Key,
                    //EndResponseTime = x.Key.EndResponseTime,
                    SentBytes = x.Sum(y => y.SendBytes),
                    ReceivedBytes = x.Sum(y => y.ReceiveBytes)
                }).ToList();

            var requestTraffic = this.httpLogMessageList
                .GroupBy(x => new 
                { 
                    x.User,
                    x.RequestMethod,
                    x.Request,
                    x.RequestLabel, 
                    EndResponseTime = x.EndResponseTime / 10000000 }
                ).Select(x => new
                {
                    x.Key,
                    //EndResponseTime = x.Key.EndResponseTime,
                    SentBytes = x.Sum(y => y.SendBytes),
                    ReceivedBytes = x.Sum(y => y.ReceiveBytes)
                }).ToList();

            StringBuilder startedRequestTimeJsonString = new();
            StringBuilder completedRequestTimeJsonString = new();
            StringBuilder totalSentBytesStringLog = new();
            StringBuilder totalReceivedBytesStringLog = new();
            
            foreach (var item in completedRequestTime)
            {
                HttpLogMessageTimeAnalytic totalLog = new(
                    item.Key.User,
                    item.Key.RequestMethod,
                    item.Key.Request,
                    item.Key.RequestLabel,
                    item.Key.StatusCode,
                    item.Key.EndResponseTime,
                    item.CompletedRequest,
                    item.ResponseTime,
                    item.SentTime,
                    item.WaitTime,
                    item.ReceivedTime);

                completedRequestTimeJsonString.Append(JsonSerializer.Serialize(totalLog) + ",\n");
            }

            foreach (var item in startedRequestTime)
            {
                startedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            foreach (var item in totalTraffic)
            {
                totalSentBytesStringLog.Append(JsonSerializer.Serialize(new HttpLogMessageByteAnalytic(item.EndResponseTime, item.SentBytes)) + ",\n");
                totalReceivedBytesStringLog.Append(JsonSerializer.Serialize(new HttpLogMessageByteAnalytic(item.EndResponseTime, item.ReceivedBytes)) + ",\n");
            }

            //
            string sourceData = @$"

<script>
const startedRequestLog = [{startedRequestTimeJsonString}]
const groupedRawLog = [{completedRequestTimeJsonString}]
const sentBytesLog = [{totalSentBytesStringLog}]
const receivedBytesLog = [{totalReceivedBytesStringLog}]
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
for (let item of startedRequestLog)
{
    if (startedRequestData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        startedRequestData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.StartRequestTime);
	let timeString = date.toISOString().substr(11, 8);

    startedRequestData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CountStartedRequest })
}

PlotlyJsLineDraw('Started Requests', 'Count', 'StartedRequestsChart', startedRequestData)


/*
**
*/
let completedRequestsData = { };
for (let item of groupedRawLog)
{
    if (completedRequestsData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        completedRequestsData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    completedRequestsData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CompletedRequests })
}

PlotlyJsLineDraw('Completed Requests', 'Count', 'CompletedRequestsChart', completedRequestsData)

/*
**
*/
let responseTimeData = {}
for(let item of groupedRawLog) {
	if (responseTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        responseTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    responseTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ResponseTime / 10000 })
}

PlotlyJsLineDraw('Response Time', 'Milliseconds', 'ResponseTimeChart', responseTimeData)

/*
**
*/
let sentTimeData = { };
for (let item of groupedRawLog)
{
    if (sentTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        sentTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    sentTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.SentTime / 10000 })
}

PlotlyJsLineDraw('Data Timed Sending', 'Milliseconds', 'SentTimeChart', sentTimeData)

/*
**
*/
let waitTimeData = { };
for (let item of groupedRawLog)
{
    if (waitTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        waitTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    waitTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.WaitTime / 10000 })
}

PlotlyJsLineDraw('Data Wait Times', 'Milliseconds', 'WaitTimeChart', waitTimeData)

/*
**
*/
let receivedTimeData = { };
for (let item of groupedRawLog)
{
    if (receivedTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        receivedTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    receivedTimeData[item.User + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ReceivedTime / 10000 })
}

PlotlyJsLineDraw('Data Timed Receiving', 'Milliseconds', 'ReceivedTimeChart', receivedTimeData)

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

PlotlyJsLineDraw('Sent Bytes', 'Bytes', 'SentBytesChart', sentBytesChartDataset, false)

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

PlotlyJsLineDraw('Received Bytes', 'Bytes', 'ReceivedBytesChart', receivedBytesChartDataset, false)
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
<div id='StartedRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedRequestsChart' style='width:99%;height:400px;'></div>
<div id='ResponseTimeChart' style='width:99%;height:400px;'></div>
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
            writer.WriteLine(htmlReport);
            writer.Flush();
            writer.Close();
        }
    }
}
