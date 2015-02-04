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

    public partial class employee : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public employee()
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
                clsEmployee clsEmployee = new clsEmployee();
                int status = 0;
                switch (mode)
                {
                    case "selectAllEmployee": Response.Write(selectAllEmployee());
                        break;
                    case "selectAllRefUser": Response.Write(selectAllRefUser());
                        break;
                    case "searchRefUser": Response.Write(searchRefUser(Request["search"].ToString()));
                        break;
                    case "insert":
                        clsEmployee.name = Request["name"];
                        clsEmployee.username = Request["username"];
                        clsEmployee.password = Request["password"];
                        clsEmployee.refUserID = !string.IsNullOrEmpty(Request["refUserID"]) ? Convert.ToInt16(Request["refUserID"]) : (int?)null;
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsEmployee.status = status;
                        Response.Write(insert(clsEmployee));
                        break;
                    case "update":
                        clsEmployee.employeeID = Convert.ToInt16(Request["id"]);
                        clsEmployee.name = Request["name"];
                        clsEmployee.username = Request["username"];
                        clsEmployee.password = Request["password"];
                        clsEmployee.refUserID = !string.IsNullOrEmpty(Request["refUserID"]) ? Convert.ToInt16(Request["refUserID"]) : (int?)null;
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsEmployee.status = status;
                        Response.Write(update(clsEmployee));
                        break;
                    case "delete":
                        clsEmployee.employeeID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsEmployee));
                        break;
                }
                Response.End();
            }
        }
        public string selectAllEmployee()
        {
            strsql = "SELECT em.*,u.firstname,u.lastname,u.username as refusername FROM employee em left outer join users u on em.refUserID=u.usersID";

            if (!string.IsNullOrEmpty(Request["filter[filters][0][field]"]))
            {
                strsql += " where " + Request["filter[filters][0][field]"] + " like '%" + Request["filter[filters][0][value]"] + "%'";
            }

            if (!string.IsNullOrEmpty(Request["sort[0][field]"]))
            {
                strsql += " order by em." + Request["sort[0][field]"] + " " + Request["sort[0][dir]"];
            }
            else
            {
                strsql += " order by em.employeeID DESC ";
            }
            
            strsql += " OFFSET " + Request["skip"] + " ROWS FETCH NEXT " + Request["take"] + " ROWS ONLY";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                dt = db.ExecuteDataSet(Dbcmd).Tables[0];

                string strsqlt = "select * from employee;";
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
        public string selectAllRefUser()
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
        public string searchRefUser(string search)
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
                strsql += "u.firstname like N'%" + search + "%' or u.lastname like N'%" + search + "%' or com.name like N'%" + search + "%' or sec.name like N'%" + search + "%' or dep.name like N'%" + search + "%' or part.name like N'%" + search + "%' and u.status=1 ";
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
        public string insert(clsEmployee clsEmployee)
        {
            strsql = "INSERT INTO employee(";
            strsql += "name,";
            strsql += "username,";
            strsql += "password,";
            strsql += "refUserID,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@name,";
            strsql += "@username,";
            strsql += "@password,";
            strsql += "@refUserID,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsEmployee.name);
                db.AddInParameter(Dbcmd, "@username", DbType.String, clsEmployee.username);
                db.AddInParameter(Dbcmd, "@password", DbType.String, clsEmployee.password);
                db.AddInParameter(Dbcmd, "@refUserID", DbType.Int16, clsEmployee.refUserID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsEmployee.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsEmployee clsEmployee)
        {
            strsql = "UPDATE employee SET ";
            strsql += "name=@name,";
            strsql += "username=@username,";
            if (!string.IsNullOrEmpty(clsEmployee.password))
            {
                strsql += "password=@password,";
            }
            strsql += "refUserID=@refUserID,";
            strsql += "status=@status ";
            strsql += "WHERE employeeID=@employeeID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsEmployee.name);
                db.AddInParameter(Dbcmd, "@username", DbType.String, clsEmployee.username);
                if (!string.IsNullOrEmpty(clsEmployee.password))
                {
                    db.AddInParameter(Dbcmd, "@password", DbType.String, clsEmployee.password);
                }

                db.AddInParameter(Dbcmd, "@refUserID", DbType.Int16, clsEmployee.refUserID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsEmployee.status);
                db.AddInParameter(Dbcmd, "@employeeID", DbType.Int16, clsEmployee.employeeID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsEmployee clsEmployee)
        {
            strsql = "DELETE FROM employee ";
            strsql += "WHERE employeeID=@employeeID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@employeeID", DbType.Int16, clsEmployee.employeeID);
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