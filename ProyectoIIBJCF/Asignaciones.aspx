<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asignaciones.aspx.cs" Inherits="ProyectoIIBJCF.Asignaciones" %>
<!DOCTYPE html><html lang="es"><head runat="server"><meta charset="utf-8" /><meta name="viewport" content="width=device-width, initial-scale=1" />
<title>Mantenimiento de asignaciones</title><link href="Content/Site.css" rel="stylesheet" /></head>
<body><form id="form1" runat="server"><header class="top-nav">
    <div class="nav-content">
        <a class="brand" href="Menu.aspx">ProyectoIIBJCF</a>
        <nav class="nav-links">
            <a href="Usuarios.aspx">Usuarios</a><a href="Equipos.aspx">Equipos</a><a href="Tecnicos.aspx">Técnicos</a>
            <a href="Reparaciones.aspx">Reparaciones</a><a href="Asignaciones.aspx">Asignaciones</a><a href="DetallesReparacion.aspx">Detalles</a>
        </nav>
        <a class="nav-back" href="Menu.aspx">Menú</a>
    </div>
</header>
<main class="page-container"><section class="page-header"><div><span class="eyebrow">Mantenimiento</span><h1>Asignaciones</h1><p>Asigna técnicos a las reparaciones registradas.</p></div></section>
<asp:Label ID="lblMessage" runat="server" Visible="false" />
<div class="crud-layout"><section class="form-card"><h2>Datos de la asignación</h2>
<div class="field-group"><label for="txtAssignmentId">ID de asignación</label><asp:TextBox ID="txtAssignmentId" runat="server" CssClass="input-control" TextMode="Number" /></div>
<div class="field-group"><label for="ddlRepairs">Reparación</label><asp:DropDownList ID="ddlRepairs" runat="server" CssClass="input-control" /></div>
<div class="field-group"><label for="ddlTechnicians">Técnico</label><asp:DropDownList ID="ddlTechnicians" runat="server" CssClass="input-control" /></div>
<div class="field-group"><label for="txtAssignmentDate">Fecha de asignación</label><asp:TextBox ID="txtAssignmentDate" runat="server" CssClass="input-control" TextMode="Date" /></div>
<div class="button-group"><asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
<asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
<asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
<asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" /></div></section>
<section class="table-card"><div class="table-heading"><h2>Asignaciones registradas</h2><p>Listado general de registros.</p></div>
<div class="table-scroll"><asp:GridView ID="gvAssignments" runat="server" AutoGenerateColumns="true" CssClass="data-grid" GridLines="None" /></div></section>
</div></main></form></body></html>
