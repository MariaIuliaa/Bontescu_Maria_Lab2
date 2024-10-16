using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bontescu_Maria_Lab2.Migrations
{
    public partial class AddAuthorsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Ștergerea coloanei vechi `Author` din tabela `Book`
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Book");

            // 2. Adăugarea coloanei `AuthorID` ca Foreign Key în tabela `Book`
            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 3. Crearea tabelei `Author` cu coloanele `ID`, `FirstName`, `LastName`
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.ID);
                });

            // 4. Crearea unui index pe coloana `AuthorID` din tabela `Book`
            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorID",
                table: "Book",
                column: "AuthorID");

            // 5. Crearea relației Foreign Key între `Book` și `Author`
            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorID",
                table: "Book",
                column: "AuthorID",
                principalTable: "Author",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

    }
}
