using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UkrStanko.Models.App
{
    public class Message
    {
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Ид пользователя")]
        public string UserId { get; set; }

        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Display(Name = "Текст")]
        public string Text { get; set; }

        [Display(Name = "Индикатор ответа")]
        public bool Response { get; set; }

        [Display(Name = "Дата")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Заявка в ответе")]
        public Requisition ResponseRequisition { get; set; }

        [Display(Name = "Предложение в ответе")]
        public Proposal ResponseProposal { get; set; }

        [Display(Name = "Сообщение в ответе")]
        public Message ResponseMessage { get; set; }

        // Вспомогательные поля
        [NotMapped]
        public string UserImagePath { get; set; }
    }
}