using System.Text.Json;

namespace WebPerformanceMeter.DataReader.CsvReader
{
    public sealed class JsonReader<TData> : DataReader<TData>
        where TData : class
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonReader(string filePath, bool cyclicalData = false, JsonSerializerOptions? options = null)
            : base(filePath, cyclicalData)
        {
            this._jsonOptions = options ?? new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            string? line;
            while ((line = this.reader.ReadLine()) != null)
            {
                var data = JsonSerializer.Deserialize<TData?>(line, this._jsonOptions);
                if (data is null)
                {
                    continue;
                }

                this.queue.Enqueue(data);
            }
        }
    }
}
