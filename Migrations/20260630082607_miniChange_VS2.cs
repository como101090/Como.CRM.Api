using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Como.CRM.Api.Migrations
{
    /// <inheritdoc />
    public partial class miniChange_VS2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PublisherMailInfo",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PublisherMailInfo",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "xojljswkfupixqig");
        }
    }
}
