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
	--Comparar cédula con otras tablas, si comparamos solo con su propia tabla existe la posibilidad de que cometas un delito
	if(exists (select * from funcionario where CedulaFuncionario = @CedulaFuncionario))
	begin
		set @MsjError = 'Esta cédula es de alguien más'
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
	--Comparar cédula con otras tabla, ¿debí usar un id o elegír otra carrera?
	if(NOT (@CedulaFuncionario = @NuevaCedulaFuncionario))
	begin
		if(exists (select * from funcionario where CedulaFuncionario = @NuevaCedulaFuncionario))
		begin
			set @MsjError = 'Esta cédula es de alguien más'
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