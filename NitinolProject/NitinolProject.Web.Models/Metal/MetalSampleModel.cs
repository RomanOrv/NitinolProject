using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitinolProject.Web.Models.Metal
{
    public class MetalSampleModel
    {
        public int MetalSampleId { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Назва зразку металу")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Номер зразку")]
        public int? SampleNumber { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Назва типу металу")]
        public int? MetalId { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Кристалічна решітка")]
        public int? CrystalLatticeId { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість навантаження, м/с")]
        public int? LoadingSpeed { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Поперечна швидкість зсуву, м/с")]
        public int? LateralShearRate { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Поздовжня швидкість зсуву, м/с")]
        public int? LongitudinalShearRate { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість деформації зсуву, 10<sup>-6</sup> c^(-1)")]
        public decimal? ShearStrainRate { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Міцність відколу, MПа")]
        public decimal? SpallStrength { get; set; }
    }
}
