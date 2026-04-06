using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace annons_web.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNumbersToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_annonsorer",
                columns: table => new
                {
                    advertiser_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_nr = table.Column<string>(type: "text", nullable: false),
                    postcode = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    delivery_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    corporate_reg_nr = table.Column<string>(type: "text", nullable: true),
                    invoice_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    advertiser_type = table.Column<int>(type: "integer", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_annonsorer_pkey", x => x.advertiser_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ads",
                columns: table => new
                {
                    ad_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ad_content = table.Column<string>(type: "text", nullable: false),
                    ad_price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    ad_fee = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    ad_advertiser_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tbl_ads_pkey", x => x.ad_id);
                    table.ForeignKey(
                        name: "tbl_ads_annonsorer_id_fkey",
                        column: x => x.ad_advertiser_id,
                        principalTable: "tbl_annonsorer",
                        principalColumn: "advertiser_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ads_ad_advertiser_id",
                table: "tbl_ads",
                column: "ad_advertiser_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_ads");

            migrationBuilder.DropTable(
                name: "tbl_annonsorer");
        }
    }
}
