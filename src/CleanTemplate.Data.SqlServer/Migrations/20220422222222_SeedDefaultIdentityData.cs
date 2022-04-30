using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanTemplate.Data.Migrations
{
    public partial class SeedDefaultIdentityData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { (short)1, "7a7276f0-7424-4ff0-b7fa-385019ae229d", "user", "USER" });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { (short)2, "772f3134-170b-419e-9d0b-a4e9c86c508f", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Description", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Registered", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1L, 0, "2d1fc03d-287d-45a9-9c18-02e9f13fc51a", null, "aspadmin@asp.net", true, false, null, "ASPADMIN@ASP.NET", "ASPADMIN", "AQAAAAEAACcQAAAAENXtjoPUXd578ktoFFQDW7mE2eCi2xFR1nZP/FGUhUIOCfiQKGWzUUOVS9uORKTHtA==", null, false, DateTime.UtcNow, "c3656fd4-cb84-459a-a1f2-0bdb4bdc1fdd", false, "aspadmin" });

            migrationBuilder.InsertData(
                table: "AppRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 1, "permission", "mt", (short)2 });

            migrationBuilder.InsertData(
                table: "AppUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "permission", "*", 1L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "AppUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
