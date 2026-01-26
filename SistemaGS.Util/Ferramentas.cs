using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using System.Globalization;
//using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace SistemaGS.Util
{
    public static class Ferramentas
    {
        public static string GenerateCode()
        {
            string guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            return guid;
        }
        public static string ConvertToSha256(string texto)
        {
            using (SHA256 sHA256Hash = SHA256.Create())
            {
                byte[] bytes = sHA256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    public static class Impresion
    {
        private static byte[] LoadImageAsByteArray(string imagePath)
        {
            byte[] imageBytes;
            if (File.Exists(imagePath))
            {
                imageBytes = File.ReadAllBytes(imagePath);
            }
            else
            {
                throw new FileNotFoundException("El archivo de imagen no se encontró para imprimir los PDF.");
            }
            return imageBytes;
        }
        private static int CalcularEdad(DateTime FechaSolicitud, DateTime FechaNacimiento)
        {
            int edad = FechaSolicitud.Year - FechaNacimiento.Year;
            if (FechaSolicitud.Month < FechaNacimiento.Month) edad--;
            else if (FechaSolicitud.Month == FechaNacimiento.Month && FechaSolicitud.Day < FechaNacimiento.Day) edad--;
            return edad;
        }
        public static byte[] GeneratePDFplanilla(AyudaDTO ayuda, PersonaDTO Solicitante, PersonaDTO Funcionario)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var PlanillaPDF =
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    #region Encabezado
                    page.MarginTop(30);
                    page.MarginBottom(30);
                    page.MarginLeft(60);
                    page.MarginRight(60);
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.AutoItem().Height(60).Image(LoadImageAsByteArray("C:\\Users\\alexh\\source\\repos\\SistemaGSwebLocal\\SistemaGS.WebAssembly\\wwwroot\\img\\FundaciónGSletrasnegras.png"));

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("REPÚBLICA BOLIVARIANA DE VENEZUELA").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("FUNDACIÓN GESTIÓN SOCIAL").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("MUNICIPIO TURÍSTICO EL MORRO LCDO. DIEGO BAUTISTA URBANEJA").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("LECHERÍA ESTADO ANZOÁTEGUI").Bold().FontSize(9);
                            col.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("PLANILLA DE SOLICITUD DE AYUDA SOCIAL").Bold().FontSize(9);
                            col.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                        });

                        row.AutoItem().Height(60).Image(LoadImageAsByteArray("C:\\Users\\alexh\\source\\repos\\SistemaGSwebLocal\\SistemaGS.WebAssembly\\wwwroot\\img\\LogoAlcaldiaLetrasAzules.png"));
                    });
                    #endregion
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Column(col2 =>
                        {
                            col2.Item().AlignRight().Text($"FECHA DE LA SOLICITUD: {ayuda.FechaSolicitud.ToString("dd/MM/yyyy")}").Bold().FontSize(9);
                            col2.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                            col2.Item().PaddingBottom(10).Text("DATOS DEL SOLICITANTE").Bold().FontSize(9);

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(0.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.7f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(25);
                                    header.Cell().Column(1).Border(1.3f).AlignMiddle().Text("NOMBRES Y APELLIDOS").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(2).Border(1.3f).AlignMiddle().Text("C.I.").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(3).Border(1.3f).AlignMiddle().Text("EDAD").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(4).Border(1.3f).AlignMiddle().Text("PROFESIÓN").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(5).Border(1.3f).AlignMiddle().Text("OCUPACIÓN").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(30);
                                tabla.Cell().RowSpan(2).Column(1).Border(1.3f).AlignMiddle().Text($"{Solicitante.Nombre} {Solicitante.Apellido}").FontSize(9).Justify().AlignCenter();
                                tabla.Cell().RowSpan(2).Column(2).Border(1.3f).AlignMiddle().Text($"{Solicitante.Cedula!.Value.ToString("N0")}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Column(3).Border(1.3f).AlignMiddle().Text($"{CalcularEdad(ayuda.FechaSolicitud, Solicitante.FechaNacimiento!.Value)}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Column(4).Border(1.3f).AlignMiddle().Text($"{Solicitante.Profesion}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Column(5).Border(1.3f).AlignMiddle().Text($"{Solicitante.Ocupacion}").FontSize(9).AlignCenter();
                            });

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(0.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.7f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(15);
                                    header.Cell().Column(1).ColumnSpan(4).Border(1.3f).AlignMiddle().Text(" LUGAR DE TRABAJO Y DIRECCIÓN").Bold().FontSize(9).Justify();
                                    header.Cell().Column(5).Border(1.3f).AlignMiddle().Text("TELÉFONO").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(35);
                                tabla.Cell().Column(1).ColumnSpan(4).Border(1.3f).AlignMiddle().Text($"{Solicitante.LugarTrabajo}; {Solicitante.DireccionTrabajo}").FontSize(9).Justify().AlignCenter();
                                tabla.Cell().RowSpan(2).Column(5).Border(1.3f).AlignMiddle().Text($"{Solicitante.TelefonoTrabajo}").FontSize(9).AlignCenter();
                            });

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(0.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.7f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(15);
                                    header.Cell().Column(1).ColumnSpan(4).Border(1.3f).AlignMiddle().Text(" DIRECCIÓN DE HABITACIÓN").Bold().FontSize(9).Justify();
                                    header.Cell().Column(5).Border(1.3f).AlignMiddle().Text("TELÉFONO").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(35);
                                tabla.Cell().Column(1).ColumnSpan(4).Border(1.3f).AlignMiddle().Text($"{Solicitante.DireccionHabitacion}").FontSize(9).Justify().AlignCenter();
                                tabla.Cell().RowSpan(2).Column(5).Border(1.3f).AlignMiddle().Text($"{Solicitante.TelefonoHabitacion}").FontSize(9).AlignCenter();
                            });

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(15);
                                    header.Cell().Column(1).Border(1.3f).AlignMiddle().Text("TIPO DE SOLICITUD").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(25);
                                tabla.Cell().Column(1).Border(1.3f).AlignMiddle().Text($"{ayuda.Categoria}").FontSize(9).Justify().AlignCenter();
                            });

                            col1.Item().PaddingTop(10).PaddingBottom(10).Text("DATOS DEL BENEFICIARIO").Bold().FontSize(9);

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(0.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.7f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(25);
                                    header.Cell().Column(1).Border(1.3f).AlignMiddle().Text("NOMBRES Y APELLIDOS").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(2).Border(1.3f).AlignMiddle().Text("C.I.").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(3).Border(1.3f).AlignMiddle().Text("EDAD").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(4).Border(1.3f).AlignMiddle().Text("FECHA DE NACIMIENTO").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(5).Border(1.3f).AlignMiddle().Text("OCUPACIÓN").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(30);
                                tabla.Cell().RowSpan(2).Column(1).Border(1.3f).AlignMiddle().Text($"{Solicitante.Nombre} {Solicitante.Apellido}").FontSize(9).Justify().AlignCenter();
                                tabla.Cell().RowSpan(2).Column(2).Border(1.3f).AlignMiddle().Text($"{Solicitante.Cedula!.Value.ToString("N0")}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Column(3).Border(1.3f).AlignMiddle().Text($"{CalcularEdad(ayuda.FechaSolicitud, Solicitante.FechaNacimiento!.Value)}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Column(4).Border(1.3f).AlignMiddle().Text($"{Solicitante.FechaNacimiento.Value.ToString("dd/MM/yyyy")}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Column(5).Border(1.3f).AlignMiddle().Text($"{Solicitante.Ocupacion}").FontSize(9).AlignCenter();
                            });

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(0.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.7f);
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(15);
                                    header.Cell().Column(1).ColumnSpan(4).Border(1.3f).AlignMiddle().Text(" DIRECCIÓN DE BENEFICIARIO").Bold().FontSize(9).Justify();
                                    header.Cell().Column(5).Border(1.3f).AlignMiddle().Text("TELÉFONO").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(35);
                                tabla.Cell().Column(1).ColumnSpan(4).Border(1.3f).AlignMiddle().Text($"{Solicitante.DireccionHabitacion}").FontSize(9).Justify().AlignCenter();
                                tabla.Cell().Column(5).Border(1.3f).AlignMiddle().Text($"{Solicitante.TelefonoHabitacion}").FontSize(9).AlignCenter();
                                tabla.Cell().RowSpan(2).Row(2).ColumnSpan(4).Column(1).Border(1.3f).AlignMiddle().Text(" DECLARO BAJO FE DE JURAMENTO LA VERDAD DE LA INFORMACIÓN\n SUMINISTRADA Y DE LOS DOCUMENTOS").Bold().FontSize(9).AlignCenter().AlignLeft();
                                tabla.Cell().Height(25);
                                tabla.Cell().Row(2).Column(5).Border(1.3f).AlignMiddle().Text("FIRMA SOLICITANTE").Bold().FontSize(9).AlignCenter();
                                tabla.Cell().Height(30);
                                tabla.Cell().Row(3).Column(5).Border(1.3f).AlignMiddle().Text("\n\n").Bold().FontSize(9).AlignCenter();
                            });

                            col1.Item().PaddingTop(10).PaddingBottom(10).
                            Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(15);
                                    header.Cell().Column(1).Border(1.3f).AlignMiddle().Text("OBSERVACIONES DEL CASO:").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(50);
                                tabla.Cell().Column(1).Border(1.3f).AlignMiddle().Text($"{ayuda.Detalle["Observaciones"]}").FontSize(9).Justify().AlignCenter();
                            });

                            col1.Item().
                            Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                tabla.Header(header =>
                                {
                                    header.Cell().Height(15);
                                    header.Cell().Column(1).Border(1.3f).AlignMiddle().Text("POR BIENESTAR SOCIAL").Bold().FontSize(9).AlignCenter();
                                    header.Cell().Column(2).Border(1.3f).AlignMiddle().Text("POR DIRECCION DE GESTIÓN SOCIAL").Bold().FontSize(9).AlignCenter();
                                });

                                tabla.Cell().Height(40);
                                tabla.Cell().Row(1).Column(1).Border(1.3f).AlignMiddle().Text("\n\n").FontSize(9).Justify().AlignCenter();
                                tabla.Cell().Row(1).Column(2).Border(1.3f).AlignMiddle().Text("\n\n").FontSize(9).Justify().AlignCenter();

                                tabla.Cell().Row(2).RowSpan(2).Column(1).Container().Row(row =>
                                {
                                    row.RelativeItem().Table(inner =>
                                    {
                                        inner.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(1.5f);
                                            columns.RelativeColumn(0.5f);
                                        });

                                        inner.Header(Iheader =>
                                        {
                                            Iheader.Cell().Height(11);
                                            Iheader.Cell().Column(1).Border(1.3f).AlignMiddle().Text("NOMBRE Y APELLIDO FUNCIONARIO:").LineHeight(1.5f).Bold().FontSize(9).AlignCenter();
                                            Iheader.Cell().Column(2).Border(1.3f).AlignMiddle().Text("C.I.").LineHeight(1.5f).Bold().FontSize(9).AlignCenter();
                                        });

                                        inner.Cell().Height(12);
                                        inner.Cell().Column(1).Border(1.3f).AlignMiddle().Text($"{Funcionario?.Nombre ?? "  Sin funcionario"} {Funcionario?.Apellido}").FontSize(9).Justify().AlignCenter();
                                        inner.Cell().Column(2).Border(1.3f).AlignMiddle().Text($"{Funcionario?.Cedula?.ToString("N0") ?? "N/A"}").FontSize(9).Justify().AlignCenter();
                                    });
                                });

                                tabla.Cell().Row(2).RowSpan(2).Column(2).Border(1.3f).AlignMiddle().Text("DIRECTOR(A) DE GESTIÓN SOCIAL").Bold().FontSize(9).Justify().AlignCenter();

                                tabla.Cell().Row(4).Column(1).ColumnSpan(2).Border(1.3f).AlignMiddle().Text("POR FUNDACIÓN GESTIÓN SOCIAL DE LECHERÍA").LineHeight(1.5f).Bold().FontSize(9).AlignCenter();

                                tabla.Cell().Row(5).Column(1).ColumnSpan(2).Border(1.3f).ExtendVertical().AlignBottom().Text("PRESIDENTE(A) DE FUNDACIÓN GESTIÓN SOCIAL DE LECHERÍA").Bold().FontSize(9).AlignCenter();
                            });
                        });
                    });
                });
            }).GeneratePdf();

            return PlanillaPDF;
        }
        public static byte[] GeneratePDFdetalle(AyudaDTO ayuda, PersonaDTO Solicitante, PersonaDTO? Funcionario)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var DetallePDF =
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    #region Encabezado
                    page.Margin(60);
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.AutoItem().Height(60).Image(LoadImageAsByteArray("C:\\Users\\alexh\\source\\repos\\SistemaGSwebLocal\\SistemaGS.WebAssembly\\wwwroot\\img\\FundaciónGSletrasnegras.png"));

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("REPÚBLICA BOLIVARIANA DE VENEZUELA").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("FUNDACIÓN GESTIÓN SOCIAL").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("MUNICIPIO TURÍSTICO EL MORRO LCDO. DIEGO BAUTISTA URBANEJA").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("LECHERÍA ESTADO ANZOÁTEGUI").Bold().FontSize(9);
                            col.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                            col.Item().AlignCenter().Text($"DETALLE DE LA AYUDA N° {ayuda.IdAyuda}").Bold().FontSize(9);
                            col.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                        });

                        row.AutoItem().Height(60).Image(LoadImageAsByteArray("C:\\Users\\alexh\\source\\repos\\SistemaGSwebLocal\\SistemaGS.WebAssembly\\wwwroot\\img\\LogoAlcaldiaLetrasAzules.png"));
                    });
                    #endregion
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("Resumen reporte:").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Fecha de solicitud: ").SemiBold().FontSize(10);
                                txt.Span(ayuda.FechaSolicitud.ToString("dd/MM/yyyy")).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Fecha de resolución: ").SemiBold().FontSize(10);
                                txt.Span(ayuda.FechaEntrega.HasValue ? ayuda.FechaEntrega.Value.ToString("dd/MM/yyyy") : "N/A").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Solicitante: ").SemiBold().FontSize(10);
                                txt.Span($"{Solicitante.Nombre} {Solicitante.Apellido}").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Funcionario: ").SemiBold().FontSize(10);
                                txt.Span($"{Funcionario?.Nombre ?? "  Sin funcionario"} {Funcionario?.Apellido}").FontSize(10);
                            });
                        });

                        col1.Spacing(5);
                        col1.Item().LineHorizontal(1.5f);
                        col1.Spacing(5);

                        #region Detalle

                        if (ayuda.Detalle.TryGetValue("Solicitud", out string? solicitud) && !(string.IsNullOrEmpty(solicitud)))
                        {
                            col1.Item().Background(Colors.Green.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Razón de la solicitud").FontSize(14);
                                column.Item().Text(solicitud);
                                column.Spacing(5);
                            });
                        }
                        if (ayuda.Detalle.TryGetValue("Observaciones", out string? observaciones) && !(string.IsNullOrEmpty(observaciones)))
                        {
                            col1.Item().Background(Colors.Blue.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Observaciones del funcionario").FontSize(14);
                                column.Item().Text(observaciones);
                                column.Spacing(5);
                            });
                        }
                        if (ayuda.Detalle.TryGetValue("Aprobado", out string? aprobado) && !(string.IsNullOrEmpty(aprobado)))
                        {
                            col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Detalle de la finalización del proceso").FontSize(14);
                                column.Item().Text(aprobado);
                                column.Spacing(5);
                            });
                        }
                        if (ayuda.Detalle.TryGetValue("Rechazado", out string? rechazado) && !(string.IsNullOrEmpty(rechazado)))
                        {
                            col1.Item().Background(Colors.Red.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Motivo de rechazo").FontSize(14);
                                column.Item().Text(rechazado);
                                column.Spacing(5);
                            });
                        }
                        #endregion

                        col1.Item().PageBreak();

                        #region Items

                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Text("Lista de ítems asociada");
                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#003666")
                                .Padding(2).Text("Nombre").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                               .Padding(2).Text("Categoría").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                               .Padding(2).Text("Cantidad Solicitada").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                               .Padding(2).Text("Cantidad Entregada").Bold().FontColor("#fff").AlignCenter();
                            });

                            foreach (var item in ayuda.ListaItems)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.ItemLista.Nombre).FontSize(11);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.ItemLista.Categoria).FontSize(11);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.ItemLista.Unidad == "VE" ? item.CantidadSolicitada.ToString("N2") : item.CantidadSolicitada.ToString("N0")).FontSize(9).AlignCenter();

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.ItemLista.Unidad == "VE" ? item.CantidadEntregada.ToString("N2") : item.CantidadEntregada.ToString("N0")).FontSize(9).AlignCenter();

                            }
                        });

                        col1.Item().PageBreak();

                        col1.Item().Background(Colors.Amber.Lighten3).Padding(20)
                            .Column(column =>
                            {
                                column.Item().Text("Total").Bold().FontSize(14);
                                column.Item().AlignLeft().Text(txt => 
                                {
                                    txt.Span("Total Solicitado (VE): ").SemiBold();
                                    txt.Span(ayuda.ListaItems.Where(li => li.ItemLista.Unidad == "VE").Sum(li => li.CantidadSolicitada).ToString("N2"));
                                });
                                column.Item().AlignLeft().Text(txt => 
                                {
                                    txt.Span("Total Solicitado (MU): ").SemiBold();
                                    txt.Span(ayuda.ListaItems.Where(li => li.ItemLista.Unidad == "MU").Sum(li => li.CantidadSolicitada).ToString("N0"));
                                });
                                column.Item().AlignLeft().Text(txt => 
                                {
                                    txt.Span("Total Solicitado (EU): ").SemiBold();
                                    txt.Span(ayuda.ListaItems.Where(li => li.ItemLista.Unidad == "EU").Sum(li => li.CantidadSolicitada).ToString("N0"));
                                });

                                column.Item().Padding(5);

                                column.Item().AlignLeft().Text(txt => 
                                {
                                    txt.Span("Total Entregado (VE): ").SemiBold();
                                    txt.Span(ayuda.ListaItems.Where(li => li.ItemLista.Unidad == "VE").Sum(li => li.CantidadEntregada).ToString("N2"));
                                });
                                column.Item().AlignLeft().Text(txt =>
                                {
                                    txt.Span("Total Entregado (MU): ").SemiBold();
                                    txt.Span(ayuda.ListaItems.Where(li => li.ItemLista.Unidad == "MU").Sum(li => li.CantidadEntregada).ToString("N0"));
                                });
                                column.Item().AlignLeft().Text(txt =>
                                {
                                    txt.Span("Total Entregado (EU): ").SemiBold();
                                    txt.Span(ayuda.ListaItems.Where(li => li.ItemLista.Unidad == "EU").Sum(li => li.CantidadEntregada).ToString("N0"));
                                });
                                column.Spacing(5);
                                decimal totalE = ayuda.ListaItems.Sum(li => li.CantidadEntregada);
                                decimal totalS = ayuda.ListaItems.Sum(li => li.CantidadSolicitada);
                                decimal porcentaje = totalE > 0 ? (totalE / totalS) * 100 : 0;
                                column.Item().AlignRight().Text(txt => 
                                {
                                    txt.Span("Completado: ").SemiBold().FontSize(12);
                                    txt.Span(porcentaje.ToString("N2") + '%').Italic().FontSize(12);
                                });
                            });

                        #endregion

                    });

                    #region footer
                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Página ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                    #endregion
                });
            }).GeneratePdf();

            return DetallePDF;
        }
        public static byte[] GeneratePDFreporte(AyudaQuery filtro, List<AyudaDTO> ayudas)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var ReportePDF =
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    #region Encabezado
                    page.Margin(60);
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.AutoItem().Height(60).Image(LoadImageAsByteArray("C:\\Users\\alexh\\source\\repos\\SistemaGSwebLocal\\SistemaGS.WebAssembly\\wwwroot\\img\\FundaciónGSletrasnegras.png"));

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("REPÚBLICA BOLIVARIANA DE VENEZUELA").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("FUNDACIÓN GESTIÓN SOCIAL").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("MUNICIPIO TURÍSTICO EL MORRO LCDO. DIEGO BAUTISTA URBANEJA").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("LECHERÍA ESTADO ANZOÁTEGUI").Bold().FontSize(9);
                            col.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("REPORTE DE AYUDAS ENTREGADAS").Bold().FontSize(9);
                            col.Item().AlignCenter().Text(" ").Bold().FontSize(9);
                        });

                        row.AutoItem().Height(60).Image(LoadImageAsByteArray("C:\\Users\\alexh\\source\\repos\\SistemaGSwebLocal\\SistemaGS.WebAssembly\\wwwroot\\img\\LogoAlcaldiaLetrasAzules.png"));
                    });
                    #endregion

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        var Items = ayudas
                            .SelectMany(a => a.ListaItems).GroupBy(li => li.ItemLista.IdItem)
                            .Select(g => new
                            {
                                IdItem = g.Key,
                                Nombre = g.First().ItemLista.Nombre,
                                Categoria = g.First().ItemLista.Categoria,
                                Unidad = g.First().ItemLista.Unidad,
                                CantidadSolicitada = g.Sum(x => x.CantidadSolicitada),
                                CantidadEntregada = g.Sum(x => x.CantidadEntregada)
                            })
                            .ToList();

                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("Resumen reporte:").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Fecha de inicio: ").SemiBold().FontSize(10);
                                txt.Span(filtro.FechaIni!.Value.ToString("dd/MM/yyyy")).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Fecha de finalización: ").SemiBold().FontSize(10);
                                txt.Span(filtro.FechaFin!.Value.ToString("dd/MM/yyyy")).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Total ayudas entregadas: ").SemiBold().FontSize(10);
                                txt.Span(ayudas.Count().ToString("N0")).FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Total ítems entregados: ").SemiBold().FontSize(10);
                                txt.Span(Items.Count().ToString("N0")).FontSize(10);
                            });
                        });

                        #region Ayudas

                        col1.Spacing(5);
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Text("Resumen ayudas sociales").Bold().FontSize(14).AlignCenter();
                        col1.Item().LineHorizontal(0.5f);
                        col1.Spacing(5);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(0.5f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#003666").AlignMiddle()
                                .Padding(2).Text("ID").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666").AlignMiddle()
                                .Padding(2).Text("Solicitante").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666").AlignMiddle()
                               .Padding(2).Text("Funcionario").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666").AlignMiddle()
                               .Padding(2).Text("Categoría").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666").AlignMiddle()
                               .Padding(2).Text("Estado").FontColor("fff").Bold().AlignCenter();

                                header.Cell().Background("#003666").AlignMiddle()
                               .Padding(2).Text("Fecha Solicitud").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666").AlignMiddle()
                               .Padding(2).Text("Fecha Resolución").Bold().FontColor("#fff").AlignCenter();
                            });
                            
                            Dictionary<string, string> color = new Dictionary<string, string>()
                            {
                                ["Por Aprobar"] = "#198754",
                                ["En Proceso"] = "#0d6efd",
                                ["Lista Para Entregar"] = "#0dcaf0",
                                ["Cerrada"] = "#333",
                                ["Rechazada"] = "#dc3545"
                            };

                            foreach (var ayuda in ayudas)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.IdAyuda.ToString()).FontSize(9);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.Solicitante.ToString("N0", new CultureInfo("es-ES"))).FontSize(9);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.Funcionario.ToString("N0", new CultureInfo("es-ES"))).FontSize(9);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.Categoria).FontSize(9).AlignCenter();

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.Estado).Bold().FontSize(9).AlignCenter();

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.FechaSolicitud.ToString("dd/MM/yyyy")).FontSize(9).AlignCenter();

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(ayuda.FechaEntrega.HasValue ? ayuda.FechaEntrega.Value.ToString("dd/MM/yyyy") : "N/A").FontSize(9).AlignCenter();
                            }
                        });

                        #endregion

                        col1.Item().PageBreak();

                        #region Items

                        col1.Spacing(5);
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Text("Resumen ítems").Bold().FontSize(14).AlignCenter();
                        col1.Item().LineHorizontal(0.5f);
                        col1.Spacing(5);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#003666")
                                .Padding(2).Text("ID ítem").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                                .Padding(2).Text("Nombre").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                               .Padding(2).Text("Categoría").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                               .Padding(2).Text("Cantidad Solicitada").Bold().FontColor("#fff").AlignCenter();

                                header.Cell().Background("#003666")
                               .Padding(2).Text("Cantidad de Entregada").Bold().FontColor("#fff").AlignCenter();
                            });

                            foreach (var item in Items)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.IdItem.ToString()).FontSize(9);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.Nombre).FontSize(9);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.Categoria).FontSize(9);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.Unidad == "VE" ? item.CantidadSolicitada.ToString("N2") : item.CantidadSolicitada.ToString("N0")).FontSize(9).AlignCenter();

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(item.Unidad == "VE" ? item.CantidadEntregada.ToString("N2") : item.CantidadEntregada.ToString("N0")).FontSize(9).AlignCenter();

                            }
                        });

                        #endregion

                        col1.Item().PageBreak();

                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                        .Column(column =>
                        {
                            column.Item().Text("Métricas").FontSize(14).Bold().Italic().Underline();
                            column.Item().AlignLeft().Text($"Total Solicitado (VE): {Items.Where(li => li.Unidad == "VE").Sum(li => li.CantidadSolicitada)}").FontSize(12);
                            column.Item().AlignLeft().Text($"Total Solicitado (MU): {Items.Where(li => li.Unidad == "MU").Sum(li => li.CantidadSolicitada)}").FontSize(12);
                            column.Item().AlignLeft().Text($"Total Solicitado (EU): {Items.Where(li => li.Unidad == "EU").Sum(li => li.CantidadSolicitada)}").FontSize(12);
                            column.Spacing(5);
                            column.Item().AlignLeft().Text($"Total Entregado (VE): {Items.Where(li => li.Unidad == "VE").Sum(li => li.CantidadEntregada)}").FontSize(12);
                            column.Item().AlignLeft().Text($"Total Entregado (MU): {Items.Where(li => li.Unidad == "MU").Sum(li => li.CantidadEntregada)}").FontSize(12);
                            column.Item().AlignLeft().Text($"Total Entregado (EU): {Items.Where(li => li.Unidad == "EU").Sum(li => li.CantidadEntregada)}").FontSize(12);
                            column.Spacing(5);

                            var itemMS = Items.Where(li => li.CantidadSolicitada == Items.Max(li => li.CantidadSolicitada)).ToList();
                            column.Item().AlignLeft().Text($"Item(s) más solicitado(s)").Italic();
                            foreach (var ms in itemMS)
                            {
                                column.Item().Text(txt =>
                                {
                                    txt.Span("Item más solicitado: ").SemiBold(); txt.Line(ms.Nombre);
                                    string x = ms.Unidad == "VE" ? ms.CantidadSolicitada.ToString("N2") : ms.CantidadSolicitada.ToString("N2");
                                    txt.Span("Cantidad solicitada: ").SemiBold(); txt.Line(x);
                                    txt.Line("");
                                });
                            }
                            column.Spacing(5);
                            decimal totalE = Items.Sum(li => li.CantidadEntregada);
                            decimal totalS = Items.Sum(li => li.CantidadSolicitada);
                            decimal porcentaje = totalE > 0 ? (totalE / totalS) * 100 : 0;
                            column.Item().AlignRight().Text($"Cumplimiento total período: %{porcentaje.ToString("N2")}");                          
                            
                        });
                    });

                    #region footer
                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Página ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                    #endregion
                });
            }).GeneratePdf();
            return ReportePDF;
        }
    }
}