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

    public partial class updateemployee : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        public updateemployee()
        {
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
            db = new DatabaseProviderFactory().Create("connString");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USER"] == null)
            {
                Response.Redirect("default.aspx");
            }
            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(mode))
            {
                strsql = "true";
                clsEmployee clsEmployee = new clsEmployee();
                switch (mode)
                {
                    case "update":
                        clsEmployee.employeeID = Convert.ToInt16(Request["id"]);
                        clsEmployee.name = Request["name"];
                        clsEmployee.username = Request["username"];
                        clsEmployee.password = Request["password"];
                    
                        Response.Write(update(clsEmployee));
                        break;
                }
                Response.End();
            }
        }
       
        public string update(clsEmployee clsEmployee)
        {
            strsql = "UPDATE employee SET ";
            strsql += "name=@name";
            if (!string.IsNullOrEmpty(clsEmployee.password))
            {
                strsql += ",password=@password";
            }
            strsql += " WHERE employeeID=@employeeID;";
            try
            {

                Dbcmd = db.GetSqlStringCommand(strsql);
                db.AddInParameter(Dbcmd, "@name", DbType.String, clsEmployee.name);

                if (!string.IsNullOrEmpty(clsEmployee.password))
                {
                    db.AddInParameter(Dbcmd, "@password", DbType.String, clsEmployee.password);
                }

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