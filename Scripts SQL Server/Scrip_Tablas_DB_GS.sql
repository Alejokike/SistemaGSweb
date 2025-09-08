CREATE DATABASE DBSISTEMA_GS;

GO

USE DBSISTEMA_GS;

GO

set dateformat dmy;

GO

--Info empresa
/*
Fundación Gestión Social 
G-200057467
Calle Los Almendrones, Quinta Araguaney, Gestión Social, Lechería, edo. Anzoátegui.
0414-8143680
fundaciongslecheria@gmail.com


CREATE TABLE Institucion(
IdNegocio int primary key identity(1,1),
RazonSocial varchar(100),
RIF varchar(20),
Direccion varchar(150),
Telefono varchar(12),
Correo varchar(50),
NombreLogo varchar(100),
UrlLogo varchar(200)
);

GO

INSERT INTO Institucion(RazonSocial, RIF, Direccion, Telefono, Correo) 
VALUES ('Fundación Gestión Social', 'G-200057467', 'Calle Los Almendrones, Quinta Araguaney, Gestión Social, Lechería, edo. Anzoátegui.', '0414-8143680', 'fundaciongslecheria@gmail.com');
*/

--Roles y usuarios
CREATE TABLE Rol(
IdRol int primary key identity(1,1),
Nombre varchar(50) not null,
);

GO

INSERT INTO ROL(Nombre) VALUES ('Administrador'), ('Asistente'), ('Lector');

GO

Drop Table Registro

CREATE TABLE Registro(
    IdRegistro int primary key identity(1,1),
    TablaAfectada nvarchar(50),
    IdRegistroAfectado int,
    Accion nvarchar(20), -- 'INSERT', 'UPDATE', 'DELETE'
    UsuarioResponsable int references Usuario(IdUsuario),
    FechaAccion datetime default getdate(),
    DetalleJSON nvarchar(MAX) CHECK (ISJSON(DetalleJSON) = 1)
);

GO

drop Table Usuario

CREATE TABLE Usuario(
IdUsuario  int primary key identity(1,1),
IdRol int references Rol(IdRol),
NombreCompleto varchar(60) not null,
Correo varchar(50) not null,
NombreUsuario varchar(50) not null unique,
Clave varchar(100) not null,
ResetearClave bit default 1,
Activo bit default 1,
FechaCreacion datetime default GETDATE(),
Cedula int references Persona(Cedula)
);

GO

/*
CREATE TABLE Menu(
IdMenu int primary key identity(1,1),
NombreMenu varchar(50) not null,
IdMenuPadre int default 0 not null
);

GO

CREATE TABLE MenuRol(
IdMenuRol int primary key identity(1,1),
IdMenu int references Menu(IdMenu),
IdRol int references Rol(IdRol),
Activo bit default 1
);

GO
*/

--Tablas estructura de datos

Drop Table persona

CREATE TABLE Persona(
Cedula int primary key,
Nombre nvarchar(60),
Apellido nvarchar(60),
FechaNacimiento DateTime,
Genero char(1),
Profesion nvarchar(30),
Ocupacion nvarchar(30),
LugarTrabajo nvarchar(60),
DireccionTrabajo nvarchar(150),
TelefonoTrabajo nvarchar(12),
DireccionHabitacion nvarchar(150),
TelefonoHabitacion nvarchar(12),
Solicitante bit not null,
Beneficiario bit not null,
Funcionario bit not null
);

GO
/*
CREATE TABLE Beneficiario(
CedulaBeneficiario int primary key,
Nombre varchar(60) not null,
Apellido varchar(60) not null,
--Edad tinyint not null,
Genero char(1) not null,
FechaNacimiento datetime not null,
Ocupacion varchar(30) not null,
DireccionBeneficiario varchar(150) not null,
TelefonoBeneficiario varchar(12) not null
);

GO

CREATE TABLE Solicitante(
CedulaSolicitante int primary key,
Nombre varchar(60) not null,
Apellido varchar(60) not null,
--Edad tinyint not null,
Genero char(1) not null,
FechaNacimiento DateTime not null,
Profesion varchar(30) not null,
Ocupacion varchar(30) not null,
LugarTrabajo varchar(60) not null,
DireccionTrabajo varchar(150) not null,
TelefonoTrabajo varchar(12) not null,
DireccionHabitacion varchar(150) not null,
TelefonoHabitacion varchar(12) not null
);

GO

CREATE TABLE Funcionario(
CedulaFuncionario int primary key,
Nombre Varchar(60) not null,
Apellido Varchar(60) not null
);

GO
*/

CREATE TABLE EstadoAyuda(
IdEstado int primary key identity(1,1),
Estado nvarchar(10)
)

GO

INSERT INTO EstadoAyuda (Estado) VALUES ('Pendiente'), ('Aprobada'), ('Entregada'), ('Rechazada');

GO

CREATE TABLE Planilla(
IdPlanilla int primary key identity(1,1),
Solicitante int references Persona(Cedula) not null,
EdadSolicitante tinyint not null,

TipoAyuda nvarchar(30) not null,
DescripcionAyuda nvarchar(500) not null,
DescripcionJSON nvarchar(MAX) CHECK (ISJSON(DescripcionJSON) = 1),

--TotalSolicitadoItems int default 0,
--TotalRecibidoItems int default 0,
--ListaItems xml,

--MontoSolicitadoAyuda decimal(10,2) default 0,
--MontoRecibidoAyuda decimal(10,2) default 0,
--RecibidoPor varchar(75) default '',

Beneficiario int references Persona(Cedula) not null,
EdadBeneficiario tinyint not null,

Observaciones nvarchar(300) not null,

Funcionario int references Persona(Cedula) not null,

FechaSolicitud datetime default getdate(),
FechaEntrega datetime default getdate(),

IdEstado int references EstadoAyuda(IdEstado),
Activo bit default 1
);

GO

CREATE TABLE Item(
IdItem int primary key identity(1,1),
TipoItem nvarchar(30),
Nombre nvarchar(80) not null unique,
Descripcion nvarchar(300) not null
);

GO

CREATE TABLE ListaItem(
IdLista int primary key identity(1,1),
IdPlanilla int references Planilla(IdPlanilla) not null,
IdItem int references Item(IdItem) not null,
DetalleJSON nvarchar(MAX) CHECK (ISJSON(DetalleJSON) = 1),
FechaSolicitud datetime default GETDATE(),
FechaEntrega datetime default GETDATE()
);

GO