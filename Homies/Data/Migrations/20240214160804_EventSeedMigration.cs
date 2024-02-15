using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homies.Data.Migrations
{
    public partial class EventSeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedOn", "Description", "End", "Name", "OrganiserId", "Start", "TypeId" },
                values: new object[] { 1, new DateTime(2024, 2, 9, 18, 8, 4, 564, DateTimeKind.Local).AddTicks(4941), "Seeded event via a EF Core migration.", new DateTime(2024, 2, 16, 4, 8, 4, 564, DateTimeKind.Local).AddTicks(4981), "Seeded Event", "9a080fb7-aa06-4a2b-823a-fcfb7dc091ee", new DateTime(2024, 2, 14, 18, 8, 4, 564, DateTimeKind.Local).AddTicks(4979), 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
