using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace datamodel.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COUNTRY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ISO2 = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    ISO3 = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUNTRY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CITY",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LAT = table.Column<double>(type: "float", nullable: false),
                    LONG = table.Column<double>(type: "float", nullable: false),
                    POPULATION = table.Column<int>(type: "int", nullable: false),
                    COUNTRY_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CITY_COUNTRY",
                        column: x => x.COUNTRY_ID,
                        principalTable: "COUNTRY",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CITY_COUNTRY_ID",
                table: "CITY",
                column: "COUNTRY_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CITY");

            migrationBuilder.DropTable(
                name: "COUNTRY");
        }
    }
}
