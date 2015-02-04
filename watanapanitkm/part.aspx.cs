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

    public partial class part : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public part()
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
                clsPart clsPart = new clsPart();
                int status = 0;
                switch (mode)
                {
                    case "verifyPartCode":
                        clsPart.code = Request["code"];
                        Response.Write(verifyPartCode(clsPart).ToString());
                        break;
                    case "selectAllDepartment": Response.Write(selectAllDepartment());
                        break;
                    case "selectAllPart": Response.Write(selectAllPart());
                        break;
                    case "selectAllHeadUser": Response.Write(selectAllHeadUser());
                        break;
                    case "searchHeadUser": Response.Write(searchHeadUser(Request["search"].ToString()));
                        break;
                    case "insert":
                        clsPart.code = Request["code"];
                        clsPart.name = Request["name"];
                        clsPart.detail = Request["detail"];
                        clsPart.departmentID = Convert.ToInt16(Request["departmentID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsPart.status = status;
                        Response.Write(insert(clsPart));
                        break;
                    case "update":
                        clsPart.partID = Convert.ToInt16(Request["id"]);
                        clsPart.code = Request["code"];
                        clsPart.name = Request["name"];
                        clsPart.detail = Request["detail"];
                        clsPart.departmentID = Convert.ToInt16(Request["departmentID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsPart.status = status;
                        Response.Write(update(clsPart));
                        break;
                    case "delete":
                        clsPart.partID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsPart));
                        break;
                }
                Response.End();
            }
        }
        public string verifyPartCode(clsPart clsPart)
        {
            strsql = "SELECT * FROM part WHERE code=@code";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsPart.code);
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
        public string selectAllDepartment()
        {
            strsql = "SELECT * FROM department where status=1";
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
        public string selectAllPart()
        {
            strsql = "SELECT p.*,d.name as departmentName,u.firstname,u.lastname FROM part p left outer join department d on p.departmentID=d.departmentID left outer join users u on u.usersID=p.headID ORDER BY p.partID DESC";
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
        public string insert(clsPart clsPart)
        {
            strsql = "INSERT INTO part(";
            strsql += "code,";
            strsql += "name,";
            strsql += "detail,";
            strsql += "departmentID,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@code,";
            strsql += "@name,";
            strsql += "@detail,";
            strsql += "@departmentID,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsPart.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsPart.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsPart.detail);
                db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, clsPart.departmentID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsPart.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsPart clsPart)
        {
            strsql = "UPDATE part SET ";
            strsql += "code=@code,";
            strsql += "name=@name,";
            strsql += "detail=@detail,";
            strsql += "departmentID=@departmentID,";
            strsql += "status=@status ";
            strsql += "WHERE partID=@partID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsPart.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsPart.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsPart.detail);
                db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, clsPart.departmentID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsPart.status);
                db.AddInParameter(Dbcmd, "@partID", DbType.Int16, clsPart.partID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsPart clsPart)
        {
            strsql = "DELETE FROM part ";
            strsql += "WHERE partID=@partID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@partID", DbType.Int16, clsPart.partID);
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