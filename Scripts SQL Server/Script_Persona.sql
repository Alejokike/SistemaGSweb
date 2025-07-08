USE DBSISTEMA_GST
GO

set dateformat dmy;
GO

CREATE PROCEDURE sp_listarPersonas(
@Buscar varchar(60) = '',
@Solicitante bit,
@Beneficiario bit,
@Funcionario bit
)
as
begin
	select 
	Cedula,
	Nombre,
	Apellido,
	Genero,
	FechaNacimiento,
	Profesion,
	Ocupacion,
	LugarTrabajo,
	DireccionTrabajo,
	TelefonoTrabajo,
	DireccionHabitacion,
	TelefonoHabitacion
	from Persona
	WHERE CONCAT(Cedula,Nombre,Apellido) like '%' + @Buscar + '%'
	AND (@Solicitante = Solicitante OR @Beneficiario = Beneficiario OR @Funcionario = Funcionario)
end

GO

CREATE PROCEDURE sp_crearPersona(
@Cedula int,
@Nombre varchar(60),
@Apellido varchar(60),
@Genero char(1),
@FechaNacimiento DateTime,
@Profesion varchar(30),
@Ocupacion varchar(30),
@LugarTrabajo varchar(60),
@DireccionTrabajo varchar(150),
@TelefonoTrabajo varchar(12),
@DireccionHabitacion varchar(150),
@TelefonoHabitacion varchar(12),
@Solicitante bit,
@Beneficiario bit,
@Funcionario bit,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	
	if(exists (select * from Persona where Cedula = @Cedula))
	begin
		set @MsjError = 'Esta cédula es de alguien más'
		return
	end

	insert into Persona(Cedula,Nombre,Apellido,Genero,FechaNacimiento,Profesion,Ocupacion,LugarTrabajo,DireccionTrabajo,TelefonoTrabajo,DireccionHabitacion,TelefonoHabitacion, Solicitante, Beneficiario, Funcionario)
	values(@Cedula,@Nombre,@Apellido,@Genero,@FechaNacimiento,@Profesion,@Ocupacion,@LugarTrabajo,@DireccionTrabajo,@TelefonoTrabajo,@DireccionHabitacion,@TelefonoHabitacion,@Solicitante, @Beneficiario, @Funcionario)
end

GO

CREATE PROCEDURE sp_editarPersona(
@NuevaCedula int,
@Cedula int,
@Nombre varchar(60),
@Apellido varchar(60),
@Genero char(1),
@FechaNacimiento DateTime,
@Profesion varchar(30),
@Ocupacion varchar(30),
@LugarTrabajo varchar(60),
@DireccionTrabajo varchar(150),
@TelefonoTrabajo varchar(12),
@DireccionHabitacion varchar(150),
@TelefonoHabitacion varchar(12),
@Solicitante bit,
@Beneficiario bit,
@Funcionario bit,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(NOT (@Cedula = @NuevaCedula))
	begin
		if(exists (select * from Persona where Cedula = @NuevaCedula))
		begin
			set @MsjError = 'Esta cédula es de alguien más'
			return
		end
		if(exists (select * from Planilla where @Cedula IN (Solicitante, Beneficiario, Funcionario)))
		begin
			BEGIN TRY
				BEGIN TRANSACTION
					
					ALTER TABLE Planilla DROP CONSTRAINT FK__Planilla__Solici__320C68B7

					update Planilla
					set
					Solicitante = @NuevaCedula
					where Solicitante = @Cedula

					update Planilla
					set
					Beneficiario = @NuevaCedula
					where Beneficiario = @Cedula

					update Planilla
					set
					Funcionario = @NuevaCedula
					where Funcionario = @Cedula

					update Persona
					set
					Cedula = @NuevaCedula,
					Nombre = @Nombre,
					Apellido = @Apellido,
					Genero = @Genero,
					FechaNacimiento = @FechaNacimiento,
					Profesion = @Profesion,
					Ocupacion = @Ocupacion,
					LugarTrabajo = @LugarTrabajo,
					DireccionTrabajo = @DireccionTrabajo,
					TelefonoTrabajo = @TelefonoTrabajo,
					DireccionHabitacion = @DireccionHabitacion,
					TelefonoHabitacion = @TelefonoHabitacion
					where Cedula = @Cedula

					ALTER TABLE Planilla ADD CONSTRAINT FK__Planilla__Solici__320C68B7
					FOREIGN KEY (Persona) REFERENCES Persona(Cedula)

				COMMIT TRANSACTION
			END TRY
			BEGIN CATCH
				ROLLBACK TRANSACTION
				SET @MsjError = ERROR_MESSAGE()
			END CATCH
		end
		else
		begin
			update Persona
			set
			Nombre = @Nombre,
			Apellido = @Apellido,
			Genero = @Genero,
			FechaNacimiento = @FechaNacimiento,
			Profesion = @Profesion,
			Ocupacion = @Ocupacion,
			LugarTrabajo = @LugarTrabajo,
			DireccionTrabajo = @DireccionTrabajo,
			TelefonoTrabajo = @TelefonoTrabajo,
			DireccionHabitacion = @DireccionHabitacion,
			TelefonoHabitacion = @TelefonoHabitacion
			where Cedula = @Cedula
		end
end

GO