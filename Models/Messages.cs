using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkrStanko.Models
{
    public static class Messages
    {
        // Сообщения об ошибках валидации
        public const string requiredErrorMessage = "Поле обязательно для заполнения!";
        public const string emailErrorMessage = "Введите корректный e-mail адрес!";
        public const string passwordErrorMessage = "Пароли не совпадают!";
        public const string rangeErrorMessage = "Допускаются значения не менее 0!";
        public const string priceErrorMessage = "В поле цены допускается ввод целых чисел!";
        public const string phoneMinLengthErrorMessage = "Минимальная длина номера телефона 7 символов!";

        // Сообщения об ошибках взаимодействия с БД
        public const string downloadErrorMessage = "Ошибка загрузки данных!";
        public const string downloadFilesErrorMessage = "Ошибка загрузки файлов!";
        public const string createErrorMessage = "Ошибка добавления данных!";
        public const string updateErrorMessage = "Ошибка обновления данных!";
        public const string deleteErrorMessage = "Ошибка удаления данных!";

        //Сообщения об успешных операциях взаимодействия с БД
        public const string successCreateMessage = "Запись успешно добавлена!";
        public const string successUpdateMessage = "Запись успешно обновлена!";
        public const string successDeleteMessage = "Запись успешно удалена!";
    }
}