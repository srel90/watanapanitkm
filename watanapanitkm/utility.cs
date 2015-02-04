using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using ICU4NET;
namespace wattanapanitkm
{
    public static class utility
    {
        public static Boolean Contains(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static List<string> WordBreak(string text)
        {
            var sb = new StringBuilder();
            var col = new List<string>();

            using (BreakIterator bi = BreakIterator.CreateWordInstance(Locale.GetUS()))
            {
                bi.SetText(text);
                int start = bi.First(), end = bi.Next();
                while (end != BreakIterator.DONE)
                {
                    col.Add(text.Substring(start, end - start));
                    start = end; end = bi.Next();
                }
            }

            return col;
        }
        public static string GetJSON(DataTable Dt)
        {

            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;

                HeadStr += '"' + StrDc[i] + '"' + " : " + '"' + StrDc[i] + i.ToString() + "¾" + '"' + ",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            Sb.Append("{" + '"' + "data" + '"' + " : [");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", HttpUtility.HtmlEncode(Dt.Rows[i][j].ToString()));
                }

                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            if (Dt.Rows.Count == 0) { Sb.Append("\"\"}"); } else { Sb.Append("],\"total\":"+Dt.Rows.Count+"}"); }

            return Sb.ToString().Replace("\n", "");
        }
        public static string GetJSON(DataTable Dt,int rowcount)
        {

            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;

                HeadStr += '"' + StrDc[i] + '"' + " : " + '"' + StrDc[i] + i.ToString() + "¾" + '"' + ",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            Sb.Append("{" + '"' + "data" + '"' + " : [");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                }

                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            if (Dt.Rows.Count == 0) { Sb.Append("\"\"}"); } else { Sb.Append("],\"total\":" + rowcount + "}"); }

            return Sb.ToString().Replace("\n", "");
        }
        public static string MD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();

        }

        public static string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

    }
}