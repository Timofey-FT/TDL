using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _2204Tag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tags_TagId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TagId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Notes");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "NoteId",
                table: "Tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TagId",
                table: "Notes",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tags_TagId",
                table: "Notes",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Notes_NoteId",
                table: "Tags",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id");
        }
    }
}
