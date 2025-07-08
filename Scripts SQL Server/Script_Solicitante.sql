USE DBSISTEMA_GST
GO

set dateformat dmy;
GO

CREATE PROCEDURE sp_listarSolicitante(
@Buscar varchar(60) = ''
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
	WHERE CONCAT(Cedula,Nombre,Apellido)
	like '%' + @Buscar + '%'
end

GO

CREATE PROCEDURE sp_crearSolicitante(
@CedulaSolicitante int,
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
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	
	if(exists (select * from Persona where Cedula = @CedulaSolicitante))
	begin
		set @MsjError = 'Esta cédula es de alguien más'
		return
	end

	insert into Persona(Cedula,Nombre,Apellido,Genero,FechaNacimiento,Profesion,Ocupacion,LugarTrabajo,DireccionTrabajo,TelefonoTrabajo,DireccionHabitacion,TelefonoHabitacion)
	values(@CedulaSolicitante,@Nombre,@Apellido,@Genero,@FechaNacimiento,@Profesion,@Ocupacion,@LugarTrabajo,@DireccionTrabajo,@TelefonoTrabajo,@DireccionHabitacion,@TelefonoHabitacion)
end

GO

CREATE PROCEDURE sp_editarSolicitante(
@NuevaCedulaSolicitante int,
@CedulaSolicitante int,
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
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(NOT (@CedulaSolicitante = @NuevaCedulaSolicitante))
	begin
		if(exists (select * from Persona where Cedula = @NuevaCedulaSolicitante))
		begin
			set @MsjError = 'Esta cédula es de alguien más'
			return
		end
	end
	if(exists (select * from Planilla where @CedulaSolicitante in (Solicitante, Beneficiario, Funcionario)))
		BEGIN TRY
			BEGIN TRANSACTION
				
				ALTER TABLE Planilla DROP CONSTRAINT FK__Planilla__Solici__320C68B7

				update Planilla
				set
				Solicitante = @NuevaCedulaSolicitante
				where Solicitante = @CedulaSolicitante

				update Persona
				set
				CedulaSolicitante = @NuevaCedulaSolicitante,
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
				where CedulaSolicitante = @CedulaSolicitante

				ALTER TABLE Planilla ADD CONSTRAINT FK__Planilla__Solici__320C68B7
				FOREIGN KEY (Solicitante) REFERENCES Solicitante(CedulaSolicitante)

			COMMIT TRANSACTION
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			SET @MsjError = ERROR_MESSAGE()
		END CATCH
end

GO

ALTER PROCEDURE sp_mostrarBeneficiados(
@CedulaSolicitante int,
@Buscar Varchar(60),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(NOT exists(select * from Planilla where Solicitante = @CedulaSolicitante))
	begin
		set @MsjError = 'Ese solicitante no existe en ningún registro'
		return
	end

	Select DISTINCT
	p.Beneficiario,
	b.Nombre,
	b.Apellido,
	b.Genero,
	b.FechaNacimiento,
	b.Ocupacion,
	b.DireccionBeneficiario,
	b.TelefonoBeneficiario
	from planilla p
	inner join Beneficiario b on p.Beneficiario = b.CedulaBeneficiario
	where (p.Solicitante = @CedulaSolicitante) AND CONCAT(p.Beneficiario, b.Nombre, b.Apellido) like '%' + @Buscar + '%'

end

GO