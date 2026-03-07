using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Simplifyingservices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRole_OrganizationMemberships_OrganizationMembershipId",
                table: "OrganizationRole");

            migrationBuilder.DropTable(
                name: "OrganizationMemberships");

            migrationBuilder.RenameColumn(
                name: "OrganizationMembershipId",
                table: "OrganizationRole",
                newName: "MembershipId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationRole_OrganizationMembershipId_Role",
                table: "OrganizationRole",
                newName: "IX_OrganizationRole_MembershipId_Role");

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Sample organization for seeding", "Acme Corp" });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_OrganizationId_UserId",
                table: "Memberships",
                columns: new[] { "OrganizationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationRole_Memberships_MembershipId",
                table: "OrganizationRole",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationRole_Memberships_MembershipId",
                table: "OrganizationRole");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "OrganizationRole",
                newName: "OrganizationMembershipId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationRole_MembershipId_Role",
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
    }
}
