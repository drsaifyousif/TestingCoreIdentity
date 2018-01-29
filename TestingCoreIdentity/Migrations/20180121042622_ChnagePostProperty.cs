using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TestingCoreIdentity.Migrations
{
    public partial class ChnagePostProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Posts",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Posts",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}
