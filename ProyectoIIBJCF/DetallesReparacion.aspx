<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetallesReparacion.aspx.cs" Inherits="ProyectoIIBJCF.DetallesReparacion" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Detalles de reparación</title>
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
                    <a href="Reparaciones.aspx">Reparaciones</a>
                    <a href="Asignaciones.aspx">Asignaciones</a>
                    <a class="active" href="DetallesReparacion.aspx">Detalles</a>
                    <a class="logout-link" href="Menu.aspx?logout=1">Cerrar sesión</a>
                </div>
            </div>
        </nav>

        <main class="page-container">
            <section class="page-header page-banner">
                <div class="page-banner-grid">
                    <div>
                        <span class="eyebrow">Mantenimiento de detalles</span>
                        <h1>Detalles de reparación</h1>
                        <p>Conserva notas y observaciones importantes sobre el trabajo realizado para que cada caso tenga mejor seguimiento.</p>
                        <div class="page-tags"><span>Notas del servicio</span><span>Fechas clave</span><span>Seguimiento final</span></div>
                        <div class="page-tip"><strong>Información principal</strong><span>La descripción del trabajo y las fechas importantes relacionadas con la atención.</span></div>
                    </div>
                    <div class="page-visual"><img src="Content/Images/repair-scene.svg" alt="Ilustración relacionada con el servicio técnico" /></div>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Formulario de detalles</h2>
                    <p class="card-subtitle">Aquí puedes guardar observaciones útiles para entender mejor el trabajo realizado.</p>
                    <div class="field-group"><label for="txtDetailId">ID del detalle</label><asp:TextBox ID="txtDetailId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Ejemplo: 1" /></div>
                    <div class="field-group"><label for="ddlRepair">Reparación</label><asp:DropDownList ID="ddlRepair" runat="server" CssClass="input-control" /></div>
                    <div class="field-group"><label for="txtDescription">Descripción</label><asp:TextBox ID="txtDescription" runat="server" CssClass="input-control" TextMode="MultiLine" MaxLength="500" placeholder="Describe el trabajo realizado" /></div>
                    <div class="field-group"><label for="txtStartDate">Fecha de inicio</label><asp:TextBox ID="txtStartDate" runat="server" CssClass="input-control" TextMode="Date" /></div>
                    <div class="field-group"><label for="txtEndDate">Fecha de fin</label><asp:TextBox ID="txtEndDate" runat="server" CssClass="input-control" TextMode="Date" /></div>
                    <div class="button-group">
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" />
                    </div>
                </section>
                <section class="table-card">
                    <div class="table-heading"><h2>Listado general</h2><p>Visualiza los detalles guardados con sus fechas de inicio y finalización.</p></div>
                    <div class="table-scroll">
                        <asp:GridView ID="gridData" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No hay registros.">
                            <EmptyDataRowStyle CssClass="empty-grid" />
                            <Columns><asp:BoundField DataField="DetalleID" HeaderText="ID" /><asp:BoundField DataField="ReparacionID" HeaderText="Reparación" /><asp:BoundField DataField="Descripcion" HeaderText="Descripción" /><asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}" /><asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy}" /></Columns>
                        </asp:GridView>
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
