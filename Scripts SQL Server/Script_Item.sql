USE DBSISTEMA_GS
GO

set dateformat dmy;
GO

CREATE PROCEDURE sp_listarItem(
@Buscar varchar(60) = ''
)
as
begin
	select 
	IdItem,
	Nombre,
	Descripccion
	from Item
	WHERE CONCAT(IdItem,Nombre)
	like '%' + @Buscar + '%'
end

GO

CREATE PROCEDURE sp_crearItem(
@Nombre varchar(80),
@Descripccion varchar(300),
@MsjError varchar(100) output
)
as
begin
	
	set @MsjError = ''

	if(exists(select * from Item where Nombre = @Nombre))
	begin
		set @MsjError = 'Ya existe ese producto'
		return
	end

	insert into Item(Nombre, Descripccion)
	values(@Nombre,@Descripccion)

end

GO

CREATE PROCEDURE sp_editarItem(
@IdItem int,
@Nombre varchar(80),
@Descripccion varchar(300),
@MsjError varchar(100) output
)
as
begin
	
	set @MsjError = ''

	if(exists(select * from Item where (Nombre = @Nombre AND IdItem != @IdItem)))
	begin
		set @MsjError = 'Ya existe ese producto'
		return
	end

	update Item
	set
	Nombre = @Nombre, 
	Descripccion = @Descripccion
	where IdItem = @IdItem

end

GO