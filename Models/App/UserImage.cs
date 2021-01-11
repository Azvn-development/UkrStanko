using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UkrStanko.Models.App
{
    public class UserImage
    {
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Путь на сервере")]
        [StringLength(400)]
        public string Path { get; set; }

        [Display(Name = "Имя файла")]
        [StringLength(400)]
        public string Name { get; set; }

        [Display(Name = "Пользователь")]
        public string UserName { get; set; }
    }
}