<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="JobAssignment.Login" %>

<!DOCTYPE html>
<html lang="en">

<head>
    
    <link rel="shortcut icon" href="img/favicon.png">

    <title>WALLET APP</title> 
    
    <link href="bootstrap-4.0.0-dist/css/bootstrap.min.css" rel="stylesheet" />


</head>

<body>

    <div class="container">

        <div class="login-form row">
            <form method="post" runat="server" class="col-lg-4">
                  <input name="__RequestVerificationToken" type="hidden"
                    value="6fGBtLZmVBZ59oUad1Fr33BuPxANKY9q3Srr5y[...]" />
                <h2 class="text-center" style="font-family: 'Century Gothic'; color: darkblue;">Bank X</h2>
                <asp:Label ID="lblmsg" runat="server" Font-Names="Cambria" Font-Size="12pt"
                    ForeColor="Red" Text="." CssClass="text-center"></asp:Label>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-user"></i></span>
                        <asp:TextBox ID="txtUserName" name="user" CssClass="form-control" placeholder="Username" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                        <asp:TextBox ID="txtPasswords" name="pass" CssClass="form-control" placeholder="Password" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Button ID="LoginBTN" CssClass="btn btn-primary" Text="Login" OnClick="onLogin" runat="server" Style="width: 100%;" />
                </div>

                
                <div class="clearfix">
                    <p style="font-family: 'Century Gothic'; color: darkblue; align-content: center;">Please use your username and password</p>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
