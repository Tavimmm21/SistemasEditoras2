using SistemasEditoras.Models;

namespace SistemasEditoras.Dados;

public class LivroRepositorio
{
	private readonly DatabaseService _databaseService;

	public LivroRepositorio(DatabaseService databaseService)
		=> _databaseService = databaseService;

	/// <summary>
	/// Lista os livros ja com o nome da editora e do autor preenchidos
	/// (propriedades [Ignore], so para exibicao).
	/// </summary>
	public async Task<List<Livro>> ListarAsync()
	{
		var conexao = await _databaseService.GetConnectionAsync();

		var livros = await conexao.Table<Livro>().ToListAsync();
		var editoras = (await conexao.Table<Editora>().ToListAsync()).ToDictionary(e => e.Id, e => e.Nome);
		var autores = (await conexao.Table<Autor>().ToListAsync()).ToDictionary(a => a.Id, a => a.Nome);

		foreach (var livro in livros)
		{
			livro.EditoraNome = editoras.TryGetValue(livro.EditoraId, out var editora) ? editora : "(editora não encontrada)";
			livro.AutorNome = autores.TryGetValue(livro.AutorId, out var autor) ? autor : "(autor não encontrado)";
		}

		return livros.OrderBy(l => l.Nome, StringComparer.CurrentCultureIgnoreCase).ToList();
	}

	public async Task InserirAsync(Livro item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.InsertAsync(item);
	}

	public async Task AlterarAsync(Livro item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.UpdateAsync(item);
	}

	public async Task ExcluirAsync(Livro item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.DeleteAsync(item);
	}
}
