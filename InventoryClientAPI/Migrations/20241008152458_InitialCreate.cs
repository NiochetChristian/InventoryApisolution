using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryClientAPI.Migrations {
	/// <inheritdoc />
	public partial class InitialCreate : Migration {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.AlterDatabase()
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new {
					ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
					ProductName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					ProductionType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					ProductStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
					LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: true)
				},
				constraints: table => {
					table.PrimaryKey("PK_Products", x => x.ProductId);
				})
				.Annotation("MySql:CharSet", "utf8mb4");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "Products");
		}
	}
}
