<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asignaciones.aspx.cs" Inherits="ProyectoIIBJCF.Asignaciones" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Asignaciones</title>
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
                    <a class="active" href="Asignaciones.aspx">Asignaciones</a>
                    <a href="DetallesReparacion.aspx">Detalles</a>
                    <a class="logout-link" href="Menu.aspx?logout=1">Cerrar sesión</a>
                </div>
            </div>
        </nav>

        <main class="page-container">
            <section class="page-header page-banner">
                <div class="page-banner-grid">
                    <div>
                        <span class="eyebrow">Mantenimiento de asignaciones</span>
                        <h1>Asignaciones</h1>
                        <p>Organiza la distribución del trabajo para saber con claridad qué persona está encargada de cada caso.</p>
                        <div class="page-tags"><span>Orden del trabajo</span><span>Responsables claros</span><span>Mejor control</span></div>
                        <div class="page-tip"><strong>Información principal</strong><span>La reparación, la persona encargada y la fecha en que se asignó la atención.</span></div>
                    </div>
                    <div class="page-visual"><img src="Content/Images/repair-scene.svg" alt="Ilustración relacionada con el servicio técnico" /></div>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Formulario de asignaciones</h2>
                    <p class="card-subtitle">Usa esta sección para distribuir mejor el trabajo y evitar confusiones.</p>
                    <div class="field-group"><label for="txtAssignmentId">ID de la asignación</label><asp:TextBox ID="txtAssignmentId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Ejemplo: 1" /></div>
                    <div class="field-group"><label for="ddlRepair">Reparación</label><asp:DropDownList ID="ddlRepair" runat="server" CssClass="input-control" /></div>
                    <div class="field-group"><label for="ddlTechnician">Técnico</label><asp:DropDownList ID="ddlTechnician" runat="server" CssClass="input-control" /></div>
                    <div class="field-group"><label for="txtAssignmentDate">Fecha de asignación</label><asp:TextBox ID="txtAssignmentDate" runat="server" CssClass="input-control" TextMode="Date" /></div>
                    <div class="button-group">
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" />
                    </div>
                </section>
                <section class="table-card">
                    <div class="table-heading"><h2>Listado general</h2><p>Consulta las asignaciones actuales y quién está a cargo de cada una.</p></div>
                    <div class="table-scroll">
                        <asp:GridView ID="gridData" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No hay registros.">
                            <EmptyDataRowStyle CssClass="empty-grid" />
                            <Columns><asp:BoundField DataField="AsignacionID" HeaderText="ID" /><asp:BoundField DataField="ReparacionID" HeaderText="Reparación" /><asp:BoundField DataField="Tecnico" HeaderText="Técnico" /><asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" /></Columns>
                        </asp:GridView>
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
