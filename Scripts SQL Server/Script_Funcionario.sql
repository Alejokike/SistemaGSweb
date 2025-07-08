USE DBSISTEMA_GS
GO

set dateformat dmy;
GO

ALTER PROCEDURE sp_listarFuncionario(
@Buscar varchar(60) = ''
)
as
begin
	select 
	CedulaFuncionario,
	Nombre,
	Apellido
	from Funcionario
	WHERE CONCAT(CedulaFuncionario,Nombre,Apellido)
	like '%' + @Buscar + '%'
end

GO

ALTER PROCEDURE sp_crearFuncionario(
@CedulaFuncionario int,
@Nombre varchar(60),
@Apellido varchar(60),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	--Comparar c�dula con otras tablas, si comparamos solo con su propia tabla existe la posibilidad de que cometas un delito
	if(exists (select * from funcionario where CedulaFuncionario = @CedulaFuncionario))
	begin
		set @MsjError = 'Esta c�dula es de alguien m�s'
		return
	end

	insert into Funcionario(CedulaFuncionario,Nombre,Apellido)
	Values(@CedulaFuncionario,@Nombre,@Apellido)
end
	
GO

ALTER PROCEDURE sp_editarFuncionario(
@NuevaCedulaFuncionario int,
@CedulaFuncionario int,
@Nombre varchar(60),
@Apellido varchar(60),
@MsjError varchar(100) output
)
as
begin
	set @MsjError = ''
	--Comparar c�dula con otras tabla, �deb� usar un id o eleg�r otra carrera?
	if(NOT (@CedulaFuncionario = @NuevaCedulaFuncionario))
	begin
		if(exists (select * from funcionario where CedulaFuncionario = @NuevaCedulaFuncionario))
		begin
			set @MsjError = 'Esta c�dula es de alguien m�s'
			return
		end
	end

	update Funcionario
	set
	CedulaFuncionario = @NuevaCedulaFuncionario,
	Nombre = @Nombre,
	Apellido = @Apellido
	where CedulaFuncionario = @CedulaFuncionario
end
	
GO