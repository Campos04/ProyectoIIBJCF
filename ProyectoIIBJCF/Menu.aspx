<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="ProyectoIIBJCF.Menu" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Menú principal</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <header class="top-nav">
            <div class="nav-content">
                <a class="brand" href="Menu.aspx">ProyectoIIBJCF</a>
                <nav class="nav-links">
                    <a href="Usuarios.aspx">Usuarios</a>
                    <a href="Equipos.aspx">Equipos</a>
                    <a href="Tecnicos.aspx">Técnicos</a>
                    <a href="Reparaciones.aspx">Reparaciones</a>
                    <a href="Asignaciones.aspx">Asignaciones</a>
                    <a href="DetallesReparacion.aspx">Detalles</a>
                </nav>
                <asp:Button ID="btnLogout" runat="server" Text="Cerrar sesión" CssClass="nav-logout" OnClick="btnLogout_Click" />
            </div>
        </header>

        <main class="page-container">
            <section class="hero-card">
                <span class="eyebrow">Panel principal</span>
                <h1>Gestión de reparaciones</h1>
                <p>Administra usuarios, equipos, técnicos, reparaciones, asignaciones y detalles desde un solo menú.</p>
                <p class="welcome-text">Sesión: <asp:Label ID="lblUser" runat="server" /></p>
            </section>

            <section class="dashboard-grid">
                <a class="dashboard-card" href="Usuarios.aspx"><span class="card-number">01</span><h2>Usuarios</h2><p>Clientes y datos de contacto.</p><span class="card-action">Abrir mantenimiento →</span></a>
                <a class="dashboard-card" href="Equipos.aspx"><span class="card-number">02</span><h2>Equipos</h2><p>Dispositivos asociados a usuarios.</p><span class="card-action">Abrir mantenimiento →</span></a>
                <a class="dashboard-card" href="Tecnicos.aspx"><span class="card-number">03</span><h2>Técnicos</h2><p>Personal técnico y especialidades.</p><span class="card-action">Abrir mantenimiento →</span></a>
                <a class="dashboard-card" href="Reparaciones.aspx"><span class="card-number">04</span><h2>Reparaciones</h2><p>Solicitudes y estado de reparación.</p><span class="card-action">Abrir mantenimiento →</span></a>
                <a class="dashboard-card" href="Asignaciones.aspx"><span class="card-number">05</span><h2>Asignaciones</h2><p>Relación entre reparaciones y técnicos.</p><span class="card-action">Abrir mantenimiento →</span></a>
                <a class="dashboard-card" href="DetallesReparacion.aspx"><span class="card-number">06</span><h2>Detalles</h2><p>Descripción y fechas del trabajo realizado.</p><span class="card-action">Abrir mantenimiento →</span></a>
            </section>
        </main>
    </form>
</body>
</html>
