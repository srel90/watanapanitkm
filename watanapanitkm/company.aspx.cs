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

    public partial class company : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public company()
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
                clsCompany clsCompany = new clsCompany();
                int status = 0;
                switch (mode)
                {
                    case "verifyCompanyCode":
                        clsCompany.code = Request["code"];
                        Response.Write(verifyCompanyCode(clsCompany).ToString());
                        break;
                    case "selectAllCompany": Response.Write(selectAllCompany());
                        break;
                    case "insert":
                        clsCompany.code = Request["code"];
                        clsCompany.name = Request["name"];
                        clsCompany.detail = Request["detail"];
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsCompany.status = status;
                        Response.Write(insert(clsCompany));
                        break;
                    case "update":
                        clsCompany.companyID = Convert.ToInt16(Request["id"]);
                        clsCompany.code = Request["code"];
                        clsCompany.name = Request["name"];
                        clsCompany.detail = Request["detail"];
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsCompany.status = status;
                        Response.Write(update(clsCompany));
                        break;
                    case "delete":
                        clsCompany.companyID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsCompany));
                        break;
                }
                Response.End();
            }
        }
        public string verifyCompanyCode(clsCompany clsCompany)
        {
            strsql = "SELECT * FROM company WHERE code=@code";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsCompany.code);
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
            strsql = "SELECT * FROM company ORDER BY companyID DESC";
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
        public string insert(clsCompany clsCompany)
        {
            strsql = "INSERT INTO company(";
            strsql += "code,";
            strsql += "name,";
            strsql += "detail,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@code,";
            strsql += "@name,";
            strsql += "@detail,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsCompany.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsCompany.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsCompany.detail);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsCompany.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsCompany clsCompany)
        {
            strsql = "UPDATE company SET ";
            strsql += "code=@code,";
            strsql += "name=@name,";
            strsql += "detail=@detail,";
            strsql += "status=@status ";
            strsql += "WHERE companyID=@companyID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsCompany.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsCompany.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsCompany.detail);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsCompany.status);
                db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, clsCompany.companyID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsCompany clsCompany)
        {
            strsql = "DELETE FROM company ";
            strsql += "WHERE companyID=@companyID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@companyID", DbType.Int16, clsCompany.companyID);
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