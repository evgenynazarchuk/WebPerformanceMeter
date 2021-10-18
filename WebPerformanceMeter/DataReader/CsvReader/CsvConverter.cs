using System;

namespace WebPerformanceMeter.DataReader.CsvReader
{
    public static class CsvConverter
    {
        // (?:^|,)(?=[^"]|(")?)"?((?(1)[^"]*|[^,"]*))"?(?=,|$)
        // (?:,|\n|^)("(?:(?:"")*[^"]*)*"|[^",\n]*|(?:\n|$))
        // (?:,"|^")(""|[\w\W]*?)(?=",|"$)|(?:,(?!")|^(?!"))([^,]*?)(?=$|,)|(\r\n|\n)
        // (?<quoted>(?<=,"|^")(?:""|[\w\W]*?)*(?=",|"$))|(?<normal>(?<=,(?!")|^(?!"))[^,]*?(?=(?<!")$|(?<!"),))|(?<eol>\r\n|\n)
        // (?:^"|,")(""|[\w\W]*?)(?=",|"$)|(?:^(?!")|,(?!"))([^,]*?)(?=$|,)|(\r\n|\n)
        // @"(?:^""|,"")(""""|[\w\W]*?)(?="",|""$)|(?:^(?!"")|,(?!""))([^,]*?)(?=$|,)|(\r\n|\n)"
        // ("([^"]*)"|[^,]*)(,|$)
        //",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))" - default
        //public static readonly Regex RegexParser = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)", RegexOptions.Compiled);

        public static object GetObjectFromCsvLine(string line, Type resultObjectType, string separator = ",")
        {
            return GetObjectFromCsvColumns(line.Split(separator), resultObjectType);
        }

        public static object GetObjectFromCsvColumns(ReadOnlySpan<string> columns, Type resultObjectType)
        {
            var entity = Activator.CreateInstance(resultObjectType);
            if (entity is null)
            {
                throw new ApplicationException("Error create entity for json");
            }

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
                    case TypeString.String:
                        properties[i].SetValue(entity, columns[i], null);
                        break;

                    case TypeString.Integer:
                        properties[i].SetValue(entity, IntegerIsRequired(columns[i]), null);
                        break;

                    case TypeString.IntegerOrNull:
                        properties[i].SetValue(entity, IntegerIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.Boolean:
                        properties[i].SetValue(entity, BooleanIsRequired(columns[i]), null);
                        break;

                    case TypeString.BooleanOrNull:
                        properties[i].SetValue(entity, BooleanIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.UnsignedInteger:
                        properties[i].SetValue(entity, UnsignedIntegerIsRequired(columns[i]), null);
                        break;

                    case TypeString.UnsignedIntegerOrNull:
                        properties[i].SetValue(entity, UnsignedIntegerIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.LongInterger:
                        properties[i].SetValue(entity, LongIntegerIsRequired(columns[i]), null);
                        break;

                    case TypeString.LongIntergerOrNull:
                        properties[i].SetValue(entity, LongIntegerIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.UnsignedLongInterger:
                        properties[i].SetValue(entity, UnsignedLongIntegerIsRequired(columns[i]), null);
                        break;

                    case TypeString.UnsignedLongIntergerOrNull:
                        properties[i].SetValue(entity, UnsignedLongIntegerIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.Float:
                        properties[i].SetValue(entity, FloatIsRequired(columns[i]), null);
                        break;

                    case TypeString.FloatOrNull:
                        properties[i].SetValue(entity, FloatIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.Double:
                        properties[i].SetValue(entity, DoubleIsRequired(columns[i]), null);
                        break;

                    case TypeString.DoubleOrNull:
                        properties[i].SetValue(entity, DoubleIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.Decimal:
                        properties[i].SetValue(entity, DecimalIsRequired(columns[i]), null);
                        break;

                    case TypeString.DecimalOrNull:
                        properties[i].SetValue(entity, DecimalIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.DateTime:
                        properties[i].SetValue(entity, DateTimeIsRequired(columns[i]), null);
                        break;

                    case TypeString.DateTimeOrNull:
                        properties[i].SetValue(entity, DateTimeIsNotRequired(columns[i]), null);
                        break;

                    case TypeString.TimeSpan:
                        properties[i].SetValue(entity, TimeSpanIsRequired(columns[i]), null);
                        break;

                    case TypeString.TimeSpanOrNull:
                        properties[i].SetValue(entity, TimeSpanIsNotRequired(columns[i]), null);
                        break;

                    default:
                        throw new ApplicationException("Unknow Type");
                }
            }

            return entity;
        }

        public static int IntegerIsRequired(ReadOnlySpan<char> column)
            => int.TryParse(column, out int columnValue) ? columnValue
            : throw new ApplicationException("Integer field is wrong");

        public static uint UnsignedIntegerIsRequired(ReadOnlySpan<char> column)
            => uint.TryParse(column, out uint columnValue) ? columnValue
            : throw new ApplicationException("Unsigned Integer field is wrong");

        public static long LongIntegerIsRequired(ReadOnlySpan<char> column)
            => long.TryParse(column, out long columnValue) ? columnValue
            : throw new ApplicationException("Long Integer field is wrong");

        public static ulong UnsignedLongIntegerIsRequired(ReadOnlySpan<char> column)
            => ulong.TryParse(column, out ulong columnValue) ? columnValue
            : throw new ApplicationException("Unsigned Long Integer field is wrong");

        public static float FloatIsRequired(ReadOnlySpan<char> column)
            => float.TryParse(column, out float columnValue) ? columnValue
            : throw new ApplicationException("Float field is wrong");

        public static double DoubleIsRequired(ReadOnlySpan<char> column)
            => double.TryParse(column, out double columnValue) ? columnValue
            : throw new ApplicationException("Double field is wrong");

        public static decimal DecimalIsRequired(ReadOnlySpan<char> column)
            => decimal.TryParse(column, out decimal columnValue) ? columnValue
            : throw new ApplicationException("Decimal field is wrong");

        public static bool BooleanIsRequired(ReadOnlySpan<char> column)
            => bool.TryParse(column, out bool columnValue) ? columnValue
            : throw new ApplicationException("Boolean field is wrong");

        public static DateTime DateTimeIsRequired(ReadOnlySpan<char> column)
            => DateTime.TryParse(column, out DateTime columnValue) ? columnValue
            : throw new ApplicationException("DateTime field is wrong");

        public static TimeSpan TimeSpanIsRequired(ReadOnlySpan<char> column)
            => TimeSpan.TryParse(column, out TimeSpan columnValue) ? columnValue
            : throw new ApplicationException("DateTime field is wrong");

        //
        public static int? IntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : IntegerIsRequired(column);

        public static uint? UnsignedIntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : UnsignedIntegerIsRequired(column);

        public static long? LongIntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : LongIntegerIsRequired(column);

        public static ulong? UnsignedLongIntegerIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : UnsignedLongIntegerIsRequired(column);

        public static float? FloatIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : FloatIsRequired(column);

        public static double? DoubleIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : DoubleIsRequired(column);

        public static decimal? DecimalIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : DecimalIsRequired(column);

        public static bool? BooleanIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : BooleanIsRequired(column);

        public static DateTime? DateTimeIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : DateTimeIsRequired(column);

        public static TimeSpan? TimeSpanIsNotRequired(ReadOnlySpan<char> column)
            => column.Length == 0 ? null : TimeSpanIsRequired(column);

        public static class TypeString
        {
            public const string String = "System.String";
            public const string Integer = "System.Int32";
            public const string UnsignedInteger = "System.UInt32";
            public const string LongInterger = "System.Int64";
            public const string UnsignedLongInterger = "System.UInt64";
            public const string Float = "System.Single";
            public const string Double = "System.Double";
            public const string Decimal = "System.Decimal";
            public const string DateTime = "System.DateTime";
            public const string TimeSpan = "System.TimeSpan";
            public const string IntegerOrNull = "System.Nullable`1[System.Int32]";
            public const string UnsignedIntegerOrNull = "System.Nullable`1[System.UInt32]";
            public const string LongIntergerOrNull = "System.Nullable`1[System.Int64]";
            public const string UnsignedLongIntergerOrNull = "System.Nullable`1[System.UInt64]";
            public const string FloatOrNull = "System.Nullable`1[System.Single]";
            public const string DoubleOrNull = "System.Nullable`1[System.Double]";
            public const string DecimalOrNull = "System.Nullable`1[System.Decimal]";
            public const string DateTimeOrNull = "System.Nullable`1[System.DateTime]";
            public const string TimeSpanOrNull = "System.Nullable`1[System.TimeSpan]";
            public const string Boolean = "System.Boolean";
            public const string BooleanOrNull = "System.Nullable`1[System.Boolean]";
        }
    }
}
