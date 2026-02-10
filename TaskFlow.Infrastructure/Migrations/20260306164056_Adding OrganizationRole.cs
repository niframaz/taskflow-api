using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingOrganizationRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgRoles",
                table: "OrganizationUserRoles");

            migrationBuilder.CreateTable(
                name: "OrganizationRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    OrganizationUserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationRole_OrganizationUserRoles_OrganizationUserRoleId",
                        column: x => x.OrganizationUserRoleId,
                        principalTable: "OrganizationUserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRole_OrganizationUserRoleId",
                table: "OrganizationRole",
                column: "OrganizationUserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationRole");

            migrationBuilder.AddColumn<string>(
                name: "OrgRoles",
                table: "OrganizationUserRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
