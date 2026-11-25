using SQLite;

namespace SistemasEditoras.Models;

/// <summary>Tabela tbleditoras do diagrama.</summary>
[Table("tbleditoras")]
public class Editora
{
	[PrimaryKey, AutoIncrement, Column("edid")]
	public int Id { get; set; }

	[Column("edinome"), MaxLength(50), NotNull]
	public string Nome { get; set; } = string.Empty;

	[Column("edisigla"), MaxLength(25), NotNull]
	public string Sigla { get; set; } = string.Empty;

	[Column("edobservacoes"), MaxLength(500), NotNull]
	public string Observacoes { get; set; } = string.Empty;

	// ALTERACAO OBRIGATORIA (comunicado): novo campo Data em tbleditoras.
	// Sem [NotNull] de proposito: assim o sqlite-net consegue adicionar a coluna
	// em um banco que ja existia (ALTER TABLE nao aceita NOT NULL sem valor padrao).
	[Column("eddata")]
	public DateTime Data { get; set; } = DateTime.Today;

	// Somente exibicao (nao persistido)
	[Ignore]
	public bool TemObservacoes => !string.IsNullOrWhiteSpace(Observacoes);
}
