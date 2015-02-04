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

    public partial class sector : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public sector()
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
                clsSector clsSector = new clsSector();
                int status = 0;
                switch (mode)
                {
                    case "verifySectorCode":
                        clsSector.code = Request["code"];
                        Response.Write(verifySectorCode(clsSector).ToString());
                        break;
                    case "selectAllSector": Response.Write(selectAllSector());
                        break;
                    case "selectAllCompany": Response.Write(selectAllCompany());
                        break;
                    case "selectAllHeadUser": Response.Write(selectAllHeadUser());
                        break;
                    case "searchHeadUser": Response.Write(searchHeadUser(Request["search"].ToString()));
                        break;
                    case "insert":
                        clsSector.code = Request["code"];
                        clsSector.name = Request["name"];
                        clsSector.detail = Request["detail"];
                        clsSector.companyID = Convert.ToInt16(Request["companyID"]);
                        clsSector.headID = Convert.ToInt16(Request["headID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsSector.status = status;
                        Response.Write(insert(clsSector));
                        break;
                    case "update":
                        clsSector.sectorID = Convert.ToInt16(Request["id"]);
                        clsSector.code = Request["code"];
                        clsSector.name = Request["name"];
                        clsSector.detail = Request["detail"];
                        clsSector.companyID = Convert.ToInt16(Request["companyID"]);
                        clsSector.headID = Convert.ToInt16(Request["headID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsSector.status = status;
                        Response.Write(update(clsSector));
                        break;
                    case "delete":
                        clsSector.sectorID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsSector));
                        break;
                }
                Response.End();
            }
        }
        public string verifySectorCode(clsSector clsSector)
        {
            strsql = "SELECT * FROM sector WHERE code=@code";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsSector.code);
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
        public string selectAllCompany()
        {
            strsql = "SELECT * FROM company where status=1";
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
            strsql = "SELECT st.*,cp.name as companyName,u.firstname,u.lastname FROM sector st left outer join company cp on st.companyID=cp.companyID left outer join users u on u.usersID=st.headID ORDER BY sectorID DESC";
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
        public string insert(clsSector clsSector)
        {
            strsql = "INSERT INTO sector(";
            strsql += "code,";
            strsql += "name,";
            strsql += "detail,";
            strsql += "companyID,";
            strsql += "headID,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@code,";
            strsql += "@name,";
            strsql += "@detail,";
            strsql += "@companyID,";
            strsql += "@headID,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsSector.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsSector.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsSector.detail);
                db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, clsSector.companyID);
                db.AddInParameter(Dbcmd, "@headID", DbType.Int16, clsSector.headID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsSector.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsSector clsSector)
        {
            strsql = "UPDATE sector SET ";
            strsql += "code=@code,";
            strsql += "name=@name,";
            strsql += "detail=@detail,";
            strsql += "companyID=@companyID,";
            strsql += "headID=@headID,";
            strsql += "status=@status ";
            strsql += "WHERE sectorID=@sectorID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsSector.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsSector.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsSector.detail);
                db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, clsSector.companyID);
                db.AddInParameter(Dbcmd, "@headID", DbType.Int16, clsSector.headID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsSector.status);
                db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, clsSector.sectorID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsSector clsSector)
        {
            strsql = "DELETE FROM sector ";
            strsql += "WHERE sectorID=@sectorID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@sectorID", DbType.Int16, clsSector.sectorID);
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