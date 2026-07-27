<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tecnicos.aspx.cs" Inherits="ProyectoIIBJCF.Tecnicos" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mantenimiento de técnicos</title>
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <main class="page-container">
            <section class="page-header">
                <div>
                    <span class="eyebrow">Mantenimiento</span>
                    <h1>Técnicos</h1>
                    <p>Agrega, consulta, modifica o elimina técnicos.</p>
                </div>
            </section>

            <asp:Label ID="lblMessage" runat="server" Visible="false" />

            <div class="crud-layout">
                <section class="form-card">
                    <h2>Datos del técnico</h2>

                    <div class="field-group">
                        <label for="txtTechnicianId">ID del técnico</label>
                        <asp:TextBox ID="txtTechnicianId" runat="server" CssClass="input-control" TextMode="Number" placeholder="Se usa para consultar, modificar o borrar" />
                    </div>

                    <div class="field-group">
                        <label for="txtName">Nombre</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="input-control" MaxLength="100" placeholder="Nombre completo" />
                    </div>

                    <div class="field-group">
                        <label for="txtSpecialty">Especialidad</label>
                        <asp:TextBox ID="txtSpecialty" runat="server" CssClass="input-control" MaxLength="100" placeholder="Ejemplo: Computadoras" />
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
