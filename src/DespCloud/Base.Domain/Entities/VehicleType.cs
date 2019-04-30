using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entities
{
    public enum VehicleType
    {
        [Display(Name = "Moto")]
        MotorCycle = 0,
        [Display(Name = "Carro")]
        Car        = 1
    }
}