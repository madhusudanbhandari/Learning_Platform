using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentUpdat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pidx",
                table: "Payments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pidx",
                table: "Payments");
        }
    }
}
