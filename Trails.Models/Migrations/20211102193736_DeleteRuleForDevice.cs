using Microsoft.EntityFrameworkCore.Migrations;

namespace Trails.Models.Migrations
{
    public partial class DeleteRuleForDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionData_Devices_DeviceId",
                table: "PositionData");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionData_Devices_DeviceId",
                table: "PositionData",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionData_Devices_DeviceId",
                table: "PositionData");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionData_Devices_DeviceId",
                table: "PositionData",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
