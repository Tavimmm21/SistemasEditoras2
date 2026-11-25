using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class LivroPrincipal : ContentPage
{
	private readonly LivroRepositorio _livros;
	private readonly EditoraRepositorio _editoras;
	private readonly AutorRepositorio _autores;
	private List<Livro> _todos = new();

	public LivroPrincipal(LivroRepositorio livros, EditoraRepositorio editoras, AutorRepositorio autores)
	{
		InitializeComponent();
		_livros = livros;
		_editoras = editoras;
		_autores = autores;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await CarregarAsync();
	}

	private async Task CarregarAsync()
	{
		try
		{
			_todos = await _livros.ListarAsync();
			AplicarFiltro();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível carregar os livros: {ex.Message}", "OK");
		}
	}

	private void AplicarFiltro()
	{
		var termo = BuscaSearchBar.Text?.Trim() ?? string.Empty;

		IEnumerable<Livro> resultado = _todos;
		if (termo.Length > 0)
		{
			resultado = _todos.Where(l =>
				Contem(l.Nome, termo) ||
				Contem(l.AutorNome, termo) ||
				Contem(l.EditoraNome, termo) ||
				Contem(l.AnoPublicacao.ToString(), termo) ||
				Contem(l.Isbn.ToString("0"), termo) ||
				Contem(l.Observacoes, termo));
		}

		LivrosCollectionView.ItemsSource = resultado.ToList();
	}

	private static bool Contem(string? texto, string termo)
		=> texto?.Contains(termo, StringComparison.CurrentCultureIgnoreCase) ?? false;

	private void OnBuscaTextChanged(object? sender, TextChangedEventArgs e) => AplicarFiltro();

	private async void OnInserirClicked(object? sender, EventArgs e)
	{
		try
		{
			// Regra de FK: um livro depende de uma editora E de um autor.
			// Bloqueia o cadastro enquanto alguma dessas tabelas estiver vazia.
			if (await _editoras.ContarAsync() == 0)
			{
				await DisplayAlert("Cadastro bloqueado",
					"Cadastre pelo menos uma editora antes de inserir um livro.",
					"OK");
				return;
			}

			if (await _autores.ContarAsync() == 0)
			{
				await DisplayAlert("Cadastro bloqueado",
					"Cadastre pelo menos um autor antes de inserir um livro.",
					"OK");
				return;
			}

			await Navigation.PushAsync(new LivroInserir(_livros, _editoras, _autores));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível abrir o cadastro: {ex.Message}", "OK");
		}
	}

	private async void OnAlterarClicked(object? sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not Livro livro)
			return;

		await Navigation.PushAsync(new LivroAlterar(_livros, _editoras, _autores, livro));
	}

	private async void OnExcluirClicked(object? sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not Livro livro)
			return;

		var confirmou = await DisplayAlert("Confirmar exclusão",
			$"Deseja realmente excluir o livro \"{livro.Nome}\"?",
			"Sim", "Não");

		if (!confirmou)
			return;

		try
		{
			await _livros.ExcluirAsync(livro);
			await CarregarAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível excluir: {ex.Message}", "OK");
		}
	}
}
