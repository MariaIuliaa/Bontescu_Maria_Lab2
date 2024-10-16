using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bontescu_Maria_Lab2.Migrations
{
    public partial class AddAuthorIDToBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adaugă coloana AuthorID la tabelul Book
            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "Book",
                type: "int",
                nullable: true);

            // Creează indexul pentru coloana AuthorID
            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorID",
                table: "Book",
                column: "AuthorID");

            // Adaugă constrângerea Foreign Key pentru AuthorID în tabelul Book
            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorID",
                table: "Book",
                column: "AuthorID",
                principalTable: "Author",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Șterge constrângerea Foreign Key
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Author_AuthorID",
                table: "Book");

            // Șterge indexul pentru AuthorID
            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorID",
                table: "Book");

            // Șterge coloana AuthorID din tabelul Book
            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Book");
        }
    }
}
