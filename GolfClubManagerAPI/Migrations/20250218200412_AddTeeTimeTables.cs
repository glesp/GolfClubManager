using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfClubManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTeeTimeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeeTimeBookings_MemberId",
                table: "TeeTimeBookings");

            migrationBuilder.DropIndex(
                name: "IX_Members_MembershipNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BookingTime",
                table: "TeeTimeBookings");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MembershipNumber",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "TeeTimeSlotId",
                table: "TeeTimeBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TeeTimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeeTimeSlots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimeBookings_MemberId_TeeTimeSlotId",
                table: "TeeTimeBookings",
                columns: new[] { "MemberId", "TeeTimeSlotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimeBookings_TeeTimeSlotId",
                table: "TeeTimeBookings",
                column: "TeeTimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeeTimeBookings_TeeTimeSlots_TeeTimeSlotId",
                table: "TeeTimeBookings",
                column: "TeeTimeSlotId",
                principalTable: "TeeTimeSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeeTimeBookings_TeeTimeSlots_TeeTimeSlotId",
                table: "TeeTimeBookings");

            migrationBuilder.DropTable(
                name: "TeeTimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TeeTimeBookings_MemberId_TeeTimeSlotId",
                table: "TeeTimeBookings");

            migrationBuilder.DropIndex(
                name: "IX_TeeTimeBookings_TeeTimeSlotId",
                table: "TeeTimeBookings");

            migrationBuilder.DropColumn(
                name: "TeeTimeSlotId",
                table: "TeeTimeBookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingTime",
                table: "TeeTimeBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MembershipNumber",
                table: "Members",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimeBookings_MemberId",
                table: "TeeTimeBookings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MembershipNumber",
                table: "Members",
                column: "MembershipNumber",
                unique: true);
        }
    }
}
