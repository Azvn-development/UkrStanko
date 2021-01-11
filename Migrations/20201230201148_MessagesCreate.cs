using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UkrStanko.Migrations
{
    public partial class MessagesCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<bool>(type: "bit", nullable: false),
                    ResponseRequisitionId = table.Column<int>(type: "int", nullable: true),
                    ResponseProposalId = table.Column<int>(type: "int", nullable: true),
                    ResponseMessageId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Messages_ResponseMessageId",
                        column: x => x.ResponseMessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Proposals_ResponseProposalId",
                        column: x => x.ResponseProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Requisitions_ResponseRequisitionId",
                        column: x => x.ResponseRequisitionId,
                        principalTable: "Requisitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ResponseMessageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageResponses_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageResponses_Messages_ResponseMessageId",
                        column: x => x.ResponseMessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProposalResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    ProposalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalResponses_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposalResponses_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    RequisitionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequisitionResponses_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisitionResponses_Requisitions_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageResponses_MessageId",
                table: "MessageResponses",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageResponses_ResponseMessageId",
                table: "MessageResponses",
                column: "ResponseMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ResponseMessageId",
                table: "Messages",
                column: "ResponseMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ResponseProposalId",
                table: "Messages",
                column: "ResponseProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ResponseRequisitionId",
                table: "Messages",
                column: "ResponseRequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalResponses_MessageId",
                table: "ProposalResponses",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalResponses_ProposalId",
                table: "ProposalResponses",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionResponses_MessageId",
                table: "RequisitionResponses",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionResponses_RequisitionId",
                table: "RequisitionResponses",
                column: "RequisitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageResponses");

            migrationBuilder.DropTable(
                name: "ProposalResponses");

            migrationBuilder.DropTable(
                name: "RequisitionResponses");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
