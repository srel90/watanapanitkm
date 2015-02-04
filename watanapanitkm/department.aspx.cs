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

    public partial class department : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public department()
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
                strsql = "true";
                clsDepartment clsDepartment = new clsDepartment();
                int status = 0;
                switch (mode)
                {
                    case "verifyDepartmentCode":
                        clsDepartment.code = Request["code"];
                        Response.Write(verifyDepartmentCode(clsDepartment).ToString());
                        break;
                    case "selectAllDepartment": Response.Write(selectAllDepartment());
                        break;
                    case "selectAllSector": Response.Write(selectAllSector());
                        break;
                    case "selectAllHeadUser": Response.Write(selectAllHeadUser());
                        break;
                    case "searchHeadUser": Response.Write(searchHeadUser(Request["search"].ToString()));
                        break;
                    case "insert":
                        clsDepartment.code = Request["code"];
                        clsDepartment.name = Request["name"];
                        clsDepartment.detail = Request["detail"];
                        clsDepartment.sectorID = Convert.ToInt16(Request["SectorID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsDepartment.status = status;
                        Response.Write(insert(clsDepartment));
                        break;
                    case "update":
                        clsDepartment.departmentID = Convert.ToInt16(Request["id"]);
                        clsDepartment.code = Request["code"];
                        clsDepartment.name = Request["name"];
                        clsDepartment.detail = Request["detail"];
                        clsDepartment.sectorID = Convert.ToInt16(Request["sectorID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsDepartment.status = status;
                        Response.Write(update(clsDepartment));
                        break;
                    case "delete":
                        clsDepartment.departmentID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsDepartment));
                        break;
                }
                Response.End();
            }
        }
        public string verifyDepartmentCode(clsDepartment clsDepartment)
        {
            strsql = "SELECT * FROM department WHERE code=@code";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsDepartment.code);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string selectAllSector()
        {
            strsql = "SELECT * FROM sector where status=1";
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
        public string selectAllDepartment()
        {
            strsql = "SELECT dp.*,st.name as sectorName,u.firstname,u.lastname FROM department dp left outer join sector st on dp.sectorID=st.sectorID left outer join users u on u.usersID=dp.headID ORDER BY departmentID DESC";
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
        public string selectAllHeadUser()
        {
            strsql = "SELECT u.*,com.name as companyName,sec.name as sectorName,dep.name as departmentName,part.name as partName from users u ";
            strsql += "left outer join company com on com.companyID=u.companyID ";
            strsql += "left outer join sector sec on sec.sectorID=u.sectorID ";
            strsql += "left outer join department dep on dep.departmentID=u.departmentID ";
            strsql += "left outer join part part on part.partID=u.partID where u.status=1 ";
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
        public string searchHeadUser(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                strsql = "SELECT u.*,com.name as companyName,sec.name as sectorName,dep.name as departmentName,part.name as partName from users u ";
                strsql += "left outer join company com on com.companyID=u.companyID ";
                strsql += "left outer join sector sec on sec.sectorID=u.sectorID ";
                strsql += "left outer join department dep on dep.departmentID=u.departmentID ";
                strsql += "left outer join part part on part.partID=u.partID where u.status=1 ";
            }
            else
            {
                strsql = "SELECT u.*,com.name as companyName,sec.name as sectorName,dep.name as departmentName,part.name as partName from users u ";
                strsql += "left outer join company com on com.companyID=u.companyID ";
                strsql += "left outer join sector sec on sec.sectorID=u.sectorID ";
                strsql += "left outer join department dep on dep.departmentID=u.departmentID ";
                strsql += "left outer join part part on part.partID=u.partID where ";
                strsql += "u.firstname like '%" + search + "%' or u.lastname like '%" + search + "%' or u.username like '%" + search + "%' or com.name like '%" + search + "%' or sec.name like '%" + search + "%' or dep.name like '%" + search + "%' or part.name like '%" + search + "%' and u.status=1 ";
            }
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
        public string insert(clsDepartment clsDepartment)
        {
            strsql = "INSERT INTO department(";
            strsql += "code,";
            strsql += "name,";
            strsql += "detail,";
            strsql += "sectorID,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@code,";
            strsql += "@name,";
            strsql += "@detail,";
            strsql += "@sectorID,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsDepartment.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsDepartment.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsDepartment.detail);
                db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, clsDepartment.sectorID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsDepartment.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsDepartment clsDepartment)
        {
            strsql = "UPDATE department SET ";
            strsql += "code=@code,";
            strsql += "name=@name,";
            strsql += "detail=@detail,";
            strsql += "sectorID=@sectorID,";
            strsql += "status=@status ";
            strsql += "WHERE departmentID=@departmentID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsDepartment.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsDepartment.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsDepartment.detail);
                db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, clsDepartment.sectorID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsDepartment.status);
                db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, clsDepartment.departmentID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsDepartment clsDepartment)
        {
            strsql = "DELETE FROM department ";
            strsql += "WHERE departmentID=@departmentID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, clsDepartment.departmentID);
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