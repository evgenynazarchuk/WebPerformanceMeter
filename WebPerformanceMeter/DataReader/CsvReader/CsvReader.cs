using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter.DataReader.CsvReader
{
    public sealed class CsvReader<TResult> : IEntityReader
        where TResult : class, new()
    {
        private readonly StreamReader StreamReader;

        private readonly ConcurrentQueue<TResult> Queue;

        private readonly Regex Parser;

        private readonly bool CyclicalData;

        public CsvReader(string path, bool hasHeader = false, bool cyclicalData = false)
        {
            Parser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled);
            StreamReader = new StreamReader(path, Encoding.UTF8, true, 65525);
            Queue = new();
            CyclicalData = cyclicalData;

            if (hasHeader)
            {
                StreamReader.ReadLine();
            }

            string? line;
            while ((line = StreamReader.ReadLine()) != null)
            {
                var row = Parser.Split(line);
                Queue.Enqueue(InitializeFromRow(row));
            }
        }

        public object? GetEntity()
        {
            Queue.TryDequeue(out TResult? result);

            if (CyclicalData && result is not null)
            {
                Queue.Enqueue(result);
            }

            return result;
        }

        private TResult InitializeFromRow(ReadOnlySpan<string> row)
        {
            var entity = new TResult();
            var properties = entity.GetType().GetProperties();

            if (row.Length != properties.Length)
            {
                throw new ApplicationException("Row length is not equal properties length");
            }

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyTypeString = properties[i].PropertyType.ToString();

                switch (propertyTypeString)
                {
                    case TypeString.String:
                        properties[i].SetValue(entity, row[i], null);
                        break;

                    case TypeString.Integer:
                        properties[i].SetValue(entity, IntegerIsRequired(row[i]), null);
                        break;

                    case TypeString.IntegerOrNull:
                        properties[i].SetValue(entity, IntegerIsNotRequired(row[i]), null);
                        break;

                    case TypeString.Boolean:
                        properties[i].SetValue(entity, BooleanIsRequired(row[i]), null);
                        break;

                    case TypeString.BooleanOrNull:
                        properties[i].SetValue(entity, BooleanIsNotRequired(row[i]), null);
                        break;

                    case TypeString.UnsignedInteger:
                        properties[i].SetValue(entity, UnsignedIntegerIsRequired(row[i]), null);
                        break;

                    case TypeString.UnsignedIntegerOrNull:
                        properties[i].SetValue(entity, UnsignedIntegerIsNotRequired(row[i]), null);
                        break;

                    case TypeString.LongInterger:
                        properties[i].SetValue(entity, LongIntegerIsRequired(row[i]), null);
                        break;

                    case TypeString.LongIntergerOrNull:
                        properties[i].SetValue(entity, LongIntegerIsNotRequired(row[i]), null);
                        break;

                    case TypeString.UnsignedLongInterger:
                        properties[i].SetValue(entity, UnsignedLongIntegerIsRequired(row[i]), null);
                        break;

                    case TypeString.UnsignedLongIntergerOrNull:
                        properties[i].SetValue(entity, UnsignedLongIntegerIsNotRequired(row[i]), null);
                        break;

                    case TypeString.Float:
                        properties[i].SetValue(entity, FloatIsRequired(row[i]), null);
                        break;

                    case TypeString.FloatOrNull:
                        properties[i].SetValue(entity, FloatIsNotRequired(row[i]), null);
                        break;

                    case TypeString.Double:
                        properties[i].SetValue(entity, DoubleIsRequired(row[i]), null);
                        break;

                    case TypeString.DoubleOrNull:
                        properties[i].SetValue(entity, DoubleIsNotRequired(row[i]), null);
                        break;

                    case TypeString.Decimal:
                        properties[i].SetValue(entity, DecimalIsRequired(row[i]), null);
                        break;

                    case TypeString.DecimalOrNull:
                        properties[i].SetValue(entity, DecimalIsNotRequired(row[i]), null);
                        break;

                    case TypeString.DateTime:
                        properties[i].SetValue(entity, DateTimeIsRequired(row[i]), null);
                        break;

                    case TypeString.DateTimeOrNull:
                        properties[i].SetValue(entity, DateTimeIsNotRequired(row[i]), null);
                        break;

                    case TypeString.TimeSpan:
                        properties[i].SetValue(entity, TimeSpanIsRequired(row[i]), null);
                        break;

                    case TypeString.TimeSpanOrNull:
                        properties[i].SetValue(entity, TimeSpanIsNotRequired(row[i]), null);
                        break;

                    default:
                        throw new ApplicationException("Unknow Type");
                }
            }

            return entity;
        }

        private static int IntegerIsRequired(ReadOnlySpan<char> column)
            => int.TryParse(column, out int columnValue) ? columnValue
            : throw new ApplicationException("Integer field is wrong");

        private static uint UnsignedIntegerIsRequired(ReadOnlySpan<char> column)
            => uint.TryParse(column, out uint columnValue) ? columnValue
            : throw new ApplicationException("Unsigned Integer field is wrong");

        private static long LongIntegerIsRequired(ReadOnlySpan<char> column)
            => long.TryParse(column, out long columnValue) ? columnValue
            : throw new ApplicationException("Long Integer field is wrong");

        private static ulong UnsignedLongIntegerIsRequired(ReadOnlySpan<char> column)
            => ulong.TryParse(column, out ulong columnValue) ? columnValue
            : throw new ApplicationException("Unsigned Long Integer field is wrong");

        private static float FloatIsRequired(ReadOnlySpan<char> column)
            => float.TryParse(column, out float columnValue) ? columnValue
            : throw new ApplicationException("Float field is wrong");

        private static double DoubleIsRequired(ReadOnlySpan<char> column)
            => double.TryParse(column, out double columnValue) ? columnValue
            : throw new ApplicationException("Double field is wrong");

        private static decimal DecimalIsRequired(ReadOnlySpan<char> column)
            => decimal.TryParse(column, out decimal columnValue) ? columnValue
            : throw new ApplicationException("Decimal field is wrong");

        private static bool BooleanIsRequired(ReadOnlySpan<char> column)
            => bool.TryParse(column, out bool columnValue) ? columnValue
            : throw new ApplicationException("Boolean field is wrong");

        private static DateTime DateTimeIsRequired(ReadOnlySpan<char> column)
            => DateTime.TryParse(column, out DateTime columnValue) ? columnValue
            : throw new ApplicationException("DateTime field is wrong");

        private static TimeSpan TimeSpanIsRequired(ReadOnlySpan<char> column)
            => TimeSpan.TryParse(column, out TimeSpan columnValue) ? columnValue
            : throw new ApplicationException("DateTime field is wrong");

        //
        private static int? IntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : IntegerIsRequired(column);

        private static uint? UnsignedIntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : UnsignedIntegerIsRequired(column);

        private static long? LongIntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : LongIntegerIsRequired(column);

        private static ulong? UnsignedLongIntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : UnsignedLongIntegerIsRequired(column);

        private static float? FloatIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : FloatIsRequired(column);

        private static double? DoubleIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : DoubleIsRequired(column);

        private static decimal? DecimalIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : DecimalIsRequired(column);

        private static bool? BooleanIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : BooleanIsRequired(column);

        private static DateTime? DateTimeIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : DateTimeIsRequired(column);

        private static TimeSpan? TimeSpanIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : TimeSpanIsRequired(column);
    }
}
