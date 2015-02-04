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

    public partial class position : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public position()
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
                clsPosition clsPosition = new clsPosition();
                int status = 0;
                switch (mode)
                {
                    case "verifyPositionName":
                        clsPosition.name = Request["name"];
                        Response.Write(verifyPositionName(clsPosition).ToString());
                        break;
                    case "selectAllPosition": Response.Write(selectAllPosition());
                        break;
                    case "insert":
                        clsPosition.name = Request["name"];
                        clsPosition.detail = Request["detail"];
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsPosition.status = status;
                        Response.Write(insert(clsPosition));
                        break;
                    case "update":
                        clsPosition.positionID = Convert.ToInt16(Request["id"]);
                        clsPosition.name = Request["name"];
                        clsPosition.detail = Request["detail"];
                        status =  !string.IsNullOrEmpty(Request["status"])? Convert.ToInt16(Request["status"]) : 0;
                        clsPosition.status = status;
                        Response.Write(update(clsPosition));
                        break;
                    case "delete":
                        clsPosition.positionID = Convert.ToInt16(Request["id"]);
                        Response.Write(delete(clsPosition));
                        break;
                }
                Response.End();
            }
        }
        public string verifyPositionName(clsPosition clsPosition)
        {
            strsql = "SELECT * FROM position WHERE name=@name";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsPosition.name);
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
        public string selectAllPosition()
        {
            strsql = "SELECT * FROM position ORDER BY positionID DESC";
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
        public string insert(clsPosition clsPosition)
        {
            strsql = "INSERT INTO position(";
            strsql += "name,";
            strsql += "detail,";
            strsql += "status";
            strsql += ")VALUES(";
            strsql += "@name,";
            strsql += "@detail,";
            strsql += "@status";
            strsql += ");";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsPosition.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsPosition.detail);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsPosition.status);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string update(clsPosition clsPosition)
        {
            strsql = "UPDATE position SET ";
            strsql += "name=@name,";
            strsql += "detail=@detail,";
            strsql += "status=@status ";
            strsql += "WHERE positionID=@positionID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsPosition.name);
                db.AddInParameter(Dbcmd, "@detail", DbType.String, clsPosition.detail);
                db.AddInParameter(Dbcmd, "@status", DbType.Int16, clsPosition.status);
                db.AddInParameter(Dbcmd, "@positionID", DbType.Int16, clsPosition.positionID);
                db.ExecuteNonQuery(Dbcmd);
                return "true";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string delete(clsPosition clsPosition)
        {
            strsql = "DELETE FROM position ";
            strsql += "WHERE positionID=@positionID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@positionID", DbType.Int16, clsPosition.positionID);
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