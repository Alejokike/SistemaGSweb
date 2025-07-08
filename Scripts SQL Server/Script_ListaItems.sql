USE DBSISTEMA_GS
GO

set dateformat dmy;
GO

ALTER procedure sp_listarListaItem
(
	@BuscarPlanilla int,
	@Buscar varchar(60) = ''
)
as
begin
	select
	li.IdLista,
	li.IdItem,
	li.IdPlanilla,
	i.Nombre,
	i.Descripccion,
	li.CantidadSolicitada,
	li.CantidadEntregada,
	li.FechaEntrega,
	li.RecibidoPor
	from
	ListaItem li
	inner join Item i on i.IdItem = li.IdItem
	WHERE  li.IdPlanilla = @BuscarPlanilla AND CONCAT(li.IdItem,i.Nombre,li.FechaEntrega,li.RecibidoPor)
	like '%' + @Buscar + '%'
end

GO

ALTER procedure sp_crearListaItem
(
@IdPlanilla int,
@IdItem int,
@CantidadSolicitada int,
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	declare @SumadorSolicitado int
	declare @SumadorEntregado int

	if(exists (select * from ListaItem where IdPlanilla = @IdPlanilla AND IdItem = @IdItem))
	begin
		set @MsjError = 'Este item ya figura en esta ayuda'
		return
	end

	insert into ListaItem(IdPlanilla,IdItem,CantidadSolicitada, FechaEntrega)
	values(@IdPlanilla,@IdItem,@CantidadSolicitada, GETDATE())

	select @SumadorSolicitado = SUM(CantidadSolicitada), @SumadorEntregado = SUM(CantidadEntregada) from ListaItem where IdPlanilla = @IdPlanilla

	update Planilla
	set
	TotalSolicitadoItems = @SumadorSolicitado,
	TotalRecibidoItems = @SumadorEntregado
	where IdPlanilla = @IdPlanilla
end

GO

ALTER procedure sp_editarListaItem
(
@IdLista int,
@IdPlanilla int,
@IdItem int,
@CantidadSolicitada int,
@CantidadEntregada int,
@FechaEntrega datetime,
@RecibidoPor varchar(75),
@MsjError varchar(100) output
)
as
begin
	--set @MsjError = ''
	--if(exists (select * from ListaItem where (IdPlanilla = @IdPlanilla AND IdItem = @IdItem AND @IdLista != IdLista)))
	--begin
	--	set @MsjError = 'Este item ya figura en esta ayuda, revise de nuevo'
	--	return
	--end

	update ListaItem
	set
	IdPlanilla = @IdPlanilla,
	IdItem = @IdItem,
	CantidadSolicitada = @CantidadSolicitada,
	CantidadEntregada = @CantidadEntregada,
	FechaEntrega = @FechaEntrega,
	RecibidoPor = @RecibidoPor
	where IdLista = @IdLista

	declare @SumadorSolicitado int
	declare @SumadorEntregado int

	select @SumadorSolicitado = SUM(CantidadSolicitada), @SumadorEntregado = SUM(CantidadEntregada) from ListaItem where IdPlanilla = @IdPlanilla

	 update Planilla
	 set
	 TotalSolicitadoItems = @SumadorSolicitado,
	 TotalRecibidoItems = @SumadorEntregado
	 where IdPlanilla = @IdPlanilla
end

GO

ALTER procedure sp_eliminarListaItem
(
@IdLista int,
@MsjError varchar(100) output
)
as
begin
	 set @MsjError = ''
	 if(NOT exists(select * from ListaItem where IdLista = @IdLista))
	 begin
		set @MsjError = 'Ese item no existe en esta lista'
		return
	 end

	 declare @SumadorSolicitado int
	 declare @SumadorEntregado int
	 declare @IdPlanilla int

	 select @IdPlanilla = IdPlanilla from ListaItem where IdLista = @IdLista
	 delete from ListaItem where IdLista = @IdLista

	 select @SumadorSolicitado = SUM(CantidadSolicitada), @SumadorEntregado = SUM(CantidadEntregada) from ListaItem where IdPlanilla = @IdPlanilla

	 update Planilla
	 set
	 TotalSolicitadoItems = @SumadorSolicitado,
	 TotalRecibidoItems = @SumadorEntregado
	 where IdPlanilla = @IdPlanilla
end

GO
/*
DROP procedure sp_listarTodo
(
	@Buscar varchar(60) = ''
)
as
begin
	select
	li.IdPlanilla,
	li.IdItem,
	i.Nombre,
	p.Solicitante,
	p.Beneficiario,
	li.CantidadSolicitada,
	li.CantidadEntregada,
	p.FechaSolicitud,
	li.FechaEntrega,
	li.RecibidoPor
	from
	ListaItem li
	inner join Item i on i.IdItem = li.IdItem
	inner join Planilla p on p.IdPlanilla = li.IdPlanilla
	WHERE  li.IdPlanilla = @BuscarPlanilla AND CONCAT(li.IdItem,i.Nombre,li.FechaEntrega,li.RecibidoPor)
	like '%' + @Buscar + '%'
end
GO
*/
ALTER procedure sp_listarTodoReporte
(
@Buscar varchar(60) = '',
@Inicio datetime,
@Fin datetime
)
as
begin
	select
	li.IdPlanilla,
	li.IdItem,
	i.Nombre,
	p.Solicitante,
	p.Beneficiario,
	li.CantidadSolicitada,
	li.CantidadEntregada,
	p.FechaSolicitud,
	li.FechaEntrega,
	li.RecibidoPor
	from
	ListaItem li
	inner join Item i on i.IdItem = li.IdItem
	inner join Planilla p on p.IdPlanilla = li.IdPlanilla
	WHERE  (p.FechaSolicitud between @Inicio AND @Fin)
	AND (CONCAT(p.IdPlanilla, li.IdItem,i.Nombre,li.FechaEntrega,li.RecibidoPor) like '%' + @Buscar + '%')
	AND li.RecibidoPor != '';
end

GO