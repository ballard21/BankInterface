using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JobAssignment
{
    public partial class Accounts : System.Web.UI.Page
    {
        string account = "";
        string transferApi = System.Configuration.ConfigurationManager.AppSettings["InterviewTransfer"];
        string statementApi = System.Configuration.ConfigurationManager.AppSettings["InterviewStatement"];
        protected void Page_Load(object sender, EventArgs e)
        {
            account = Request.QueryString["acc"];
            if (IsPostBack == false)
            {
                btnDownload.Visible = false;
                txtStartDate.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                statement(txtStartDate.Text, txtEndDate.Text);
            }
        }
        protected void DownloadReportClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    lblmsg.Text = "Start Date Cannot be null";
                }
                else if (string.IsNullOrEmpty(txtEndDate.Text))
                {
                    lblmsg.Text = "End Date Cannot be null";
                }
                else
                {
                    DownlaodStatement(txtStartDate.Text, txtEndDate.Text);
                }
            }
            catch (Exception ee)
            {
                lblmsg.Text = "Error Occured. " + ee.Message;
            }
        }

        protected void SearchClicked(object sender, EventArgs e)
        {
            try
            {
                statement(txtStartDate.Text, txtEndDate.Text);
            }
            catch (Exception ee)
            {
                lblmsg.Text = "" + ee.Message;
            }
        }


        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public void statement(string startDate, string endDate)
        {
            TransactionResp resp = new TransactionResp();
            btnDownload.Visible = false;
            try
            {
                HttpClient client1 = new HttpClient();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string urlParameters = "";
                // client1.BaseAddress = new Uri(transferApi);


                var builder = new UriBuilder(statementApi);
                //builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["account"] = account;
                query["startDate"] = startDate;
                query["endDate"] = endDate;
                builder.Query = query.ToString();
                string url = builder.ToString();

                HttpResponseMessage response = client1.GetAsync(url).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.

                    Task<string> responseBody = GetjsonString(response);

                    string finalResult = responseBody.Result;

                    var result = JsonConvert.DeserializeObject<StatementResp>(finalResult);

                    DataTable statementSet = ToDataTable(result.result);
                    //  DataTable dtActivity = handler.GetPendingUserApprovals();
                    GridData.DataSource = statementSet;
                    GridData.DataBind();
                    GridData.Visible = true;

                    btnDownload.Visible = true;
                }
                else
                {
                    Task<string> responseBody = GetjsonString(response);
                    string finalResult = responseBody.Result;
                    var result = JsonConvert.DeserializeObject<StatementResp>(finalResult);

                    lblmsg.Text = result.message;

                }

                client1.Dispose();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DownlaodStatement(string startDate, string endDate)
        {
            TransactionResp resp = new TransactionResp();

            try
            {
                HttpClient client1 = new HttpClient();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string urlParameters = "";
                // client1.BaseAddress = new Uri(transferApi);


                var builder = new UriBuilder(statementApi);
                //builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["account"] = account;
                query["startDate"] = startDate;
                query["endDate"] = endDate;
                builder.Query = query.ToString();
                string url = builder.ToString();

                HttpResponseMessage response = client1.GetAsync(url).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.

                    Task<string> responseBody = GetjsonString(response);

                    string finalResult = responseBody.Result;

                    var result = JsonConvert.DeserializeObject<StatementResp>(finalResult);

                    DataTable statementSet = ToDataTable(result.result);
                    string filePath = @"C:\Users\Public\Documents\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".csv";
                    ToCSV(statementSet, filePath);
                }
                else
                {
                    Task<string> responseBody = GetjsonString(response);
                    string finalResult = responseBody.Result;
                    var result = JsonConvert.DeserializeObject<StatementResp>(finalResult);

                    lblmsg.Text = result.message;

                }

                client1.Dispose();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ToCSV(DataTable dtDataTable, string strFilePath)
        {

            var dataList = new List<string>();
            string header = "";
            foreach (DataColumn column in dtDataTable.Columns)
            {
                header += column.ColumnName + ",";
            }
            // dataList.AddRange(header.Split(new string[] { "," }, StringSplitOptions.None));
            dataList.Add(header);
            foreach (DataRow drow in dtDataTable.Rows)
            {
                string row = "";
                foreach (DataColumn column in dtDataTable.Columns)
                {
                    row += drow[column.ColumnName].ToString().Replace(",", "_") + ",";

                }
                dataList.Add(row);

            }
            File.WriteAllLines(strFilePath, dataList.ToArray());
            string ext = Path.GetExtension(strFilePath);
            DownloadingFile(strFilePath, ext);

        }
        public void DownloadingFile(string filePath, string type)
        {

            switch (type)
            {
                case ".xls":
                    type = "excel";
                    break;

                case ".xlsx":
                    type = "excel";

                    break;

                case ".pdf":
                    type = "pdf";

                    break;
                case ".csv":
                    type = "csv";

                    break;
                case ".docx":
                    type = "docx";

                    break;
                case ".doc":
                    type = "doc";

                    break;
            }

            if (!(string.IsNullOrEmpty(type)))
            {
                try
                {
                    //filePath = @"E:\+Jackie\downloadedfiles\PayGo UMEME UAT.docx";
                    FileInfo file = new FileInfo(filePath);
                    HttpContext.Current.Response.Redirect("", false);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                    HttpContext.Current.Response.ContentType = type;
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.TransmitFile(file.FullName);
                    HttpContext.Current.Response.End();
                }
                catch (Exception ee)
                {
                    lblmsg.Text = "" + ee.Message;
                }
            }
        }


        public static DataSet ToDataSets<T>(IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }

        public void transfer(string phone, string amount, string name, string accountNumber)
        {
            TransactionResp resp = new TransactionResp();

            try
            {
                HttpClient client1 = new HttpClient();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string urlParameters = "";
                client1.BaseAddress = new Uri(transferApi);

                object bodyData = new
                {
                    phoneNumber = phone,
                    amount = amount,
                    name = name,
                    accountNumber = accountNumber
                };

                var json = JsonConvert.SerializeObject(bodyData);
                var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage response = client1.PostAsync(urlParameters, content).Result;// Blocking call! Program will wait here until a response is received or a timeout occurs.

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Task<string> responseBody = GetjsonString(response);
                    string finalResult = responseBody.Result;

                    var result = JsonConvert.DeserializeObject<TransactionResp>(finalResult);
                    lblmsg.Text = "Error Code 200 Transaction Successful with Transaction ID " + result.tranId;

                }
                else if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    Task<string> responseBody = GetjsonString(response);
                    string finalResult = responseBody.Result;

                    var result = JsonConvert.DeserializeObject<TransactionResp>(finalResult);
                    lblmsg.Text = "Error Code 202 Transaction Pending with Transaction ID " + result.tranId;

                }
                else
                {
                    Task<string> responseBody = GetjsonString(response);
                    string finalResult = responseBody.Result;
                    var result = JsonConvert.DeserializeObject<TransactionRespError>(finalResult);
                    if (result.TranId < 0)
                        lblmsg.Text = result.Message;
                    else
                        lblmsg.Text = "Error Code 400 Transaction Failed. " + result.Message;

                }

                client1.Dispose();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<string> GetjsonString(HttpResponseMessage response)
        {
            string responseBody = "";
            try
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseBody;
        }

        protected void TransferSearchClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    lblmsg.Text = "Name Cannot be null";
                }
                else if (string.IsNullOrEmpty(txtNarration.Text))
                {
                    lblmsg.Text = "Description Cannot be null";
                }
                else if (string.IsNullOrEmpty(txtPhone.Text))
                {
                    lblmsg.Text = "Phone Cannot be null";
                }
                else if (string.IsNullOrEmpty(txtAmount.Text))
                {
                    lblmsg.Text = "Amount Cannot be null";
                }
                else
                {
                    transfer(txtPhone.Text, txtAmount.Text, txtName.Text, account);
                }
            }
            catch (Exception ee)
            {
                lblmsg.Text = "" + ee.Message;
            }
        }
    }
}