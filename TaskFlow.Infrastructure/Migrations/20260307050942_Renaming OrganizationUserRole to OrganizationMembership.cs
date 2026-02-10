using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamingOrganizationUserRoletoOrganizationMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRole_OrganizationUserRoles_OrganizationUserRoleId",
                table: "OrganizationRole");

            migrationBuilder.DropTable(
                name: "OrganizationUserRoles");

            migrationBuilder.RenameColumn(
                name: "OrganizationUserRoleId",
                table: "OrganizationRole",
                newName: "OrganizationMembershipId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationRole_OrganizationUserRoleId_Role",
                table: "OrganizationRole",
                newName: "IX_OrganizationRole_OrganizationMembershipId_Role");

            migrationBuilder.CreateTable(
                name: "OrganizationMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationMemberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationMemberships_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMemberships_OrganizationId_UserId",
                table: "OrganizationMemberships",
                columns: new[] { "OrganizationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMemberships_UserId",
                table: "OrganizationMemberships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationRole_OrganizationMemberships_OrganizationMembershipId",
                table: "OrganizationRole",
                column: "OrganizationMembershipId",
                principalTable: "OrganizationMemberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRole_OrganizationMemberships_OrganizationMembershipId",
                table: "OrganizationRole");

            migrationBuilder.DropTable(
                name: "OrganizationMemberships");

            migrationBuilder.RenameColumn(
                name: "OrganizationMembershipId",
                table: "OrganizationRole",
                newName: "OrganizationUserRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationRole_OrganizationMembershipId_Role",
                table: "OrganizationRole",
                newName: "IX_OrganizationRole_OrganizationUserRoleId_Role");

            migrationBuilder.CreateTable(
                name: "OrganizationUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationUserRoles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRoles_OrganizationId_UserId",
                table: "OrganizationUserRoles",
                columns: new[] { "OrganizationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUserRoles_UserId",
                table: "OrganizationUserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationRole_OrganizationUserRoles_OrganizationUserRoleId",
                table: "OrganizationRole",
                column: "OrganizationUserRoleId",
                principalTable: "OrganizationUserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
