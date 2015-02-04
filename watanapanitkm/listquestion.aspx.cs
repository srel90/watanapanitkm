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

    public partial class listquestion : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public DataTable data;
        public listquestion()
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
                clsQuestion clsQuestion = new clsQuestion();
                clsAnswer clsAnswer = new clsAnswer();
                switch (mode)
                {
                    case "search": Response.Write(search(Request["search"].ToString()));
                        break;
                    case "selectAll": Response.Write(selectAll());
                        break;
                    case "searchQuestionByID": Response.Write(searchQuestionByID());
                        break;
                    case "insert":
                        clsQuestion.title = Request["title"].Replace(Environment.NewLine, "<br>");
                        clsQuestion.question = Request["question"].Replace(Environment.NewLine, "<br>");
                        clsQuestion.datetime = DateTime.Now;
                        clsQuestion.fullName = Request["fullName"];
                        clsQuestion.employeeID = Convert.ToInt16(((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString());
                        Response.Write(insert(clsQuestion));
                        break;
                    case "reply":
                        clsAnswer.qid = Convert.ToInt16(Request["qid"]);
                        clsAnswer.answer = Request["answer"].Replace(Environment.NewLine, "<br>");
                        clsAnswer.datetime = DateTime.Now;
                        clsAnswer.fullName = Request["fullName"];
                        clsAnswer.employeeID = Convert.ToInt16(((System.Data.DataTable)Session["USER"]).Rows[0]["employeeID"].ToString());
                        Response.Write(reply(clsAnswer));
                        break;
                    
                }
                Response.End();
            }
        }
        public string insert(clsQuestion clsQuestion)
        {
            strsql = "INSERT INTO question(";
            strsql += "title,";
            strsql += "question,";
            strsql += "datetime,";
            strsql += "employeeID,";
            strsql += "fullName,";
            strsql += "count";
            strsql += ")VALUES(";
            strsql += "@title,";
            strsql += "@question,";
            strsql += "@datetime,";
            strsql += "@employeeID,";
            strsql += "@fullName,";
            strsql += "'0'";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@title", DbType.String, Server.HtmlEncode(clsQuestion.title));
                db.AddInParameter(Dbcmd, "@question", DbType.String, Server.HtmlEncode(clsQuestion.question));
                db.AddInParameter(Dbcmd, "@datetime", DbType.DateTime, clsQuestion.datetime);
                db.AddInParameter(Dbcmd, "@employeeID", DbType.Int16, clsQuestion.employeeID);
                db.AddInParameter(Dbcmd, "@fullName", DbType.String, clsQuestion.fullName);
                db.ExecuteNonQuery(Dbcmd);

                return selectAll();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string reply(clsAnswer clsAnswer)
        {
            strsql = "INSERT INTO answer(";
            strsql += "qid,";
            strsql += "answer,";
            strsql += "datetime,";
            strsql += "employeeID,";
            strsql += "fullName";
            strsql += ")VALUES(";
            strsql += "@qid,";
            strsql += "@answer,";
            strsql += "@datetime,";
            strsql += "@employeeID,";
            strsql += "@fullName";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@qid", DbType.Int32, clsAnswer.qid);
                db.AddInParameter(Dbcmd, "@answer", DbType.String, Server.HtmlEncode(clsAnswer.answer));
                db.AddInParameter(Dbcmd, "@datetime", DbType.DateTime, clsAnswer.datetime);
                db.AddInParameter(Dbcmd, "@employeeID", DbType.Int16, clsAnswer.employeeID);
                db.AddInParameter(Dbcmd, "@fullName", DbType.String, clsAnswer.fullName);
                db.ExecuteNonQuery(Dbcmd);

                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string searchQuestionByID()
        {
            try
            {
                strsql = "SELECT q.*,a.answer,a.fullName as ansFullName,a.datetime as ansDateTime from question q left outer join answer a on q.ID=a.qid where q.ID='"+Request["ID"]+"' order by a.ID desc";
                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                return utility.GetJSON(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public string search(string search = "")
        {

            try
            {
                int page = 0;
                int take = 6;

                if (string.IsNullOrEmpty(Request["page"])) { page = 0; } else { page = Convert.ToInt16(Request["page"]); }
                if (string.IsNullOrEmpty(Request["take"])) { take = 6; } else { take = Convert.ToInt16(Request["take"]); }
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
                    searchTermBits.Add(" q.title like N'%" + words[i] + "%'");
                    searchTermBits.Add(" q.question like N'%" + words[i] + "%'");
                    searchTermBits.Add(" q.fullName like N'%" + words[i] + "%'");
                }
                strsql = "SELECT  q.* from question q where ";
                strsql += String.Join(" or ", searchTermBits.ToArray());
                if (string.IsNullOrEmpty(search))
                {
                    strsql = "SELECT q.* from question q  order by q.id desc ";
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

                        if (utility.Contains(dt.Rows[i]["title"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["question"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["fullName"].ToString(), words[j]) )
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
                dv.Sort = "order desc,id desc";
                DataTable sortedDT = dv.ToTable().AsEnumerable().Skip((page * take)).Take(take).CopyToDataTable();

                StringBuilder sb = new StringBuilder();
                Double rowcount = sortedDT.Rows.Count;

                for (int i = 0; i < rowcount; i++)
                {
                    sb.Append("<div class=\"col-md-12\">");
                    sb.Append("<div class=\"box box-solid box-info\" style=\"display: block;overflow: auto;\">");
                    sb.Append("<div class=\"box-header\">");
                    sb.Append("<h4 class=\"box-title\">" + sortedDT.Rows[i]["title"].ToString() + "</h4>");

                    sb.Append("<div class=\"box-tools pull-right\">");
                    sb.Append("<div class=\"label bg-aqua\">" + string.Format("Result:{0:0.0%}", Convert.ToDouble(sortedDT.Rows[i]["order"].ToString())) + "</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("<div class=\"box-body\">");
                    sb.Append("<p>");
                    sb.Append(sortedDT.Rows[i]["question"].ToString());
                    sb.Append("</p>");
                    sb.Append("<p>");
                    sb.Append(sortedDT.Rows[i]["fullName"].ToString());
                    sb.Append("</p>");
                    sb.Append("<button class=\"btn btn-info pull-right\" style=\"margin-bottom: 10px;\" onClick=\"script.View(" + sortedDT.Rows[i]["id"].ToString() + ");\">View</button>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.AppendLine();
                }
                sb.AppendLine();
                sb.Append("<div id=\"temppage\" style=\"display:none\">");
                sb.Append("<li><a href=\"#\">&laquo;</a></li>");
                Double pages = Math.Ceiling((double)rowscount / 6);
                string ac = "";
                for (int j = 0; j < pages; j++)
                {
                    if (page == j) { ac = "class=\"active\""; } else { ac = ""; }
                    sb.Append("<li " + ac + "><a href=\"javascript:script.setpage(" + j + ")\">" + (j + 1) + "</a></li>");
                }
                sb.Append("<li><a href=\"#\">&raquo;</a></li>");
                sb.Append("</div>");

                return sb.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string selectAll(string search="")
        {
            try
            {
                int page=0;
                int take=6;
               
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
                    searchTermBits.Add(" q.title like N'%" + words[i] + "%'");
                    searchTermBits.Add(" q.question like N'%" + words[i] + "%'");
                    searchTermBits.Add(" q.fullName like N'%" + words[i] + "%'");

                }
                strsql = "SELECT  q.* from question q where ";
                strsql += String.Join(" or ", searchTermBits.ToArray());
                if (string.IsNullOrEmpty(search))
                {
                    strsql = "SELECT q.* from question q  order by q.id desc ";

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

                        if (utility.Contains(dt.Rows[i]["title"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["question"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["fullName"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["answer"].ToString(), words[j]) || utility.Contains(dt.Rows[i]["ansFullName"].ToString(), words[j]) )
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
                dv.Sort = "order desc,id desc";
                DataTable sortedDT = dv.ToTable().AsEnumerable().Skip((page * take)).Take(take).CopyToDataTable();

                StringBuilder sb = new StringBuilder();
                Double rowcount = sortedDT.Rows.Count;

                for (int i = 0; i < rowcount; i++)
                {
                    sb.Append("<div class=\"col-md-12\">");
                    sb.Append("<div class=\"box box-solid box-info\" style=\"display: block;overflow: auto;\">");
                    sb.Append("<div class=\"box-header\">");
                    sb.Append("<h4 class=\"box-title\">"+sortedDT.Rows[i]["title"].ToString()+"</h4>");

                    sb.Append("<div class=\"box-tools pull-right\">");
                    sb.Append("<div class=\"label bg-aqua\">" + string.Format("Result:{0:0.0%}", Convert.ToDouble(sortedDT.Rows[i]["order"].ToString())) + "</div>");
                    sb.Append("</div>");

                    sb.Append("</div>");
                    sb.Append("<div class=\"box-body\">");
                    sb.Append("<p>");
                    sb.Append(sortedDT.Rows[i]["question"].ToString());
                    sb.Append("</p>");
                    sb.Append("<p>");
                    sb.Append(sortedDT.Rows[i]["fullName"].ToString());
                    sb.Append("</p>");
                    sb.Append("<button class=\"btn btn-info pull-right\" style=\"margin-bottom: 10px;\" onClick=\"script.View(" + sortedDT.Rows[i]["id"].ToString() + ");\">View</button>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.AppendLine();
                }
                sb.AppendLine();
                sb.Append("<div id=\"temppage\" style=\"display:none\">");
                sb.Append("<li><a href=\"#\">&laquo;</a></li>");
                Double pages = Math.Ceiling((double)rowscount / 6);
                string ac="";
                for (int j = 0; j < pages; j++)
                {
                    if (page==j) { ac = "class=\"active\""; } else { ac = ""; }
                    sb.Append("<li " + ac + "><a href=\"javascript:script.setpage("+j+")\">" + (j+1) + "</a></li>");
                }
                sb.Append("<li><a href=\"#\">&raquo;</a></li>");
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