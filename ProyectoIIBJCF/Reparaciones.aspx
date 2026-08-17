<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reparaciones.aspx.cs" Inherits="ProyectoIIBJCF.Reparaciones" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Reparaciones</title>
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
                    <a href="Equipos.aspx">Equipos</a>
                    <a href="Tecnicos.aspx">Técnicos</a>
                    <a class="active" href="Reparaciones.aspx">Reparaciones</a>
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
                        <span class="eyebrow">Mantenimiento de reparaciones</span>
                        <h1>Reparaciones</h1>
                        <p>Lleva el control del proceso de atención de cada equipo para que el estado del caso sea fácil de consultar.</p>
                        <div class="page-tags"><span>Seguimiento del caso</span><span>Estado del proceso</span><span>Atención continua</span></div>
                        <div class="page-tip"><strong>Información principal</strong><span>Equipo relacionado, fecha de ingreso y estado actual del proceso de reparación.</span></div>
                    </div>
                    <div class="page-visual"><img src="Content/Images/repair-scene.svg" alt="Ilustración relacionada con el servicio técnico" /></div>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Formulario de reparaciones</h2>
                    <p class="card-subtitle">Selecciona el equipo y actualiza el estado del caso según el avance de la atención.</p>
                    <div class="field-group"><label for="txtRepairId">ID de la reparación</label><asp:TextBox ID="txtRepairId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Ejemplo: 1" /></div>
                    <div class="field-group"><label for="ddlEquipment">Equipo</label><asp:DropDownList ID="ddlEquipment" runat="server" CssClass="input-control" /></div>
                    <div class="field-group"><label for="txtRequestDate">Fecha de solicitud</label><asp:TextBox ID="txtRequestDate" runat="server" CssClass="input-control" TextMode="Date" /></div>
                    <div class="field-group"><label for="ddlStatus">Estado</label><asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-control" /></div>
                    <div class="button-group">
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" />
                    </div>
                </section>
                <section class="table-card">
                    <div class="table-heading"><h2>Listado general</h2><p>Revisa las reparaciones registradas con su fecha y estado actual.</p></div>
                    <div class="table-scroll">
                        <asp:GridView ID="gridData" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No hay registros.">
                            <EmptyDataRowStyle CssClass="empty-grid" />
                            <Columns><asp:BoundField DataField="ReparacionID" HeaderText="ID" /><asp:BoundField DataField="Equipo" HeaderText="Equipo" /><asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" /><asp:BoundField DataField="Estado" HeaderText="Estado" /></Columns>
                        </asp:GridView>
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
