using SistemasEditoras.Models;

namespace SistemasEditoras.Dados;

public class AutorRepositorio
{
	private readonly DatabaseService _databaseService;

	public AutorRepositorio(DatabaseService databaseService)
		=> _databaseService = databaseService;

	public async Task<List<Autor>> ListarAsync()
	{
		var conexao = await _databaseService.GetConnectionAsync();
		var lista = await conexao.Table<Autor>().ToListAsync();
		return lista.OrderBy(x => x.Nome, StringComparer.CurrentCultureIgnoreCase).ToList();
	}

	public async Task<int> ContarAsync()
	{
		var conexao = await _databaseService.GetConnectionAsync();
		return await conexao.Table<Autor>().CountAsync();
	}

	/// <summary>Informa se ha livros ligados a este autor (FK autid em tbllivros).</summary>
	public async Task<bool> PossuiLivrosAsync(int autorId)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		var total = await conexao.Table<Livro>().Where(l => l.AutorId == autorId).CountAsync();
		return total > 0;
	}

	public async Task InserirAsync(Autor item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.InsertAsync(item);
	}

	public async Task AlterarAsync(Autor item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.UpdateAsync(item);
	}

	public async Task ExcluirAsync(Autor item)
	{
		var conexao = await _databaseService.GetConnectionAsync();
		await conexao.DeleteAsync(item);
	}
}
