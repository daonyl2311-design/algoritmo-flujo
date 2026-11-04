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

            // ================= EJERCICIO 1: CAJERO AUTOMÁTICO =================
            const string PIN_CORRECTO = "2580";
            const int MAX_INTENTOS = 3;
            decimal saldo = 10000.00m;
            bool cuentaBloqueada = false;

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
        }
    }
}
