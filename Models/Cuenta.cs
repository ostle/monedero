using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monedero.Models
{
    public class Cuenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CuentaId { get; set; }
        public double Saldo { get; set; }
        public List<Movimiento> Movimiento { get; set; }
    }
}
