using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_account_roles_tb_m_roles_role_guid",
                table: "tb_m_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_bookings_tb_m_employees_employee_guid",
                table: "tb_tr_bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_bookings_tb_m_rooms_room_guid",
                table: "tb_tr_bookings");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tb_m_accounts",
                type: "nvarchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_account_roles_tb_m_roles_role_guid",
                table: "tb_m_account_roles",
                column: "role_guid",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_bookings_tb_m_employees_employee_guid",
                table: "tb_tr_bookings",
                column: "employee_guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_bookings_tb_m_rooms_room_guid",
                table: "tb_tr_bookings",
                column: "room_guid",
                principalTable: "tb_m_rooms",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_account_roles_tb_m_roles_role_guid",
                table: "tb_m_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_bookings_tb_m_employees_employee_guid",
                table: "tb_tr_bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_bookings_tb_m_rooms_room_guid",
                table: "tb_tr_bookings");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tb_m_accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_account_roles_tb_m_roles_role_guid",
                table: "tb_m_account_roles",
                column: "role_guid",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_bookings_tb_m_employees_employee_guid",
                table: "tb_tr_bookings",
                column: "employee_guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_bookings_tb_m_rooms_room_guid",
                table: "tb_tr_bookings",
                column: "room_guid",
                principalTable: "tb_m_rooms",
                principalColumn: "guid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
