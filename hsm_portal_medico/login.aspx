<%@ Page Language="C#" Inherits="hsm_portal_medico.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>login</title>
</head>
<body>
    <form id="frmLogin" runat="server">
        <table width="300px" cellpadding="0" border="0" style="font-family: Tahoma; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">
            <tr height="30px" valign="middle">
                <th colspan="2" style="font-weight: bold; color: #ffffff; background-color: #ff0000; font-size: larger;">
                    Kleros Clinica Login
                </th>
            </tr>
            <tr height="5px" valign="middle">
                <th colspan="2">
                </th>
            </tr>
            <tr>
                <td width="40%" style="text-align: right">
                    Usuario:&nbsp;</td>
                <td width="60%" style="text-align: left">
                    <asp:TextBox ID="txtUsuario" runat="server" Columns="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="40%" style="text-align: right">
                    Senha:&nbsp;</td>
                <td width="60%" style="text-align: left">
                    <asp:TextBox ID="txtSenha" runat="server" Columns="15" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr height="40px" valign="middle">
                <th colspan="2">
                    <asp:Label ID="lblMensagem" runat="server" Font-Size="Small" 
                        ForeColor="#CC3300"></asp:Label>
                    <br />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" Width="100px" OnClick="btnOkClicked"/>
                </th>
            </tr>
        </table>
    </form>
</body>
</html>
