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
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace wattanapanitkm
{

    public partial class report1 : System.Web.UI.Page
    {
        private Database db;
        private DbCommand Dbcmd;
        private DataTable dt;
        private DataSet ds = new DataSet();
        private string strsql;
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private List<string> path=new List<string>();
        public report1()
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
            if (!IsPostBack)
            {
                String InstalledPrinters;
                for (int count = 0; count < PrinterSettings.InstalledPrinters.Count; count++)
                {
                    InstalledPrinters = PrinterSettings.InstalledPrinters[count];
                    DropDownList1.Items.Add(InstalledPrinters);
                }
                dt = ((System.Data.DataTable)Session["headposition_taskover"]);
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("jtdatask.rdlc");
                ReportDataSource datasource = new ReportDataSource("wattanapanitDataSet", dt);
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.LocalReport.Refresh();
            }

        }
        private Stream CreateStream(string name, string fileNameExtension,
        Encoding encoding,
            string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(Server.MapPath(name + "." + fileNameExtension),
                FileMode.Create);
            m_streams.Add(stream);
            
            path.Add(Server.MapPath(name + "." + fileNameExtension));
            return stream;
        }
        private void Export(LocalReport report)
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              "  <PageWidth>8.5in</PageWidth>" +
              "  <PageHeight>11in</PageHeight>" +
              "  <MarginTop>0.25in</MarginTop>" +
              "  <MarginLeft>0.25in</MarginLeft>" +
              "  <MarginRight>0.25in</MarginRight>" +
              "  <MarginBottom>0.25in</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
            ev.Graphics.DrawImage(pageImage, 0, 0);

            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        private void Print()
        {
            string printerName="";

            printerName = DropDownList1.Text;
            if (string.IsNullOrEmpty(printerName)) { printerName = "Microsoft XPS Document Writer"; }

            if (m_streams == null || m_streams.Count == 0)
                return;

            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".",
                    printerName);
                Response.Write(msg);
                return;
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();
            path=null;
            
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            Export(ReportViewer1.LocalReport);
            m_currentPageIndex = 0;
            Print();
        }

    }
}