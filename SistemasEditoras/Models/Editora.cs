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

	// Somente exibicao (nao persistido)
	[Ignore]
	public bool TemObservacoes => !string.IsNullOrWhiteSpace(Observacoes);
}
