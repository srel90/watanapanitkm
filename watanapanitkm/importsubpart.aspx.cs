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

    public partial class importsubpart : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataSet ds = new DataSet();
        private string strsql;
        public importsubpart()
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
                                Response.Write("There is no sub-part data file");
                                Response.End();
                                return;
                            }
                            List<clsSubPart> cd = new List<clsSubPart>();
                            HttpPostedFile file = Request.Files[0];
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath(@"uploads"), fileName);
                            file.SaveAs(path);
                            if (File.Exists(path))
                            {
                                foreach (var worksheet in Workbook.Worksheets(@path))
                                {


                                    if (worksheet.Rows[0].Cells[0].Text != "SubPart_Id")
                                    {
                                        Response.Write("Invalid sub-part data file");
                                        Response.End();
                                        return;
                                    }
                                    int i = 0;
                                    foreach (var row in worksheet.Rows)
                                    {
                                        if (i != 0)
                                        {
                                            clsSubPart clsSubPart = new clsSubPart();
                                            clsSubPart.subPartID = Convert.ToInt16(row.Cells[0].Text);
                                            clsSubPart.code = row.Cells[1] == null || row.Cells[1].Text == "NULL" ? "" : row.Cells[1].Text;
                                            clsSubPart.name = row.Cells[2] == null || row.Cells[2].Text == "NULL" ? "" : row.Cells[2].Text;
                                            clsSubPart.detail = row.Cells[3] == null || row.Cells[3].Text == "NULL" ? "" : row.Cells[3].Text;
                                            clsSubPart.partID = row.Cells[4] == null || row.Cells[4].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[4].Text);
                                            clsSubPart.status = row.Cells[5] == null || row.Cells[5].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[5].Text);
                                            
                                            cd.Add(clsSubPart);
                                        }
                                        i++;
                                    }
                                }
                                for (int j = 0; j < cd.Count; j++)
                                {
                                    string sql = "select subPartID from subPart where subPartID=" + cd[j].subPartID;
                                    Dbcmd = db.GetSqlStringCommand(sql);
                                    if (db.ExecuteDataSet(Dbcmd).Tables[0].Rows.Count == 0)
                                    {
                                        strsql = "INSERT INTO subPart(";
                                        //strsql += "subPartID,";
                                        strsql += "code,";
                                        strsql += "name,";
                                        strsql += "detail,";
                                        strsql += "partID,";
                                        strsql += "status";
                                        strsql += ")VALUES(";
                                        //strsql += "@subPartID,";
                                        strsql += "@code,";
                                        strsql += "@name,";
                                        strsql += "@detail,";
                                        strsql += "@partID,";
                                        strsql += "@status";
                                        strsql += ");";
                                        Dbcmd = db.GetSqlStringCommand(strsql);
                                        //db.AddInParameter(Dbcmd, "@subPartID", DbType.Int16, cd[j].subPartID);
                                        db.AddInParameter(Dbcmd, "@code", DbType.String, cd[j].code);
                                        db.AddInParameter(Dbcmd, "@name", DbType.String, cd[j].name);
                                        db.AddInParameter(Dbcmd, "@detail", DbType.String, cd[j].detail);
                                        db.AddInParameter(Dbcmd, "@partID", DbType.Int16, cd[j].partID);
                                        db.AddInParameter(Dbcmd, "@status", DbType.Int16, cd[j].status);
                                        db.ExecuteNonQuery(Dbcmd);
                                    }
                                }
                                Response.Write("true");
                            }
                        break;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                }
                Response.End();

            }
        }
    }
}