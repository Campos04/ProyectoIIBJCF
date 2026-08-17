<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProyectoIIBJCF.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body class="login-body">
    <form id="form1" runat="server">
        <main class="login-shell">
            <section class="login-card">
                <span class="eyebrow">Proyecto II</span>
                <h1>Sistema de reparaciones</h1>
                <p class="login-subtitle">Ingresa tus credenciales para acceder al mantenimiento.</p>

                <asp:Label ID="lblMessage" runat="server" Visible="false" />

                <div class="field-group">
                    <label for="txtUser">Usuario</label>
                    <asp:TextBox ID="txtUser" runat="server" CssClass="input-control" MaxLength="50" autocomplete="username" />
                </div>

                <div class="field-group">
                    <label for="txtPassword">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="input-control" TextMode="Password" MaxLength="100" autocomplete="current-password" />
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Iniciar sesión" CssClass="button primary login-button" OnClick="btnLogin_Click" />
            </section>
        </main>
    </form>
</body>
</html>
