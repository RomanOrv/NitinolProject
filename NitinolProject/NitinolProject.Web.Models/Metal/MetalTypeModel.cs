using System.ComponentModel.DataAnnotations;

namespace NitinolProject.Web.Models.Metal
{
    public class MetalTypeModel
    {
        public int MetalTypeId { get; set; }
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Тип металу")]
        public string MetalName { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість навантаження V, м/с")]
        public int LoadingSpeed { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Поздовжня швидкість зсуву VL, м/с")]
        public int LongitudinalShearRate { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Поперечна швидкість зсуву Vt, м/с")]
        public int LateralShearRate { get; set; }

        public decimal ShearStrainRate { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Міцність відколу σ, ГПа")]
        public int SpallStrength { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість навантаження")]
        public decimal LoadingSpeedC { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Поздовжня швидкість зсуву")]
        public decimal LongitudinalShearRateC { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Поперечна швидкість зсуву")]
        public decimal LateralShearRateC { get; set; }
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість деформації зсуву")]
        public decimal ShearStrainRateC { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Міцність відколу")]
        public decimal SpallStrengthC { get; set; }
    }
}