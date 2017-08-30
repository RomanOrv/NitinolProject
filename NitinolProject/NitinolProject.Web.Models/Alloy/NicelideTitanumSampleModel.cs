using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NitinolProject.Web.Models.Alloy
{
   public class NicelideTitanumSampleModel
    {
        public int NicelideTitanumSampleId { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Назва сплаву")]
        public string NickelideTitaniumAlloyName { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Номер зразку")]
        public int? SampleNumber { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Товщина зразку, мм")]
        public decimal? SampleThickness { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Товщина ударника, мм")]
        public decimal? HammerThickness { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість ударника, м/с")]
        public int? HammerSpeed { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість відколу, м/с")]
        public int? SpallSpeed { get; set; }

        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Міцність відколу, ГПа")]
        public decimal? SpallStrength { get; set; }
    }
}
