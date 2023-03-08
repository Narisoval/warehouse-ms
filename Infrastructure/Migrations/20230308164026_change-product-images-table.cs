using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeproductimagestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
            
           migrationBuilder.Sql( @"WITH UpdatedImages AS (
                    UPDATE ""Products""
                    SET ""MainImage"" = ""ProductImages"".""Image""
                    FROM ""ProductImages""
                    WHERE ""ProductImages"".""ProductId"" = ""Products"".""Id"" AND ""ProductImages"".""IsMain"" = true
                    RETURNING ""ProductImages"".""Id""
                )
                DELETE FROM ""ProductImages""
                WHERE ""Id"" IN (SELECT ""Id"" FROM UpdatedImages);"); 
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ProductImages");


            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductImages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ProductImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                columns: new[] { "Id", "ProductId" });
        }
    }
}
