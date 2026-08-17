<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProyectoIIBJCF.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="login-page">
            <div class="login-shell">
                <section class="login-card">
                    <div class="login-grid">
                        <div class="login-visual">
                            <div class="login-visual-content">
                                <div class="login-brand">🔧 Taller Técnico</div>
                                <h1>Servicio técnico para computadoras y equipos con un entorno claro y profesional.</h1>
                                <p>Ingresa al sistema para administrar la recepción de equipos, el seguimiento del servicio y la atención general del taller.</p>
                                <div class="login-illustration">
                                    <img src="Content/Images/service-hero.svg" alt="Ilustración de una computadora en proceso de reparación" />
                                </div>
                            </div>
                        </div>
                        <div class="login-form-panel">
                            <span class="eyebrow">Acceso seguro</span>
                            <h1>Iniciar sesión</h1>
                            <p>Escribe tus credenciales para entrar al sistema y continuar con la gestión interna del taller.</p>
                            <asp:Label ID="lblMessage" runat="server" Visible="false" />
                            <div class="field-group">
                                <label for="txtUsername">Usuario</label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-control" MaxLength="50" placeholder="Escribe tu usuario" />
                            </div>
                            <div class="field-group">
                                <label for="txtPassword">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="input-control" TextMode="Password" MaxLength="100" placeholder="Escribe tu contraseña" />
                            </div>
                            <asp:Button ID="btnLogin" runat="server" Text="Entrar" CssClass="button primary login-button" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
