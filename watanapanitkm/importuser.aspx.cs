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

    public partial class importuser : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataSet ds = new DataSet();
        private string strsql;
        public importuser()
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
                                Response.Write("There is no JTDA user data file");
                                Response.End();
                                return;
                            }
                            List<clsUser> cd = new List<clsUser>();
                            HttpPostedFile file = Request.Files[0];
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath(@"uploads"), fileName);
                            file.SaveAs(path);
                            if (File.Exists(path))
                            {
                                foreach (var worksheet in Workbook.Worksheets(@path))
                                {
                                    
                                    
                                    if (worksheet.Rows[0].Cells[0].Text != "User_Id")
                                    {
                                        Response.Write("Invalid JTDA user data file");
                                        Response.End();
                                        return;
                                    }
                                    int i = 0;
                                    foreach (var row in worksheet.Rows)
                                    {
                                        if (i != 0)
                                        {
                                            clsUser clsUser = new clsUser();
                                            clsUser.usersID = Convert.ToInt16(row.Cells[0].Text);
                                            clsUser.firstname = row.Cells[1] == null || row.Cells[1].Text=="NULL" ? "" : row.Cells[1].Text;
                                            clsUser.lastname = row.Cells[2] == null || row.Cells[2].Text == "NULL" ? "" : row.Cells[2].Text;
                                            clsUser.username = row.Cells[3] == null || row.Cells[3].Text == "NULL" ? "" : row.Cells[3].Text;
                                            clsUser.password = row.Cells[3] == null || row.Cells[3].Text == "NULL" ? "" : row.Cells[3].Text;
                                            clsUser.companyID = row.Cells[4] == null || row.Cells[4].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[4].Text);
                                            clsUser.sectorID = row.Cells[5] == null || row.Cells[5].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[5].Text);
                                            clsUser.departmentID = row.Cells[6] == null || row.Cells[6].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[6].Text);
                                            clsUser.partID = row.Cells[7] == null || row.Cells[7].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[7].Text);
                                            clsUser.supPartID = row.Cells[8] == null || row.Cells[8].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[8].Text);
                                            clsUser.positionID = row.Cells[9] == null || row.Cells[9].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[9].Text);
                                            //clsUser.headUserID = row.Cells[10] == null || row.Cells[10].Text == "NULL" ? (int?)null : Convert.ToInt16(row.Cells[10].Text);
                                            clsUser.status=1;
                                            cd.Add(clsUser);
                                        }
                                        i++;
                                    }
                                }
                                for (int j = 0; j < cd.Count; j++)
                                {
                                    string sql = "select usersID from users where usersID=" + cd[j].usersID;
                                    Dbcmd = db.GetSqlStringCommand(sql);
                                    if (db.ExecuteDataSet(Dbcmd).Tables[0].Rows.Count == 0)
                                    {
                                        strsql = "INSERT INTO users(";
                                        strsql += "usersID,";
                                        strsql += "firstname,";
                                        strsql += "lastname,";
                                        strsql += "username,";
                                        strsql += "password,";
                                        strsql += "companyID,";
                                        strsql += "sectorID,";
                                        strsql += "departmentID,";
                                        strsql += "partID,";
                                        strsql += "subPartID,";
                                        strsql += "positionID,";
                                        //strsql += "headUserID,";
                                        strsql += "status";
                                        strsql += ")VALUES(";
                                        strsql += "@usersID,";
                                        strsql += "@firstname,";
                                        strsql += "@lastname,";
                                        strsql += "@username,";
                                        strsql += "@password,";
                                        strsql += "@companyID,";
                                        strsql += "@sectorID,";
                                        strsql += "@departmentID,";
                                        strsql += "@partID,";
                                        strsql += "@subPartID,";
                                        strsql += "@positionID,";
                                        //strsql += "@headUserID,";
                                        strsql += "@status";
                                        strsql += ");";
                                        Dbcmd = db.GetSqlStringCommand(strsql);
                                        db.AddInParameter(Dbcmd, "@usersID", DbType.Int16, cd[j].usersID);
                                        db.AddInParameter(Dbcmd, "@firstname", DbType.String, cd[j].firstname);
                                        db.AddInParameter(Dbcmd, "@lastname", DbType.String, cd[j].lastname);
                                        db.AddInParameter(Dbcmd, "@username", DbType.String, cd[j].username);
                                        db.AddInParameter(Dbcmd, "@password", DbType.String, cd[j].password);
                                        db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, cd[j].companyID);
                                        db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, cd[j].sectorID);
                                        db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, cd[j].departmentID);
                                        db.AddInParameter(Dbcmd, "@partID", DbType.Int16, cd[j].partID);
                                        db.AddInParameter(Dbcmd, "@subPartID", DbType.Int16, cd[j].supPartID);
                                        db.AddInParameter(Dbcmd, "@positionID", DbType.Int16, cd[j].positionID);
                                        //db.AddInParameter(Dbcmd, "@headUserID", DbType.Int16, cd[j].headUserID);
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