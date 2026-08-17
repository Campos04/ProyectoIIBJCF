<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Equipos.aspx.cs" Inherits="ProyectoIIBJCF.Equipos" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Equipos</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="top-nav">
            <div class="nav-content">
                <a class="brand" href="Menu.aspx"><span class="brand-mark">🛠</span><span class="brand-copy"><span class="brand-title">Soporte Técnico</span><span class="brand-subtitle">Atención y seguimiento</span></span></a>
                <div class="nav-links">
                    <a href="Menu.aspx">Inicio</a>
                    <a href="Usuarios.aspx">Usuarios</a>
                    <a class="active" href="Equipos.aspx">Equipos</a>
                    <a href="Tecnicos.aspx">Técnicos</a>
                    <a href="Reparaciones.aspx">Reparaciones</a>
                    <a href="Asignaciones.aspx">Asignaciones</a>
                    <a href="DetallesReparacion.aspx">Detalles</a>
                    <a class="logout-link" href="Menu.aspx?logout=1">Cerrar sesión</a>
                </div>
            </div>
        </nav>

        <main class="page-container">
            <section class="page-header page-banner">
                <div class="page-banner-grid">
                    <div>
                        <span class="eyebrow">Mantenimiento de equipos</span>
                        <h1>Equipos</h1>
                        <p>Registra los dispositivos para darles un seguimiento más claro durante su ingreso, revisión y atención.</p>
                        <div class="page-tags"><span>Control de equipos</span><span>Seguimiento visual</span><span>Orden en cada caso</span></div>
                        <div class="page-tip"><strong>Información principal</strong><span>Tipo de equipo, modelo y la persona a la que pertenece cada dispositivo.</span></div>
                    </div>
                    <div class="page-visual"><img src="Content/Images/repair-scene.svg" alt="Ilustración relacionada con el servicio técnico" /></div>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Formulario de equipos</h2>
                    <p class="card-subtitle">Selecciona el usuario correspondiente para que cada equipo quede bien asociado.</p>
                    <div class="field-group"><label for="txtEquipmentId">ID del equipo</label><asp:TextBox ID="txtEquipmentId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Ejemplo: 1" /></div>
                    <div class="field-group"><label for="txtEquipmentType">Tipo de equipo</label><asp:TextBox ID="txtEquipmentType" runat="server" CssClass="input-control" MaxLength="80" placeholder="Laptop, impresora, celular..." /></div>
                    <div class="field-group"><label for="txtModel">Modelo</label><asp:TextBox ID="txtModel" runat="server" CssClass="input-control" MaxLength="100" placeholder="Modelo del equipo" /></div>
                    <div class="field-group"><label for="ddlUsers">Usuario propietario</label><asp:DropDownList ID="ddlUsers" runat="server" CssClass="input-control" /></div>
                    <div class="button-group">
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" />
                    </div>
                </section>
                <section class="table-card">
                    <div class="table-heading"><h2>Listado general</h2><p>Consulta todos los equipos registrados con su información principal.</p></div>
                    <div class="table-scroll">
                        <asp:GridView ID="gridData" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No hay registros.">
                            <EmptyDataRowStyle CssClass="empty-grid" />
                            <Columns><asp:BoundField DataField="EquipoID" HeaderText="ID" /><asp:BoundField DataField="TipoEquipo" HeaderText="Tipo" /><asp:BoundField DataField="Modelo" HeaderText="Modelo" /><asp:BoundField DataField="Usuario" HeaderText="Usuario" /></Columns>
                        </asp:GridView>
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
