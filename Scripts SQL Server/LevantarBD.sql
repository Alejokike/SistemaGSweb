CREATE DATABASE DBSISTEMA_GS;

GO

USE DBSISTEMA_GS;

GO

set dateformat dmy;

GO

CREATE TABLE Registro(
IdRegistro int primary key identity(1,1),
TablaAfectada nvarchar(12),
IdRegistroAfectado int,
Accion nvarchar(12),
UsuarioResponsable int,
Detalle nvarchar(MAX) CHECK (ISJSON(Detalle) = 1),
FechaAccion datetime
)

GO

CREATE TABLE Rol(
IdRol int primary key identity(1,1),
Nombre varchar(50) not null,
);

GO

INSERT INTO Rol(Nombre) VALUES ('Administrador'), ('Asistente'), ('Lector'), ('Solicitante');

GO

CREATE TABLE Usuario(
-- Claves
Cedula  int primary key,
IdRol int references Rol(IdRol),
--

--Datos Perfil
NombreUsuario nvarchar(50) not null unique,
Correo nvarchar(50) not null,
Clave nvarchar(100) not null,
Activo bit default 1,
--

-- Metadata
ResetearClave bit default 1,
FechaCreacion datetime default GETDATE()
);

insert into Usuario (Cedula,IdRol, Correo, NombreUsuario, Clave, ResetearClave) values
(
29954744,
1,
'alejandriqueparcedo@gmail.com',
'Alekike',
'2d27f0d5ce289b128aeb5bdbe97d69865111fb057c4d9a6fbf6351666ef30eab',
0
)

GO

CREATE TABLE Persona(
Cedula int primary key,
Nombre nvarchar(60),
Apellido nvarchar(60),
FechaNacimiento DateTime,
Genero nvarchar(1),
Profesion nvarchar(30),
Ocupacion nvarchar(30),
LugarTrabajo nvarchar(60),
DireccionTrabajo nvarchar(150),
TelefonoTrabajo nvarchar(12),
DireccionHabitacion nvarchar(150),
TelefonoHabitacion nvarchar(12)
);

GO
Drop table Item
CREATE TABLE Item(
IdItem int primary key identity(1,1),
Nombre nvarchar(80) not null unique,
Categoria nvarchar(50),
Descripcion nvarchar(300) not null,
Unidad nvarchar(2) default 'EU',
Cantidad decimal default 0,
FechaCreacion datetime default GETDATE(),
Activo bit default 1
);

/*
VE
EU
ME
*/

GO

CREATE TABLE Inventario(
IdTransaccion int primary key identity(1,1),
TipoOperacion nvarchar(3),
Item int,
Unidad nvarchar(2),
Cantidad decimal,
Concepto nvarchar(60),
Fecha Datetime
);

 /*
 REC
 DEV
 ASI
 RET
 */

GO

CREATE TABLE Ayuda(
-- Claves
IdAyuda int primary key identity(1,1),

Solicitante int references Persona(Cedula) not null,
Funcionario int references Persona(Cedula) not null,

Categoria nvarchar(50),
--

Detalle nvarchar(MAX) CHECK (ISJSON(Detalle) = 1),
ListaItems nvarchar(MAX) CHECK (ISJSON(ListaItems) = 1),
Estado nvarchar(20),

/*
Pendiente
En Proceso
Lista para entregar
Cerrada
Rechazada

Descripcion y observaciones
Tasa del dia
unidad (EA, VE)
Cantidad Solicitada
Cantidad Entregada
Fecha de entrega
RecibidoPor varchar(75) default '',
*/

FechaSolicitud datetime default getdate(),
FechaEntrega datetime default getdate()
);

GO

/*
medicamentos, 
insumos quirúrgicos, 
citas para exámenes de laboratorio, 
citas para exámenes de diagnóstico, 
equipos de movilidad asistida, 
prótesis, 
apoyo financiero para cirugías, 
útiles escolares, 
uniformes, 
alimentos
financiamiento deportivo
*/