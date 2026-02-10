using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingOrganizationRoleback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationUserRoles_OrganizationId",
                table: "OrganizationUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationRole_OrganizationUserRoleId",
                table: "OrganizationRole");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRoles_OrganizationId_UserId",
                table: "OrganizationUserRoles",
                columns: new[] { "OrganizationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRole_OrganizationUserRoleId_Role",
                table: "OrganizationRole",
                columns: new[] { "OrganizationUserRoleId", "Role" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationUserRoles_OrganizationId_UserId",
                table: "OrganizationUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationRole_OrganizationUserRoleId_Role",
                table: "OrganizationRole");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRoles_OrganizationId",
                table: "OrganizationUserRoles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRole_OrganizationUserRoleId",
                table: "OrganizationRole",
                column: "OrganizationUserRoleId");
        }
    }
}
