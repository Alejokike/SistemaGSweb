USE DBSISTEMA_GST
GO

set dateformat dmy;
GO

CREATE PROCEDURE sp_mostrarBeneficiados(
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