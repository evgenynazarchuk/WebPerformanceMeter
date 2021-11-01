using System.Linq;
using System.Text;
using System.Text.Json;

namespace WebPerformanceMeter.Reports
{
    public class WebSocketReportHtmlBuilder : HtmlBuilder<WebSocketLogMessage>
    {
        public WebSocketReportHtmlBuilder(string sourceJsonFilePath, string destinationHtmlFilePath)
            : base(sourceJsonFilePath, destinationHtmlFilePath) { }

        protected override string GenerateHtml()
        {
            // connection report
            var connectionStartTime = this.logs
                .Where(x => x.ActionType == "connect")
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

            var connectionEndTime = this.logs
                .Where(x => x.ActionType == "connect")
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

            var connectionResponseTime = this.logs
                .Where(x => x.ActionType == "connect")
                .GroupBy(x => new
                {
                    x.UserName,
                    x.ActionType,
                    x.Label,
                    EndRequestTime = x.EndTime / 10000000
                })
                .Select(x => new WebSocketLogByResponseTime(
                    x.Key.UserName,
                    x.Key.ActionType,
                    x.Key.Label,
                    x.Key.EndRequestTime,
                    (long)x.Average(y => y.EndTime - y.StartTime)
                ))
                .ToList();
            // end

            // sending report
            var sendingStartTime = this.logs
                .Where(x => x.ActionType == "send")
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

            var sendingEndTime = this.logs
                .Where(x => x.ActionType == "send")
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

            var sendingResponseTime = this.logs
                .Where(x => x.ActionType == "send")
                .GroupBy(x => new
                {
                    x.UserName,
                    x.ActionType,
                    x.Label,
                    EndRequestTime = x.EndTime / 10000000
                })
                .Select(x => new WebSocketLogByResponseTime(
                    x.Key.UserName,
                    x.Key.ActionType,
                    x.Key.Label,
                    x.Key.EndRequestTime,
                    (long)x.Average(y => y.EndTime - y.StartTime)
                ))
                .ToList();
            // end

            // receive report
            var receivingStartTime = this.logs
                .Where(x => x.ActionType == "receive")
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

            var receivingEndTime = this.logs
                .Where(x => x.ActionType == "receive")
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

            var receivingResponseTime = this.logs
                .Where(x => x.ActionType == "receive")
                .GroupBy(x => new
                {
                    x.UserName,
                    x.ActionType,
                    x.Label,
                    EndRequestTime = x.EndTime / 10000000
                })
                .Select(x => new WebSocketLogByResponseTime(
                    x.Key.UserName,
                    x.Key.ActionType,
                    x.Key.Label,
                    x.Key.EndRequestTime,
                    (long)x.Average(y => y.EndTime - y.StartTime)
                ))
                .ToList();
            // end

            // connection report
            var disconnectionStartTime = this.logs
                .Where(x => x.ActionType == "disconnect")
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

            var disconnectionEndTime = this.logs
                .Where(x => x.ActionType == "disconnect")
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

            var disconnectionResponseTime = this.logs
                .Where(x => x.ActionType == "disconnect")
                .GroupBy(x => new
                {
                    x.UserName,
                    x.ActionType,
                    x.Label,
                    EndRequestTime = x.EndTime / 10000000
                })
                .Select(x => new WebSocketLogByResponseTime(
                    x.Key.UserName,
                    x.Key.ActionType,
                    x.Key.Label,
                    x.Key.EndRequestTime,
                    (long)x.Average(y => y.EndTime - y.StartTime)
                ))
                .ToList();
            // end

            // connection json report
            StringBuilder connectionStartTimeJsonString = new();
            StringBuilder connectionEndTimeJsonString = new();
            StringBuilder connectionResponseTimeJsonString = new();

            foreach (var item in connectionStartTime)
            {
                connectionStartTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in connectionEndTime)
            {
                connectionEndTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in connectionResponseTime)
            {
                connectionResponseTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            // sending json report
            StringBuilder sendingStartTimeJsonString = new();
            StringBuilder sendingEndTimeJsonString = new();
            StringBuilder sendingResponseTimeJsonString = new();

            foreach (var item in sendingStartTime)
            {
                sendingStartTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in sendingEndTime)
            {
                sendingEndTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in sendingResponseTime)
            {
                sendingResponseTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            // receiving json report
            StringBuilder receivingStartTimeJsonString = new();
            StringBuilder receivingEndTimeJsonString = new();
            StringBuilder receivingResponseTimeJsonString = new();

            foreach (var item in receivingStartTime)
            {
                receivingStartTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in receivingEndTime)
            {
                receivingEndTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in receivingResponseTime)
            {
                receivingResponseTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }

            // disconnection json report
            StringBuilder disconnectionStartTimeJsonString = new();
            StringBuilder disconnectionEndTimeJsonString = new();
            StringBuilder disconnectionResponseTimeJsonString = new();

            foreach (var item in disconnectionStartTime)
            {
                disconnectionStartTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in connectionEndTime)
            {
                disconnectionEndTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }
            foreach (var item in connectionResponseTime)
            {
                disconnectionResponseTimeJsonString.Append(JsonSerializer.Serialize(item) + ",\n");
            }


            //
            //
            //

            //
            string sourceData = @$"

<script>

const groupedConnectionStartTime  = [{connectionStartTimeJsonString}]
const groupedConnectionEndTime = [{connectionEndTimeJsonString}]
const groupedConnectionResponseTime = [{connectionResponseTimeJsonString}]

const groupedSendingStartTime  = [{sendingStartTimeJsonString}]
const groupedSendingEndTime = [{sendingEndTimeJsonString}]
const groupedSendingResponseTime = [{sendingResponseTimeJsonString}]

const groupedReceivingStartTime  = [{receivingStartTimeJsonString}]
const groupedReceivingEndTime = [{receivingEndTimeJsonString}]
const groupedReceivingResponseTime = [{receivingResponseTimeJsonString}]

const groupedDisconnectionStartTime  = [{disconnectionStartTimeJsonString}]
const groupedDisconnectionEndTime = [{disconnectionEndTimeJsonString}]
const groupedDisconnectionResponseTime = [{disconnectionResponseTimeJsonString}]

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
let connectionStartTimePlotlyData = { };
for (let item of groupedConnectionStartTime)
{
    // add plotly line
    if (connectionStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        connectionStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    connectionStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

let connectionEndTimePlotlyData = { };
for (let item of groupedConnectionEndTime)
{
    // add plotly line
    if (connectionEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        connectionEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    connectionEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}


let connectionResponseTimePlotlyData = { };
for (let item of groupedConnectionResponseTime)
{
    // add plotly line
    if (connectionResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        connectionResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.EndTime);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    connectionResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.ResponseTime / 10000 })
}


PlotlyJsLineDraw('Running connection requests', 'Count', 'RunningConnectionRequestsChart', connectionStartTimePlotlyData)
PlotlyJsLineDraw('Completed connection requests', 'Count', 'CompletedConnectionRequestsChart', connectionEndTimePlotlyData)
PlotlyJsLineDraw('Response time connection requests', 'Milliseconds', 'ResponseTimeConnectionRequests', connectionResponseTimePlotlyData)



/*
**
*/
let sendingStartTimePlotlyData = { };
for (let item of groupedSendingStartTime)
{
    // add plotly line
    if (sendingStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        sendingStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    sendingStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

let sendingEndTimePlotlyData = { };
for (let item of groupedSendingEndTime)
{
    // add plotly line
    if (sendingEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        sendingEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    sendingEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}


let sendingResponseTimePlotlyData = { };
for (let item of groupedSendingResponseTime)
{
    // add plotly line
    if (sendingResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        sendingResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.EndTime);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    sendingResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.ResponseTime / 10000 })
}


PlotlyJsLineDraw('Running sending messages', 'Count', 'RunningSendingRequestsChart', sendingStartTimePlotlyData)
PlotlyJsLineDraw('Completed sending messages', 'Count', 'CompletedSendingRequestsChart', sendingEndTimePlotlyData)
PlotlyJsLineDraw('Response time sending messages', 'Milliseconds', 'ResponseTimeSendingRequests', sendingResponseTimePlotlyData)



/*
**
*/
let receivingStartTimePlotlyData = { };
for (let item of groupedReceivingStartTime)
{
    // add plotly line
    if (receivingStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        receivingStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    receivingStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

let receivingEndTimePlotlyData = { };
for (let item of groupedReceivingEndTime)
{
    // add plotly line
    if (receivingEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        receivingEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    receivingEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}


let receivingResponseTimePlotlyData = { };
for (let item of groupedReceivingResponseTime)
{
    // add plotly line
    if (receivingResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        receivingResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.EndTime);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    receivingResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.ResponseTime / 10000 })
}

PlotlyJsLineDraw('Running receiving messages', 'Count', 'RunningReceivingRequestsChart', receivingStartTimePlotlyData)
PlotlyJsLineDraw('Completed receiving messages', 'Count', 'CompletedReceivingRequestsChart', receivingEndTimePlotlyData)
PlotlyJsLineDraw('Response time receiving messages', 'Milliseconds', 'ResponseTimeReceivingRequests', receivingResponseTimePlotlyData)


/*
**
*/
let disConnectionStartTimePlotlyData = { };
for (let item of groupedDisconnectionStartTime)
{
    // add plotly line
    if (disConnectionStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        disConnectionStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    disConnectionStartTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}

let disConnectionEndTimePlotlyData = { };
for (let item of groupedDisconnectionEndTime)
{
    // add plotly line
    if (disConnectionEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        disConnectionEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.Time);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    disConnectionEndTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.Count })
}


let disConnectionResponseTimePlotlyData = { };
for (let item of groupedDisconnectionResponseTime)
{
    // add plotly line
    if (disConnectionResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] == undefined)
    {
        disConnectionResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label] = []
    }

    // convert to readable time
	let date = new Date(0);
	date.setSeconds(item.EndTime);
	let timeString = date.toISOString().substr(11, 8);

    // add point for line
    disConnectionResponseTimePlotlyData[item.UserName + ' ' + item.ActionType + ' ' + item.Label].push({ x: timeString, y: item.ResponseTime / 10000 })
}


PlotlyJsLineDraw('Running disconnection requests', 'Count', 'RunningDisconnectionRequestsChart', disConnectionStartTimePlotlyData)
PlotlyJsLineDraw('Completed disconnection requests', 'Count', 'CompletedDisconnectionRequestsChart', disConnectionEndTimePlotlyData)
PlotlyJsLineDraw('Response time disconnection requests', 'Milliseconds', 'ResponseTimeDisconnectionRequests', disConnectionResponseTimePlotlyData)


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

<div id='RunningConnectionRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedConnectionRequestsChart' style='width:99%;height:400px;'></div>
<div id='ResponseTimeConnectionRequests' style='width:99%;height:400px;'></div>

<div id='RunningSendingRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedSendingRequestsChart' style='width:99%;height:400px;'></div>
<div id='ResponseTimeSendingRequests' style='width:99%;height:400px;'></div>

<div id='RunningReceivingRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedReceivingRequestsChart' style='width:99%;height:400px;'></div>
<div id='ResponseTimeReceivingRequests' style='width:99%;height:400px;'></div>

<div id='RunningDisconnectionRequestsChart' style='width:99%;height:400px;'></div>
<div id='CompletedDisconnectionRequestsChart' style='width:99%;height:400px;'></div>
<div id='ResponseTimeDisconnectionRequests' style='width:99%;height:400px;'></div>



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
