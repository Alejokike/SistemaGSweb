USE DBSISTEMA_GS
GO

set dateformat dmy;
GO

--Creación de procedimientos--
ALTER procedure sp_listarPlanilla  
(  
 @Buscar varchar(60) = ''  
)  
as  
begin
	select   
	p.IdPlanilla,  
	p.Solicitante[CedulaSolicitante],  
	s.Nombre[NombreSolicitante],  
	s.Apellido[ApellidoSolicitante],  
	p.EdadSolicitante,  
	s.Genero[GeneroSolicitante],
	s.FechaNacimiento[FechaNacimientoSolicitante],
	s.Profesion[ProfesionSolicitante],  
	s.Ocupacion[OcupacionSolicitante],  
	s.LugarTrabajo[LugarTrabajoSolicitante],  
	s.DireccionTrabajo[DireccionTrabajoSolicitante],  
	s.TelefonoTrabajo[TelefonoTrabajoSolicitante],  
	s.DireccionHabitacion[DireccionHabitacionSolicitante],  
	s.TelefonoHabitacion[TelefonoHabitacionSolicitante],  
	p.TipoAyuda,  
	p.DescripcionAyuda,  
	p.TotalSolicitadoItems,  
	p.TotalRecibidoItems,  
	p.MontoSolicitadoAyuda,  
	p.MontoRecibidoAyuda,  
	p.RecibidoPor,  
	p.Beneficiario[CedulaBeneficiario],  
	b.Nombre[NombreBeneficiario],  
	b.Apellido[ApellidoBeneficiario],  
	p.EdadBeneficiario,  
	b.Genero[GeneroBeneficiario],  
	b.FechaNacimiento[FechaNacimientoBeneficiario],  
	b.Ocupacion[OcupacionBeneficiario],  
	b.DireccionBeneficiario[DireccionBeneficiario],  
	b.TelefonoBeneficiario[TelefonoBeneficiario],  
	p.Observaciones,  
	p.Funcionario[CedulaFuncionario],  
	f.Nombre[NombreFuncionario],  
	f.Apellido[ApellidoFuncionario],  
	p.FechaSolicitud,  
	p.FechaEntrega,  
	p.Activo  
	from  
	Planilla p  
	inner join Solicitante s on p.Solicitante = s.CedulaSolicitante  
	inner join Beneficiario b on p.Beneficiario = b.CedulaBeneficiario  
	inner join Funcionario f on p.Funcionario = f.CedulaFuncionario  
	WHERE CONCAT(p.IdPlanilla,p.Solicitante,s.Nombre,s.Apellido,p.TipoAyuda,p.Beneficiario,b.Nombre,b.Apellido,p.Funcionario,f.Nombre,f.Apellido,p.FechaEntrega,p.FechaSolicitud, iif(p.Activo = 1, 'Abierto', 'Cerrado'))  
	like '%' + @Buscar + '%'  
end  

GO

ALTER procedure sp_crearPlanilla
(
@Solicitante int,
@EdadSolicitante tinyint,
@Beneficiario int,
@EdadBeneficiario tinyint,
@TipoAyuda varchar(30),
@DescripcionAyuda varchar(500),
@TotalSolicitado int,
@MontoSolicitado decimal(10,2),
@Observaciones varchar(300),
@Funcionario int,
@IdPlanilla int output,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''

	if(@MontoSolicitado < 0)
	begin
		set @MsjError = 'El monto no puede ser un número negativo'
		return
	end
	BEGIN TRY
		BEGIN TRANSACTION
			insert into Planilla(
			Solicitante,
			EdadSolicitante,
			TipoAyuda,
			DescripcionAyuda,
			TotalSolicitadoItems,
			MontoSolicitadoAyuda,
			Beneficiario,
			EdadBeneficiario,
			Observaciones,
			Funcionario)
			values(
			@Solicitante,
			@EdadSolicitante,
			@TipoAyuda,
			@DescripcionAyuda,
			@TotalSolicitado, 
			@MontoSolicitado,
			@Beneficiario,
			@EdadBeneficiario,
			@Observaciones,
			@Funcionario);
			select @IdPlanilla = Max(IdPlanilla) from Planilla;
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		SET @MsjError = ERROR_MESSAGE()
	END CATCH
end

GO

ALTER PROCEDURE sp_editarPlanilla(
@IdPlanilla int,
@Solicitante int,
@EdadSolicitante tinyint,
@TipoAyuda varchar(30),
@DescripcionAyuda varchar(500),
@MontoSolicitado decimal(10,2),
@MontoRecibido decimal(10,2),
@RecibidoPor varchar(75),
@Beneficiario int,
@EdadBeneficiario tinyint,
@Observaciones varchar(300),
@Funcionario int,
@FechaSolicitud datetime,
@FechaEntrega datetime,
@Activo bit,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	if(@FechaEntrega < @FechaSolicitud)
	begin
		set @MsjError = 'Fecha de entrega no valida, revise nuevamente'
		return 
	end
	BEGIN TRY
		BEGIN TRANSACTION
			update Planilla
			set
			Solicitante = @Solicitante,
			EdadSolicitante = @EdadSolicitante,
			TipoAyuda = @TipoAyuda,
			DescripcionAyuda = @DescripcionAyuda,
			MontoSolicitadoAyuda = @MontoSolicitado,
			MontoRecibidoAyuda = @MontoRecibido,
			RecibidoPor = @RecibidoPor,
			Beneficiario = @Beneficiario,
			EdadBeneficiario = @EdadBeneficiario,
			Observaciones = @Observaciones,
			Funcionario = @Funcionario,
			FechaSolicitud = @FechaSolicitud,
			FechaEntrega = @FechaEntrega,
			Activo = @Activo
			where IdPlanilla = @IdPlanilla
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
			SET @MsjError = ERROR_MESSAGE();
	END CATCH
end

GO

ALTER PROCEDURE sp_eliminarPlanilla(
@IdPlanilla int,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	BEGIN TRY
		BEGIN TRANSACTION
			declare @Solicitante int
			declare @Beneficiario int
			declare @Funcionario int

			select 
			@Solicitante = Solicitante,
			@Beneficiario = Beneficiario,
			@Funcionario = Funcionario
			from Planilla where IdPlanilla = @IdPlanilla

			declare @ContarSolicitante int
			declare @ContarBeneficiario int
			declare @ContarFuncionario int

			select @ContarSolicitante = count(*) from Planilla where Solicitante = @Solicitante
			select @ContarBeneficiario = count(*) from Planilla where Beneficiario = @Beneficiario
			select @ContarFuncionario = count(*) from Planilla where Funcionario = @Funcionario

			delete from ListaItem where IdPlanilla = @IdPlanilla;
			delete from Planilla where IdPlanilla = @IdPlanilla;

			if(@ContarSolicitante = 1)
			begin
				delete from Solicitante where CedulaSolicitante = @Solicitante;
			end

			if(@ContarBeneficiario = 1)
			begin
				delete from Beneficiario where CedulaBeneficiario = @Beneficiario;
			end

			if(@ContarFuncionario = 1)
			begin
				delete from Funcionario where CedulaFuncionario = @Funcionario;
			end

		COMMIT TRANSACTION 
	END TRY
	BEGIN CATCH 
		ROLLBACK TRANSACTION 
			SET @MsjError = ERROR_MESSAGE() 
		END CATCH
end

GO

ALTER PROCEDURE sp_mostrarReporte(
@Buscar varchar(60),
@Inicio datetime,
@Fin datetime
)
as
begin

	set dateformat dmy

	select 
	p.IdPlanilla,
	p.Solicitante[CedulaSolicitante],
	s.Nombre[NombreSolicitante],
	s.Apellido[ApellidoSolicitante],
	p.EdadSolicitante,
	s.Genero[GeneroSolicitante],
	s.Profesion[ProfesionSolicitante],
	s.Ocupacion[OcupacionSolicitante],
	s.LugarTrabajo[LugarTrabajoSolicitante],
	s.DireccionTrabajo[DireccionTrabajoSolicitante],
	s.TelefonoTrabajo[TelefonoTrabajoSolicitante],
	s.DireccionHabitacion[DireccionHabitacionSolicitante],
	s.TelefonoHabitacion[TelefonoHabitacionSolicitante],
	p.TipoAyuda,
	p.DescripcionAyuda,
	p.TotalSolicitadoItems,
	p.TotalRecibidoItems,
	p.MontoSolicitadoAyuda,
	p.MontoRecibidoAyuda,
	p.RecibidoPor,
	p.Beneficiario[CedulaBeneficiario],
	b.Nombre[NombreBeneficiario],
	b.Apellido[ApellidoBeneficiario],
	p.EdadBeneficiario,
	b.Genero[GeneroBeneficiario],
	b.FechaNacimiento[FechaNacimientoBeneficiario],
	b.Ocupacion[OcupacionBeneficiario],
	b.DireccionBeneficiario[DireccionBeneficiario],
	b.TelefonoBeneficiario[TelefonoBeneficiario],
	p.Observaciones,
	p.Funcionario[CedulaFuncionario],
	f.Nombre[NombreFuncionario],
	f.Apellido[ApellidoFuncionario],
	p.FechaSolicitud,
	p.FechaEntrega,
	p.Activo
	from
	Planilla p
	inner join Solicitante s on p.Solicitante = s.CedulaSolicitante
	inner join Beneficiario b on p.Beneficiario = b.CedulaBeneficiario
	inner join Funcionario f on p.Funcionario = f.CedulaFuncionario
	WHERE (p.FechaSolicitud between @Inicio AND @Fin)
	AND (CONCAT(p.IdPlanilla,p.Solicitante,s.Nombre,s.Apellido,p.TipoAyuda,p.Beneficiario,b.Nombre,b.Apellido,p.Funcionario,f.Nombre,f.Apellido,p.FechaEntrega,p.FechaSolicitud, iif(p.Activo = 1, 'Abierto', 'Cerrado'))
	like '%' + @Buscar + '%')
	AND Activo = 0
end

GO