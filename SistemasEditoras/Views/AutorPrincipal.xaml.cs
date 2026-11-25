using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class AutorPrincipal : ContentPage
{
	private readonly AutorRepositorio _autores;
	private List<Autor> _todos = new();

	public AutorPrincipal(AutorRepositorio autores)
	{
		InitializeComponent();
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
			_todos = await _autores.ListarAsync();
			AplicarFiltro();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível carregar os autores: {ex.Message}", "OK");
		}
	}

	private void AplicarFiltro()
	{
		var termo = BuscaSearchBar.Text?.Trim() ?? string.Empty;

		IEnumerable<Autor> resultado = _todos;
		if (termo.Length > 0)
		{
			resultado = _todos.Where(a =>
				Contem(a.Nome, termo) ||
				Contem(a.Pseudonimo, termo) ||
				Contem(a.Observacoes, termo));
		}

		AutoresCollectionView.ItemsSource = resultado.ToList();
	}

	private static bool Contem(string? texto, string termo)
		=> texto?.Contains(termo, StringComparison.CurrentCultureIgnoreCase) ?? false;

	private void OnBuscaTextChanged(object? sender, TextChangedEventArgs e) => AplicarFiltro();

	private async void OnInserirClicked(object? sender, EventArgs e)
		=> await Navigation.PushAsync(new AutorInserir(_autores));

	private async void OnAlterarClicked(object? sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not Autor autor)
			return;

		await Navigation.PushAsync(new AutorAlterar(_autores, autor));
	}

	private async void OnExcluirClicked(object? sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not Autor autor)
			return;

		try
		{
			// Regra de FK: nao deixa orfaos em tbllivros.
			if (await _autores.PossuiLivrosAsync(autor.Id))
			{
				await DisplayAlert("Não é possível excluir",
					$"O autor \"{autor.Nome}\" possui livros cadastrados. Exclua ou altere esses livros primeiro.",
					"OK");
				return;
			}

			var confirmou = await DisplayAlert("Confirmar exclusão",
				$"Deseja realmente excluir o autor \"{autor.Nome}\"?",
				"Sim", "Não");

			if (!confirmou)
				return;

			await _autores.ExcluirAsync(autor);
			await CarregarAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível excluir: {ex.Message}", "OK");
		}
	}
}
