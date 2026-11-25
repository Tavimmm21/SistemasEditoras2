using SQLite;

namespace SistemasEditoras.Models;

/// <summary>
/// Tabela tbllivros do diagrama.
/// Cada livro pertence a UMA editora (edid) e a UM autor (autid).
/// </summary>
[Table("tbllivros")]
public class Livro
{
	[PrimaryKey, AutoIncrement, Column("livid")]
	public int Id { get; set; }

	[Column("livnome"), MaxLength(50), NotNull]
	public string Nome { get; set; } = string.Empty;

	[Column("livanopublicacao"), NotNull]
	public int AnoPublicacao { get; set; }

	// decimal(14) no diagrama. O SQLite guarda como numero real, que representa
	// com exatidao inteiros de ate 14 digitos.
	[Column("livisbn"), NotNull]
	public decimal Isbn { get; set; }

	[Column("livobservacoes"), MaxLength(500), NotNull]
	public string Observacoes { get; set; } = string.Empty;

	// Chaves estrangeiras
	[Column("edid"), NotNull, Indexed]
	public int EditoraId { get; set; }

	[Column("autid"), NotNull, Indexed]
	public int AutorId { get; set; }

	// Somente exibicao (nao persistido): mostra o nome em vez do ID
	[Ignore]
	public string EditoraNome { get; set; } = string.Empty;

	[Ignore]
	public string AutorNome { get; set; } = string.Empty;

	[Ignore]
	public bool TemObservacoes => !string.IsNullOrWhiteSpace(Observacoes);
}
