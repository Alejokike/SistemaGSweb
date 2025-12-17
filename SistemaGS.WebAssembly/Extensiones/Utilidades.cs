using Microsoft.AspNetCore.Components;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using SistemaGS.DTO.ModelDTO;
using System.ComponentModel.DataAnnotations;
//using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SistemaGS.WebAssembly.Extensiones
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
    public static class Utilidades
    {
        public static int CalcularDistanciaLevenshtein(string fuente, string destino)
        {
            if (string.IsNullOrEmpty(fuente))
                return string.IsNullOrEmpty(destino) ? 0 : destino.Length;

            if (string.IsNullOrEmpty(destino))
                return fuente.Length;

            int[,] matriz = new int[fuente.Length + 1, destino.Length + 1];

            for (int i = 0; i <= fuente.Length; i++)
                matriz[i, 0] = i;

            for (int j = 0; j <= destino.Length; j++)
                matriz[0, j] = j;

            for (int i = 1; i <= fuente.Length; i++)
            {
                for (int j = 1; j <= destino.Length; j++)
                {
                    int costo = (fuente[i - 1] == destino[j - 1]) ? 0 : 1;

                    matriz[i, j] = Math.Min(
                        Math.Min(matriz[i - 1, j] + 1,
                                 matriz[i, j - 1] + 1),
                        matriz[i - 1, j - 1] + costo
                    );
                }
            }

            return matriz[fuente.Length, destino.Length];
        }
        public static List<ValidationResult> ValidateRecursive(object obj, string prefix = "")
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(obj);
            Validator.TryValidateObject(obj, context, results, validateAllProperties: true);

            var adjustedResults = new List<ValidationResult>();
            foreach (var result in results)
            {
                if (result.MemberNames.Any())
                {
                    var adjustedMembers = result.MemberNames
                        .Select(m => string.IsNullOrEmpty(prefix) ? m : $"{prefix}.{m}");
                    adjustedResults.Add(new ValidationResult(result.ErrorMessage, adjustedMembers));
                }
                else
                {
                    adjustedResults.Add(result);
                }
            }

            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value != null && !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string) && !prop.PropertyType.IsEnum && prop.PropertyType != typeof(DateTime))
                {
                    adjustedResults.AddRange(ValidateRecursive(value, string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}"));
                }
            }
            return adjustedResults;
        }
        public static decimal ValidateNumeric(ChangeEventArgs e, string unidad)
        {
            var valor = e.Value?.ToString() ?? "";

            if (unidad != "VE")
            {
                valor = Regex.Replace(valor, @"[^0-9]", "");
            }
            else
            {
                valor = Regex.Replace(valor, @"[^0-9,]", "");
                valor = Regex.Replace(valor, @"(,.*?,)", ",");
                valor = Regex.Replace(valor, @"(,\d{0,2}).*", "$1");
            }

            decimal.TryParse(valor, out decimal resultado);

            return resultado;
        }
        public static string FormatearValor(object valor)
        {
            if (valor is null) return "N/A";

            return valor switch
            {
                DateTime fecha => fecha.ToString("dd/MM/yyyy"),
                decimal d => d.ToString("N2"),
                bool b => b ? "Sí" : "No",
                _ => valor?.ToString() ?? ""
            };
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
            else if(FechaSolicitud.Month == FechaNacimiento.Month && FechaSolicitud.Day < FechaNacimiento.Day) edad--;
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
                                tabla.Cell().RowSpan(2).Column(2).Border(1.3f).AlignMiddle().Text($"{Solicitante.Cedula}").FontSize(9).AlignCenter();
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
                                tabla.Cell().RowSpan(2).Column(2).Border(1.3f).AlignMiddle().Text($"{Solicitante.Cedula}").FontSize(9).AlignCenter();
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
                                        inner.Cell().Column(1).Border(1.3f).AlignMiddle().Text($"{Funcionario.Nombre} {Funcionario.Apellido}").FontSize(9).Justify().AlignCenter();
                                        inner.Cell().Column(2).Border(1.3f).AlignMiddle().Text($"{Funcionario.Cedula}").FontSize(9).Justify().AlignCenter();
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
    }
}
