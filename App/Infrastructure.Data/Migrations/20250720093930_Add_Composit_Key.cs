using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Composit_Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccomodationAvailabilities_Rooms_RoomId",
                table: "AccomodationAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportationsAvailabilities_Seats_SeatId",
                table: "TransportationsAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_TransportationsAvailabilities_SeatId",
                table: "TransportationsAvailabilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TransportationId",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_AccomodationAvailabilities_RoomId",
                table: "AccomodationAvailabilities");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Transportations");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Transportations");

            migrationBuilder.AddColumn<Guid>(
                name: "TransportationId",
                table: "TransportationsAvailabilities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccomodationId",
                table: "AccomodationAvailabilities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                columns: new[] { "TransportationId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                columns: new[] { "AccomodationId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportationsAvailabilities_TransportationId_SeatId",
                table: "TransportationsAvailabilities",
                columns: new[] { "TransportationId", "SeatId" });

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationAvailabilities_AccomodationId_RoomId",
                table: "AccomodationAvailabilities",
                columns: new[] { "AccomodationId", "RoomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccomodationAvailabilities_Rooms_AccomodationId_RoomId",
                table: "AccomodationAvailabilities",
                columns: new[] { "AccomodationId", "RoomId" },
                principalTable: "Rooms",
                principalColumns: new[] { "AccomodationId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportationsAvailabilities_Seats_TransportationId_SeatId",
                table: "TransportationsAvailabilities",
                columns: new[] { "TransportationId", "SeatId" },
                principalTable: "Seats",
                principalColumns: new[] { "TransportationId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccomodationAvailabilities_Rooms_AccomodationId_RoomId",
                table: "AccomodationAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportationsAvailabilities_Seats_TransportationId_SeatId",
                table: "TransportationsAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_TransportationsAvailabilities_TransportationId_SeatId",
                table: "TransportationsAvailabilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_AccomodationAvailabilities_AccomodationId_RoomId",
                table: "AccomodationAvailabilities");

            migrationBuilder.DropColumn(
                name: "TransportationId",
                table: "TransportationsAvailabilities");

            migrationBuilder.DropColumn(
                name: "AccomodationId",
                table: "AccomodationAvailabilities");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FromDate",
                table: "Transportations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ToDate",
                table: "Transportations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationsAvailabilities_SeatId",
                table: "TransportationsAvailabilities",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TransportationId",
                table: "Seats",
                column: "TransportationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccomodationId",
                table: "Rooms",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationAvailabilities_RoomId",
                table: "AccomodationAvailabilities",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccomodationAvailabilities_Rooms_RoomId",
                table: "AccomodationAvailabilities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportationsAvailabilities_Seats_SeatId",
                table: "TransportationsAvailabilities",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
