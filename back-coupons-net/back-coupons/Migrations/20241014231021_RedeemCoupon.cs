using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_coupons.Migrations
{
    /// <inheritdoc />
    public partial class RedeemCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RedeemCoupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateRedeem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CouponId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedeemCoupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedeemCoupons_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RedeemCoupons_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedeemCoupons_ClientId",
                table: "RedeemCoupons",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RedeemCoupons_CouponId",
                table: "RedeemCoupons",
                column: "CouponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedeemCoupons");
        }
    }
}
