using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vaccination
{
    public partial class Vacciation : System.Web.UI.Page
    {
        public DateTime Date { get; set; }

        // On Page Load Loaction Bind
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                var path = Server.MapPath(@"~/Files/Location.csv");

                using (var reader = new StreamReader(path))
                {
                    List<string> Countries = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        Countries.Add(reader.ReadLine());
                    }
                    ddlLocation.DataSource = Countries;
                    ddlLocation.DataBind();
                };
            }

        }
        //Download File
        protected void btnDownloadDataFile_Click(object sender, EventArgs e)
        {
            string root = @"C:\VacciantionFiles";

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            string FileName = txtFileDownloadLink.Text;


            bool FilePath = File.Exists(root + @"\" + ddlLocation.SelectedItem.Text + ".csv");
            if (FilePath == false)
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    if (txtFileDownloadLink.Text != (lblLink.Text + ddlLocation.SelectedItem.Text + "csv"))
                    {
                        using (WebClient wc = new WebClient())
                        {
                            Uri uri = new Uri(txtFileDownloadLink.Text);
                            wc.DownloadFileAsync(uri, @"C:\VacciantionFiles\" + ddlLocation.Text + ".csv");
                            wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                            wc.DownloadFileCompleted += Client_DownloadFileCompleted;
                        }
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Invalid Link";
                        txtFileDownloadLink.Text = "";
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Kindly Provide Download File Link";
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Orange;
                lblMessage.Text = ddlLocation.SelectedItem.Text + " " + "File Already Exist No Need To Download Again Or Delete First";
                txtFileDownloadLink.Text = "";
                btnDownloadDataFile.Enabled = false;
            }

        }
        //Process File Data
        protected void btnProcessDataFile_Click(object sender, EventArgs e)
        {
            bool FilePath = File.Exists(@"C:\VacciantionFiles" + @"\" + Convert.ToString(ddlLocation.SelectedItem.Text + ".csv"));
            if (FilePath == true)
            {
                using (var reader = new StreamReader(@"C:\VacciantionFiles" + @"\" + Convert.ToString(ddlLocation.SelectedItem.Text + ".csv")))
                {
                    List<string> location = new List<string>();
                    List<string> Date = new List<string>();
                    List<string> Vaccine = new List<string>();
                    List<string> SourceUrl = new List<string>();
                    List<string> TotalVaccinations = new List<string>();
                    List<string> PeopleVaccinated = new List<string>();
                    List<string> PeopleFullyVaccinated = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        bool QuationRemove = line.Contains("\"");
                        if (QuationRemove == true)
                        {
                            var regex = new Regex("\\\"(.*?)\\\"");
                            var output = regex.Replace(line, m => m.Value.Replace(',', '-'));
                            string[] values = output.Split(',');
                            location.Add(values[0]);
                            Date.Add(values[1]);
                            Vaccine.Add(values[2]);
                            SourceUrl.Add(values[3]);
                            TotalVaccinations.Add(values[4]);
                            PeopleVaccinated.Add(values[5]);
                            PeopleFullyVaccinated.Add(values[6]);
                        }
                        else
                        {
                            string[] values = line.Split(',');
                            location.Add(values[0]);
                            Date.Add(values[1]);
                            Vaccine.Add(values[2]);
                            SourceUrl.Add(values[3]);
                            TotalVaccinations.Add(values[4]);
                            PeopleVaccinated.Add(values[5]);
                            PeopleFullyVaccinated.Add(values[6]);
                        }
                        Date.Remove("date");
                        TotalVaccinations.Remove("total_vaccinations");
                        PVactionDetail.Visible = true;
                        PEmailDetail.Visible = true;
                    }
                    List<DateTime> dt = new List<DateTime>();
                    foreach (var item in Date)
                    {
                        dt.Add(Convert.ToDateTime(item));
                    }
                    DateTime LastedDate = dt.Max(p => p);
                    DateTime StartDate = LastedDate.AddDays(-7);
                    

                    //var d= dt.FirstOrDefault(x => x.Date == StartDate);

                    for (int i = 0; i < TotalVaccinations.Count; i++)
                    {
                        lblTotalVaccination.InnerText = string.Join(",", TotalVaccinations);
                        lblLastestDate.Text = LastedDate.Date.ToShortDateString();
                        txtEmailBody.InnerText = "Latest date:- " + string.Join("", LastedDate.Date.ToShortDateString()) + Environment.NewLine + "Total Vaccinations:- " + string.Join(",", TotalVaccinations) + Environment.NewLine + "- Average number of vaccinations in the last 7 days:- ";
                    }
                }
                btnDownloadDataFile.Enabled = false;
                lblMessage.Text = "";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Warning !! File Does't Exist Download the File First";
            }
        }
        //Delete Method
        protected void btnDeleteFile_Click(object sender, EventArgs e)
        {
            bool FilePath = File.Exists(@"C:\VacciantionFiles" + @"\" + Convert.ToString(ddlLocation.SelectedItem.Text + ".csv"));

            if (FilePath == true)
            {
                File.Delete(@"C:\VacciantionFiles" + @"\" + Convert.ToString(ddlLocation.SelectedItem.Text + ".csv"));

                ClearFields();
                lblMessage.ForeColor = System.Drawing.Color.Purple;
                lblMessage.Text = "File Deleted Successfully";
                btnDownloadDataFile.Enabled = true;
                ddlLocation.SelectedIndex = -1;

            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Vaccination file does not exist";
            }

        }
        //Email
        protected async void btnEmailReport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTo.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtCC.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtBCC.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(txtSubject.Text))
                        {
                            string ApiKey = "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";
                            var client = new SendGridClient(ApiKey);
                            var ToEmails = new List<EmailAddress>() { new EmailAddress(txtTo.Text), new EmailAddress(txtCC.Text), new EmailAddress(txtBCC.Text) };
                            var FromEmail = new EmailAddress(txtFrom.Text, "");
                            string EmailBoday = "SampleTex";
                            string EmailSubject = txtSubject.Text;
                            string EmailHtmlContent = txtEmailBody.InnerText;
                            var Email = MailHelper.CreateSingleEmailToMultipleRecipients(FromEmail, ToEmails, EmailSubject, EmailBoday, EmailHtmlContent);
                            var Response = await client.SendEmailAsync(Email);
                            if (Response.IsSuccessStatusCode == true)
                            {

                                ClearFields("Email Sent Sucessefuly");

                            }
                        }
                        else
                        {
                            lblEmailInfo.ForeColor = System.Drawing.Color.DarkRed;
                            lblEmailInfo.Text = "Subject is Required";
                        }
                    }
                    else
                    {
                        lblEmailInfo.ForeColor = System.Drawing.Color.DarkRed;
                        lblEmailInfo.Text = "BCC Email is Required";
                    }
                }
                else
                {
                    lblEmailInfo.ForeColor = System.Drawing.Color.DarkRed;
                    lblEmailInfo.Text = "CC is Required";
                }
            }
            else
            {
                lblEmailInfo.ForeColor = System.Drawing.Color.DarkRed;
                lblEmailInfo.Text = "To Email Required";
            }
        }
        //Location
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocation.SelectedItem.Text != "Select Country")
            {
                if (PVactionDetail.Visible != true)
                {
                    txtFileDownloadLink.Text = "https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/vaccinations/country_data/" + ddlLocation.SelectedItem.Text + ".csv";
                    lblMessage.Text = "";

                }
                else
                {

                    ClearFields("");
                    txtFileDownloadLink.Text = "https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/vaccinations/country_data/" + ddlLocation.SelectedItem.Text + ".csv";

                }

            }

            else
            {
                ClearFields("");

            }

        }
        //Fiels Clear
        protected void ClearFields(string EmailMessage = "")
        {
            PVactionDetail.Visible = false;
            PEmailDetail.Visible = false;
            lblMessage.ForeColor = System.Drawing.Color.DarkBlue;
            lblMessage.Text = EmailMessage;
            btnDownloadDataFile.Enabled = true;
            lblLastestDate.Text = "";
            lblAverageNumberVaccination.Text = "";
            lblTotalVaccination.InnerText = "";
            txtEmailBody.InnerText = "";
            txtTo.Text = "";
            txtBCC.Text = "";
            txtSubject.Text = "";
            txtCC.Text = "";
            txtFileDownloadLink.Text = "";
        }
        //ProgressBar
        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Thread.Sleep(e.ProgressPercentage);

        }
        //FileCompleted
        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Download Completed";
        }
    }
}