using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Data.Common;

namespace UkrStanko.Models
{
    public static class DbErrorsInterpreter
    {
        // Перевод сообщений об ошибке из БД
        public static string GetDbUpdateExceptionMessage(DbUpdateException ex)
        {
            var sqlex = ex.InnerException as DbException;

            if(sqlex != null)
            {
                if (sqlex.Message.Contains("Cannot insert duplicate key row")) return "Аналогичная запись уже содержится в базе!";
            } // if

            return "";
        } // GetDbUpdateExceptionMessage
    }
}