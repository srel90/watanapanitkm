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

    public partial class main : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public DataTable data;
        public main()
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
            data = null;
            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(mode))
            {
                strsql = "true";
                clsLink clsLink = new clsLink();
                switch (mode)
                {
                    case "search": Response.Write(searchJTDA(Request["search"].ToString()));
                        break;
                    case "searchJTDAByID": Response.Write(searchJTDAByID());
                        break;
                    
                }
                Response.End();
            }
        }
        public string searchJTDAByID()
        {
            try
            {
                strsql = "SELECT jtda.*,concat(users.firstname,' ',users.lastname) as practitioner from jtda jtda left outer join users users on jtda.practitionersID=users.usersID  where jtdaID='" + Request["jtdaID"] + "'";
                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                return utility.GetJSON(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string searchJTDA(string search)
        {

            try
            {


                List<string> words = new List<string>();
                List<string> searchTermBits = new List<string>();
                words = utility.WordBreak(search);
                for (int i = 0; i < words.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(words[i]) || string.IsNullOrEmpty(words[i]))
                    {
                        words.RemoveAt(i);
                    }
                }
                for (int i = 0; i < words.Count; i++)
                {
                    searchTermBits.Add(" docNo like N'%" + words[i] + "%'");
                    searchTermBits.Add(" title like N'%" + words[i] + "%'");
                    searchTermBits.Add(" systemName like N'%" + words[i] + "%'");
                    searchTermBits.Add(" detail like N'%" + words[i] + "%'");
                    searchTermBits.Add(" jobType like N'%" + words[i] + "%'");
                    searchTermBits.Add(" solution like N'%" + words[i] + "%'");
                }
                strsql = "SELECT * from jtda where ";
                strsql += String.Join(" or ", searchTermBits.ToArray());
                if (string.IsNullOrEmpty(search))
                {
                    strsql = "SELECT * from jtda  order by jtdaID desc ";

                }

                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                int rowscount = dt.Rows.Count;
                if (rowscount == 0) { Response.Write("There is no record found!"); Response.End(); }
                int[,] records = new int[rowscount, words.Count];
                double sqrtofword = Math.Sqrt(words.Count);
                List<double> totals = new List<double>();
                for (int i = 0; i < rowscount; i++)
                {
                    double sumofword = 0;
                    double total = 0;
                    for (int j = 0; j < words.Count; j++)
                    {

                        if (utility.Contains(dt.Rows[i]["jobType"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["systemName"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["docNo"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["title"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["detail"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["solution"].ToString(), words[j]))
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
                dv.Sort = "order desc,jtdaID desc";
                DataTable sortedDT = dv.ToTable().AsEnumerable().Skip((Convert.ToInt32(Request["page"]) * Convert.ToInt32(Request["take"]))).Take(Convert.ToInt32(Request["take"])).CopyToDataTable();

                StringBuilder sb = new StringBuilder();
                Double rowcount = sortedDT.Rows.Count;

                for (int i = 0; i < rowcount; i++)
                {
                    sb.Append("<div class=\"col-md-12\">");
                    sb.Append("<div class=\"box box-solid box-info\" style=\"display: block;overflow: auto;\">");
                    sb.Append("<div class=\"box-header\">");
                    sb.Append("<h4 class=\"box-title\"><a href='javascript:script.View(" + sortedDT.Rows[i]["jtdaID"].ToString() + ");'>" + sortedDT.Rows[i]["title"].ToString() + "</a></h4>");

                    sb.Append("<div class=\"box-tools pull-right\">");
                    sb.Append("<div class=\"label bg-aqua\">" + string.Format("Result:{0:0.0%}", Convert.ToDouble(sortedDT.Rows[i]["order"].ToString())) + "</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("<div class=\"box-body\">");
                    sb.Append("<p>");
                    sb.Append(sortedDT.Rows[i]["detail"].ToString());
                    sb.Append("</p>");
                    sb.Append("<p>");
                    sb.Append(sortedDT.Rows[i]["solution"].ToString());
                    sb.Append("</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.AppendLine();
                }
                sb.AppendLine();
                sb.Append("<div id=\"temppage\" style=\"display:none\">");
                if (Convert.ToInt16(Request["page"].ToString()) != 0)
                {
                    sb.Append("<li><a href=\"javascript:script.setpage(" + (Convert.ToInt16(Request["page"].ToString()) - 1) + ")\">&laquo;</a></li>");
                }
                    Double page = Math.Ceiling((double)rowscount / 6);
                string ac="";
                for (int j = 0; j < page; j++)
                {
                        if (Request["page"].ToString().Equals(j.ToString())) { ac = "class=\"active\""; } else { ac = ""; }
                        sb.Append("<li " + ac + "><a href=\"javascript:script.setpage(" + j + ")\">" + (j + 1) + "</a></li>");
                }
                if (Convert.ToInt16(Request["page"].ToString()) != (page-1))
                {
                    sb.Append("<li><a href=\"javascript:script.setpage(" + (Convert.ToInt16(Request["page"].ToString()) + 1) + ")\">&raquo;</a></li>");
                }
                sb.Append("</div>");

                return sb.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
    }
}