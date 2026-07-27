<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Equipos.aspx.cs" Inherits="ProyectoIIBJCF.Equipos" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mantenimiento de equipos</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="page-container">
            <section class="page-header">
                <div>
                    <span class="eyebrow">Mantenimiento</span>
                    <h1>Equipos</h1>
                    <p>Agrega, consulta, modifica o elimina equipos.</p>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Datos del equipo</h2>

                    <div class="field-group">
                        <label for="txtEquipmentId">ID del equipo</label>
                        <asp:TextBox ID="txtEquipmentId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Se usa para consultar, modificar o borrar" />
                    </div>

                    <div class="field-group">
                        <label for="txtEquipmentType">Tipo de equipo</label>
                        <asp:TextBox ID="txtEquipmentType" runat="server" CssClass="input-control" MaxLength="80" placeholder="Ejemplo: Laptop" />
                    </div>

                    <div class="field-group">
                        <label for="txtModel">Modelo</label>
                        <asp:TextBox ID="txtModel" runat="server" CssClass="input-control" MaxLength="100" placeholder="Ejemplo: Latitude 5420" />
                    </div>

                    <div class="field-group">
                        <label for="ddlUsers">Usuario propietario</label>
                        <asp:DropDownList ID="ddlUsers" runat="server" CssClass="input-control" />
                    </div>

                    <div class="button-group">
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" CssClass="button primary" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Consultar" CssClass="button secondary" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Modificar" CssClass="button warning" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Borrar" CssClass="button danger" OnClick="btnDelete_Click" />
                    </div>
                </section>
            </div>
        </main>
    </form>
</body>
</html>
