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

        //[DisplayName("Назва сплаву")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Назва сплаву")]
        public string NickelideTitaniumAlloyName { get; set; }

        //[DisplayName("Номер зразку")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Номер зразку")]
        public int? SampleNumber { get; set; }

        //[DisplayName("Товщина зразку, мм")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Товщина зразку, мм")]
        public decimal? SampleThickness { get; set; }

        //[DisplayName("Товщина ударника, мм")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Товщина ударника, мм")]
        public decimal? HammerThickness { get; set; }

        //[DisplayName("Швидкість ударника, м/с")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість ударника, м/с")]
        public int? HammerSpeed { get; set; }

        //[DisplayName("Швидкість відколу, м/с")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Швидкість відколу, м/с")]
        public int? SpallSpeed { get; set; }

        //[DisplayName("Міцність відколу, ГПа")]
        [Required(ErrorMessage = "Це поле обов'язкове"), Display(Name = "Міцність відколу, ГПа")]
        public decimal? SpallStrength { get; set; }
    }
}
