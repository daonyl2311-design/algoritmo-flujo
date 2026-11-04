using System;
using System.Globalization;
using System.Text;

namespace AhorroSeguro
{
    internal class Program
    {
        // Nota: Main se declara sin "string[] args" porque el proyecto no permite arreglos.
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo cultura = CultureInfo.InvariantCulture; // punto decimal y coma de miles: 10,000.00
            CultureInfo.CurrentCulture = cultura;

            // ================= DATOS DEL EJERCICIO 1 =================
            // Se conservan mientras el programa esté abierto (saldo y bloqueo de cuenta).
            const string PIN_CORRECTO = "2580";
            const int MAX_INTENTOS = 3;
            decimal saldo = 10000.00m;
            bool cuentaBloqueada = false;

            int opcionPrincipal = 0;

            // ================= MENÚ PRINCIPAL (do-while) =================
            do
            {
                Console.WriteLine();
                Console.WriteLine("==========================================");
                Console.WriteLine("       COOPERATIVA \"AHORRO SEGURO\"");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Ejercicio 1: Cajero automático");
                Console.WriteLine("2. Ejercicio 2: Simulador de préstamo");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcionPrincipal))
                {
                    opcionPrincipal = 0; // fuerza la opción inválida (default)
                }

                switch (opcionPrincipal)
                {
                    // =====================================================
                    // EJERCICIO 1: CAJERO AUTOMÁTICO
                    // =====================================================
                    case 1:
                    {
                        Console.WriteLine();
                        Console.WriteLine("----------- CAJERO AUTOMÁTICO -----------");

                        if (cuentaBloqueada)
                        {
                            Console.WriteLine("Su cuenta está BLOQUEADA por exceder los intentos de PIN.");
                            Console.WriteLine("Comuníquese con la cooperativa para desbloquearla.");
                        }
                        else
                        {
                            // ---------- Validación del PIN (while) ----------
                            int intentos = 0;
                            bool accesoConcedido = false;

                            while (intentos < MAX_INTENTOS && !accesoConcedido)
                            {
                                Console.Write("Ingrese su PIN de 4 dígitos: ");
                                string pin = Console.ReadLine();
                                int pinNumerico;

                                if (pin == null || pin.Length != 4 ||
                                    !int.TryParse(pin, NumberStyles.None, cultura, out pinNumerico))
                                {
                                    intentos++;
                                    Console.WriteLine("Error: el PIN debe tener exactamente 4 dígitos numéricos. " +
                                                      "Intentos restantes: " + (MAX_INTENTOS - intentos));
                                }
                                else if (pin == PIN_CORRECTO)
                                {
                                    accesoConcedido = true;
                                }
                                else
                                {
                                    intentos++;
                                    Console.WriteLine("PIN incorrecto. Intentos restantes: " + (MAX_INTENTOS - intentos));
                                }
                            }

                            if (!accesoConcedido)
                            {
                                cuentaBloqueada = true;
                                Console.WriteLine("Ha agotado los 3 intentos. Su cuenta ha sido BLOQUEADA.");
                            }
                            else
                            {
                                Console.WriteLine("PIN correcto. ¡Bienvenido(a)!");

                                int cantidadDepositos = 0;
                                int cantidadRetiros = 0;
                                int opcionCajero = 0;

                                // ---------- Menú del cajero (do-while) ----------
                                do
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("1. Consultar saldo");
                                    Console.WriteLine("2. Depositar");
                                    Console.WriteLine("3. Retirar");
                                    Console.WriteLine("4. Salir");
                                    Console.Write("Seleccione una opción: ");

                                    if (!int.TryParse(Console.ReadLine(), out opcionCajero))
                                    {
                                        opcionCajero = 0;
                                    }

                                    switch (opcionCajero)
                                    {
                                        case 1:
                                            Console.WriteLine("Su saldo actual es: RD$ " + saldo.ToString("N2"));
                                            break;

                                        case 2:
                                        {
                                            Console.Write("Monto a depositar: RD$ ");
                                            decimal deposito;

                                            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out deposito))
                                            {
                                                Console.WriteLine("Error: debe ingresar un monto numérico válido.");
                                            }
                                            else if (deposito <= 0)
                                            {
                                                Console.WriteLine("Error: el monto a depositar debe ser mayor que cero.");
                                            }
                                            else if (deposito != Math.Round(deposito, 2))
                                            {
                                                Console.WriteLine("Error: el monto no puede tener más de 2 decimales.");
                                            }
                                            else
                                            {
                                                saldo += deposito;
                                                cantidadDepositos++;
                                                Console.WriteLine("Depósito exitoso. Nuevo saldo: RD$ " + saldo.ToString("N2"));
                                            }
                                            break;
                                        }

                                        case 3:
                                        {
                                            Console.Write("Monto a retirar (múltiplo de RD$ 100): RD$ ");
                                            decimal retiro;

                                            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out retiro))
                                            {
                                                Console.WriteLine("Error: debe ingresar un monto numérico válido.");
                                            }
                                            else if (retiro <= 0)
                                            {
                                                Console.WriteLine("Error: el monto a retirar debe ser mayor que cero.");
                                            }
                                            else if (retiro > saldo)
                                            {
                                                Console.WriteLine("Error: fondos insuficientes. Su saldo es RD$ " + saldo.ToString("N2"));
                                            }
                                            else if (retiro % 100 != 0)
                                            {
                                                Console.WriteLine("Error: el monto debe ser múltiplo de RD$ 100.");
                                            }
                                            else
                                            {
                                                saldo -= retiro;
                                                cantidadRetiros++;
                                                Console.WriteLine("Retiro exitoso. Nuevo saldo: RD$ " + saldo.ToString("N2"));
                                            }
                                            break;
                                        }

                                        case 4:
                                            Console.WriteLine();
                                            Console.WriteLine("----------- RESUMEN DE LA SESIÓN -----------");
                                            Console.WriteLine("Cantidad de depósitos: " + cantidadDepositos);
                                            Console.WriteLine("Cantidad de retiros:   " + cantidadRetiros);
                                            Console.WriteLine("Saldo final:           RD$ " + saldo.ToString("N2"));
                                            Console.WriteLine("Gracias por usar el cajero.");
                                            break;

                                        default:
                                            Console.WriteLine("Opción inválida. Debe elegir entre 1 y 4.");
                                            break;
                                    }
                                } while (opcionCajero != 4);
                            }
                        }
                        break;
                    }

                    // =====================================================
                    // EJERCICIO 2: SIMULADOR DE PRÉSTAMO
                    // =====================================================
                    case 2:
                    {
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
                        break;
                    }

                    case 3:
                        Console.WriteLine("Gracias por usar el sistema de la cooperativa \"Ahorro Seguro\". ¡Hasta luego!");
                        break;

                    default:
                        Console.WriteLine("Opción inválida. Debe elegir 1, 2 o 3.");
                        break;
                }

            } while (opcionPrincipal != 3);
        }
    }
}
