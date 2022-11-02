<%@ Page Title="" Language="C#" MasterPageFile="~/Interview.Master" AutoEventWireup="true" CodeBehind="Homes.aspx.cs" Inherits="JobAssignment.Homes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div style="padding: 5px; width: 100%; margin-bottom: 20px; background-color: blue; color: white">
        <h4>BankX</h4>

    </div>
     <a href="Login.aspx"><i class="icon_key_alt"></i>Log Out</a>
    <div class="container">
        <div class="row">
            <%foreach (JobAssignment.Account acc in (List<JobAssignment.Account>)Session["AccList"])
                { %>
            <div class="card col-3" style="margin: 5px;">
                <div class="card-header"><a href="Accounts.aspx?acc=<%=acc.accountNumber%>"><%=acc.accountNumber%></a></div>
                    
                <div class="card-body">
                   <div class="card-text">
                        <h4><%=acc.balance%></h4>
                    </div>
                </div>
            </div>
            <%} %>
        </div>
    </div>
</asp:Content>
