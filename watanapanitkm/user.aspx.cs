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

    public partial class user : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public user()
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
                clsUser clsUser = new clsUser();
                int status = 0;
                switch (mode)
                {
                    case "selectAllUser": Response.Write(selectAllUser());
                        break;
                    case "selectAllPosition": Response.Write(selectAllPosition());
                        break;
                    case "selectAllCompany": Response.Write(selectAllCompany());
                        break;
                    case "selectAllSector": Response.Write(selectAllSector());
                        break;
                    case "selectAllDepartment": Response.Write(selectAllDepartment());
                        break;
                    case "selectAllPart": Response.Write(selectAllPart());
                        break;
                    case "selectAllSubPart": Response.Write(selectAllSubPart());
                        break;
                    case "insert":
                        clsUser.firstname = Request["firstname"];
                        clsUser.lastname = Request["lastname"];
                        clsUser.username = Request["username"];
                        clsUser.password = Request["password"];
                        clsUser.companyID = !string.IsNullOrEmpty(Request["companyID"]) ? Convert.ToInt16(Request["companyID"]):(int?)null;
                        clsUser.sectorID = !string.IsNullOrEmpty(Request["sectorID"]) ? Convert.ToInt16(Request["sectorID"]) : (int?)null;
                        clsUser.departmentID = !string.IsNullOrEmpty(Request["departmentID"]) ? Convert.ToInt16(Request["departmentID"]) : (int?)null;
                        clsUser.partID = !string.IsNullOrEmpty(Request["partID"]) ? Convert.ToInt16(Request["partID"]) : (int?)null;
                        clsUser.supPartID = !string.IsNullOrEmpty(Request["supPartID"]) ? Convert.ToInt16(Request["supPartID"]) : (int?)null;
                        clsUser.positionID = !string.IsNullOrEmpty(Request["positionID"]) ? Convert.ToInt16(Request["positionID"]) : (int?)null;
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsUser.status = status;
                        Response.Write(insert(clsUser));
                        break;
                    case "update":
                        clsUser.usersID = Convert.ToInt16(Request["id"]);
                        clsUser.firstname = Request["firstname"];
                        clsUser.lastname = Request["lastname"];
                        clsUser.username = Request["username"];
                        clsUser.password = Request["password"];
                        clsUser.companyID = !string.IsNullOrEmpty(Request["companyID"]) ? Convert.ToInt16(Request["companyID"]):(int?)null;
                        clsUser.sectorID = !string.IsNullOrEmpty(Request["sectorID"]) ? Convert.ToInt16(Request["sectorID"]) : (int?)null;
                        clsUser.departmentID = !string.IsNullOrEmpty(Request["departmentID"]) ? Convert.ToInt16(Request["departmentID"]) : (int?)null;
                        clsUser.partID = !string.IsNullOrEmpty(Request["partID"]) ? Convert.ToInt16(Request["partID"]) : (int?)null;
                        clsUser.supPartID = !string.IsNullOrEmpty(Request["supPartID"]) ? Convert.ToInt16(Request["supPartID"]) : (int?)null;
                        clsUser.positionID = !string.IsNullOrEmpty(Request["positionID"]) ? Convert.ToInt16(Request["positionID"]) : (int?)null;
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsUser.status = status;
                        Response.Write(update(clsUser));
                        break;
                    case "delete":
                        clsUser.usersID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsUser));
                        break;
                }
                Response.End();
            }
        }
        public string selectAllUser()
        {
            strsql = "SELECT u.*,p.name as positionName,com.name as companyName,sec.name as sectorName,dep.name as departmentName,part.name as partName,spart.name as subPartName FROM users u ";
            strsql += "left outer join company com on u.companyID=com.companyID ";
            strsql += "left outer join sector sec on u.sectorID=sec.sectorID ";
            strsql += "left outer join department dep on u.departmentID=dep.departmentID ";
            strsql += "left outer join part part on u.partID=part.partID ";
            strsql += "left outer join subPart spart on u.subPartID=spart.subPartID ";
            strsql += "left outer join position p on u.positionID=p.positionID ";
            if (!string.IsNullOrEmpty(Request["filter[filters][0][field]"]))
            {
                strsql += " where " + Request["filter[filters][0][field]"] + " like '%" + Request["filter[filters][0][value]"] + "%'";
            }

            if (!string.IsNullOrEmpty(Request["sort[0][field]"]))
            {
                strsql += " order by " + Request["sort[0][field]"] + " " + Request["sort[0][dir]"];
            }
            else
            {
                strsql += " order by usersID DESC ";
            }
            strsql += " OFFSET "+Request["skip"]+" ROWS FETCH NEXT "+Request["take"]+" ROWS ONLY";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];

                string strsqlt = "select * from users;";
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
        public string selectAllPosition()
        {
            strsql = "SELECT * FROM position where status=1;";
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
        public string selectAllCompany()
        {
            strsql = "SELECT * FROM company where status=1;";
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
        public string selectAllSector()
        {
            strsql = "SELECT * FROM sector where status=1;";
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
            strsql = "SELECT * FROM department where status=1;";
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
            strsql = "SELECT * FROM part where status=1;";
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
        public string selectAllSubPart()
        {
            strsql = "SELECT * FROM subPart where status=1;";
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
        public string insert(clsUser clsUser)
        {
            strsql = "INSERT INTO users(";
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
            strsql += "status";
            strsql += ")VALUES(";
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
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@firstname", DbType.String, clsUser.firstname);
                db.AddInParameter(Dbcmd, "@lastname", DbType.String, clsUser.lastname);
                db.AddInParameter(Dbcmd, "@username", DbType.String, clsUser.username);
                db.AddInParameter(Dbcmd, "@password", DbType.String, clsUser.password);
                db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, clsUser.companyID);
                db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, clsUser.sectorID);
                db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, clsUser.departmentID);
                db.AddInParameter(Dbcmd, "@partID", DbType.Int16, clsUser.partID);
                db.AddInParameter(Dbcmd, "@subPartID", DbType.Int16, clsUser.supPartID);
                db.AddInParameter(Dbcmd, "@positionID", DbType.Int16, clsUser.positionID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsUser.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsUser clsUser)
        {
            strsql = "UPDATE users SET ";
            strsql += "firstname=@firstname,";
            strsql += "lastname=@lastname,";
            strsql += "username=@username,";
            if (!string.IsNullOrEmpty(clsUser.password))
            {
                strsql += "password=@password,";
            }
            strsql += "companyID=@companyID,";
            strsql += "sectorID=@sectorID,";
            strsql += "departmentID=@departmentID,";
            strsql += "partID=@partID,";
            strsql += "subPartID=@subPartID,";
            strsql += "positionID=@positionID,";
            strsql += "status=@status ";
            strsql += "WHERE usersID=@usersID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@firstname", DbType.String, clsUser.firstname);
                db.AddInParameter(Dbcmd, "@lastname", DbType.String, clsUser.lastname);
                db.AddInParameter(Dbcmd, "@username", DbType.String, clsUser.username);
                if (!string.IsNullOrEmpty(clsUser.password))
                {
                    db.AddInParameter(Dbcmd, "@password", DbType.String, clsUser.password);
                }
                
                db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, clsUser.companyID);
                db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, clsUser.sectorID);
                db.AddInParameter(Dbcmd, "@departmentID", DbType.Int16, clsUser.departmentID);
                db.AddInParameter(Dbcmd, "@partID", DbType.Int16, clsUser.partID);
                db.AddInParameter(Dbcmd, "@subPartID", DbType.Int16, clsUser.supPartID);
                db.AddInParameter(Dbcmd, "@positionID", DbType.Int16, clsUser.positionID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsUser.status);
                db.AddInParameter(Dbcmd, "@usersID", DbType.Int16, clsUser.usersID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsUser clsUser)
        {
            strsql = "DELETE FROM users ";
            strsql += "WHERE usersID=@usersID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@usersID", DbType.Int16, clsUser.usersID);
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