<%@ Page Title="" Language="C#" MasterPageFile="~/Interview.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="JobAssignment.Accounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="padding: 5px; width: 100%; margin-bottom: 20px; background-color: blue; color: white">
        <h4>BankX</h4>
    </div>
     <a href="Login.aspx"><i class="icon_key_alt"></i>Log Out</a>
    <div class="container">
        <asp:Label ID="lblmsg" runat="server" Font-Names="Cambria" Font-Size="12pt"
            ForeColor="Red" Text="." CssClass="text-center"></asp:Label>

            <div class="row">

                <div class="col-lg-3" style="height:100vh; background-color: #f7f7f7; border-radius: 10px; padding-top: 10px;">
                    <h5>Transact</h5>
                    <div class="form-group">
                        <asp:Label Text="Recipient Name" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtName" />
                    </div>
                    <div class="form-group">
                        <asp:Label Text="Recipient Phone Number" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtPhone" />
                    </div>
                    <div class="form-group">
                        <asp:Label Text="Amount" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtAmount" />
                    </div>
                    <div class="form-group">
                         <asp:Label Text="Description" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNarration" />
                   
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Transfer" OnClick="TransferSearchClicked" />

                    </div>

                </div>
                <div class="col-lg-6 mx-3">
                    <h5>Statement</h5>
                    <div class="row">
                        <asp:TextBox runat="server" placeholder="Start Date" CssClass="form-control col-lg-4" TextMode="Date" ID="txtStartDate" />
                        <asp:TextBox runat="server" placeholder="End Date" CssClass="form-control col-lg-4 mx-2" TextMode="Date" ID="txtEndDate" />
                        <asp:Button runat="server" CssClass="btn btn-success" Text="Search" OnClick="SearchClicked" />
                    </div>
                    <div class="row" style="margin-top:30px">

                        <div class="col-lg-12">
                            <div class="table-responsive">
                                <asp:GridView ID="GridData" runat="server" Width="100%" CssClass="table table-bordered table-hover">
                                </asp:GridView>
                            </div>
                            
                        </div>
                    </div>
                    <div class="row">
                        <asp:Button runat="server" ID="btnDownload" CssClass="btn btn-primary" OnClick="DownloadReportClicked" Text="Download" />

                    </div>
                </div>
            </div>
    </div>
</asp:Content>
