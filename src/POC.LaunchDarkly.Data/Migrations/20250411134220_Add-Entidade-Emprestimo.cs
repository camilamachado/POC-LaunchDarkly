using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC.LaunchDarkly.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntidadeEmprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emprestimo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "Identificador do empréstimo"),
                    DataCriacao = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Data de criação do empréstimo"),
                    Status = table.Column<string>(type: "VARCHAR(20)", nullable: false, comment: "Status atual do empréstimo"),
                    CPF = table.Column<string>(type: "VARCHAR(11)", unicode: false, maxLength: 255, nullable: false, comment: "CPF do solicitante do empréstimo"),
                    ValorSolicitado = table.Column<decimal>(type: "numeric(18,2)", nullable: false, comment: "Valor solicitado para o empréstimo"),
                    PrazoEmMeses = table.Column<int>(type: "INT", nullable: false, comment: "Prazo de pagamento em meses"),
                    Finalidade = table.Column<string>(type: "VARCHAR(100)", unicode: false, maxLength: 255, nullable: false, comment: "Finalidade declarada para o empréstimo"),
                    NumeroConta = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, comment: "Número da conta vinculada ao empréstimo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emprestimo");
        }
    }
}
