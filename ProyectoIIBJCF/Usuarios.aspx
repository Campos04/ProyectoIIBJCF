<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="ProyectoIIBJCF.Usuarios" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Usuarios</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="top-nav">
            <div class="nav-content">
                <a class="brand" href="Menu.aspx"><span class="brand-mark">🛠</span><span class="brand-copy"><span class="brand-title">Soporte Técnico</span><span class="brand-subtitle">Atención y seguimiento</span></span></a>
                <div class="nav-links">
                    <a href="Menu.aspx">Inicio</a>
                    <a class="active" href="Usuarios.aspx">Usuarios</a>
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
            <section class="page-header page-banner">
                <div class="page-banner-grid">
                    <div>
                        <span class="eyebrow">Mantenimiento de usuarios</span>
                        <h1>Usuarios</h1>
                        <p>Administra a las personas relacionadas con el servicio para mantener su información localizada y disponible cuando se necesite.</p>
                        <div class="page-tags"><span>Información clara</span><span>Datos de contacto</span><span>Atención ordenada</span></div>
                        <div class="page-tip"><strong>Información principal</strong><span>Nombre, correo y teléfono para identificar mejor a cada persona dentro del sistema.</span></div>
                    </div>
                    <div class="page-visual"><img src="Content/Images/repair-scene.svg" alt="Ilustración relacionada con el servicio técnico" /></div>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Formulario de usuarios</h2>
                    <p class="card-subtitle">Usa el ID cuando necesites buscar, actualizar o eliminar un registro en particular.</p>
                    <div class="field-group"><label for="txtUserId">ID del usuario</label><asp:TextBox ID="txtUserId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Ejemplo: 1" /></div>
                    <div class="field-group"><label for="txtName">Nombre</label><asp:TextBox ID="txtName" runat="server" CssClass="input-control" MaxLength="100" placeholder="Nombre completo" /></div>
                    <div class="field-group"><label for="txtEmail">Correo electrónico</label><asp:TextBox ID="txtEmail" runat="server" CssClass="input-control" MaxLength="150" placeholder="correo@ejemplo.com" /></div>
                    <div class="field-group"><label for="txtPhone">Teléfono</label><asp:TextBox ID="txtPhone" runat="server" CssClass="input-control" MaxLength="25" placeholder="8888-8888" /></div>
                    <div class="button-group">
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" />
                    </div>
                </section>
                <section class="table-card">
                    <div class="table-heading"><h2>Listado general</h2><p>Visualiza todos los usuarios registrados actualmente.</p></div>
                    <div class="table-scroll">
                        <asp:GridView ID="gridData" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No hay registros.">
                            <EmptyDataRowStyle CssClass="empty-grid" />
                            <Columns><asp:BoundField DataField="UsuarioID" HeaderText="ID" /><asp:BoundField DataField="Nombre" HeaderText="Nombre" /><asp:BoundField DataField="CorreoElectronico" HeaderText="Correo" /><asp:BoundField DataField="Telefono" HeaderText="Teléfono" /></Columns>
                        </asp:GridView>
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
