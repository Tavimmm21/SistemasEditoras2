using SistemasEditoras.Models;

namespace SistemasEditoras.Dados;

public class EditoraRepositorio
{
	private readonly DatabaseService _databaseService;

	public EditoraRepositorio(DatabaseService databaseService)
		=> _databaseService = databaseService;

	public async Task<List<Editora>> ListarAsync()
	{
		var conexao = await _databaseService.GetConnectionAsync();
		var lista = await conexao.Table<Editora>().ToListAsync();
		return lista.OrderBy(x => x.Nome, StringComparer.CurrentCultureIgnoreCase).ToList();
	}

	public async Task<int> ContarAsync()
	{
		var conexao = await _databaseService.GetConnectionAsync();
		return await conexao.Table<Editora>().CountAsync();
	}

	/// <summary>Informa se ha livros ligados a esta editora (FK edid em tbllivros).</summary>
	public async Task<bool> PossuiLivrosAsync(int editoraId)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		var total = await conexao.Table<Livro>().Where(l => l.EditoraId == editoraId).CountAsync();
		return total > 0;
	}

	public async Task InserirAsync(Editora item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.InsertAsync(item);
	}

	public async Task AlterarAsync(Editora item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.UpdateAsync(item);
	}

	public async Task ExcluirAsync(Editora item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.DeleteAsync(item);
	}
}
