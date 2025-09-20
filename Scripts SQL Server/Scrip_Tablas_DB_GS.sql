CREATE DATABASE DBSISTEMA_GS;

GO

USE DBSISTEMA_GS;

GO

set dateformat dmy;

GO

--Roles y usuarios

CREATE TABLE Rol(
IdRol int primary key identity(1,1),
Nombre varchar(50) not null,
);
select * from Rol
GO

INSERT INTO ROL(Nombre) VALUES ('Administrador'), ('Asistente'), ('Lector'), ('Solicitante');

GO

CREATE TABLE Usuario(
-- Claves
IdUsuario  int primary key identity(1,1),
IdRol int references Rol(IdRol),
--

--Datos Perfil
Correo varchar(50) not null,
NombreUsuario varchar(50) not null unique,
Clave varchar(100) not null,
--

-- Campos
Cedula int references Persona(Cedula) unique,
--

-- Metadata
ResetearClave bit default 1,
Activo bit default 1,
FechaCreacion datetime default GETDATE()
);

insert into Usuario (IdRol, Correo, NombreUsuario, Clave, Cedula, ResetearClave) values
(
1,
'alejandriqueparcedo@gmail.com', 
'Alekike',
'2d27f0d5ce289b128aeb5bdbe97d69865111fb057c4d9a6fbf6351666ef30eab',
29954744,
0
)
select * from Usuario
GO

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

--Tablas estructura de datos

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

-- Ayudas
CREATE TABLE EstadoAyuda(
IdEstado int primary key identity(1,1),
Estado nvarchar(10)
);

INSERT INTO EstadoAyuda (Estado) VALUES ('Pendiente'), ('Aprobada'), ('Entregada'), ('Rechazada');

GO

CREATE TABLE Categoria(
IdCategoria int primary key identity(1,1),
Nombre nvarchar(50)
);

INSERT INTO Categoria (Nombre) VALUES ('Medicamentos'), ('Insumos quirúrgicos'), ('Exámenes de laboratorio'), ('Exámenes de diagnosticos'), ('Consultas médicas'), ('Ayudas funerarias'), ('Equipos de movilidad asistida'), ('Apoyo financiero para cirugías'), ('Prótesis'), ('Útiles escolares'), ('Uniformes'), ('Alimentos'), ('Financiamiento deportivo');

GO

CREATE TABLE Item(
IdItem int primary key identity(1,1),
Categoria int references Categoria(IdCategoria),
Nombre nvarchar(80) not null unique,
Descripcion nvarchar(300) not null
);

GO

CREATE TABLE Planilla(
-- Claves
IdPlanilla int primary key identity(1,1),

Solicitante int references Persona(Cedula) not null,
EdadSolicitante tinyint not null,
Beneficiario int references Persona(Cedula) not null,
EdadBeneficiario tinyint not null,
Funcionario int references Persona(Cedula) not null,

Categoria int references Categoria(IdCategoria) not null,
--

DescripcionJSON nvarchar(MAX) CHECK (ISJSON(DescripcionJSON) = 1),

/*
Descripcion y observaciones
Tasa del dia
unidad (EA, VE)
Cantidad Solicitada
Cantidad Entregada
Fecha de entrega
RecibidoPor varchar(75) default '',
*/

FechaSolicitud datetime default getdate(),
FechaEntrega datetime default getdate(),

Activo bit default 1
);

GO