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

    public partial class subpart : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public subpart()
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
                clsSubPart clsSubPart = new clsSubPart();
                int status = 0;
                switch (mode)
                {
                    case "verifySubPartCode":
                        clsSubPart.code = Request["code"];
                        Response.Write(verifySubPartCode(clsSubPart).ToString());
                        break;
                    case "selectAllPart": Response.Write(selectAllPart());
                        break;
                    case "selectAllSubPart": Response.Write(selectAllSubPart());
                        break;
                    case "insert":
                        clsSubPart.code = Request["code"];
                        clsSubPart.name = Request["name"];
                        clsSubPart.detail = Request["detail"];
                        clsSubPart.partID = Convert.ToInt16(Request["partID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsSubPart.status = status;
                        Response.Write(insert(clsSubPart));
                        break;
                    case "update":
                        clsSubPart.subPartID = Convert.ToInt16(Request["id"]);
                        clsSubPart.code = Request["code"];
                        clsSubPart.name = Request["name"];
                        clsSubPart.detail = Request["detail"];
                        clsSubPart.partID = Convert.ToInt16(Request["partID"]);
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsSubPart.status = status;
                        Response.Write(update(clsSubPart));
                        break;
                    case "delete":
                        clsSubPart.subPartID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsSubPart));
                        break;
                }
                Response.End();
            }
        }
        public string verifySubPartCode(clsSubPart clsSubPart)
        {
            strsql = "SELECT * FROM subpart WHERE code=@code";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsSubPart.code);
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
        public string selectAllPart()
        {
            strsql = "SELECT * FROM part where status=1";
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
            strsql = "SELECT sp.*,p.name as partName FROM subpart sp left outer join part p on sp.partID=p.partID ORDER BY sp.subPartID DESC";
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
        public string insert(clsSubPart clsSubPart)
        {
            strsql = "INSERT INTO subpart(";
            strsql += "code,";
            strsql += "name,";
            strsql += "detail,";
            strsql += "partID,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@code,";
            strsql += "@name,";
            strsql += "@detail,";
            strsql += "@partID,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsSubPart.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsSubPart.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsSubPart.detail);
                db.AddInParameter(Dbcmd, "@partID", DbType.Int16, clsSubPart.partID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsSubPart.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsSubPart clsSubPart)
        {
            strsql = "UPDATE subpart SET ";
            strsql += "code=@code,";
            strsql += "name=@name,";
            strsql += "detail=@detail,";
            strsql += "partID=@partID,";
            strsql += "status=@status ";
            strsql += "WHERE subPartID=@subPartID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@code", DbType.String, clsSubPart.code);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsSubPart.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsSubPart.detail);
                db.AddInParameter(Dbcmd, "@partID", DbType.Int16, clsSubPart.partID);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsSubPart.status);
                db.AddInParameter(Dbcmd, "@subPartID", DbType.Int16, clsSubPart.subPartID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsSubPart clsSubPart)
        {
            strsql = "DELETE FROM subpart ";
            strsql += "WHERE subPartID=@subPartID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@subPartID", DbType.Int16, clsSubPart.subPartID);
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