/********************************************************************
    created:	2021/8/20 11:18:53
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace RLib.Base
{
    public static class CsvEx
    {
        public static CsvConfiguration DefaultConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound  = null,
            HeaderValidated = (e ) =>
            {
                //RLibBase.Logger.Error(e);
                Debug.WriteLine(e);
            }
        };

        public static List<T> FromCsv<T>(this string path, bool withHeader = true, CsvConfiguration config = null)
        {
            try
            {
                using (var stream = File.OpenRead(path))
                    return stream.FromCsv<T>(withHeader, config);
            }
            catch(Exception e)
            {
                Logger.Error(e);
                return new List<T>();
            }
        }
        public static List<T> FromCsv<T>(this Stream path,  bool withHeader = true, CsvConfiguration config = null)
        {
            try
            { 
                if (config == null)
                    config = DefaultConfiguration;
                config.HasHeaderRecord = withHeader;

                List<T> ret = new List<T>();

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    ret.AddRange(csv.GetRecords<T>());
                }

                return ret;
            }
            catch (Exception e)
            {
                Logger.Error($"Error read Csv {path}:{e}");
                return new List<T>();
            }
        }

        public static IEnumerable<dynamic> FromCsv(this string path, bool withHeader = true, CsvConfiguration config = null)
        {
            try
            {
                using (var stream = File.OpenRead(path))
                    return stream.FromCsv(withHeader, config);
            }
            catch(Exception e)
            {
                Logger.Error(e);
                return new List<dynamic>();
            }
        }
        public static IEnumerable<dynamic> FromCsv(this Stream path,bool withHeader = true, CsvConfiguration config = null)
        {
            try
            {
                if (config == null)
                    config = DefaultConfiguration;
                config.HasHeaderRecord = withHeader;

                List<dynamic> ret = new List<dynamic>();

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    ret.AddRange(csv.GetRecords<dynamic>());
                }

                return ret;
            }
            catch (Exception e)
            {
                Logger.Error($"Error read Csv {path}:{e}");
                return new List<dynamic>();
            }
        }

        public static IEnumerable<TRow> FromCsv<THeader, TRow>(this string path, out THeader header, CsvConfiguration config = null)
        {
            try
            {
                using (var stream = File.OpenRead(path))
                    return stream.FromCsv<THeader, TRow>(out header, config);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                header = default(THeader);
                return new List<TRow>();
            }
        }
        public static IEnumerable<TRow> FromCsv<THeader, TRow>(this Stream path,  out THeader header, CsvConfiguration config = null)
        {
            try
            {
                if (config == null)
                    config = DefaultConfiguration;

                config.HasHeaderRecord = false;

                header = default(THeader);
                List<TRow> ret = new List<TRow>();
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read();
                    header = csv.GetRecord<THeader>();

                    while (csv.Read())
                    {
                        ret.Add(csv.GetRecord<TRow>());
                    }
                }

                return ret;
            }
            catch (Exception e)
            {
                Logger.Error($"Error read Csv {path}:{e}");

                header = default(THeader);
                return new List<TRow>();
            }
        }


        public static bool ToCsv(this IEnumerable<dynamic> rows, dynamic header, string path, CsvConfiguration config = null)
        {
            try
            {
                if (config == null)
                    config = new CsvConfiguration(CultureInfo.InvariantCulture) { };

                config.HasHeaderRecord = false;

                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecord(header);
                    csv.NextRecord();
                    csv.WriteRecords(rows);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Write Csv {path}:{e}");
                return false;
            }

            return true;
        }

        public static bool ToCsv(this IEnumerable<dynamic> rows, string path, bool withHeader = true, CsvConfiguration config= null)
        {
            try
            {
                if (config == null)
                {
                    config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                    };
                }

                config.HasHeaderRecord = withHeader;

                using (var writer = new StreamWriter(path))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(rows);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Write Csv {path}:{e}");
                return false;
            }

            return true;
        }
        public static bool AppendCsv(this IEnumerable<dynamic> rows, string path, CsvConfiguration config= null)
        {
            try
            {
                if (config == null)
                    config = new CsvConfiguration(CultureInfo.InvariantCulture) { };

                config.HasHeaderRecord = false;

                using (var stream = File.Open(path, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(rows);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Error Append Csv {path}:{e}");
                return false;
            }

            return true;
        }

    }
}
