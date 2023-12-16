using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseManager.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Performances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Runs = table.Column<int>(type: "int", nullable: false),
                    Table = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageSeconds = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performances", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Performances");
        }
    }
}
