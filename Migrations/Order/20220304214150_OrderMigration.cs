using Microsoft.EntityFrameworkCore.Migrations;

namespace OutdoorsmanBackend.Migrations.Order
{
    public partial class OrderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    userId = table.Column<int>(type: "INTEGER", nullable: false),
                    firstName = table.Column<string>(type: "TEXT", nullable: true),
                    lastName = table.Column<string>(type: "TEXT", nullable: true),
                    streetAddress = table.Column<string>(type: "TEXT", nullable: true),
                    city = table.Column<string>(type: "TEXT", nullable: true),
                    state = table.Column<string>(type: "TEXT", nullable: true),
                    zipcode = table.Column<string>(type: "TEXT", nullable: true),
                    orderProducts = table.Column<string>(type: "TEXT", nullable: true),
                    creditNumber = table.Column<string>(type: "TEXT", nullable: true),
                    expDate = table.Column<string>(type: "TEXT", nullable: true),
                    cvv = table.Column<string>(type: "TEXT", nullable: true),
                    totalPrice = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.orderId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
