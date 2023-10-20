using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vista.App.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryCode = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryCode);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BookingReference = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerCategories",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryCode = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerCategories", x => new { x.TrainerId, x.CategoryCode });
                    table.ForeignKey(
                        name: "FK_TrainerCategories_Categories_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "Categories",
                        principalColumn: "CategoryCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerCategories_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryCode", "CategoryName" },
                values: new object[] { "ED", "Equality and Diversity" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryCode", "CategoryName" },
                values: new object[] { "HS", "Health and Safety" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryCode", "CategoryName" },
                values: new object[] { "IT", "Information Technology" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryCode", "CategoryName" },
                values: new object[] { "LM", "Leadership and Management" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryCode", "CategoryName" },
                values: new object[] { "MAR", "Marketing" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "Location", "Name" },
                values: new object[] { 1, "Middlesbrough", "Palmer Patel" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "Location", "Name" },
                values: new object[] { 2, "Stockton-on-Tees", "Cater Moon" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "Location", "Name" },
                values: new object[] { 3, "Middlesbrough", "Alex Dickerson" });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "Location", "Name" },
                values: new object[] { 4, "Stockton-on-Tees", "Sally Johnson" });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 1, null, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 2, null, new DateTime(2023, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 3, null, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 4, null, new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 5, "TST-99", new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 6, null, new DateTime(2023, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 7, null, new DateTime(2023, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 8, null, new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 9, null, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 10, null, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 11, null, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 12, null, new DateTime(2023, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 13, null, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 14, null, new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 15, null, new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 16, "TST-98", new DateTime(2023, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 17, "TST-97", new DateTime(2023, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 18, null, new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 19, null, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 20, null, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 21, null, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 22, null, new DateTime(2023, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 23, null, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 24, "TST-96", new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 25, null, new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 26, null, new DateTime(2023, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 27, null, new DateTime(2023, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 28, null, new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 29, null, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 30, "TST-94", new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 31, null, new DateTime(2023, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 32, null, new DateTime(2023, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 33, "TST-95", new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 34, null, new DateTime(2023, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 35, null, new DateTime(2023, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "BookingReference", "SessionDate", "TrainerId" },
                values: new object[] { 36, null, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "ED", 1 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "IT", 1 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "ED", 2 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "HS", 2 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "LM", 2 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "IT", 3 });

            migrationBuilder.InsertData(
                table: "TrainerCategories",
                columns: new[] { "CategoryCode", "TrainerId" },
                values: new object[] { "LM", 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_TrainerId",
                table: "Sessions",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerCategories_CategoryCode",
                table: "TrainerCategories",
                column: "CategoryCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "TrainerCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Trainers");
        }
    }
}
