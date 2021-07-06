<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" Async="true" CodeBehind="Vacciation.aspx.cs" Inherits="Vaccination.Vacciation" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vaccination</title>
    <style type="text/css">
        #PVaccination legend {
            color: Red;
            font-size: 25px
        }

        #PVactionDetail legend {
            color: Green;
            font-size: 25px
        }

        #PEmailDetail legend {
            color: blue;
            font-size: 25px
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>



        <asp:Panel ID="PVaccination" runat="server" GroupingText="Covid19 Vaccination">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table>
                        <tr>
                            <td style="width: auto"><b>Sample Link:</b></td>
                            <td>
                                <asp:Label ID="lblLink" runat="server">https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/vaccinations/country_data/</asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>File Download Link:</b></td>
                            <td>
                                <asp:TextBox ID="txtFileDownloadLink" Style="width: 700px" runat="server"></asp:TextBox></td>
                            <td><b>Countries:</b></td>
                            <td>
                                <asp:DropDownList ID="ddlLocation" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="-1" Text="Select Country" />
                                </asp:DropDownList></td>
                            <td>
                                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                                    <ProgressTemplate>
                                        <div class="div1">
                                            <asp:Image ID="Image1" Width="30px" ImageUrl="loader.gif" AlternateText="Processing" runat="server" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>

                        </tr>
                        <tr>
                            <td><b>Download Data File:</b></td>
                            <td>

                                <asp:Button ID="btnDownloadDataFile" runat="server" Text="Download Data File" OnClick="btnDownloadDataFile_Click" />

                            </td>

                        </tr>
                        <tr>
                            <td><b>Process Data File:</b></td>
                            <td>
                                <asp:Button ID="btnProcessDataFile" runat="server" Text="Process Data File " OnClick="btnProcessDataFile_Click" /></td>
                        </tr>
                        <tr>
                            <td><b>Delete Location File:</b></td>

                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete File" OnClick="btnDeleteFile_Click" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label></td>

                        </tr>
                    </table>
                    <asp:Panel ID="PVactionDetail" Visible="false" runat="server" GroupingText="Vaccination Detail">

                        <table id="tblProcessInformation">
                            <tr>
                                <td><b>Lastest Date:</b></td>
                                <td>
                                    <asp:Label ID="lblLastestDate" runat="server" /></td>
                            </tr>
                            <tr>
                                <td><b>Total Vaccination:</b></td>
                                <td>
                                    <asp:Label ID="lblTotalVaccination" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td><b>Last 7 Days:</b></td>
                                <td>
                                    <asp:Label ID="lblAverageNumberVaccination" runat="server" /></td>
                                <td><asp:HiddenField ID="hdnEmail" runat="server" /> </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <asp:Panel ID="PEmailDetail" Visible="false" runat="server" GroupingText="Email Detail">

                        <table class="auto-style2">
                            <tr>
                                <td class="auto-style6"><b>From:</b></td>
                                <td>
                                    <asp:TextBox ID="txtFrom" runat="server" Enabled="false" Width="593px">mailer@sitealive.com</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style6"><b>To:</b></td>
                                <td>
                                    <asp:TextBox ID="txtTo" runat="server" placeholder="faraz@sitealive.com" Width="593px"></asp:TextBox></td>
                                <td>
                                    <asp:LinkButton ID="lbtntoEmail" OnClick="lbtntoEmail_Click" Text="faraz@sitealive.com" runat="server">faraz@sitealive.com</asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td class="auto-style6"><b>CC:</b></td>
                                <td>
                                    <asp:TextBox ID="txtCC" runat="server" placeholder="sikander@sitealive.com" Width="593px"></asp:TextBox></td>
                                <td>

                                    <asp:LinkButton ID="lbtnCC" OnClick="lbtnCC_Click" Text="sikander@sitealive.com" runat="server">sikander@sitealive.com</asp:LinkButton>
                            </tr>
                            <tr>
                                <td class="auto-style6"><b>BCC:</b></td>
                                <td>
                                    <asp:TextBox ID="txtBCC" runat="server" placeholder="usman@sitealive.com" Width="593px"></asp:TextBox></td>
                                <td>
                                    <asp:LinkButton ID="lbtnBCC" OnClick="lbtnBCC_Click" Text="usman@sitealive.com" runat="server">usman@sitealive.com</asp:LinkButton>
                            </tr>
                            <tr>
                                <td class="auto-style6"><b>Subject:</b></td>
                                <td>
                                    <asp:TextBox ID="txtSubject" runat="server" placeholder="subject" Width="593px" Value="Vax stats"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="auto-style6"><b>Email Body</b></td>
                                <td>
                                    <textarea id="txtEmailBody" style="resize: none" rows="10"  readonly="readonly" cols="83" runat="server" class="auto-style5"></textarea>
                                </td>
                                <td>
                                    <asp:Label ID="lblEmailInfo" runat="server" ForeColor="Orange" Text="Kindly Copy Paste the Email In Requiesd Field or Put Your Desired Valid Email Address" /></td>
                            </tr>
                            <tr>
                                <td class="auto-style6"><b>Email Report</b></td>
                                <td>
                                    <asp:Button ID="btnEmailReport" runat="server" Text="Email Report" OnClick="btnEmailReport_Click" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </form>
</body>
</html>
