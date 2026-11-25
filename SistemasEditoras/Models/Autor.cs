using SQLite;

namespace SistemasEditoras.Models;

/// <summary>Tabela tblautores do diagrama.</summary>
[Table("tblautores")]
public class Autor
{
	[PrimaryKey, AutoIncrement, Column("autid")]
	public int Id { get; set; }

	[Column("autnome"), MaxLength(50), NotNull]
	public string Nome { get; set; } = string.Empty;

	// Atencao: o nome da coluna no diagrama e "aupseudonimo" (sem o "t").
	[Column("aupseudonimo"), MaxLength(50), NotNull]
	public string Pseudonimo { get; set; } = string.Empty;

	[Column("autobservacoes"), MaxLength(500), NotNull]
	public string Observacoes { get; set; } = string.Empty;

	// Somente exibicao (nao persistido)
	[Ignore]
	public bool TemObservacoes => !string.IsNullOrWhiteSpace(Observacoes);
}
