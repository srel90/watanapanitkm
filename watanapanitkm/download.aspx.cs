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

    public partial class download : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public download()
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
                clsDownload clsDownload = new clsDownload();
                switch (mode)
                {
                    case "searchDownload": Response.Write(searchDownload(Request["search"].ToString()));
                        break;
                    case "selectAllDownload": Response.Write(selectAllDownload());
                        break;
                    case "insert":
                        Response.Write(insert(clsDownload));
                        break;
                    case "delete":
                        Response.Write(delete(clsDownload));
                        break;
                }
                Response.End();
            }
        }
        
        public string searchDownload(string search)
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
                for (int i = 0; i < words.Count; i++)
                {                
                        searchTermBits.Add(" title like N'%" + words[i] + "%'");
                        searchTermBits.Add(" path like N'%" + words[i] + "%'");
                        searchTermBits.Add(" type like N'%" + words[i] + "%'");
                }
                strsql = "SELECT * from download where ";
                strsql += String.Join(" or ", searchTermBits.ToArray());
                if (string.IsNullOrEmpty(search))
                {
                    strsql = "SELECT * from download";

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

                        if (utility.Contains(dt.Rows[i]["title"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["path"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["type"].ToString(), words[j]))
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
        public string selectAllDownload()
        {
            strsql = "SELECT * FROM download ORDER BY downloadID DESC";
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
        public string insert(clsDownload clsDownload)
        {
            strsql = "INSERT INTO download(";
            strsql += "title,";
            strsql += "path,";
            strsql += "size,";
            strsql += "type";
            strsql += ")VALUES(";
            strsql += "@title,";
            strsql += "@path,";
            strsql += "@size,";
            strsql += "@type";
            strsql += ");";
            try
            {
                HttpPostedFile file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(@"downloads"), fileName);
                if (!File.Exists(path))
                {
                    file.SaveAs(path);
                }
                else
                {
                    Response.Write("File is already exist!");
                    Response.End();
                }
                clsDownload.title = string.IsNullOrEmpty(Request["title"]) ? fileName : Request["title"];
                clsDownload.path = fileName;
                clsDownload.size = file.ContentLength.ToString();
                clsDownload.type = file.ContentType.ToString();


                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@title", DbType.String, clsDownload.title);
                db.AddInParameter(Dbcmd, "@path", DbType.String, clsDownload.path);
                db.AddInParameter(Dbcmd, "@size", DbType.String, clsDownload.size);
                db.AddInParameter(Dbcmd, "@type", DbType.String, clsDownload.type);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsDownload clsDownload)
        {
            
            try
            {
                clsDownload.downloadID = Convert.ToInt16(Request["id"]);
                string sql = "select * from download where downloadID=" + clsDownload.downloadID;
                Dbcmd = db.GetSqlStringCommand(sql);
                DataTable dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                string fileName = dt.Rows[0]["path"].ToString();
                string path = Path.Combine(Server.MapPath(@"downloads"), fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                strsql = "DELETE FROM download ";
                strsql += "WHERE downloadID=@downloadID;";

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@downloadID", DbType.Int16, clsDownload.downloadID);
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