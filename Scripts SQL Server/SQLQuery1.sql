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
*/
/*
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

CREATE TABLE Usuario(
IdUsuario  int primary key identity(1,1),
IdRol int references Rol(IdRol),
NombreCompleto varchar(60) not null,
Correo varchar(50) not null,
NombreUsuario varchar(50) not null unique,
Clave varchar(100) not null,
ResetearClave bit default 1,
Activo bit default 1,
FechaCreacion datetime default GETDATE()
);

GO

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

--Tablas estructura de datos
CREATE TABLE Persona(
Cedula int primary key,
Nombre varchar(60),
Apellido varchar(60),
FechaNacimiento DateTime,
Genero char(1),
Profesion varchar(30),
Ocupacion varchar(30),
LugarTrabajo varchar(60),
DireccionTrabajo varchar(150),
TelefonoTrabajo varchar(12),
DireccionHabitacion varchar(150),
TelefonoHabitacion varchar(12),
Solicitante bit not null,
Beneficiario bit not null,
Funcionario bit not null
);

GO

--CREATE TABLE Beneficiario(
--CedulaBeneficiario int primary key,
--Nombre varchar(60) not null,
--Apellido varchar(60) not null,
----Edad tinyint not null,
--Genero char(1) not null,
--FechaNacimiento datetime not null,
--Ocupacion varchar(30) not null,
--DireccionBeneficiario varchar(150) not null,
--TelefonoBeneficiario varchar(12) not null
--);

--GO

--CREATE TABLE Solicitante(
--CedulaSolicitante int primary key,
--Nombre varchar(60) not null,
--Apellido varchar(60) not null,
----Edad tinyint not null,
--Genero char(1) not null,
--FechaNacimiento DateTime not null,
--Profesion varchar(30) not null,
--Ocupacion varchar(30) not null,
--LugarTrabajo varchar(60) not null,
--DireccionTrabajo varchar(150) not null,
--TelefonoTrabajo varchar(12) not null,
--DireccionHabitacion varchar(150) not null,
--TelefonoHabitacion varchar(12) not null
--);

--GO

--CREATE TABLE Funcionario(
--CedulaFuncionario int primary key,
--Nombre Varchar(60) not null,
--Apellido Varchar(60) not null
--);

--GO

CREATE TABLE Item(
IdItem int primary key identity(1,1),
Nombre varchar(80) not null unique,
Descripccion varchar(300) not null
);

GO

CREATE TABLE Planilla(
IdPlanilla int primary key identity(1,1),
Solicitante int references Persona(Cedula) not null,
EdadSolicitante tinyint not null,

TipoAyuda varchar(30) not null,
DescripcionAyuda varchar(500) not null,

TotalSolicitadoItems int default 0,
TotalRecibidoItems int default 0,
--ListaItems xml,

MontoSolicitadoAyuda decimal(10,2) default 0,
MontoRecibidoAyuda decimal(10,2) default 0,
RecibidoPor varchar(75) default '',

Beneficiario int references Persona(Cedula) not null,
EdadBeneficiario tinyint not null,

Observaciones varchar(300) not null,

Funcionario int references Persona(Cedula) not null,

FechaSolicitud datetime default getdate(),
FechaEntrega datetime default getdate(),
Activo bit default 1
);

GO

CREATE TABLE ListaItem(
IdLista int primary key identity(1,1),
IdPlanilla int references Planilla(IdPlanilla) not null,
IdItem int references Item(IdItem) not null,
CantidadSolicitada int not null,
CantidadEntregada int default 0,
FechaEntrega datetime default GETDATE(),
RecibidoPor varchar(75) default ''
);

GO