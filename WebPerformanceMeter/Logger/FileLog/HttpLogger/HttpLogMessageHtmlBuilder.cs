using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace WebPerformanceMeter.Report
{
    public class HttpLogMessageHtmlBuilder
    {
        public HttpLogMessageHtmlBuilder(
            string httpClientToolLogFileName,
            string httpClientReport)
        {
            this._httpLogMessageList = new();
            this._reader = new(httpClientToolLogFileName, Encoding.UTF8, false, 65535);
            this._writer = new(httpClientReport, false, Encoding.UTF8, 65355);
        }

        private readonly StreamReader _reader;

        private readonly StreamWriter _writer;

        private readonly List<HttpLogMessage> _httpLogMessageList;

        public void ReadHttpLogMessage()
        {
            string? line;
            HttpLogMessage? httpLogMessage;

            while ((line = this._reader.ReadLine()) != null)
            {
                httpLogMessage = JsonSerializer.Deserialize<HttpLogMessage>(line);

                if (httpLogMessage is null)
                {
                    throw new ApplicationException("Error convertation");
                }

                this._httpLogMessageList.Add(httpLogMessage);
            }

            this._reader.Close();
        }

        public void BuildHtml()
        {
            this.ReadHttpLogMessage();

            if (this._httpLogMessageList is null)
            {
                return;
            }

            var startedRequest = this._httpLogMessageList
                .GroupBy(x => new
                {
                    x.UserName,
                    x.RequestMethod,
                    x.Request,
                    x.RequestLabel,
                    x.StatusCode,
                    StartRequestTime = (long)(x.StartSendRequestTime / 10000000)
                })
                .Select(x => new HttpLogMessageByStartedRequest(
                    x.Key.UserName,
                    x.Key.RequestMethod,
                    x.Key.Request,
                    x.Key.RequestLabel,
                    x.Key.StatusCode,
                    x.Key.StartRequestTime,
                    x.LongCount()))
                .ToList();

            var completedRequest = this._httpLogMessageList
                .GroupBy(x => new
                {
                    x.UserName,
                    x.RequestMethod,
                    x.Request,
                    x.RequestLabel,
                    x.StatusCode,
                    EndResponseTime = (long)(x.EndResponseTime / 10000000)
                })
                .Select(x => new HttpLogMessageByCompletedRequest(
                    x.Key.UserName,
                    x.Key.RequestMethod,
                    x.Key.Request,
                    x.Key.RequestLabel,
                    x.Key.StatusCode,
                    x.Key.EndResponseTime,
                    x.LongCount(),
                    responseTime: x.Average(y => y.EndResponseTime - y.StartSendRequestTime), // response time
                    sentTime: x.Average(y => y.StartWaitResponseTime - y.StartSendRequestTime), // sent time
                    waitTime: x.Average(y => y.StartResponseTime - y.StartWaitResponseTime), // wait time
                    receiveTime: x.Average(y => y.EndResponseTime - y.StartResponseTime), // receive time
                    x.Sum(y => y.SendBytes),
                    x.Sum(y => y.ReceiveBytes)))
                .ToList();

            // traffic
            var totalTraffic = this._httpLogMessageList
                .GroupBy(x => new { EndResponseTime = x.EndResponseTime / 10000000 })
                .Select(x => new HttpLogMessageTotalTraffic(
                    x.Key.EndResponseTime,
                    x.Sum(y => y.SendBytes),
                    x.Sum(y => y.ReceiveBytes)))
                .ToList();

            var userTraffic = this._httpLogMessageList
                .GroupBy(x => new 
                { 
                    x.UserName, 
                    EndResponseTime = x.EndResponseTime / 10000000 }
                ).Select(x => new HttpLogMessageUserTraffic(
                    x.Key.UserName,
                    x.Key.EndResponseTime,
                    x.Sum(y => y.SendBytes),
                    x.Sum(y => y.ReceiveBytes)))
                .ToList();

            StringBuilder startedRequestTimeJsonString = new();
            StringBuilder completedRequestTimeJsonString = new();
            StringBuilder totalTrafficJsonStringLog = new();
            StringBuilder userTrafficJsonStringLog = new();
            
            foreach (var item in completedRequest)
            {
                completedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            foreach (var item in startedRequest)
            {
                startedRequestTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            foreach (var item in totalTraffic)
            {
                totalTrafficJsonStringLog.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            foreach (var item in userTraffic)
            {
                userTrafficJsonStringLog.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            //
            string sourceData = @$"

<script>
const startedRequestRawLog = [{startedRequestTimeJsonString}]
const completedRequestRawLog = [{completedRequestTimeJsonString}]
const totalTrafficLog = [{totalTrafficJsonStringLog}]
const userTrafficLog = [{userTrafficJsonStringLog}]
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
    if (startedRequestData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        startedRequestData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.StartRequestTime);
	let timeString = date.toISOString().substr(11, 8);

    startedRequestData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CountStartedRequest })
}

PlotlyJsLineDraw('Started Requests', 'Count', 'StartedRequestsChart', startedRequestData)


/*
**
*/
let completedRequestsData = { };
for (let item of completedRequestRawLog)
{
    if (completedRequestsData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        completedRequestsData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    completedRequestsData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.CompletedRequests })
}

PlotlyJsLineDraw('Completed Requests', 'Count', 'CompletedRequestsChart', completedRequestsData)

/*
**
*/
let responseTimeData = {}
for(let item of completedRequestRawLog) {
	if (responseTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        responseTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    responseTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ResponseTime / 10000 })
}

PlotlyJsLineDraw('Response Time', 'Milliseconds', 'ResponseTimeChart', responseTimeData)

/*
**
*/
let sentTimeData = { };
for (let item of completedRequestRawLog)
{
    if (sentTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        sentTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    sentTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.SentTime / 10000 })
}

PlotlyJsLineDraw('Data Timed Sending', 'Milliseconds', 'SentTimeChart', sentTimeData)

/*
**
*/
let waitTimeData = { };
for (let item of completedRequestRawLog)
{
    if (waitTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        waitTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    waitTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.WaitTime / 10000 })
}

PlotlyJsLineDraw('Data Wait Times', 'Milliseconds', 'WaitTimeChart', waitTimeData)

/*
**
*/
let receivedTimeData = { };
for (let item of completedRequestRawLog)
{
    if (receivedTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        receivedTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }
	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);
    receivedTimeData[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ReceivedTime / 10000 })
}

PlotlyJsLineDraw('Data Timed Receiving', 'Milliseconds', 'ReceivedTimeChart', receivedTimeData)

/*
**
*/
let sentBytesChartDataset = []
sentBytesChartDataset.push({
	x: totalTrafficLog.map(item => {
		let date = new Date(0);
		date.setSeconds(item.Time);
		return date.toISOString().substr(11, 8);
	}),
	y: totalTrafficLog.map(item => item.SentBytes),
	type: 'scatter'
})

PlotlyJsLineDraw('Total Sent Bytes', 'Bytes', 'SentBytesChart', sentBytesChartDataset, false)


/*
**
*/
let receivedBytesChartDataset = []
receivedBytesChartDataset.push({
	x: totalTrafficLog.map(item => {
		let date = new Date(0);
		date.setSeconds(item.Time);
		return date.toISOString().substr(11, 8);
	}),
	y: totalTrafficLog.map(item => item.ReceivedBytes),
	type: 'scatter'
})

PlotlyJsLineDraw('Total Received Bytes', 'Bytes', 'ReceivedBytesChart', receivedBytesChartDataset, false)


/*
**
*/
let userTrafficSentChartDataset = {}
for (let item of userTrafficLog)
{
    if (userTrafficSentChartDataset[item.UserName] == undefined)
    {
        userTrafficSentChartDataset[item.UserName] = []
    }

	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    userTrafficSentChartDataset[item.UserName].push({ x: timeString, y: item.SentBytes })
}

PlotlyJsLineDraw('User Sent Bytes Chart', 'Bytes', 'UserSentBytesChart', userTrafficSentChartDataset)


/*
**
*/
let userTrafficReceivedChartDataset = {}
for (let item of userTrafficLog)
{
    if (userTrafficReceivedChartDataset[item.UserName] == undefined)
    {
        userTrafficReceivedChartDataset[item.UserName] = []
    }

	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    userTrafficReceivedChartDataset[item.UserName].push({ x: timeString, y: item.ReceivedBytes })
}

PlotlyJsLineDraw('User Received Bytes Chart', 'Bytes', 'UserReceivedBytesChart', userTrafficReceivedChartDataset)


/*
**
*/
let requestTrafficSentChartDataset = { }
for (let item of completedRequestRawLog)
{
    if (requestTrafficSentChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        requestTrafficSentChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    requestTrafficSentChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.SentBytes })
}

PlotlyJsLineDraw('Request Sent Bytes Chart', 'Bytes', 'RequestSentBytesChart', requestTrafficSentChartDataset)


/*
**
*/
let requestTrafficReceivedChartDataset = { }
for (let item of completedRequestRawLog)
{
    if (requestTrafficReceivedChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] == undefined)
    {
        requestTrafficReceivedChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode] = []
    }

	let date = new Date(0);
	date.setSeconds(item.EndResponseTime);
	let timeString = date.toISOString().substr(11, 8);

    requestTrafficReceivedChartDataset[item.UserName + ' ' + item.RequestMethod + ' ' + item.Request + ' ' + item.RequestLabel + ' ' + item.StatusCode].push({ x: timeString, y: item.ReceivedBytes })
}

PlotlyJsLineDraw('Request Received Bytes Chart', 'Bytes', 'RequestReceivedBytesChart', requestTrafficReceivedChartDataset)

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
<div id='UserSentBytesChart' style='width:99%;height:400px;'></div>
<div id='UserReceivedBytesChart' style='width:99%;height:400px;'></div>
<div id='RequestSentBytesChart' style='width:99%;height:400px;'></div>
<div id='RequestReceivedBytesChart' style='width:99%;height:400px;'></div>
{sourceData}
{plotlyJsLineDraw}
{charts}
</body>
</html>
";



            //
            _writer.WriteLine(htmlReport);
            _writer.Flush();
            _writer.Close();
        }
    }
}
