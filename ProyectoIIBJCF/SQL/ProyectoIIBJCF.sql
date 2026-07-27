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

        CONSTRAINT PK_Usuarios PRIMARY KEY (UsuarioID)
    );
END;
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
        CONSTRAINT FK_Equipos_Usuarios FOREIGN KEY (UsuarioID)
            REFERENCES dbo.Usuarios (UsuarioID)
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
        CONSTRAINT FK_Reparaciones_Equipos FOREIGN KEY (EquipoID)
            REFERENCES dbo.Equipos (EquipoID)
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
        CONSTRAINT FK_Asignaciones_Reparaciones FOREIGN KEY (ReparacionID)
            REFERENCES dbo.Reparaciones (ReparacionID),
        CONSTRAINT FK_Asignaciones_Tecnicos FOREIGN KEY (TecnicoID)
            REFERENCES dbo.Tecnicos (TecnicoID)
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
        CONSTRAINT FK_DetallesReparacion_Reparaciones FOREIGN KEY (ReparacionID)
            REFERENCES dbo.Reparaciones (ReparacionID)
    );
END;
GO

SELECT N'Base de datos ProyectoIIBJCF creada correctamente.' AS Resultado;
GO
