<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:absolute;top:0;left:0;right:0;bottom:0;width:400px">
        <input type="hidden" name="submit" value="submit" runat="server" id="submit" />
        <input type="hidden" name="site" value="" runat="server" id="site" />
        <input type="hidden" name="return_page" value="" runat="server" id="return_page" />
        <table width="100%">
            <tr>
                <td width="21%">
                    username:
                </td>
                <td width="79%">
                    <input type="text" name="username" runat="server" id="username" />
                </td>
            </tr>
            <tr>
                <td>
                    password:
                </td>
                <td>
                    <input type="password" name="password" runat="server" id="password" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button Text="Log In" runat="server" ID="btnLogin" OnClick="getLogin" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
