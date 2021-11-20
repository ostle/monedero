using System.ComponentModel.DataAnnotations;

namespace Monedero.Models
{
    public enum TipoMovimiento
    {
        [Display(Name = "Ingreso")]
        Ingreso=1,
        [Display(Name = "Egreso")]
        Egreso=2,
        /* [Display(Name = "Transferencia")]
        Transferencia=3 */
    }    
}
