using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using UkrStanko.Models;
using UkrStanko.Models.App;

namespace UkrStanko.ViewModels.Notices
{
    public class IndexViewModel
    {
        public List<Notice> Notices { get; set; } // список новостей
        public int ReadedNoticesCount { get; set; } // кол-во прочитанных пользователем новостей

        public int AllNoticesCount { get; set; } // кол-во всех новостей

        public int DownloadedCount { get; set; } // кол-во загруженных новостей

        // Конструктор
        public IndexViewModel() { } // IndexViewModel
        public IndexViewModel(List<Proposal> proposals, List<Requisition> requisitions, List<Message> messages, int readedNoticesCount, int downloadedCount = 0)
        {
            Notices = new List<Notice>();

            int maxLength = requisitions.Count > proposals.Count ?
                requisitions.Count > messages.Count ? requisitions.Count : messages.Count : 
                proposals.Count > messages.Count ? proposals.Count : messages.Count;

            for (int i = 0; i < maxLength; i++)
            {
                if (requisitions.Count >= i + 1) Notices.Add(new Notice { Date = requisitions[i].CreateDate, Requisition = requisitions[i] });
                if (proposals.Count >= i + 1) Notices.Add(new Notice { Date = proposals[i].CreateDate, Proposal = proposals[i] });
                if (messages.Count >= i + 1) Notices.Add(new Notice { Date = messages[i].CreateDate, Message = messages[i] });
            } // foreach

            AllNoticesCount = Notices.Count;
            if (downloadedCount != -1)
            {
                Notices = Notices
                    .OrderBy(i => i.Date)
                    .SkipLast(downloadedCount)
                    .TakeLast(downloadedCount == 0 ? (Notices.Count - readedNoticesCount > 50 ? Notices.Count - readedNoticesCount : 50) : 50)
                    .ToList();
            } else
            {
                Notices = Notices
                    .OrderBy(i => i.Date)
                    .ToList();
            } // if

            DownloadedCount = downloadedCount + Notices.Count;
            ReadedNoticesCount = readedNoticesCount;
        } // IndexViewModel
    }
}
