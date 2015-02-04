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

namespace wattanapanitkm
{
    
    public partial class _default : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public _default()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            db = new DatabaseProviderFactory().Create("connString");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string mode=Request["mode"];
            if (!String.IsNullOrEmpty(mode))
            {
                strsql = "true";
                switch(mode){
                    case "checkLogin":
                        clsEmployee clsEmployee = new clsEmployee();
                        clsEmployee.username = Request["username"];
                        clsEmployee.password = Request["password"];
                        Response.Write(checkUser(clsEmployee).ToString());
                        break;
                }
                Response.End();
            }
        }
        public string checkUser(clsEmployee clsEmployee)
        {
            strsql = "SELECT em.*,us.companyID,us.sectorID,us.departmentID,us.partID,us.subPartID FROM employee em left outer join users us on em.refUserID=us.usersID WHERE em.username=@username AND em.password=@password AND em.status=1 ";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@username", DbType.String, clsEmployee.username);
                db.AddInParameter(Dbcmd, "@password", DbType.String, clsEmployee.password);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    Session["USER"] = dt;
                    DataTable dts;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["refUserID"].ToString()))
                    {
                        strsql = "select * from sector where headID='" + dt.Rows[0]["refUserID"] + "'";
                        Dbcmd = db.GetSqlStringCommand(strsql);
                        dts = db.ExecuteDataSet(Dbcmd).Tables[0];
                        if (dts.Rows.Count != 0)
                        {
                            Session["headposition"] = "sector";
                            strsql = "select *,DATEDIFF(day,jtda.requiringDateTime,GETDATE()) as numday from jtda jtda ";
                            strsql += " left outer join users users on jtda.practitionersID=users.usersID where DATEDIFF(day,jtda.requiringDateTime,GETDATE())>20 and (jtda.workProgress is null or jtda.workProgress<>100) and users.partID='" + dt.Rows[0]["partID"] + "' and users.departmentID='" + dt.Rows[0]["departmentID"] + "' and users.sectorID='" + dt.Rows[0]["sectorID"] + "' and users.companyID='" + dt.Rows[0]["companyID"] + "'";
                            Dbcmd = db.GetSqlStringCommand(strsql);
                            Session["headposition_taskover"] = db.ExecuteDataSet(Dbcmd).Tables[0];
                        }
                        else
                        {
                            strsql = "select * from department where headID='" + dt.Rows[0]["refUserID"] + "'";
                            dts.Clear();
                            Dbcmd = db.GetSqlStringCommand(strsql);
                            dts = db.ExecuteDataSet(Dbcmd).Tables[0];
                            if (dts.Rows.Count != 0)
                            {
                                Session["headposition"] = "department";
                                strsql = "select *,DATEDIFF(day,jtda.requiringDateTime,GETDATE()) as numday from jtda jtda ";
                                strsql += " left outer join users users on jtda.practitionersID=users.usersID where DATEDIFF(day,jtda.requiringDateTime,GETDATE())>14 and (jtda.workProgress is null or jtda.workProgress<>100) and users.partID='" + dt.Rows[0]["partID"] + "' and users.departmentID='" + dt.Rows[0]["departmentID"] + "' and users.sectorID='" + dt.Rows[0]["sectorID"] + "' and users.companyID='" + dt.Rows[0]["companyID"] + "'";
                                Dbcmd = db.GetSqlStringCommand(strsql);
                                Session["headposition_taskover"] = db.ExecuteDataSet(Dbcmd).Tables[0];
                            }
                            else
                            {
                                strsql = "select * from part where headID='" + dt.Rows[0]["refUserID"] + "'";
                                dts.Clear();
                                Dbcmd = db.GetSqlStringCommand(strsql);
                                dts = db.ExecuteDataSet(Dbcmd).Tables[0];
                                if (dts.Rows.Count != 0)
                                {
                                    Session["headposition"] = "part";
                                    strsql = "select jtda.*,DATEDIFF(day,jtda.requiringDateTime,GETDATE()) as numday,concat(users.firstname,' ',users.lastname) as practitioner,concat(users2.firstname,' ',users2.lastname) as reporter from jtda jtda ";
                                    strsql += " left outer join users users on jtda.practitionersID=users.usersID ";
                                    strsql += " left outer join users users2 on jtda.reporterID=users2.usersID ";
                                    strsql += " where DATEDIFF(day,jtda.requiringDateTime,GETDATE())>7 and (jtda.workProgress is null or jtda.workProgress<>100) and users.partID='" + dt.Rows[0]["partID"] + "' and users.departmentID='" + dt.Rows[0]["departmentID"] + "' and users.sectorID='" + dt.Rows[0]["sectorID"] + "' and users.companyID='" + dt.Rows[0]["companyID"] + "'";
                                    Dbcmd = db.GetSqlStringCommand(strsql);
                                    dts.Clear();
                                    dts = db.ExecuteDataSet(Dbcmd).Tables[0];
                                    Session["headposition_taskover"] = db.ExecuteDataSet(Dbcmd).Tables[0];
                                }
                                else
                                {
                                    Session["headposition"] = "normal";
                                    Session["headposition_taskover"] = null;
                                }
                            }
                        }
                    }
                    
                    

                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}