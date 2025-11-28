using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFieldsToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Thêm cột AppUserId (thường là nullable, nvarchar(450) là chuẩn cho User ID)
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            // 2. Thêm cột Email (Bắt buộc, nên đặt defaultValue cho các hàng cũ nếu DB đã có dữ liệu)
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false, // Theo [Required] trong model
                defaultValue: ""); // Giá trị mặc định cho các hàng đã tồn tại

            // 3. Thêm cột PhoneNumber (Bắt buộc, MaxLength 15)
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false, // Theo [Required] trong model
                defaultValue: ""); // Giá trị mặc định cho các hàng đã tồn tại
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa các cột nếu thực hiện rollback (chuyển về trạng thái cũ)
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");
        }
    }
}