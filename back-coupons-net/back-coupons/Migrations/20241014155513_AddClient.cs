using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_coupons.Migrations
{
    /// <inheritdoc />
    public partial class AddClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailCoupon_Coupons_CouponId",
                table: "DetailCoupon");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCoupon_Products_ProductId",
                table: "DetailCoupon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailCoupon",
                table: "DetailCoupon");

            migrationBuilder.RenameTable(
                name: "DetailCoupon",
                newName: "DetailCoupons");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCoupon_ProductId",
                table: "DetailCoupons",
                newName: "IX_DetailCoupons_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCoupon_CouponId",
                table: "DetailCoupons",
                newName: "IX_DetailCoupons_CouponId");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailCoupons",
                table: "DetailCoupons",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identification = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Identification",
                table: "Clients",
                column: "Identification",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCoupons_Coupons_CouponId",
                table: "DetailCoupons",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCoupons_Products_ProductId",
                table: "DetailCoupons",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailCoupons_Coupons_CouponId",
                table: "DetailCoupons");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCoupons_Products_ProductId",
                table: "DetailCoupons");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailCoupons",
                table: "DetailCoupons");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Coupons");

            migrationBuilder.RenameTable(
                name: "DetailCoupons",
                newName: "DetailCoupon");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCoupons_ProductId",
                table: "DetailCoupon",
                newName: "IX_DetailCoupon_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCoupons_CouponId",
                table: "DetailCoupon",
                newName: "IX_DetailCoupon_CouponId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailCoupon",
                table: "DetailCoupon",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCoupon_Coupons_CouponId",
                table: "DetailCoupon",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCoupon_Products_ProductId",
                table: "DetailCoupon",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
