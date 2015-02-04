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

    public partial class jtda : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public jtda()
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

            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(mode))
            {
                strsql = "true";
                clsJTDA clsJTDA = new clsJTDA();
                switch (mode)
                {
                    case "searchJTDA": Response.Write(searchJTDA(Request["search"].ToString()));
                        break;
                    case "selectAllJTDAByEmployeeID": Response.Write(selectAllJTDAByEmployeeID());
                        break;
                    case "insert":
                        Response.Write(insert(clsJTDA));
                        break;
                    case "update":
                        Response.Write(update(clsJTDA));
                        break;
                    case "delete":
                        Response.Write(delete(clsJTDA));
                        break;
                }
                Response.End();
            }
        }

        public string searchJTDA(string search)
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
                        searchTermBits.Add(" detail like N'%" + words[i] + "%'");
                        searchTermBits.Add(" solution like N'%" + words[i] + "%'");
                }
                strsql = "SELECT * from jtda where (";
                strsql += String.Join(" or ", searchTermBits.ToArray());
                if (((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString().Equals("admin"))
                {
                    strsql += String.Join(strsql, " ) and employeeID IS NOT NULL order by employeeID,jtdaID asc ");
                }
                else
                {
                    strsql += String.Join(strsql, " ) and employeeID='" + ((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString() + "'");
                }
                if (string.IsNullOrEmpty(search))
                {
                    strsql = "SELECT * from jtda where employeeID IS NOT NULL order by employeeID,jtdaID asc ";

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

                        if (utility.Contains(dt.Rows[i]["title"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["detail"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["solution"].ToString(), words[j]))
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
                
                DataTable sortedDT = dv.ToTable().AsEnumerable().Skip((Convert.ToInt32(Request["skip"]) * Convert.ToInt32(Request["take"]))).Take(Convert.ToInt32(Request["take"])).CopyToDataTable();
                return utility.GetJSON(sortedDT);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string selectAllJTDAByEmployeeID()
        {
            if (((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString().Equals("admin"))
            {
                strsql = "SELECT * from jtda where employeeID IS NOT NULL order by employeeID,jtdaID asc ";
                strsql += " OFFSET " + Request["skip"] + " ROWS FETCH NEXT " + Request["take"] + " ROWS ONLY";
            }
            else
            {
                strsql = "SELECT * from jtda where employeeID='" + ((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString() + "' order by jtdaID desc";
                strsql += " OFFSET " + Request["skip"] + " ROWS FETCH NEXT " + Request["take"] + " ROWS ONLY";
            }
            
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                string strsqlt = "";
                if (((System.Data.DataTable)Session["USER"]).Rows[0]["username"].ToString().Equals("admin"))
                {
                    strsqlt = "SELECT * from jtda where employeeID IS NOT NULL order by employeeID,jtdaID asc ";
                }
                else
                {
                    strsqlt = "SELECT * from jtda where employeeID='" + ((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString() + "' order by jtdaID desc";
                }
                int rowcount = 0;
                Dbcmd = db.GetSqlStringCommand(strsqlt);
                rowcount = db.ExecuteDataSet(Dbcmd).Tables[0].Rows.Count;

                return utility.GetJSON(dt, rowcount);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string insert(clsJTDA clsJTDA)
        {
            strsql = "INSERT INTO jtda(";
            strsql += "title,";
            strsql += "detail,";
            strsql += "solution,";
            strsql += "employeeID";
            strsql += ")VALUES(";
            strsql += "@title,";
            strsql += "@detail,";
            strsql += "@solution,";
            strsql += "@employeeID";
            strsql += ");";
            try
            {
                clsJTDA.title = Request["title"];
                clsJTDA.detail = Request["detail"];
                clsJTDA.solution = Request["solution"];
                clsJTDA.employeeID = Convert.ToInt16(((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString());

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@title", DbType.String, clsJTDA.title);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsJTDA.detail);
                db.AddInParameter(Dbcmd, "@solution", DbType.String, clsJTDA.solution);
                db.AddInParameter(Dbcmd, "@employeeID", DbType.Int16, clsJTDA.employeeID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsJTDA clsJTDA)
        {
            strsql = "UPDATE jtda SET ";
            strsql += "title=@title,";
            strsql += "detail=@detail,";
            strsql += "solution=@solution";
            strsql += " WHERE jtdaID=@jtdaID ";
            
            try
            {
                clsJTDA.title = Request["title"];
                clsJTDA.detail = Request["detail"];
                clsJTDA.solution = Request["solution"];
                clsJTDA.employeeID = Convert.ToInt16(((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString());
                clsJTDA.jtdaID = Convert.ToInt16(Request["id"]);

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@title", DbType.String, clsJTDA.title);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsJTDA.detail);
                db.AddInParameter(Dbcmd, "@solution", DbType.String, clsJTDA.solution);
                db.AddInParameter(Dbcmd, "@jtdaID", DbType.Int16, clsJTDA.jtdaID);
                
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsJTDA clsJTDA)
        {
            
            try
            {
                clsJTDA.jtdaID = Convert.ToInt16(Request["id"]);
                strsql = "DELETE FROM jtda ";
                strsql += "WHERE jtdaID=@jtdaID;";

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@jtdaID", DbType.Int16, clsJTDA.jtdaID);
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