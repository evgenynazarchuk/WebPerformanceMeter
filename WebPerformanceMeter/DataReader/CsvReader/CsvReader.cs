using System;

namespace WebPerformanceMeter.DataReader.CsvReader
{
    public sealed class CsvReader<TData> : DataReader<TData>
        where TData : class, new()
    {
        public CsvReader(string filePath, bool hasHeader = false, string separator = ",", bool cyclicalData = false)
            : base(filePath, cyclicalData)
        {
            this._hasHeader = hasHeader;

            if (this._hasHeader)
            {
                this.reader.ReadLine();
            }

            string? line;
            while ((line = this.reader.ReadLine()) != null)
            {
                var columns = line.Split(separator);
                this.queue.Enqueue(GetObjectFromCsvColumns<TData>(columns));
            }
        }

        private static ResultObjectType GetObjectFromCsvColumns<ResultObjectType>(ReadOnlySpan<string> columns)
            where ResultObjectType : class, new()
        {
            var entity = new ResultObjectType();
            var properties = entity.GetType().GetProperties();

            if (columns.Length != properties.Length)
            {
                throw new ApplicationException("Row length is not equal properties length");
            }

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyTypeString = properties[i].PropertyType.ToString();

                switch (propertyTypeString)
                {
                    case CsvConverter.TypeString.String:
                        properties[i].SetValue(entity, columns[i], null);
                        break;

                    case CsvConverter.TypeString.Integer:
                        properties[i].SetValue(entity, CsvConverter.IntegerIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.IntegerOrNull:
                        properties[i].SetValue(entity, CsvConverter.IntegerIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.Boolean:
                        properties[i].SetValue(entity, CsvConverter.BooleanIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.BooleanOrNull:
                        properties[i].SetValue(entity, CsvConverter.BooleanIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.UnsignedInteger:
                        properties[i].SetValue(entity, CsvConverter.UnsignedIntegerIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.UnsignedIntegerOrNull:
                        properties[i].SetValue(entity, CsvConverter.UnsignedIntegerIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.LongInterger:
                        properties[i].SetValue(entity, CsvConverter.LongIntegerIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.LongIntergerOrNull:
                        properties[i].SetValue(entity, CsvConverter.LongIntegerIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.UnsignedLongInterger:
                        properties[i].SetValue(entity, CsvConverter.UnsignedLongIntegerIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.UnsignedLongIntergerOrNull:
                        properties[i].SetValue(entity, CsvConverter.UnsignedLongIntegerIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.Float:
                        properties[i].SetValue(entity, CsvConverter.FloatIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.FloatOrNull:
                        properties[i].SetValue(entity, CsvConverter.FloatIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.Double:
                        properties[i].SetValue(entity, CsvConverter.DoubleIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.DoubleOrNull:
                        properties[i].SetValue(entity, CsvConverter.DoubleIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.Decimal:
                        properties[i].SetValue(entity, CsvConverter.DecimalIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.DecimalOrNull:
                        properties[i].SetValue(entity, CsvConverter.DecimalIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.DateTime:
                        properties[i].SetValue(entity, CsvConverter.DateTimeIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.DateTimeOrNull:
                        properties[i].SetValue(entity, CsvConverter.DateTimeIsNotRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.TimeSpan:
                        properties[i].SetValue(entity, CsvConverter.TimeSpanIsRequired(columns[i]), null);
                        break;

                    case CsvConverter.TypeString.TimeSpanOrNull:
                        properties[i].SetValue(entity, CsvConverter.TimeSpanIsNotRequired(columns[i]), null);
                        break;

                    default:
                        throw new ApplicationException("Unknow Type");
                }
            }

            return entity;
        }

        private readonly bool _hasHeader = false;
    }
}
