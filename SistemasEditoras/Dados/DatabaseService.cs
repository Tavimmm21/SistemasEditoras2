using Microsoft.Maui.Storage;
using SistemasEditoras.Models;
using SQLite;

namespace SistemasEditoras.Dados;

/// <summary>
/// Cuida somente da conexao com o banco SQLite e da criacao das tabelas.
/// </summary>
public class DatabaseService
{
	private const string NomeDoBanco = "editoras.db3";

	private readonly SemaphoreSlim _trava = new(1, 1);
	private SQLiteAsyncConnection? _connection;

	public async Task<SQLiteAsyncConnection> GetConnectionAsync()
	{
		if (_connection is not null)
			return _connection;

		// A trava evita duas conexoes/criacoes de tabela ao mesmo tempo.
		await _trava.WaitAsync();
		try
		{
			if (_connection is not null)
				return _connection;

			var dbPath = Path.Combine(FileSystem.AppDataDirectory, NomeDoBanco);
			var conexao = new SQLiteAsyncConnection(dbPath);

			await conexao.CreateTableAsync<Editora>();
			await conexao.CreateTableAsync<Autor>();
			await conexao.CreateTableAsync<Livro>();

			_connection = conexao;
			return _connection;
		}
		finally
		{
			_trava.Release();
		}
	}
}
