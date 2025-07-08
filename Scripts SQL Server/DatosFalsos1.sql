set dateformat dmy;
declare @Descri varchar(500);
set @Descri = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Sed ut perspiciatis unde omnis iste natus error.......';
declare @Obser varchar(300);
set @Obser = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillu';

insert into Planilla(Solicitante,EdadSolicitante,TipoAyuda,DescripcionAyuda,TotalSolicitadoItems,TotalRecibidoItems,MontoSolicitadoAyuda,MontoRecibidoAyuda,RecibidoPor,Beneficiario,EdadBeneficiario,Observaciones,Funcionario,FechaSolicitud,FechaEntrega, Activo)
values