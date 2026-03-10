namespace _03075.Proyecto_1.SusanMurillo.Models
{
    public class Empleados
    {

        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public double SalarioXDia { get; set; }
        public int VacacionesAcumuladas { get; set; }
        public DateTime FechadeRetiro { get; set; }
        public decimal MontoLiquidacin { get; set; }

        public decimal MontoLiquidacion
        {
            get
            {
                if (FechadeRetiro == DateTime.MinValue || FechadeRetiro.Year < 1900)
                    return 0m;

                decimal montoLiquidacion = 0m;

                int diasDelMesActual = FechadeRetiro.Day;

                // Convierte SalarioXDia (float) a decimal antes de multiplicar
                decimal salarioXDiaDecimal = (decimal)SalarioXDia;

                decimal salarioMesProporcional = diasDelMesActual * salarioXDiaDecimal;

                decimal pagoVacaciones = VacacionesAcumuladas * salarioXDiaDecimal;

                DateTime fechaInicioAguinaldo;
                DateTime fechaFinAguinaldo;

                if (FechadeRetiro.Month == 12)
                {
                    fechaInicioAguinaldo = new DateTime(FechadeRetiro.Year, 12, 1);
                    fechaFinAguinaldo = new DateTime(FechadeRetiro.Year + 1, 11, 30);
                }
                else
                {
                    fechaInicioAguinaldo = new DateTime(FechadeRetiro.Year - 1, 12, 1);
                    fechaFinAguinaldo = new DateTime(FechadeRetiro.Year, 11, 30);
                }

                DateTime fechaFinCalculo = FechadeRetiro < fechaFinAguinaldo ? FechadeRetiro : fechaFinAguinaldo;

                decimal diasPeriodoAguinaldo = (decimal)(fechaFinCalculo - fechaInicioAguinaldo).TotalDays;

                decimal aguinaldoCompleto = salarioXDiaDecimal * 30;
                decimal aguinaldoProporcional = (diasPeriodoAguinaldo / 365m) * aguinaldoCompleto;

                montoLiquidacion = salarioMesProporcional + pagoVacaciones + aguinaldoProporcional;

                return Math.Round(montoLiquidacion, 0);
            }
        }
    }
}
