USE [master];
GO

IF DB_ID(N'ProyectoIIBJCF') IS NULL
BEGIN
    CREATE DATABASE [ProyectoIIBJCF];
END;
GO

USE [ProyectoIIBJCF];
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        UsuarioID INT IDENTITY(1,1) NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        CorreoElectronico NVARCHAR(150) NOT NULL,
        Telefono NVARCHAR(25) NULL,
        UsuarioLogin NVARCHAR(50) NULL,
        ClaveHash VARBINARY(32) NULL,
        Rol NVARCHAR(20) NOT NULL CONSTRAINT DF_Usuarios_Rol DEFAULT(N'Usuario'),
        CONSTRAINT PK_Usuarios PRIMARY KEY (UsuarioID)
    );
END;
GO

IF COL_LENGTH('dbo.Usuarios', 'UsuarioLogin') IS NULL
    ALTER TABLE dbo.Usuarios ADD UsuarioLogin NVARCHAR(50) NULL;
GO
IF COL_LENGTH('dbo.Usuarios', 'ClaveHash') IS NULL
    ALTER TABLE dbo.Usuarios ADD ClaveHash VARBINARY(32) NULL;
GO
IF COL_LENGTH('dbo.Usuarios', 'Rol') IS NULL
    ALTER TABLE dbo.Usuarios ADD Rol NVARCHAR(20) NOT NULL CONSTRAINT DF_Usuarios_Rol DEFAULT(N'Usuario');
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_Usuarios_UsuarioLogin' AND object_id = OBJECT_ID('dbo.Usuarios'))
    CREATE UNIQUE INDEX UX_Usuarios_UsuarioLogin ON dbo.Usuarios(UsuarioLogin) WHERE UsuarioLogin IS NOT NULL;
GO

IF OBJECT_ID(N'dbo.Equipos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Equipos
    (
        EquipoID INT IDENTITY(1,1) NOT NULL,
        TipoEquipo NVARCHAR(80) NOT NULL,
        Modelo NVARCHAR(100) NOT NULL,
        UsuarioID INT NOT NULL,
        CONSTRAINT PK_Equipos PRIMARY KEY (EquipoID),
        CONSTRAINT FK_Equipos_Usuarios FOREIGN KEY (UsuarioID) REFERENCES dbo.Usuarios (UsuarioID)
    );
END;
GO

IF OBJECT_ID(N'dbo.Tecnicos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tecnicos
    (
        TecnicoID INT IDENTITY(1,1) NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Especialidad NVARCHAR(100) NOT NULL,
        CONSTRAINT PK_Tecnicos PRIMARY KEY (TecnicoID)
    );
END;
GO

IF OBJECT_ID(N'dbo.Reparaciones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Reparaciones
    (
        ReparacionID INT IDENTITY(1,1) NOT NULL,
        EquipoID INT NOT NULL,
        FechaSolicitud DATE NOT NULL,
        Estado NVARCHAR(30) NOT NULL,
        CONSTRAINT PK_Reparaciones PRIMARY KEY (ReparacionID),
        CONSTRAINT FK_Reparaciones_Equipos FOREIGN KEY (EquipoID) REFERENCES dbo.Equipos (EquipoID)
    );
END;
GO

IF OBJECT_ID(N'dbo.Asignaciones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Asignaciones
    (
        AsignacionID INT IDENTITY(1,1) NOT NULL,
        ReparacionID INT NOT NULL,
        TecnicoID INT NOT NULL,
        FechaAsignacion DATE NOT NULL,
        CONSTRAINT PK_Asignaciones PRIMARY KEY (AsignacionID),
        CONSTRAINT FK_Asignaciones_Reparaciones FOREIGN KEY (ReparacionID) REFERENCES dbo.Reparaciones (ReparacionID),
        CONSTRAINT FK_Asignaciones_Tecnicos FOREIGN KEY (TecnicoID) REFERENCES dbo.Tecnicos (TecnicoID)
    );
END;
GO

IF OBJECT_ID(N'dbo.DetallesReparacion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DetallesReparacion
    (
        DetalleID INT IDENTITY(1,1) NOT NULL,
        ReparacionID INT NOT NULL,
        Descripcion NVARCHAR(500) NOT NULL,
        FechaInicio DATE NULL,
        FechaFin DATE NULL,
        CONSTRAINT PK_DetallesReparacion PRIMARY KEY (DetalleID),
        CONSTRAINT FK_DetallesReparacion_Reparaciones FOREIGN KEY (ReparacionID) REFERENCES dbo.Reparaciones (ReparacionID)
    );
END;
GO

-- Usuario inicial para probar el login: admin / Admin123
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE UsuarioLogin = N'admin')
BEGIN
    INSERT INTO dbo.Usuarios (Nombre, CorreoElectronico, Telefono, UsuarioLogin, ClaveHash, Rol)
    VALUES (N'Administrador', N'admin@proyecto.local', NULL, N'admin', HASHBYTES('SHA2_256', N'Admin123'), N'Administrador');
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Login
    @UsuarioLogin NVARCHAR(50),
    @Clave NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UsuarioID, Nombre, Rol
    FROM dbo.Usuarios
    WHERE UsuarioLogin = @UsuarioLogin
      AND ClaveHash = HASHBYTES('SHA2_256', @Clave);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Usuarios_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono, UsuarioLogin, Rol
    FROM dbo.Usuarios
    ORDER BY UsuarioID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Usuarios_Consultar @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
    FROM dbo.Usuarios
    WHERE UsuarioID = @UsuarioID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Usuarios_Agregar
    @Nombre NVARCHAR(100),
    @CorreoElectronico NVARCHAR(150),
    @Telefono NVARCHAR(25) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Usuarios (Nombre, CorreoElectronico, Telefono)
    VALUES (@Nombre, @CorreoElectronico, @Telefono);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Usuarios_Modificar
    @UsuarioID INT,
    @Nombre NVARCHAR(100),
    @CorreoElectronico NVARCHAR(150),
    @Telefono NVARCHAR(25) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Usuarios
    SET Nombre = @Nombre, CorreoElectronico = @CorreoElectronico, Telefono = @Telefono
    WHERE UsuarioID = @UsuarioID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Usuarios_Borrar @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Usuarios WHERE UsuarioID = @UsuarioID AND UsuarioLogin IS NULL;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Equipos_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT E.EquipoID, E.TipoEquipo, E.Modelo, E.UsuarioID, U.Nombre AS Usuario
    FROM dbo.Equipos E
    INNER JOIN dbo.Usuarios U ON U.UsuarioID = E.UsuarioID
    ORDER BY E.EquipoID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Equipos_Consultar @EquipoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EquipoID, TipoEquipo, Modelo, UsuarioID
    FROM dbo.Equipos
    WHERE EquipoID = @EquipoID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Equipos_Agregar
    @TipoEquipo NVARCHAR(80),
    @Modelo NVARCHAR(100),
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Equipos (TipoEquipo, Modelo, UsuarioID)
    VALUES (@TipoEquipo, @Modelo, @UsuarioID);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Equipos_Modificar
    @EquipoID INT,
    @TipoEquipo NVARCHAR(80),
    @Modelo NVARCHAR(100),
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Equipos
    SET TipoEquipo = @TipoEquipo, Modelo = @Modelo, UsuarioID = @UsuarioID
    WHERE EquipoID = @EquipoID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Equipos_Borrar @EquipoID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Equipos WHERE EquipoID = @EquipoID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tecnicos_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TecnicoID, Nombre, Especialidad
    FROM dbo.Tecnicos
    ORDER BY TecnicoID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tecnicos_Consultar @TecnicoID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TecnicoID, Nombre, Especialidad
    FROM dbo.Tecnicos
    WHERE TecnicoID = @TecnicoID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tecnicos_Agregar
    @Nombre NVARCHAR(100),
    @Especialidad NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Tecnicos (Nombre, Especialidad)
    VALUES (@Nombre, @Especialidad);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tecnicos_Modificar
    @TecnicoID INT,
    @Nombre NVARCHAR(100),
    @Especialidad NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Tecnicos SET Nombre = @Nombre, Especialidad = @Especialidad
    WHERE TecnicoID = @TecnicoID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Tecnicos_Borrar @TecnicoID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Tecnicos WHERE TecnicoID = @TecnicoID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reparaciones_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT R.ReparacionID, R.EquipoID, E.TipoEquipo + N' - ' + E.Modelo AS Equipo,
           R.FechaSolicitud, R.Estado
    FROM dbo.Reparaciones R
    INNER JOIN dbo.Equipos E ON E.EquipoID = R.EquipoID
    ORDER BY R.ReparacionID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reparaciones_Consultar @ReparacionID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ReparacionID, EquipoID, FechaSolicitud, Estado
    FROM dbo.Reparaciones WHERE ReparacionID = @ReparacionID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reparaciones_Agregar
    @EquipoID INT, @FechaSolicitud DATE, @Estado NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Reparaciones (EquipoID, FechaSolicitud, Estado)
    VALUES (@EquipoID, @FechaSolicitud, @Estado);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reparaciones_Modificar
    @ReparacionID INT, @EquipoID INT, @FechaSolicitud DATE, @Estado NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Reparaciones
    SET EquipoID = @EquipoID, FechaSolicitud = @FechaSolicitud, Estado = @Estado
    WHERE ReparacionID = @ReparacionID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Reparaciones_Borrar @ReparacionID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Reparaciones WHERE ReparacionID = @ReparacionID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Asignaciones_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT A.AsignacionID, A.ReparacionID, A.TecnicoID, T.Nombre AS Tecnico, A.FechaAsignacion
    FROM dbo.Asignaciones A
    INNER JOIN dbo.Tecnicos T ON T.TecnicoID = A.TecnicoID
    ORDER BY A.AsignacionID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Asignaciones_Consultar @AsignacionID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AsignacionID, ReparacionID, TecnicoID, FechaAsignacion
    FROM dbo.Asignaciones WHERE AsignacionID = @AsignacionID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Asignaciones_Agregar
    @ReparacionID INT, @TecnicoID INT, @FechaAsignacion DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Asignaciones (ReparacionID, TecnicoID, FechaAsignacion)
    VALUES (@ReparacionID, @TecnicoID, @FechaAsignacion);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Asignaciones_Modificar
    @AsignacionID INT, @ReparacionID INT, @TecnicoID INT, @FechaAsignacion DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Asignaciones
    SET ReparacionID = @ReparacionID, TecnicoID = @TecnicoID, FechaAsignacion = @FechaAsignacion
    WHERE AsignacionID = @AsignacionID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Asignaciones_Borrar @AsignacionID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Asignaciones WHERE AsignacionID = @AsignacionID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Detalles_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DetalleID, ReparacionID, Descripcion, FechaInicio, FechaFin
    FROM dbo.DetallesReparacion
    ORDER BY DetalleID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Detalles_Consultar @DetalleID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DetalleID, ReparacionID, Descripcion, FechaInicio, FechaFin
    FROM dbo.DetallesReparacion WHERE DetalleID = @DetalleID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Detalles_Agregar
    @ReparacionID INT, @Descripcion NVARCHAR(500), @FechaInicio DATE = NULL, @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.DetallesReparacion (ReparacionID, Descripcion, FechaInicio, FechaFin)
    VALUES (@ReparacionID, @Descripcion, @FechaInicio, @FechaFin);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Detalles_Modificar
    @DetalleID INT, @ReparacionID INT, @Descripcion NVARCHAR(500), @FechaInicio DATE = NULL, @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.DetallesReparacion
    SET ReparacionID = @ReparacionID, Descripcion = @Descripcion, FechaInicio = @FechaInicio, FechaFin = @FechaFin
    WHERE DetalleID = @DetalleID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Detalles_Borrar @DetalleID INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.DetallesReparacion WHERE DetalleID = @DetalleID;
    SELECT @@ROWCOUNT AS FilasAfectadas;
END;
GO
