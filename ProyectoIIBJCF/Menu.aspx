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
        <nav class="top-nav">
            <div class="nav-content">
                <a class="brand" href="Menu.aspx">
                    <span class="brand-mark">🛠</span>
                    <span class="brand-copy">
                        <span class="brand-title">Soporte Técnico</span>
                        <span class="brand-subtitle">Recepción · Diagnóstico · Entrega</span>
                    </span>
                </a>
                <div class="nav-links">
                    <a class="active" href="Menu.aspx">Inicio</a>
                    <a href="Usuarios.aspx">Usuarios</a>
                    <a href="Equipos.aspx">Equipos</a>
                    <a href="Tecnicos.aspx">Técnicos</a>
                    <a href="Reparaciones.aspx">Reparaciones</a>
                    <a href="Asignaciones.aspx">Asignaciones</a>
                    <a href="DetallesReparacion.aspx">Detalles</a>
                    <a class="logout-link" href="Menu.aspx?logout=1">Cerrar sesión</a>
                </div>
            </div>
        </nav>

        <main class="page-container">
            <section class="hero-card">
                <div class="hero-grid">
                    <div>
                        <span class="eyebrow">Taller técnico</span>
                        <h1>Bienvenido al panel del taller técnico.</h1>
                        <p>Consulta la información del taller, organiza cada equipo recibido y mantén el seguimiento de cada servicio de una manera más clara y profesional.</p>
                    </div>
                    <div class="hero-visual">
                        <img src="Content/Images/service-hero.svg" alt="Computadora siendo revisada y reparada" />
                    </div>
                </div>
            </section>

            <section class="dashboard-grid">
                <a class="dashboard-card" href="Usuarios.aspx"><span class="dashboard-icon">👤</span><span class="card-number">Área 01</span><h2>Usuarios</h2><p>Administra la información de los clientes y personas relacionadas con la atención del taller.</p><span class="card-action">Entrar →</span></a>
                <a class="dashboard-card" href="Equipos.aspx"><span class="dashboard-icon">💻</span><span class="card-number">Área 02</span><h2>Equipos</h2><p>Registra computadoras, laptops y otros dispositivos que ingresan al taller.</p><span class="card-action">Entrar →</span></a>
                <a class="dashboard-card" href="Tecnicos.aspx"><span class="dashboard-icon">🧑‍🔧</span><span class="card-number">Área 03</span><h2>Técnicos</h2><p>Organiza el personal técnico encargado del diagnóstico y la reparación.</p><span class="card-action">Entrar →</span></a>
                <a class="dashboard-card" href="Reparaciones.aspx"><span class="dashboard-icon">📋</span><span class="card-number">Área 04</span><h2>Reparaciones</h2><p>Da seguimiento al estado de cada equipo mientras pasa por revisión y reparación.</p><span class="card-action">Entrar →</span></a>
                <a class="dashboard-card" href="Asignaciones.aspx"><span class="dashboard-icon">🔄</span><span class="card-number">Área 05</span><h2>Asignaciones</h2><p>Define con claridad qué técnico atenderá cada caso dentro del taller.</p><span class="card-action">Entrar →</span></a>
                <a class="dashboard-card" href="DetallesReparacion.aspx"><span class="dashboard-icon">📝</span><span class="card-number">Área 06</span><h2>Detalles</h2><p>Guarda observaciones importantes sobre el trabajo realizado en cada equipo.</p><span class="card-action">Entrar →</span></a>
            </section>
        </main>
    </form>
</body>
</html>
