using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    static class Common
    {
        public static string GetAppFolder()
        {
            //return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

            var userPath = Environment.GetFolderPath(Environment
                                             .SpecialFolder.ApplicationData);

            return userPath;
        }

        //public static string imageFolderPath = @"D:\JentecDTR\Pics\";
        public static string imageFolderPath {
            get { return Path.Combine(GetAppFolder(), "pics"); }
        }

        //Courtesy of Stackoverflow
        public static object DbNullIfNull(this object obj)
        {
            return obj != null ? obj : DBNull.Value;
        }

        //Courtesy of Stackoverflow
        public static T? GetValue<T>(this DataRow row, string columnName) where T : struct
        {
            if (row.IsNull(columnName))
                return null;

            return row[columnName] as T?;
        }

        public static string GetText(this DataRow row, string columnName)
        {
            if (row.IsNull(columnName))
                return string.Empty;

            return row[columnName] as string ?? string.Empty;
        }

        public static string FirstCharToUpper(string input)
        {
            string text = input.ToLower();

            if (String.IsNullOrEmpty(input))
                return String.Empty;
            return text.First().ToString().ToUpper() + String.Join("", text.Skip(1));
        }
    }
}