using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PendeloApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class kmPerDayMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KmPerDay",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kilometers = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KmPerDay", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KmPerDay_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KmPerDay_UserID",
                table: "KmPerDay",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KmPerDay");
        }
    }
}
