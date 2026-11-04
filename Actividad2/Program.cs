using System;
using System.Globalization;
using System.Text;

namespace AhorroSeguro
{
    internal class Program
    {
        // Main sin "string[] args" porque el proyecto no permite arreglos.
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo cultura = CultureInfo.InvariantCulture;
            CultureInfo.CurrentCulture = cultura;

            // ================= EJERCICIO 2: SIMULADOR DE PRÉSTAMO =================
            string respuesta;

            do
            {
                Console.WriteLine();
                Console.WriteLine("---------- SIMULADOR DE PRÉSTAMO ----------");

                // ---------- Monto (do-while hasta que sea válido) ----------
                decimal monto = 0;
                bool montoValido = false;
                do
                {
                    Console.Write("Monto del préstamo (RD$ 5,000 a 500,000): RD$ ");
                    if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out monto))
                    {
                        Console.WriteLine("Error: debe ingresar un monto numérico válido.");
                    }
                    else if (monto < 5000 || monto > 500000)
                    {
                        Console.WriteLine("Error: el monto debe estar entre RD$ 5,000 y RD$ 500,000.");
                    }
                    else
                    {
                        montoValido = true;
                    }
                } while (!montoValido);

                // ---------- Plazo (while hasta que sea válido) ----------
                int plazo = 0;
                bool plazoValido = false;
                while (!plazoValido)
                {
                    Console.Write("Plazo en meses (6 a 60): ");
                    if (!int.TryParse(Console.ReadLine(), out plazo))
                    {
                        Console.WriteLine("Error: debe ingresar un número entero de meses.");
                    }
                    else if (plazo < 6 || plazo > 60)
                    {
                        Console.WriteLine("Error: el plazo debe estar entre 6 y 60 meses.");
                    }
                    else
                    {
                        plazoValido = true;
                    }
                }

                // ---------- Categoría (switch para la tasa) ----------
                char categoria = ' ';
                double tasaAnual = 0;
                bool categoriaValida = false;
                while (!categoriaValida)
                {
                    Console.Write("Categoría del socio (A, B o C): ");
                    string entradaCategoria = Console.ReadLine();
                    if (entradaCategoria == null ||
                        !char.TryParse(entradaCategoria.Trim().ToUpper(), out categoria))
                    {
                        Console.WriteLine("Error: debe ingresar una sola letra (A, B o C).");
                    }
                    else
                    {
                        switch (categoria)
                        {
                            case 'A':
                                tasaAnual = 0.12;
                                categoriaValida = true;
                                break;
                            case 'B':
                                tasaAnual = 0.15;
                                categoriaValida = true;
                                break;
                            case 'C':
                                tasaAnual = 0.18;
                                categoriaValida = true;
                                break;
                            default:
                                Console.WriteLine("Error: la categoría debe ser A, B o C.");
                                break;
                        }
                    }
                }

                // ---------- Cálculo de la cuota fija ----------
                // Cuota = Monto * [ i(1+i)^n ] / [ (1+i)^n - 1 ]
                double i = tasaAnual / 12;
                double potencia = Math.Pow(1 + i, plazo);
                decimal cuota = Math.Round(monto * (decimal)(i * potencia / (potencia - 1)), 2);
                decimal tasaMensual = (decimal)i;

                Console.WriteLine();
                Console.WriteLine("Monto:        RD$ " + monto.ToString("N2"));
                Console.WriteLine("Plazo:        " + plazo + " meses");
                Console.WriteLine("Categoría:    " + categoria + " (tasa anual " + (tasaAnual * 100) + " %)");
                Console.WriteLine("Cuota fija:   RD$ " + cuota.ToString("N2"));

                // ---------- Tabla de amortización (for) ----------
                Console.WriteLine();
                Console.WriteLine(string.Format("{0,4} {1,15} {2,15} {3,15} {4,15}",
                    "Mes", "Cuota", "Interés", "Abono capital", "Saldo"));
                Console.WriteLine(new string('-', 68));

                decimal saldoPendiente = monto;
                decimal totalPagado = 0;
                decimal totalIntereses = 0;
                decimal totalCapital = 0;

                for (int mes = 1; mes <= plazo; mes++)
                {
                    decimal interes = Math.Round(saldoPendiente * tasaMensual, 2);
                    decimal abono = cuota - interes;
                    decimal cuotaDelMes = cuota;

                    // En el último mes se ajustan los centavos del redondeo para que el saldo quede en 0
                    if (mes == plazo)
                    {
                        abono = saldoPendiente;
                        cuotaDelMes = abono + interes;
                    }

                    saldoPendiente -= abono;
                    totalPagado += cuotaDelMes;
                    totalIntereses += interes;
                    totalCapital += abono;

                    Console.WriteLine(string.Format("{0,4} {1,15:N2} {2,15:N2} {3,15:N2} {4,15:N2}",
                        mes, cuotaDelMes, interes, abono, saldoPendiente));
                }

                Console.WriteLine(new string('-', 68));
                Console.WriteLine(string.Format("{0,4} {1,15:N2} {2,15:N2} {3,15:N2}",
                    "TOT", totalPagado, totalIntereses, totalCapital));
                Console.WriteLine();
                Console.WriteLine("Total pagado:        RD$ " + totalPagado.ToString("N2"));
                Console.WriteLine("Total de intereses:  RD$ " + totalIntereses.ToString("N2"));
                Console.WriteLine("Total a capital:     RD$ " + totalCapital.ToString("N2"));

                Console.WriteLine();
                Console.Write("¿Desea simular otro préstamo? (S/N): ");
                respuesta = Console.ReadLine();
                if (respuesta == null)
                {
                    respuesta = "N";
                }
                respuesta = respuesta.Trim().ToUpper();

            } while (respuesta == "S");
        }
    }
}
