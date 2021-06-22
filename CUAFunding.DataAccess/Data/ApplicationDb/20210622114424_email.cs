using Microsoft.EntityFrameworkCore.Migrations;

namespace CUAFunding.DataAccess.Data.ApplicationDb
{
    public partial class email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NeededEquipment_Equipment_EquipmentId",
                table: "NeededEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_NeededEquipment_Projects_ProjectId",
                table: "NeededEquipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NeededEquipment",
                table: "NeededEquipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.RenameTable(
                name: "NeededEquipment",
                newName: "NeededEquipments");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "Equipments");

            migrationBuilder.RenameIndex(
                name: "IX_NeededEquipment_ProjectId",
                table: "NeededEquipments",
                newName: "IX_NeededEquipments_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_NeededEquipment_EquipmentId",
                table: "NeededEquipments",
                newName: "IX_NeededEquipments_EquipmentId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NeededEquipments",
                table: "NeededEquipments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NeededEquipments_Equipments_EquipmentId",
                table: "NeededEquipments",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NeededEquipments_Projects_ProjectId",
                table: "NeededEquipments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NeededEquipments_Equipments_EquipmentId",
                table: "NeededEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_NeededEquipments_Projects_ProjectId",
                table: "NeededEquipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NeededEquipments",
                table: "NeededEquipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Donations");

            migrationBuilder.RenameTable(
                name: "NeededEquipments",
                newName: "NeededEquipment");

            migrationBuilder.RenameTable(
                name: "Equipments",
                newName: "Equipment");

            migrationBuilder.RenameIndex(
                name: "IX_NeededEquipments_ProjectId",
                table: "NeededEquipment",
                newName: "IX_NeededEquipment_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_NeededEquipments_EquipmentId",
                table: "NeededEquipment",
                newName: "IX_NeededEquipment_EquipmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NeededEquipment",
                table: "NeededEquipment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NeededEquipment_Equipment_EquipmentId",
                table: "NeededEquipment",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NeededEquipment_Projects_ProjectId",
                table: "NeededEquipment",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
