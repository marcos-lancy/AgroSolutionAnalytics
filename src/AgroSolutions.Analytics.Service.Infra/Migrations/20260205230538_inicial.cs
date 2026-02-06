using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSolutions.Analytics.Service.Infra.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TalhaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoAlerta = table.Column<int>(type: "integer", nullable: false),
                    Mensagem = table.Column<string>(type: "varchar(100)", nullable: true),
                    DataOcorrencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Resolvido = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");
        }
    }
}
