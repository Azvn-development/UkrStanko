using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UkrStanko.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentMachineTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineTypes_MachineTypes_ParentMachineTypeId",
                        column: x => x.ParentMachineTypeId,
                        principalTable: "MachineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReadedNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadedNoticesCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadedNotices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MachineTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_MachineTypes_MachineTypeId",
                        column: x => x.MachineTypeId,
                        principalTable: "MachineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    PurshasePrice = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposals_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requisitions_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ProposalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalImages_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MachineTypes",
                columns: new[] { "Id", "Name", "ParentMachineTypeId" },
                values: new object[,]
                {
                    { 1, "Без группы", null },
                    { 2, "Токарные", null },
                    { 10, "Фрезерные", null },
                    { 19, "Сверлильные", null },
                    { 24, "ГРС", null },
                    { 28, "Шлифовальные", null },
                    { 34, "Долбежные", null },
                    { 38, "Зубофрезерные", null },
                    { 43, "Пресса", null },
                    { 54, "Вальцы", null },
                    { 57, "Листогибы", null }
                });

            migrationBuilder.InsertData(
                table: "MachineTypes",
                columns: new[] { "Id", "Name", "ParentMachineTypeId" },
                values: new object[,]
                {
                    { 3, "Токарные_1", 2 },
                    { 33, "Шлифовальные_5", 28 },
                    { 35, "Долбежные_1", 34 },
                    { 36, "Долбежные_2", 34 },
                    { 37, "Долбежные_3", 34 },
                    { 39, "Зубофрезерные_1", 38 },
                    { 40, "Зубофрезерные_2", 38 },
                    { 41, "Зубофрезерные_3", 38 },
                    { 42, "Зубофрезерные_4", 38 },
                    { 44, "Пресса_1", 43 },
                    { 32, "Шлифовальные_4", 28 },
                    { 45, "Пресса_2", 43 },
                    { 47, "Пресса_4", 43 },
                    { 48, "Пресса_5", 43 },
                    { 49, "Пресса_6", 43 },
                    { 50, "Пресса_7", 43 },
                    { 51, "Пресса_8", 43 },
                    { 52, "Пресса_9", 43 },
                    { 53, "Пресса_10", 43 },
                    { 55, "Вальцы_1", 54 },
                    { 56, "Вальцы_2", 54 },
                    { 46, "Пресса_3", 43 },
                    { 31, "Шлифовальные_3", 28 },
                    { 30, "Шлифовальные_2", 28 },
                    { 29, "Шлифовальные_1", 28 },
                    { 4, "Токарные_2", 2 },
                    { 5, "Токарные_3", 2 },
                    { 6, "Токарные_4", 2 },
                    { 7, "Токарные_5", 2 },
                    { 8, "Токарные_6", 2 },
                    { 9, "Токарные_7", 2 },
                    { 11, "Фрезерные_1", 10 },
                    { 12, "Фрезерные_2", 10 },
                    { 13, "Фрезерные_3", 10 },
                    { 14, "Фрезерные_4", 10 },
                    { 15, "Фрезерные_5", 10 },
                    { 16, "Фрезерные_6", 10 },
                    { 17, "Фрезерные_7", 10 },
                    { 18, "Фрезерные_8", 10 },
                    { 20, "Сверлильные_1", 19 },
                    { 21, "Сверлильные_2", 19 },
                    { 22, "Сверлильные_3", 19 }
                });

            migrationBuilder.InsertData(
                table: "MachineTypes",
                columns: new[] { "Id", "Name", "ParentMachineTypeId" },
                values: new object[,]
                {
                    { 23, "Сверлильные_4", 19 },
                    { 25, "ГРС_1", 24 },
                    { 26, "ГРС_2", 24 },
                    { 27, "ГРС_3", 24 },
                    { 58, "Листогибы_1", 57 },
                    { 59, "Листогибы_2", 57 }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 3, "16К20" },
                    { 221, 37, "7М450" },
                    { 220, 37, "7Д450" },
                    { 219, 37, "7405" },
                    { 218, 36, "ГД320" },
                    { 217, 36, "7403" },
                    { 216, 36, "7Д430" },
                    { 215, 36, "7М430" },
                    { 222, 37, "ГД500" },
                    { 214, 35, "7402" },
                    { 212, 33, "3М174В" },
                    { 211, 33, "3М174" },
                    { 210, 33, "3У144МВФ2" },
                    { 209, 32, "3М173МВФ2" },
                    { 208, 32, "3М173" },
                    { 207, 32, "3В164" },
                    { 206, 32, "3У143МВ" },
                    { 213, 35, "7А420" },
                    { 205, 32, "3У143МВФ2" },
                    { 223, 39, "5К310" },
                    { 225, 39, "5306" },
                    { 241, 44, "КЕ2130" },
                    { 240, 44, "КД2130" },
                    { 239, 42, "53А80Д" },
                    { 238, 42, "53А80К" },
                    { 237, 42, "53А80Н" },
                    { 236, 42, "53А80" },
                    { 235, 42, "5К32А" },
                    { 224, 39, "53А20" },
                    { 234, 42, "5К32" },
                    { 232, 41, "5К324А" },
                    { 231, 41, "5К324" },
                    { 230, 41, "53А50" },
                    { 229, 40, "5Д312ЕЗ60" },
                    { 228, 40, "5В312" },
                    { 227, 40, "5Д312" },
                    { 226, 40, "53А30" },
                    { 233, 41, "5Е32" },
                    { 204, 32, "3А164Б" },
                    { 203, 32, "3А164" },
                    { 202, 32, "3М143МВ" },
                    { 180, 31, "3У131М" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 179, 31, "3М131" },
                    { 178, 31, "3У131В" },
                    { 177, 31, "3У131" },
                    { 176, 31, "3У130" },
                    { 175, 31, "3А130" },
                    { 174, 31, "3М152ВФ20" },
                    { 181, 31, "3У131ВМ" },
                    { 173, 31, "3М152ВМ" },
                    { 171, 31, "3М152" },
                    { 170, 31, "3А151" },
                    { 169, 31, "3М151" },
                    { 168, 31, "3Б151" },
                    { 167, 30, "3Д722" },
                    { 166, 30, "3Л722А" },
                    { 165, 30, "3Л722В" },
                    { 172, 31, "3М152В" },
                    { 182, 31, "3У132ВМ" },
                    { 183, 31, "3М161" },
                    { 184, 31, "3М161ВФ20" },
                    { 201, 32, "3У144" },
                    { 200, 32, "3У143" },
                    { 199, 32, "3У142МВФ2" },
                    { 198, 32, "3У142МВ" },
                    { 197, 32, "3У142В" },
                    { 196, 32, "3У142" },
                    { 195, 31, "3У133" },
                    { 194, 31, "3М162МВФ2" },
                    { 193, 31, "3М162В" },
                    { 192, 31, "3М162" },
                    { 191, 31, "3В161" },
                    { 190, 31, "3М132МВФ2" },
                    { 189, 31, "3М132" },
                    { 188, 31, "3У132М" },
                    { 187, 31, "3Б161" },
                    { 186, 31, "3У132В" },
                    { 185, 31, "3У132" },
                    { 242, 44, "К2130" },
                    { 243, 44, "КД2330" },
                    { 244, 44, "К1430" },
                    { 245, 44, "КЕ2330" },
                    { 302, 52, "PYE100" },
                    { 301, 52, "П6330" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 300, 51, "ДВ2428" },
                    { 299, 51, "ДГ2428Б" },
                    { 298, 51, "ДГ2428" },
                    { 297, 51, "ДЕ2428" },
                    { 296, 50, "ДЕ2432" },
                    { 303, 53, "П6328" },
                    { 295, 50, "ДБ2432Б" },
                    { 293, 50, "ДГ2432А" },
                    { 292, 50, "ДБ2432" },
                    { 291, 50, "ДГ2432" },
                    { 290, 50, "Д2432" },
                    { 289, 50, "ПД476" },
                    { 288, 50, "П476" },
                    { 287, 49, "ДГ2430А" },
                    { 294, 50, "ДБ2432А" },
                    { 304, 53, "PYE63" },
                    { 305, 55, "И2220" },
                    { 306, 55, "ИБ2220" },
                    { 323, 59, "ИБ1430" },
                    { 322, 59, "ИА1430А" },
                    { 321, 59, "И1430" },
                    { 320, 58, "ИА1330" },
                    { 319, 58, "И1330А" },
                    { 318, 58, "ИВ1330А" },
                    { 317, 58, "И1330" },
                    { 316, 56, "ИВ2222" },
                    { 315, 56, "И2222Б" },
                    { 314, 56, "ИБ2222В" },
                    { 313, 56, "ИБ2222" },
                    { 312, 56, "И2222" },
                    { 311, 55, "ИБ2220В" },
                    { 310, 55, "ИБ2220Б" },
                    { 309, 55, "ИБ2220Г" },
                    { 308, 55, "И2220Б" },
                    { 307, 55, "И2220А" },
                    { 286, 49, "ДБ2430Б" },
                    { 164, 30, "3Л722" },
                    { 285, 49, "ДБ2430" },
                    { 283, 49, "ДЕ2430" },
                    { 261, 46, "КД2326А" },
                    { 260, 46, "КИ1426" },
                    { 259, 46, "КД1426" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 258, 46, "КД2326" },
                    { 257, 46, "К2126" },
                    { 256, 46, "КД2126" },
                    { 255, 45, "КД2128" },
                    { 262, 46, "КД2126К" },
                    { 254, 45, "КИ2128" },
                    { 252, 45, "КД2328" },
                    { 251, 45, "КД2328К" },
                    { 250, 45, "КД2128К" },
                    { 249, 45, "КД2128Е" },
                    { 248, 45, "КИ1428" },
                    { 247, 45, "К2128" },
                    { 246, 44, "КЕ1430" },
                    { 253, 45, "КД1428" },
                    { 263, 46, "КД1426Б" },
                    { 264, 46, "КИ2126" },
                    { 265, 47, "КД2324" },
                    { 282, 49, "Д2430" },
                    { 281, 48, "КД1422" },
                    { 280, 48, "К2122" },
                    { 279, 48, "КД2322Г" },
                    { 278, 48, "КД2122" },
                    { 277, 48, "КД2322" },
                    { 276, 47, "К2324" },
                    { 275, 47, "КД2124Г" },
                    { 274, 47, "КД1424К" },
                    { 273, 47, "КД1424Б" },
                    { 272, 47, "КД2324К" },
                    { 271, 47, "КД2124К" },
                    { 270, 47, "КД2324В" },
                    { 269, 47, "КД2124Е" },
                    { 268, 47, "КД2124" },
                    { 267, 47, "КД1424" },
                    { 266, 47, "КД2324Е" },
                    { 284, 49, "ДГ2430" },
                    { 324, 59, "ИБ1430Б" },
                    { 163, 30, "3Б722" },
                    { 161, 29, "3Д711АФ1" },
                    { 58, 8, "16К40Ф101" },
                    { 57, 8, "16К40" },
                    { 56, 7, "1М63БФ101" },
                    { 55, 7, "1М63Ф101" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 54, 7, "1М63Н" },
                    { 53, 7, "163" },
                    { 52, 7, "1М63" },
                    { 59, 8, "ДИП400" },
                    { 51, 7, "ДИП300" },
                    { 49, 6, "16К20Т1" },
                    { 48, 6, "16А20Ф3С42" },
                    { 47, 6, "16А20ТС1" },
                    { 46, 6, "16А20Ф3С39" },
                    { 45, 6, "16А20Ф3" },
                    { 44, 5, "1Е61" },
                    { 43, 5, "УТ16" },
                    { 50, 6, "16К20Ф3" },
                    { 42, 5, "1М61" },
                    { 60, 8, "1А64" },
                    { 62, 9, "1М65" },
                    { 78, 12, "67К25" },
                    { 77, 12, "6Б76" },
                    { 76, 12, "ВМ130" },
                    { 75, 12, "676П" },
                    { 74, 12, "676" },
                    { 73, 11, "СФ55" },
                    { 72, 11, "67М20" },
                    { 61, 9, "ДИП500" },
                    { 71, 11, "67К20" },
                    { 69, 11, "6720ВФ1" },
                    { 68, 11, "6720В" },
                    { 67, 11, "675П" },
                    { 66, 11, "6А75" },
                    { 65, 11, "675" },
                    { 64, 9, "1Н65" },
                    { 63, 9, "165" },
                    { 70, 11, "6Б75" },
                    { 41, 5, "1А616" },
                    { 40, 5, "ТВ320" },
                    { 39, 5, "16Б16П" },
                    { 17, 3, "МК6056" },
                    { 16, 3, "16Е20" },
                    { 15, 3, "ФТ11" },
                    { 14, 3, "16Д25" },
                    { 13, 3, "16Д20" },
                    { 12, 3, "16К25" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 11, 3, "ЛТ10" },
                    { 18, 3, "1К625" },
                    { 10, 3, "ИТ1М" },
                    { 8, 3, "ДИП200" },
                    { 7, 3, "ТС20" },
                    { 6, 3, "ТС25" },
                    { 5, 3, "ТС75" },
                    { 4, 3, "ТС70" },
                    { 3, 3, "КА280" },
                    { 2, 3, "1К62" },
                    { 9, 3, "1К62Д" },
                    { 19, 3, "ЛТ11" },
                    { 20, 3, "1В62Г" },
                    { 21, 3, "1В62" },
                    { 38, 5, "16Б16КА" },
                    { 37, 5, "16Б16КП" },
                    { 36, 4, "16И05" },
                    { 35, 4, "ИС1-1" },
                    { 34, 4, "ИЖ" },
                    { 33, 4, "95ТС-1" },
                    { 32, 4, "16Б05" },
                    { 31, 4, "ИЖ250ИТВМ" },
                    { 30, 4, "ИЖ250ИТВ" },
                    { 29, 4, "ИЖ250ИТП" },
                    { 28, 4, "ИЖ250" },
                    { 27, 4, "1И611П" },
                    { 26, 3, "SNB500" },
                    { 25, 3, "SNA500" },
                    { 24, 3, "SNA400" },
                    { 23, 3, "С11" },
                    { 22, 3, "16К25Г" },
                    { 79, 12, "6725П" },
                    { 80, 12, "67К25В" },
                    { 81, 12, "ВМ130М" },
                    { 82, 12, "67К25ПФ1" },
                    { 139, 26, "2622ВФ1" },
                    { 138, 26, "2622В" },
                    { 137, 26, "2622Г" },
                    { 136, 26, "2622ГФ1" },
                    { 135, 26, "2622" },
                    { 134, 25, "2620ГФ1" },
                    { 133, 25, "2620В" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 140, 26, "2А622" },
                    { 132, 25, "2620Е" },
                    { 130, 25, "2620ВФ1" },
                    { 129, 25, "2А620Ф20" },
                    { 128, 25, "2А620Ф2" },
                    { 127, 25, "2А620Ф11" },
                    { 126, 25, "2А620Ф1" },
                    { 125, 25, "2А620" },
                    { 124, 25, "2620" },
                    { 131, 25, "2620Ф11" },
                    { 141, 26, "2А622Ф2" },
                    { 142, 26, "2А622Ф1" },
                    { 143, 26, "2А622Ф11" },
                    { 160, 29, "3Д711ВФ11" },
                    { 159, 29, "3Д711АФ10" },
                    { 158, 29, "3Д711" },
                    { 157, 29, "3Е711В" },
                    { 156, 29, "3Е711ВФ11" },
                    { 155, 29, "3Е711ВФ1" },
                    { 154, 29, "3Е711" },
                    { 153, 29, "3Г71М" },
                    { 152, 29, "3Г71" },
                    { 151, 27, "2Л614Ф1" },
                    { 150, 27, "2А614" },
                    { 149, 27, "2Н614" },
                    { 148, 27, "2М614ГФ1" },
                    { 147, 27, "2М614" },
                    { 146, 27, "2Л614" },
                    { 145, 26, "2А622МФ2" },
                    { 144, 26, "2А622Ф20" },
                    { 123, 23, "СНС12" },
                    { 162, 29, "ОШ55" },
                    { 122, 23, "НС12" },
                    { 120, 22, "МН18" },
                    { 98, 15, "6Р82Ш" },
                    { 97, 14, "6Т82Г" },
                    { 96, 14, "6Р82Г" },
                    { 95, 14, "6Т82" },
                    { 94, 14, "6Р82" },
                    { 93, 13, "6М12" },
                    { 92, 13, "6Т12Ф20" },
                    { 99, 15, "6Т82Ш" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "Id", "MachineTypeId", "Name" },
                values: new object[,]
                {
                    { 91, 13, "СФ40" },
                    { 89, 13, "FSS315" },
                    { 88, 13, "6Т12" },
                    { 87, 13, "6Р12" },
                    { 86, 12, "6Р80Ш" },
                    { 85, 12, "6Т80Ш" },
                    { 84, 12, "FU315" },
                    { 83, 12, "СФ676" },
                    { 90, 13, "СФ35" },
                    { 100, 15, "6Д82Ш" },
                    { 101, 15, "6М82Ш" },
                    { 102, 16, "6Р13" },
                    { 119, 22, "2Н118" },
                    { 118, 21, "2С125" },
                    { 117, 21, "2Г125" },
                    { 116, 21, "2Н125" },
                    { 115, 20, "2С132" },
                    { 114, 20, "2Н135" },
                    { 113, 18, "FU400" },
                    { 112, 18, "6Т83Ш" },
                    { 111, 18, "6Р83Ш" },
                    { 110, 17, "6Т83Г" },
                    { 109, 17, "6Т83" },
                    { 108, 17, "6Р83Г" },
                    { 107, 17, "6Р83" },
                    { 106, 16, "ВМ127" },
                    { 105, 16, "6М13" },
                    { 104, 16, "FSS400" },
                    { 103, 16, "6Т13" },
                    { 121, 23, "2М112" },
                    { 325, 59, "ИА1430" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Machines_MachineTypeId",
                table: "Machines",
                column: "MachineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Name",
                table: "Machines",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTypes_Name",
                table: "MachineTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MachineTypes_ParentMachineTypeId",
                table: "MachineTypes",
                column: "ParentMachineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalImages_ProposalId",
                table: "ProposalImages",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_MachineId",
                table: "Proposals",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Requisitions_MachineId",
                table: "Requisitions",
                column: "MachineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProposalImages");

            migrationBuilder.DropTable(
                name: "ReadedNotices");

            migrationBuilder.DropTable(
                name: "Requisitions");

            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "MachineTypes");
        }
    }
}
