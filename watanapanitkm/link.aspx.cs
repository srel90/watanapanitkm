using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.IO;
using System.Text;

namespace wattanapanitkm
{

    public partial class link : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public link()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            db = new DatabaseProviderFactory().Create("connString");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USER"] == null)
            {
                Response.Redirect("default.aspx");
            }

            //strsql = "SELECT * FROM download ORDER BY downloadID DESC";
            //Dbcmd = db.GetSqlStringCommand(strsql);
            //dt = db.ExecuteDataSet(Dbcmd).Tables[0];
            //StringBuilder sb = new StringBuilder();
            //Double rowcount = dt.Rows.Count;
            //Double page = Math.Ceiling(rowcount/5);
            //for (int i = 0; i < rowcount; i++)
            //{
            //    sb.Append("<tr>");
            //    sb.Append("<td>"); sb.Append(dt.Rows[i]["downloadID"].ToString()); sb.Append("</td>");
            //    sb.Append("<td>"); sb.Append(dt.Rows[i]["title"].ToString()); sb.Append("</td>");
            //    sb.Append("<td><a href=\"downloads/" + dt.Rows[i]["path"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["title"].ToString() + "</a>");
            //    sb.Append("<td>"); sb.Append(dt.Rows[i]["size"].ToString()); sb.Append("</td>");
            //    sb.Append("<td>"); sb.Append(dt.Rows[i]["type"].ToString()); sb.Append("</td>");
            //    sb.Append("</tr>");
            //    sb.AppendLine();
            //}
            //listdownload.Text=sb.ToString();
            //sb.Clear();
            //sb.Append("<li><a href=\"#\">&laquo;</a></li>");
            //for (int j = 1; j <= page;j++ )
            //{  
            //    sb.Append("<li><a href=\"#\">"+j+"</a></li>");
            //}
            //sb.Append("<li><a href=\"#\">&raquo;</a></li>");
            //pagination.Text = sb.ToString();

            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(mode))
            {
                strsql = "true";
                clsLink clsLink = new clsLink();
                switch (mode)
                {
                    case "searchLink": Response.Write(searchLink(Request["search"].ToString()));
                        break;
                    case "selectAllLink": Response.Write(selectAllLink());
                        break;
                    case "insert":
                        Response.Write(insert(clsLink));
                        break;
                    case "delete":
                        Response.Write(delete(clsLink));
                        break;
                }
                Response.End();
            }
        }
        
        public string searchLink(string search)
        {
            
            try
            {
                
            
                List<string> words = new List<string>();
                List<string> searchTermBits = new List<string>();
                words=utility.WordBreak(search);
                for (int i = 0; i < words.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(words[i]) || string.IsNullOrEmpty(words[i]))
                    {
                    words.RemoveAt(i);
                    }
                }
                for(int i=0;i<words.Count;i++){
                    
                        searchTermBits.Add(" title like N'%" + words[i] + "%'");
                        searchTermBits.Add(" path like N'%" + words[i] + "%'");                       
                }
                strsql = "SELECT * from link where ";
                strsql += String.Join(" or ", searchTermBits.ToArray());
                if (string.IsNullOrEmpty(search))
                {
                    strsql = "SELECT * from link";

                }
            

                Dbcmd = db.GetSqlStringCommand(strsql);

                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                int rowscount=dt.Rows.Count;
                int[,] records = new int[rowscount, words.Count];
                double sqrtofword = Math.Sqrt(words.Count);
                List<double> totals=new List<double>();
                for (int i = 0; i < rowscount; i++)
                {
                    double sumofword = 0;
                    double total = 0;
                    for (int j = 0; j < words.Count; j++)
                    {

                        if (utility.Contains(dt.Rows[i]["title"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["path"].ToString(), words[j]))
                        {
                            records[i, j] = 1;
                        }
                        else
                        {
                            records[i, j] = 0;
                        }
                        sumofword += records[i, j];  
                    }
                    total = sumofword / (sqrtofword * Math.Sqrt(sumofword));
                    totals.Add(total);
                }
                dt.Columns.Add("order", typeof(double));
                for (int i = 0; i < rowscount; i++)
                {
                    dt.Rows[i]["order"] = totals[i];
                }
                DataView dv = dt.DefaultView;
                dv.Sort = "order desc";
                DataTable sortedDT = dv.ToTable();
                return utility.GetJSON(sortedDT);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string selectAllLink()
        {
            strsql = "SELECT * FROM link ORDER BY linkID DESC";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                return utility.GetJSON(dt);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string insert(clsLink clsLink)
        {
            strsql = "INSERT INTO link(";
            strsql += "title,";
            strsql += "path";
            strsql += ")VALUES(";
            strsql += "@title,";
            strsql += "@path";
            strsql += ");";
            try
            {
                clsLink.title = Request["title"];
                clsLink.path = Request["path"];

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@title", DbType.String, clsLink.title);
                db.AddInParameter(Dbcmd, "@path", DbType.String, clsLink.path);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsLink clsLink)
        {
            
            try
            {

                strsql = "DELETE FROM link ";
                strsql += "WHERE linkID=@linkID;";

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@linkID", DbType.Int16, clsLink.linkID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}