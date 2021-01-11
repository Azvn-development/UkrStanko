﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace UkrStanko.Models.App.Context
{
    public class AppDbContext: DbContext
    {
        // Конструктор
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        {
            Database.Migrate();
        } // UkrStankoContext

        // Инициализация БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MachineType>().HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<MachineType>().HasData(new List<MachineType>
            {
                new MachineType { Id = 1, Name = "Без группы", ParentMachineTypeId = null },
                new MachineType { Id = 2, Name = "Токарные", ParentMachineTypeId = null },
                new MachineType { Id = 3, Name = "Токарные_1", ParentMachineTypeId = 2 },
                new MachineType { Id = 4, Name = "Токарные_2", ParentMachineTypeId = 2 },
                new MachineType { Id = 5, Name = "Токарные_3", ParentMachineTypeId = 2 },
                new MachineType { Id = 6, Name = "Токарные_4", ParentMachineTypeId = 2 },
                new MachineType { Id = 7, Name = "Токарные_5", ParentMachineTypeId = 2 },
                new MachineType { Id = 8, Name = "Токарные_6", ParentMachineTypeId = 2 },
                new MachineType { Id = 9, Name = "Токарные_7", ParentMachineTypeId = 2 },
                new MachineType { Id = 10, Name = "Фрезерные", ParentMachineTypeId = null },
                new MachineType { Id = 11, Name = "Фрезерные_1", ParentMachineTypeId = 10 },
                new MachineType { Id = 12, Name = "Фрезерные_2", ParentMachineTypeId = 10 },
                new MachineType { Id = 13, Name = "Фрезерные_3", ParentMachineTypeId = 10 },
                new MachineType { Id = 14, Name = "Фрезерные_4", ParentMachineTypeId = 10 },
                new MachineType { Id = 15, Name = "Фрезерные_5", ParentMachineTypeId = 10 },
                new MachineType { Id = 16, Name = "Фрезерные_6", ParentMachineTypeId = 10 },
                new MachineType { Id = 17, Name = "Фрезерные_7", ParentMachineTypeId = 10 },
                new MachineType { Id = 18, Name = "Фрезерные_8", ParentMachineTypeId = 10 },
                new MachineType { Id = 19, Name = "Сверлильные", ParentMachineTypeId = null },
                new MachineType { Id = 20, Name = "Сверлильные_1", ParentMachineTypeId = 19 },
                new MachineType { Id = 21, Name = "Сверлильные_2", ParentMachineTypeId = 19 },
                new MachineType { Id = 22, Name = "Сверлильные_3", ParentMachineTypeId = 19 },
                new MachineType { Id = 23, Name = "Сверлильные_4", ParentMachineTypeId = 19 },
                new MachineType { Id = 24, Name = "ГРС", ParentMachineTypeId = null },
                new MachineType { Id = 25, Name = "ГРС_1", ParentMachineTypeId = 24 },
                new MachineType { Id = 26, Name = "ГРС_2", ParentMachineTypeId = 24 },
                new MachineType { Id = 27, Name = "ГРС_3", ParentMachineTypeId = 24 },
                new MachineType { Id = 28, Name = "Шлифовальные", ParentMachineTypeId = null },
                new MachineType { Id = 29, Name = "Шлифовальные_1", ParentMachineTypeId = 28 },
                new MachineType { Id = 30, Name = "Шлифовальные_2", ParentMachineTypeId = 28 },
                new MachineType { Id = 31, Name = "Шлифовальные_3", ParentMachineTypeId = 28 },
                new MachineType { Id = 32, Name = "Шлифовальные_4", ParentMachineTypeId = 28 },
                new MachineType { Id = 33, Name = "Шлифовальные_5", ParentMachineTypeId = 28 },
                new MachineType { Id = 34, Name = "Долбежные", ParentMachineTypeId = null },
                new MachineType { Id = 35, Name = "Долбежные_1", ParentMachineTypeId = 34 },
                new MachineType { Id = 36, Name = "Долбежные_2", ParentMachineTypeId = 34 },
                new MachineType { Id = 37, Name = "Долбежные_3", ParentMachineTypeId = 34 },
                new MachineType { Id = 38, Name = "Зубофрезерные", ParentMachineTypeId = null },
                new MachineType { Id = 39, Name = "Зубофрезерные_1", ParentMachineTypeId = 38 },
                new MachineType { Id = 40, Name = "Зубофрезерные_2", ParentMachineTypeId = 38 },
                new MachineType { Id = 41, Name = "Зубофрезерные_3", ParentMachineTypeId = 38 },
                new MachineType { Id = 42, Name = "Зубофрезерные_4", ParentMachineTypeId = 38 },
                new MachineType { Id = 43, Name = "Пресса", ParentMachineTypeId = null },
                new MachineType { Id = 44, Name = "Пресса_1", ParentMachineTypeId = 43 },
                new MachineType { Id = 45, Name = "Пресса_2", ParentMachineTypeId = 43 },
                new MachineType { Id = 46, Name = "Пресса_3", ParentMachineTypeId = 43 },
                new MachineType { Id = 47, Name = "Пресса_4", ParentMachineTypeId = 43 },
                new MachineType { Id = 48, Name = "Пресса_5", ParentMachineTypeId = 43 },
                new MachineType { Id = 49, Name = "Пресса_6", ParentMachineTypeId = 43 },
                new MachineType { Id = 50, Name = "Пресса_7", ParentMachineTypeId = 43 },
                new MachineType { Id = 51, Name = "Пресса_8", ParentMachineTypeId = 43 },
                new MachineType { Id = 52, Name = "Пресса_9", ParentMachineTypeId = 43 },
                new MachineType { Id = 53, Name = "Пресса_10", ParentMachineTypeId = 43 },
                new MachineType { Id = 54, Name = "Вальцы", ParentMachineTypeId = null },
                new MachineType { Id = 55, Name = "Вальцы_1", ParentMachineTypeId = 54 },
                new MachineType { Id = 56, Name = "Вальцы_2", ParentMachineTypeId = 54 },
                new MachineType { Id = 57, Name = "Листогибы", ParentMachineTypeId = null },
                new MachineType { Id = 58, Name = "Листогибы_1", ParentMachineTypeId = 57 },
                new MachineType { Id = 59, Name = "Листогибы_2", ParentMachineTypeId = 57 }
            });

            modelBuilder.Entity<Machine>().HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<Machine>().HasData(new List<Machine>
            {
                new Machine { Id = 1, Name = "16К20", MachineTypeId = 3 },
                new Machine { Id = 2, Name = "1К62", MachineTypeId = 3 },
                new Machine { Id = 3, Name = "КА280", MachineTypeId = 3 },
                new Machine { Id = 4, Name = "ТС70", MachineTypeId = 3 },
                new Machine { Id = 5, Name = "ТС75", MachineTypeId = 3 },
                new Machine { Id = 6, Name = "ТС25", MachineTypeId = 3 },
                new Machine { Id = 7, Name = "ТС20", MachineTypeId = 3 },
                new Machine { Id = 8, Name = "ДИП200", MachineTypeId = 3 },
                new Machine { Id = 9, Name = "1К62Д", MachineTypeId = 3 },
                new Machine { Id = 10, Name = "ИТ1М", MachineTypeId = 3 },
                new Machine { Id = 11, Name = "ЛТ10", MachineTypeId = 3 },
                new Machine { Id = 12, Name = "16К25", MachineTypeId = 3 },
                new Machine { Id = 13, Name = "16Д20", MachineTypeId = 3 },
                new Machine { Id = 14, Name = "16Д25", MachineTypeId = 3 },
                new Machine { Id = 15, Name = "ФТ11", MachineTypeId = 3 },
                new Machine { Id = 16, Name = "16Е20", MachineTypeId = 3 },
                new Machine { Id = 17, Name = "МК6056", MachineTypeId = 3 },
                new Machine { Id = 18, Name = "1К625", MachineTypeId = 3 },
                new Machine { Id = 19, Name = "ЛТ11", MachineTypeId = 3 },
                new Machine { Id = 20, Name = "1В62Г", MachineTypeId = 3 },
                new Machine { Id = 21, Name = "1В62", MachineTypeId = 3 },
                new Machine { Id = 22, Name = "16К25Г", MachineTypeId = 3 },
                new Machine { Id = 23, Name = "С11", MachineTypeId = 3 },
                new Machine { Id = 24, Name = "SNA400", MachineTypeId = 3 },
                new Machine { Id = 25, Name = "SNA500", MachineTypeId = 3 },
                new Machine { Id = 26, Name = "SNB500", MachineTypeId = 3 },
                new Machine { Id = 27, Name = "1И611П", MachineTypeId = 4 },
                new Machine { Id = 28, Name = "ИЖ250", MachineTypeId = 4 },
                new Machine { Id = 29, Name = "ИЖ250ИТП", MachineTypeId = 4 },
                new Machine { Id = 30, Name = "ИЖ250ИТВ", MachineTypeId = 4 },
                new Machine { Id = 31, Name = "ИЖ250ИТВМ", MachineTypeId = 4 },
                new Machine { Id = 32, Name = "16Б05", MachineTypeId = 4 },
                new Machine { Id = 33, Name = "95ТС-1", MachineTypeId = 4 },
                new Machine { Id = 34, Name = "ИЖ", MachineTypeId = 4 },
                new Machine { Id = 35, Name = "ИС1-1", MachineTypeId = 4 },
                new Machine { Id = 36, Name = "16И05", MachineTypeId = 4 },
                new Machine { Id = 37, Name = "16Б16КП", MachineTypeId = 5 },
                new Machine { Id = 38, Name = "16Б16КА", MachineTypeId = 5 },
                new Machine { Id = 39, Name = "16Б16П", MachineTypeId = 5 },
                new Machine { Id = 40, Name = "ТВ320", MachineTypeId = 5 },
                new Machine { Id = 41, Name = "1А616", MachineTypeId = 5 },
                new Machine { Id = 42, Name = "1М61", MachineTypeId = 5 },
                new Machine { Id = 43, Name = "УТ16", MachineTypeId = 5 },
                new Machine { Id = 44, Name = "1Е61", MachineTypeId = 5 },
                new Machine { Id = 45, Name = "16А20Ф3", MachineTypeId = 6 },
                new Machine { Id = 46, Name = "16А20Ф3С39", MachineTypeId = 6 },
                new Machine { Id = 47, Name = "16А20ТС1", MachineTypeId = 6 },
                new Machine { Id = 48, Name = "16А20Ф3С42", MachineTypeId = 6 },
                new Machine { Id = 49, Name = "16К20Т1", MachineTypeId = 6 },
                new Machine { Id = 50, Name = "16К20Ф3", MachineTypeId = 6 },
                new Machine { Id = 51, Name = "ДИП300", MachineTypeId = 7 },
                new Machine { Id = 52, Name = "1М63", MachineTypeId = 7 },
                new Machine { Id = 53, Name = "163", MachineTypeId = 7 },
                new Machine { Id = 54, Name = "1М63Н", MachineTypeId = 7 },
                new Machine { Id = 55, Name = "1М63Ф101", MachineTypeId = 7 },
                new Machine { Id = 56, Name = "1М63БФ101", MachineTypeId = 7 },
                new Machine { Id = 57, Name = "16К40", MachineTypeId = 8 },
                new Machine { Id = 58, Name = "16К40Ф101", MachineTypeId = 8 },
                new Machine { Id = 59, Name = "ДИП400", MachineTypeId = 8 },
                new Machine { Id = 60, Name = "1А64", MachineTypeId = 8 },
                new Machine { Id = 61, Name = "ДИП500", MachineTypeId = 9 },
                new Machine { Id = 62, Name = "1М65", MachineTypeId = 9 },
                new Machine { Id = 63, Name = "165", MachineTypeId = 9 },
                new Machine { Id = 64, Name = "1Н65", MachineTypeId = 9 },
                new Machine { Id = 65, Name = "675", MachineTypeId = 11 },
                new Machine { Id = 66, Name = "6А75", MachineTypeId = 11 },
                new Machine { Id = 67, Name = "675П", MachineTypeId = 11 },
                new Machine { Id = 68, Name = "6720В", MachineTypeId = 11 },
                new Machine { Id = 69, Name = "6720ВФ1", MachineTypeId = 11 },
                new Machine { Id = 70, Name = "6Б75", MachineTypeId = 11 },
                new Machine { Id = 71, Name = "67К20", MachineTypeId = 11 },
                new Machine { Id = 72, Name = "67М20", MachineTypeId = 11 },
                new Machine { Id = 73, Name = "СФ55", MachineTypeId = 11 },
                new Machine { Id = 74, Name = "676", MachineTypeId = 12 },
                new Machine { Id = 75, Name = "676П", MachineTypeId = 12 },
                new Machine { Id = 76, Name = "ВМ130", MachineTypeId = 12 },
                new Machine { Id = 77, Name = "6Б76", MachineTypeId = 12 },
                new Machine { Id = 78, Name = "67К25", MachineTypeId = 12 },
                new Machine { Id = 79, Name = "6725П", MachineTypeId = 12 },
                new Machine { Id = 80, Name = "67К25В", MachineTypeId = 12 },
                new Machine { Id = 81, Name = "ВМ130М", MachineTypeId = 12 },
                new Machine { Id = 82, Name = "67К25ПФ1", MachineTypeId = 12 },
                new Machine { Id = 83, Name = "СФ676", MachineTypeId = 12 },
                new Machine { Id = 84, Name = "FU315", MachineTypeId = 12 },
                new Machine { Id = 85, Name = "6Т80Ш", MachineTypeId = 12 },
                new Machine { Id = 86, Name = "6Р80Ш", MachineTypeId = 12 },
                new Machine { Id = 87, Name = "6Р12", MachineTypeId = 13 },
                new Machine { Id = 88, Name = "6Т12", MachineTypeId = 13 },
                new Machine { Id = 89, Name = "FSS315", MachineTypeId = 13 },
                new Machine { Id = 90, Name = "СФ35", MachineTypeId = 13 },
                new Machine { Id = 91, Name = "СФ40", MachineTypeId = 13 },
                new Machine { Id = 92, Name = "6Т12Ф20", MachineTypeId = 13 },
                new Machine { Id = 93, Name = "6М12", MachineTypeId = 13 },
                new Machine { Id = 94, Name = "6Р82", MachineTypeId = 14 },
                new Machine { Id = 95, Name = "6Т82", MachineTypeId = 14 },
                new Machine { Id = 96, Name = "6Р82Г", MachineTypeId = 14 },
                new Machine { Id = 97, Name = "6Т82Г", MachineTypeId = 14 },
                new Machine { Id = 98, Name = "6Р82Ш", MachineTypeId = 15 },
                new Machine { Id = 99, Name = "6Т82Ш", MachineTypeId = 15 },
                new Machine { Id = 100, Name = "6Д82Ш", MachineTypeId = 15 },
                new Machine { Id = 101, Name = "6М82Ш", MachineTypeId = 15 },
                new Machine { Id = 102, Name = "6Р13", MachineTypeId = 16 },
                new Machine { Id = 103, Name = "6Т13", MachineTypeId = 16 },
                new Machine { Id = 104, Name = "FSS400", MachineTypeId = 16 },
                new Machine { Id = 105, Name = "6М13", MachineTypeId = 16 },
                new Machine { Id = 106, Name = "ВМ127", MachineTypeId = 16 },
                new Machine { Id = 107, Name = "6Р83", MachineTypeId = 17 },
                new Machine { Id = 108, Name = "6Р83Г", MachineTypeId = 17 },
                new Machine { Id = 109, Name = "6Т83", MachineTypeId = 17 },
                new Machine { Id = 110, Name = "6Т83Г", MachineTypeId = 17 },
                new Machine { Id = 111, Name = "6Р83Ш", MachineTypeId = 18 },
                new Machine { Id = 112, Name = "6Т83Ш", MachineTypeId = 18 },
                new Machine { Id = 113, Name = "FU400", MachineTypeId = 18 },
                new Machine { Id = 114, Name = "2Н135", MachineTypeId = 20 },
                new Machine { Id = 115, Name = "2С132", MachineTypeId = 20 },
                new Machine { Id = 116, Name = "2Н125", MachineTypeId = 21 },
                new Machine { Id = 117, Name = "2Г125", MachineTypeId = 21 },
                new Machine { Id = 118, Name = "2С125", MachineTypeId = 21 },
                new Machine { Id = 119, Name = "2Н118", MachineTypeId = 22 },
                new Machine { Id = 120, Name = "МН18", MachineTypeId = 22 },
                new Machine { Id = 121, Name = "2М112", MachineTypeId = 23 },
                new Machine { Id = 122, Name = "НС12", MachineTypeId = 23 },
                new Machine { Id = 123, Name = "СНС12", MachineTypeId = 23 },
                new Machine { Id = 124, Name = "2620", MachineTypeId = 25 },
                new Machine { Id = 125, Name = "2А620", MachineTypeId = 25 },
                new Machine { Id = 126, Name = "2А620Ф1", MachineTypeId = 25 },
                new Machine { Id = 127, Name = "2А620Ф11", MachineTypeId = 25 },
                new Machine { Id = 128, Name = "2А620Ф2", MachineTypeId = 25 },
                new Machine { Id = 129, Name = "2А620Ф20", MachineTypeId = 25 },
                new Machine { Id = 130, Name = "2620ВФ1", MachineTypeId = 25 },
                new Machine { Id = 131, Name = "2620Ф11", MachineTypeId = 25 },
                new Machine { Id = 132, Name = "2620Е", MachineTypeId = 25 },
                new Machine { Id = 133, Name = "2620В", MachineTypeId = 25 },
                new Machine { Id = 134, Name = "2620ГФ1", MachineTypeId = 25 },
                new Machine { Id = 135, Name = "2622", MachineTypeId = 26 },
                new Machine { Id = 136, Name = "2622ГФ1", MachineTypeId = 26 },
                new Machine { Id = 137, Name = "2622Г", MachineTypeId = 26 },
                new Machine { Id = 138, Name = "2622В", MachineTypeId = 26 },
                new Machine { Id = 139, Name = "2622ВФ1", MachineTypeId = 26 },
                new Machine { Id = 140, Name = "2А622", MachineTypeId = 26 },
                new Machine { Id = 141, Name = "2А622Ф2", MachineTypeId = 26 },
                new Machine { Id = 142, Name = "2А622Ф1", MachineTypeId = 26 },
                new Machine { Id = 143, Name = "2А622Ф11", MachineTypeId = 26 },
                new Machine { Id = 144, Name = "2А622Ф20", MachineTypeId = 26 },
                new Machine { Id = 145, Name = "2А622МФ2", MachineTypeId = 26 },
                new Machine { Id = 146, Name = "2Л614", MachineTypeId = 27 },
                new Machine { Id = 147, Name = "2М614", MachineTypeId = 27 },
                new Machine { Id = 148, Name = "2М614ГФ1", MachineTypeId = 27 },
                new Machine { Id = 149, Name = "2Н614", MachineTypeId = 27 },
                new Machine { Id = 150, Name = "2А614", MachineTypeId = 27 },
                new Machine { Id = 151, Name = "2Л614Ф1", MachineTypeId = 27 },
                new Machine { Id = 152, Name = "3Г71", MachineTypeId = 29 },
                new Machine { Id = 153, Name = "3Г71М", MachineTypeId = 29 },
                new Machine { Id = 154, Name = "3Е711", MachineTypeId = 29 },
                new Machine { Id = 155, Name = "3Е711ВФ1", MachineTypeId = 29 },
                new Machine { Id = 156, Name = "3Е711ВФ11", MachineTypeId = 29 },
                new Machine { Id = 157, Name = "3Е711В", MachineTypeId = 29 },
                new Machine { Id = 158, Name = "3Д711", MachineTypeId = 29 },
                new Machine { Id = 159, Name = "3Д711АФ10", MachineTypeId = 29 },
                new Machine { Id = 160, Name = "3Д711ВФ11", MachineTypeId = 29 },
                new Machine { Id = 161, Name = "3Д711АФ1", MachineTypeId = 29 },
                new Machine { Id = 162, Name = "ОШ55", MachineTypeId = 29 },
                new Machine { Id = 163, Name = "3Б722", MachineTypeId = 30 },
                new Machine { Id = 164, Name = "3Л722", MachineTypeId = 30 },
                new Machine { Id = 165, Name = "3Л722В", MachineTypeId = 30 },
                new Machine { Id = 166, Name = "3Л722А", MachineTypeId = 30 },
                new Machine { Id = 167, Name = "3Д722", MachineTypeId = 30 },
                new Machine { Id = 168, Name = "3Б151", MachineTypeId = 31 },
                new Machine { Id = 169, Name = "3М151", MachineTypeId = 31 },
                new Machine { Id = 170, Name = "3А151", MachineTypeId = 31 },
                new Machine { Id = 171, Name = "3М152", MachineTypeId = 31 },
                new Machine { Id = 172, Name = "3М152В", MachineTypeId = 31 },
                new Machine { Id = 173, Name = "3М152ВМ", MachineTypeId = 31 },
                new Machine { Id = 174, Name = "3М152ВФ20", MachineTypeId = 31 },
                new Machine { Id = 175, Name = "3А130", MachineTypeId = 31 },
                new Machine { Id = 176, Name = "3У130", MachineTypeId = 31 },
                new Machine { Id = 177, Name = "3У131", MachineTypeId = 31 },
                new Machine { Id = 178, Name = "3У131В", MachineTypeId = 31 },
                new Machine { Id = 179, Name = "3М131", MachineTypeId = 31 },
                new Machine { Id = 180, Name = "3У131М", MachineTypeId = 31 },
                new Machine { Id = 181, Name = "3У131ВМ", MachineTypeId = 31 },
                new Machine { Id = 182, Name = "3У132ВМ", MachineTypeId = 31 },
                new Machine { Id = 183, Name = "3М161", MachineTypeId = 31 },
                new Machine { Id = 184, Name = "3М161ВФ20", MachineTypeId = 31 },
                new Machine { Id = 185, Name = "3У132", MachineTypeId = 31 },
                new Machine { Id = 186, Name = "3У132В", MachineTypeId = 31 },
                new Machine { Id = 187, Name = "3Б161", MachineTypeId = 31 },
                new Machine { Id = 188, Name = "3У132М", MachineTypeId = 31 },
                new Machine { Id = 189, Name = "3М132", MachineTypeId = 31 },
                new Machine { Id = 190, Name = "3М132МВФ2", MachineTypeId = 31 },
                new Machine { Id = 191, Name = "3В161", MachineTypeId = 31 },
                new Machine { Id = 192, Name = "3М162", MachineTypeId = 31 },
                new Machine { Id = 193, Name = "3М162В", MachineTypeId = 31 },
                new Machine { Id = 194, Name = "3М162МВФ2", MachineTypeId = 31 },
                new Machine { Id = 195, Name = "3У133", MachineTypeId = 31 },
                new Machine { Id = 196, Name = "3У142", MachineTypeId = 32 },
                new Machine { Id = 197, Name = "3У142В", MachineTypeId = 32 },
                new Machine { Id = 198, Name = "3У142МВ", MachineTypeId = 32 },
                new Machine { Id = 199, Name = "3У142МВФ2", MachineTypeId = 32 },
                new Machine { Id = 200, Name = "3У143", MachineTypeId = 32 },
                new Machine { Id = 201, Name = "3У144", MachineTypeId = 32 },
                new Machine { Id = 202, Name = "3М143МВ", MachineTypeId = 32 },
                new Machine { Id = 203, Name = "3А164", MachineTypeId = 32 },
                new Machine { Id = 204, Name = "3А164Б", MachineTypeId = 32 },
                new Machine { Id = 205, Name = "3У143МВФ2", MachineTypeId = 32 },
                new Machine { Id = 206, Name = "3У143МВ", MachineTypeId = 32 },
                new Machine { Id = 207, Name = "3В164", MachineTypeId = 32 },
                new Machine { Id = 208, Name = "3М173", MachineTypeId = 32 },
                new Machine { Id = 209, Name = "3М173МВФ2", MachineTypeId = 32 },
                new Machine { Id = 210, Name = "3У144МВФ2", MachineTypeId = 33 },
                new Machine { Id = 211, Name = "3М174", MachineTypeId = 33 },
                new Machine { Id = 212, Name = "3М174В", MachineTypeId = 33 },
                new Machine { Id = 213, Name = "7А420", MachineTypeId = 35 },
                new Machine { Id = 214, Name = "7402", MachineTypeId = 35 },
                new Machine { Id = 215, Name = "7М430", MachineTypeId = 36 },
                new Machine { Id = 216, Name = "7Д430", MachineTypeId = 36 },
                new Machine { Id = 217, Name = "7403", MachineTypeId = 36 },
                new Machine { Id = 218, Name = "ГД320", MachineTypeId = 36 },
                new Machine { Id = 219, Name = "7405", MachineTypeId = 37 },
                new Machine { Id = 220, Name = "7Д450", MachineTypeId = 37 },
                new Machine { Id = 221, Name = "7М450", MachineTypeId = 37 },
                new Machine { Id = 222, Name = "ГД500", MachineTypeId = 37 },
                new Machine { Id = 223, Name = "5К310", MachineTypeId = 39 },
                new Machine { Id = 224, Name = "53А20", MachineTypeId = 39 },
                new Machine { Id = 225, Name = "5306", MachineTypeId = 39 },
                new Machine { Id = 226, Name = "53А30", MachineTypeId = 40 },
                new Machine { Id = 227, Name = "5Д312", MachineTypeId = 40 },
                new Machine { Id = 228, Name = "5В312", MachineTypeId = 40 },
                new Machine { Id = 229, Name = "5Д312ЕЗ60", MachineTypeId = 40 },
                new Machine { Id = 230, Name = "53А50", MachineTypeId = 41 },
                new Machine { Id = 231, Name = "5К324", MachineTypeId = 41 },
                new Machine { Id = 232, Name = "5К324А", MachineTypeId = 41 },
                new Machine { Id = 233, Name = "5Е32", MachineTypeId = 41 },
                new Machine { Id = 234, Name = "5К32", MachineTypeId = 42 },
                new Machine { Id = 235, Name = "5К32А", MachineTypeId = 42 },
                new Machine { Id = 236, Name = "53А80", MachineTypeId = 42 },
                new Machine { Id = 237, Name = "53А80Н", MachineTypeId = 42 },
                new Machine { Id = 238, Name = "53А80К", MachineTypeId = 42 },
                new Machine { Id = 239, Name = "53А80Д", MachineTypeId = 42 },
                new Machine { Id = 240, Name = "КД2130", MachineTypeId = 44 },
                new Machine { Id = 241, Name = "КЕ2130", MachineTypeId = 44 },
                new Machine { Id = 242, Name = "К2130", MachineTypeId = 44 },
                new Machine { Id = 243, Name = "КД2330", MachineTypeId = 44 },
                new Machine { Id = 244, Name = "К1430", MachineTypeId = 44 },
                new Machine { Id = 245, Name = "КЕ2330", MachineTypeId = 44 },
                new Machine { Id = 246, Name = "КЕ1430", MachineTypeId = 44 },
                new Machine { Id = 247, Name = "К2128", MachineTypeId = 45 },
                new Machine { Id = 248, Name = "КИ1428", MachineTypeId = 45 },
                new Machine { Id = 249, Name = "КД2128Е", MachineTypeId = 45 },
                new Machine { Id = 250, Name = "КД2128К", MachineTypeId = 45 },
                new Machine { Id = 251, Name = "КД2328К", MachineTypeId = 45 },
                new Machine { Id = 252, Name = "КД2328", MachineTypeId = 45 },
                new Machine { Id = 253, Name = "КД1428", MachineTypeId = 45 },
                new Machine { Id = 254, Name = "КИ2128", MachineTypeId = 45 },
                new Machine { Id = 255, Name = "КД2128", MachineTypeId = 45 },
                new Machine { Id = 256, Name = "КД2126", MachineTypeId = 46 },
                new Machine { Id = 257, Name = "К2126", MachineTypeId = 46 },
                new Machine { Id = 258, Name = "КД2326", MachineTypeId = 46 },
                new Machine { Id = 259, Name = "КД1426", MachineTypeId = 46 },
                new Machine { Id = 260, Name = "КИ1426", MachineTypeId = 46 },
                new Machine { Id = 261, Name = "КД2326А", MachineTypeId = 46 },
                new Machine { Id = 262, Name = "КД2126К", MachineTypeId = 46 },
                new Machine { Id = 263, Name = "КД1426Б", MachineTypeId = 46 },
                new Machine { Id = 264, Name = "КИ2126", MachineTypeId = 46 },
                new Machine { Id = 265, Name = "КД2324", MachineTypeId = 47 },
                new Machine { Id = 266, Name = "КД2324Е", MachineTypeId = 47 },
                new Machine { Id = 267, Name = "КД1424", MachineTypeId = 47 },
                new Machine { Id = 268, Name = "КД2124", MachineTypeId = 47 },
                new Machine { Id = 269, Name = "КД2124Е", MachineTypeId = 47 },
                new Machine { Id = 270, Name = "КД2324В", MachineTypeId = 47 },
                new Machine { Id = 271, Name = "КД2124К", MachineTypeId = 47 },
                new Machine { Id = 272, Name = "КД2324К", MachineTypeId = 47 },
                new Machine { Id = 273, Name = "КД1424Б", MachineTypeId = 47 },
                new Machine { Id = 274, Name = "КД1424К", MachineTypeId = 47 },
                new Machine { Id = 275, Name = "КД2124Г", MachineTypeId = 47 },
                new Machine { Id = 276, Name = "К2324", MachineTypeId = 47 },
                new Machine { Id = 277, Name = "КД2322", MachineTypeId = 48 },
                new Machine { Id = 278, Name = "КД2122", MachineTypeId = 48 },
                new Machine { Id = 279, Name = "КД2322Г", MachineTypeId = 48 },
                new Machine { Id = 280, Name = "К2122", MachineTypeId = 48 },
                new Machine { Id = 281, Name = "КД1422", MachineTypeId = 48 },
                new Machine { Id = 282, Name = "Д2430", MachineTypeId = 49 },
                new Machine { Id = 283, Name = "ДЕ2430", MachineTypeId = 49 },
                new Machine { Id = 284, Name = "ДГ2430", MachineTypeId = 49 },
                new Machine { Id = 285, Name = "ДБ2430", MachineTypeId = 49 },
                new Machine { Id = 286, Name = "ДБ2430Б", MachineTypeId = 49 },
                new Machine { Id = 287, Name = "ДГ2430А", MachineTypeId = 49 },
                new Machine { Id = 288, Name = "П476", MachineTypeId = 50 },
                new Machine { Id = 289, Name = "ПД476", MachineTypeId = 50 },
                new Machine { Id = 290, Name = "Д2432", MachineTypeId = 50 },
                new Machine { Id = 291, Name = "ДГ2432", MachineTypeId = 50 },
                new Machine { Id = 292, Name = "ДБ2432", MachineTypeId = 50 },
                new Machine { Id = 293, Name = "ДГ2432А", MachineTypeId = 50 },
                new Machine { Id = 294, Name = "ДБ2432А", MachineTypeId = 50 },
                new Machine { Id = 295, Name = "ДБ2432Б", MachineTypeId = 50 },
                new Machine { Id = 296, Name = "ДЕ2432", MachineTypeId = 50 },
                new Machine { Id = 297, Name = "ДЕ2428", MachineTypeId = 51 },
                new Machine { Id = 298, Name = "ДГ2428", MachineTypeId = 51 },
                new Machine { Id = 299, Name = "ДГ2428Б", MachineTypeId = 51 },
                new Machine { Id = 300, Name = "ДВ2428", MachineTypeId = 51 },
                new Machine { Id = 301, Name = "П6330", MachineTypeId = 52 },
                new Machine { Id = 302, Name = "PYE100", MachineTypeId = 52 },
                new Machine { Id = 303, Name = "П6328", MachineTypeId = 53 },
                new Machine { Id = 304, Name = "PYE63", MachineTypeId = 53 },
                new Machine { Id = 305, Name = "И2220", MachineTypeId = 55 },
                new Machine { Id = 306, Name = "ИБ2220", MachineTypeId = 55 },
                new Machine { Id = 307, Name = "И2220А", MachineTypeId = 55 },
                new Machine { Id = 308, Name = "И2220Б", MachineTypeId = 55 },
                new Machine { Id = 309, Name = "ИБ2220Г", MachineTypeId = 55 },
                new Machine { Id = 310, Name = "ИБ2220Б", MachineTypeId = 55 },
                new Machine { Id = 311, Name = "ИБ2220В", MachineTypeId = 55 },
                new Machine { Id = 312, Name = "И2222", MachineTypeId = 56 },
                new Machine { Id = 313, Name = "ИБ2222", MachineTypeId = 56 },
                new Machine { Id = 314, Name = "ИБ2222В", MachineTypeId = 56 },
                new Machine { Id = 315, Name = "И2222Б", MachineTypeId = 56 },
                new Machine { Id = 316, Name = "ИВ2222", MachineTypeId = 56 },
                new Machine { Id = 317, Name = "И1330", MachineTypeId = 58 },
                new Machine { Id = 318, Name = "ИВ1330А", MachineTypeId = 58 },
                new Machine { Id = 319, Name = "И1330А", MachineTypeId = 58 },
                new Machine { Id = 320, Name = "ИА1330", MachineTypeId = 58 },
                new Machine { Id = 321, Name = "И1430", MachineTypeId = 59 },
                new Machine { Id = 322, Name = "ИА1430А", MachineTypeId = 59 },
                new Machine { Id = 323, Name = "ИБ1430", MachineTypeId = 59 },
                new Machine { Id = 324, Name = "ИБ1430Б", MachineTypeId = 59 },
                new Machine { Id = 325, Name = "ИА1430", MachineTypeId = 59 }
            });

            modelBuilder.Entity<RequisitionResponse>()
                .HasOne(i => i.Requisition)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProposalResponse>()
                .HasOne(i => i.Proposal)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MessageResponse>()
                .HasOne(i => i.Message)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MessageResponse>()
                .HasOne(i => i.ResponseMessage)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        } // OnModelCreating

        // Набор таблиц
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RequisitionResponse> RequisitionResponses { get; set; }
        public DbSet<ProposalResponse> ProposalResponses { get; set; }
        public DbSet<MessageResponse> MessageResponses { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<ProposalImage> ProposalImages { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<ReadedNotices> ReadedNotices { get; set; }
    }
}