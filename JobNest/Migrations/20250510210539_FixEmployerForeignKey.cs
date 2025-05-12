using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobNest.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployerForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_EmployerId1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_EmployerId1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EmployerId1",
                table: "Jobs");

            migrationBuilder.AlterColumn<string>(
                name: "EmployerId",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmployerId",
                table: "Jobs",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_EmployerId",
                table: "Jobs",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_EmployerId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_EmployerId",
                table: "Jobs");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployerId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "EmployerId1",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmployerId1",
                table: "Jobs",
                column: "EmployerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_EmployerId1",
                table: "Jobs",
                column: "EmployerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
