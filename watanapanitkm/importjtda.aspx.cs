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
using Excel;

namespace wattanapanitkm
{

    public partial class importjtda : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataSet ds = new DataSet();
        private string strsql;
        public importjtda()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            db = new DatabaseProviderFactory().Create("connString");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USER"] == null || !((DataTable)Session["USER"]).Rows[0]["username"].ToString().Equals("admin"))
            {
                Response.Redirect("default.aspx");
            }
            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(mode))
            {
                
                switch (mode)
                {
                    case "import":
                        try
                        {
                            if (Request.Files.Count == 0)
                            {
                                Response.Write("There is no JTDA data file");
                                Response.End();
                                return;
                            }
                            List<clsJTDA> cd = new List<clsJTDA>();
                            HttpPostedFile file = Request.Files[0];
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath(@"uploads"), fileName);
                            file.SaveAs(path);
                            if (File.Exists(path))
                            {
                                foreach (var worksheet in Workbook.Worksheets(@path))
                                {


                                    if (worksheet.Rows[0].Cells[0].Text != "เลขที่เอกสาร")
                                    {
                                        Response.Write("Invalid JTDA data file");
                                        Response.End();
                                        return;
                                    }
                                    int i = 0;
                                    foreach (var row in worksheet.Rows)
                                    {
                                        if (i != 0)
                                        {
                                            clsJTDA clsJTDA = new clsJTDA();
                                            //clsJTDA.jtdaID = Convert.ToInt16(row.Cells[0].Text);
                                            clsJTDA.docNo = row.Cells[0] == null || row.Cells[0].Text == "NULL" ? "" : row.Cells[0].Text;
                                            clsJTDA.title = row.Cells[1] == null || row.Cells[1].Text == "NULL" ? "" : row.Cells[1].Text;
                                            clsJTDA.informedDateTime = row.Cells[2] == null || row.Cells[2].Text == "NULL" ? (DateTime?)null : Convert.ToDateTime(row.Cells[2].Text);
                                            clsJTDA.requiringDateTime = row.Cells[3] == null || row.Cells[3].Text == "NULL" ? (DateTime?)null : Convert.ToDateTime(row.Cells[3].Text);
                                            clsJTDA.successDateTime = row.Cells[4] == null || row.Cells[4].Text == "NULL" ? (DateTime?)null : Convert.ToDateTime(row.Cells[4].Text);
                                            clsJTDA.reporterID = row.Cells[5] == null || row.Cells[5].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[5].Text);
                                            clsJTDA.systemName = row.Cells[6] == null || row.Cells[6].Text == "NULL" ? "" : row.Cells[6].Text;
                                            clsJTDA.detail = row.Cells[7] == null || row.Cells[7].Text == "NULL" ? "" : row.Cells[7].Text;
                                            clsJTDA.jobType = row.Cells[8] == null || row.Cells[8].Text == "NULL" ? "" : row.Cells[8].Text;
                                            clsJTDA.solution = row.Cells[9] == null || row.Cells[9].Text == "NULL" ? "" : row.Cells[9].Text;
                                            clsJTDA.practitionersID = row.Cells[10] == null || row.Cells[10].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[10].Text);
                                            clsJTDA.workProgress = row.Cells[11] == null || row.Cells[11].Text == "NULL" ? (Double?)null : Convert.ToDouble(row.Cells[11].Text);
                                            cd.Add(clsJTDA);
                                        }
                                        i++;
                                    }
                                }
                                for (int j = 0; j < cd.Count; j++)
                                {
                                    
                                        strsql = "INSERT INTO jtda(";
                                        strsql += "docNo,";
                                        strsql += "title,";
                                        strsql += "informedDateTime,";
                                        strsql += "requiringDateTime,";
                                        strsql += "successDateTime,";
                                        strsql += "reporterID,";
                                        strsql += "systemName,";
                                        strsql += "detail,";
                                        strsql += "jobType,";
                                        strsql += "solution,";
                                        strsql += "practitionersID,";
                                        strsql += "workProgress";
                                        strsql += ")VALUES(";
                                        strsql += "@docNo,";
                                        strsql += "@title,";
                                        strsql += "@informedDateTime,";
                                        strsql += "@requiringDateTime,";
                                        strsql += "@successDateTime,";
                                        strsql += "@reporterID,";
                                        strsql += "@systemName,";
                                        strsql += "@detail,";
                                        strsql += "@jobType,";
                                        strsql += "@solution,";
                                        strsql += "@practitionersID,";
                                        strsql += "@workProgress";
                                        strsql += ");";
                                        Dbcmd = db.GetSqlStringCommand(strsql);
                                        //db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, cd[j].companyID);
                                        db.AddInParameter(Dbcmd, "@docNo", DbType.String, cd[j].docNo);
                                        db.AddInParameter(Dbcmd, "@title", DbType.String, cd[j].title);
                                        db.AddInParameter(Dbcmd, "@informedDateTime", DbType.DateTime, cd[j].informedDateTime);
                                        db.AddInParameter(Dbcmd, "@requiringDateTime", DbType.DateTime, cd[j].requiringDateTime);
                                        db.AddInParameter(Dbcmd, "@successDateTime", DbType.DateTime, cd[j].successDateTime);
                                        db.AddInParameter(Dbcmd, "@reporterID", DbType.Int16, cd[j].reporterID);
                                        db.AddInParameter(Dbcmd, "@systemName", DbType.String, cd[j].systemName);
                                        db.AddInParameter(Dbcmd, "@detail", DbType.String, cd[j].detail);
                                        db.AddInParameter(Dbcmd, "@jobType", DbType.String, cd[j].jobType);
                                        db.AddInParameter(Dbcmd, "@solution", DbType.String, cd[j].solution);
                                        db.AddInParameter(Dbcmd, "@practitionersID", DbType.Int16, cd[j].practitionersID);
                                        db.AddInParameter(Dbcmd, "@workProgress", DbType.String, cd[j].workProgress);
                                        db.ExecuteNonQuery(Dbcmd);
                                    
                                }
                                Response.Write("true");
                            }
                        break;
                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex.Message);
                                throw new Exception(ex.Message);
                            }
                }
                Response.End();

            }
        }
    }
}