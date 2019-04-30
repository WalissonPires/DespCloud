using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entities
{
    public enum OrderStatus
    {
        [Display(Name = "Aberta")]
        Opened   = 0,
        [Display(Name = "Fechada")]
        Closed   = 1,
        [Display(Name = "Cancelada")]
        Canceled = 2
    }
}