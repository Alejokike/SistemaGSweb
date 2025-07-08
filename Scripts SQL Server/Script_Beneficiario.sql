USE DBSISTEMA_GS
GO

set dateformat dmy;
GO

ALTER PROCEDURE sp_listarBeneficiario(
@Buscar varchar(60) = ''
)
as
begin
	select 
	CedulaBeneficiario,
	Nombre,
	Apellido,
	Genero,
	FechaNacimiento,
	Ocupacion,
	DireccionBeneficiario,
	TelefonoBeneficiario
	from Beneficiario
	WHERE CONCAT(CedulaBeneficiario,Nombre,Apellido)
	like '%' + @Buscar + '%'
end

GO

ALTER PROCEDURE sp_crearBeneficiario(
@CedulaBeneficiario int,
@Nombre varchar(60),
@Apellido varchar(60),
@Genero char(1),
@FechaNacimiento datetime,
@Ocupacion varchar(30),
@DireccionBeneficiario varchar(150),
@TelefonoBeneficiario varchar(12),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	
	if(exists (select * from Beneficiario where CedulaBeneficiario = @CedulaBeneficiario))
	begin
		set @MsjError = 'Esta cédula es de alguien más'
		return
	end

	insert into Beneficiario(CedulaBeneficiario,Nombre,Apellido,Genero,FechaNacimiento,Ocupacion,DireccionBeneficiario,TelefonoBeneficiario)
	values(@CedulaBeneficiario,@Nombre,@Apellido,@Genero,@FechaNacimiento,@Ocupacion,@DireccionBeneficiario,@TelefonoBeneficiario)
end

GO

ALTER PROCEDURE sp_editarBeneficiario(
@NuevaCedulaBeneficiario int,
@CedulaBeneficiario int,
@Nombre varchar(60),
@Apellido varchar(60),
@Genero char(1),
@FechaNacimiento datetime,
@Ocupacion varchar(30),
@DireccionBeneficiario varchar(150),
@TelefonoBeneficiario varchar(12),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	--Comparar cedula con otras tablas
	if(NOT (@CedulaBeneficiario = @NuevaCedulaBeneficiario))
	begin
		if(exists (select * from Beneficiario where CedulaBeneficiario = @NuevaCedulaBeneficiario))
		begin
			set @MsjError = 'Esta cédula es de alguien más'
			return
		end
	end

	update Beneficiario
	set
	CedulaBeneficiario = @NuevaCedulaBeneficiario,
	Nombre = @Nombre,
	Apellido = @Apellido,
	Genero = @Genero,
	FechaNacimiento  = @FechaNacimiento,
	Ocupacion = @Ocupacion,
	DireccionBeneficiario = @DireccionBeneficiario,
	TelefonoBeneficiario = @TelefonoBeneficiario
	where CedulaBeneficiario = @CedulaBeneficiario
end

GO

ALTER PROCEDURE sp_mostrarListaItemsBeneficiado(
@CedulaBeneficiario int,
@Buscar varchar(60) = ''
)
as
begin
	select
	i.IdItem,
	i.Nombre,
	i.Descripccion[Descripcion],
	li.CantidadSolicitada,
	li.CantidadEntregada,
	p.FechaSolicitud,
	li.FechaEntrega,
	li.RecibidoPor
	from Planilla p
	inner join ListaItem li on li.IdPlanilla = p.IdPlanilla
	inner join Item i on li.IdItem = i.IdItem
	where p.Beneficiario = @CedulaBeneficiario
	AND p.TipoAyuda != 'Ayudas funerarias'
	AND p.TipoAyuda != 'Apoyo financiero para cirugías'
	AND CONCAT(i.Nombre,p.FechaSolicitud,li.FechaEntrega) like '%' + @Buscar + '%'
	order by
	i.Nombre ASC,
	li.CantidadEntregada DESC;

end

GO

ALTER PROCEDURE sp_mostrarItemsBeneficiado(
@CedulaBeneficiario int
)
as
begin
	select
	i.IdItem,
	i.Nombre,
	SUM(li.CantidadSolicitada) as TotalSolicitado,
	SUM(li.CantidadEntregada) as TotalEntregado
	from Planilla p
	inner join ListaItem li on p.IdPlanilla = li.IdPlanilla
	inner join Item i on li.IdItem = i.IdItem
	where p.Beneficiario = @CedulaBeneficiario
	AND p.TipoAyuda != 'Ayudas funerarias'
	AND p.TipoAyuda != 'Apoyo financiero para cirugías'
	group by
	i.IdItem, i.Nombre
	order by
	i.Nombre ASC;

end

GO

ALTER PROCEDURE sp_mostrarDineroEntregadoBeneficiario(
@CedulaBeneficiario int,
@Buscar varchar(60) = ''
)
as
begin
	select
	TipoAyuda,
	MontoSolicitadoAyuda,
	MontoRecibidoAyuda,
	FechaSolicitud,
	FechaEntrega,
	RecibidoPor
	from Planilla
	where Beneficiario = @CedulaBeneficiario
	AND CONCAT(TipoAyuda, FechaSolicitud, FechaEntrega) LIKE '%' + @Buscar + '%'
end

GO

ALTER PROCEDURE sp_mostrarTotalDineroEntregadoBeneficiario(
@CedulaBeneficiario int
)
as
begin
	select
	TipoAyuda,
	sum(MontoSolicitadoAyuda) as TotalSolicitado,
	sum(MontoRecibidoAyuda) as TotalRecibido
	from Planilla
	where Beneficiario = @CedulaBeneficiario
	group by TipoAyuda, FechaEntrega
	order by TipoAyuda, FechaEntrega
end