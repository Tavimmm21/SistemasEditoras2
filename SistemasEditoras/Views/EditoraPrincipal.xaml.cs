using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class EditoraPrincipal : ContentPage
{
	private readonly EditoraRepositorio _editoras;
	private List<Editora> _todas = new();

	public EditoraPrincipal(EditoraRepositorio editoras)
	{
		InitializeComponent();
		_editoras = editoras;
	}

	// Recarrega a lista toda vez que a tela aparece (inclusive ao voltar do Inserir/Alterar).
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await CarregarAsync();
	}

	private async Task CarregarAsync()
	{
		try
		{
			_todas = await _editoras.ListarAsync();
			AplicarFiltro();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível carregar as editoras: {ex.Message}", "OK");
		}
	}

	private void AplicarFiltro()
	{
		var termo = BuscaSearchBar.Text?.Trim() ?? string.Empty;

		IEnumerable<Editora> resultado = _todas;
		if (termo.Length > 0)
		{
			resultado = _todas.Where(e =>
				Contem(e.Nome, termo) ||
				Contem(e.Sigla, termo) ||
				Contem(e.Observacoes, termo));
		}

		EditorasCollectionView.ItemsSource = resultado.ToList();
	}

	private static bool Contem(string? texto, string termo)
		=> texto?.Contains(termo, StringComparison.CurrentCultureIgnoreCase) ?? false;

	private void OnBuscaTextChanged(object? sender, TextChangedEventArgs e) => AplicarFiltro();

	private async void OnInserirClicked(object? sender, EventArgs e)
		=> await Navigation.PushAsync(new EditoraInserir(_editoras));

	private async void OnAlterarClicked(object? sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not Editora editora)
			return;

		await Navigation.PushAsync(new EditoraAlterar(_editoras, editora));
	}

	private async void OnExcluirClicked(object? sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not Editora editora)
			return;

		try
		{
			// Regra de FK: nao deixa orfaos em tbllivros.
			if (await _editoras.PossuiLivrosAsync(editora.Id))
			{
				await DisplayAlert("Não é possível excluir",
					$"A editora \"{editora.Nome}\" possui livros cadastrados. Exclua ou altere esses livros primeiro.",
					"OK");
				return;
			}

			var confirmou = await DisplayAlert("Confirmar exclusão",
				$"Deseja realmente excluir a editora \"{editora.Nome}\"?",
				"Sim", "Não");

			if (!confirmou)
				return;

			await _editoras.ExcluirAsync(editora);
			await CarregarAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível excluir: {ex.Message}", "OK");
		}
	}
}
