 USE DBSISTEMA_GS
GO

set dateformat dmy;
GO

insert into Rol (Nombre) values
('Administrador'),('Asistente'),('Lector');

insert into Usuario (IdRol, NombreCompleto, Correo, NombreUsuario, Clave, ResetearClave) values
(
1, 
'Alejandro Enrique Yanes Aparcedo', 
'alejandriqueparcedo@gmail.com', 
'Alekike',
'2d27f0d5ce289b128aeb5bdbe97d69865111fb057c4d9a6fbf6351666ef30eab',
0
)

insert into Usuario (IdRol, NombreCompleto, Correo, NombreUsuario, Clave, ResetearClave) values
(
1, 
'Daniel Sandez', 
'', 
'DanielS',
'29d1a7e977ce25c4bd822f1872c33a0f657592c1b9faaf75fddace1f61f4af9e',
0
)

select * from Usuario;

GO
--Procedimiento rol

CREATE PROCEDURE sp_listarRol
(
@Buscar varchar(60) = ''
)
as
begin
	select
	IdRol,
	Nombre
	from Rol
	Where Nombre like '%' + @Buscar + '%'
end

GO

--Procedimiento Usuario

CREATE PROCEDURE sp_listarUsuario
(
@Buscar varchar(60) = ''
)
as
begin
	select
	u.IdUsuario,
	u.IdRol,
	r.Nombre[NombreRol],
	u.NombreCompleto,
	u.NombreUsuario,
	u.Correo,
	u.Activo
	from Usuario u
	inner join rol r on r.IdRol = u.IdRol
	where CONCAT(r.Nombre, u.NombreCompleto, u.NombreUsuario, u.Correo, iif(u.Activo = 1, 'Si', 'No')) like '%' + @Buscar + '%'
end

GO

CREATE PROCEDURE sp_crearUsuario
(
@IdRol int,
@NombreCompleto varchar(60),
@NombreUsuario varchar(50),
@Correo varchar(50),
@Clave varchar(100),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(exists (select * from Usuario where Correo = @Correo))
	begin
		set @MsjError = 'Este correo es de alguien más'
		return
	end
	if(exists (select * from Usuario where NombreCompleto = @NombreUsuario))
	begin
		set @MsjError = 'Este usuario es de alguien más'
		return
	end

	BEGIN TRY  
		BEGIN TRANSACTION  
			insert into Usuario (IdRol, NombreCompleto, Correo, NombreUsuario, Clave) 
			values (@IdRol, @NombreCompleto, @Correo, @NombreUsuario, @Clave)
		COMMIT TRANSACTION  
	END TRY  
	BEGIN CATCH  
		ROLLBACK TRANSACTION  
		SET @MsjError = ERROR_MESSAGE()  
	END CATCH 
end

GO

CREATE PROCEDURE sp_editarUsuario
(
@IdUsuario int,
@IdRol int,
@NombreCompleto varchar(60),
@NombreUsuario varchar(50),
@Correo varchar(50),
@Activo bit,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(exists (select * from Usuario where Correo = @Correo AND IdUsuario != @IdUsuario))
	begin
		set @MsjError = 'Este correo es de alguien más'
		return
	end
	if(exists (select * from Usuario where NombreUsuario = @NombreUsuario AND IdUsuario != @IdUsuario))
	begin
		set @MsjError = 'Este usuario es de alguien más'
		return
	end

	BEGIN TRY  
		BEGIN TRANSACTION  
			update Usuario set
			IdRol = @IdRol,
			NombreCompleto = @NombreCompleto, 
			Correo = @Correo, 
			NombreUsuario = @NombreUsuario,
			Activo = @Activo
			where IdUsuario = @IdUsuario
		COMMIT TRANSACTION  
	END TRY  
	BEGIN CATCH  
		ROLLBACK TRANSACTION  
		SET @MsjError = ERROR_MESSAGE()  
	END CATCH 
end

GO

CREATE PROCEDURE sp_iniciarSesion(
@NombreUsuario varchar(50),
@Clave varchar(100),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(NOT exists(select * from Usuario where NombreUsuario = @NombreUsuario))
	begin
		set @MsjError = 'Este usuario no existe'
		return
	end
	else if(NOT exists(select * from Usuario where (NombreUsuario = @NombreUsuario) AND (Clave = @Clave)))
	begin
		set @MsjError = 'Clave incorrecta'
	end
	select 
		u.IdUsuario,
		u.IdRol,
		r.Nombre[NombreRol],
		u.NombreCompleto,
		u.NombreUsuario,
		u.Correo,
		u.Resetearclave,
		u.Activo
	from Usuario u
	inner join rol r on r.IdRol = u.IdRol
	where NombreUsuario = @NombreUsuario AND Clave = @Clave
end

GO

CREATE PROCEDURE sp_cambiarClave(
@NombreUsuario varchar(50),
@Clave varchar(100),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(NOT exists(select * from Usuario where NombreUsuario = @NombreUsuario))
	begin
		set @MsjError = 'Este usuario no existe'
		return
	end

	update Usuario set
	Clave = @Clave
	where NombreUsuario = @NombreUsuario

end

GO