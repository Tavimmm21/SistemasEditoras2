using SistemasEditoras.Dados;
using SistemasEditoras.Helpers;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class LivroInserir : ContentPage
{
	private readonly LivroRepositorio _livros;
	private readonly EditoraRepositorio _editoras;
	private readonly AutorRepositorio _autores;
	private bool _carregado;

	public LivroInserir(LivroRepositorio livros, EditoraRepositorio editoras, AutorRepositorio autores)
	{
		InitializeComponent();
		_livros = livros;
		_editoras = editoras;
		_autores = autores;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_carregado)
			return;

		try
		{
			EditoraPicker.ItemsSource = await _editoras.ListarAsync();
			AutorPicker.ItemsSource = await _autores.ListarAsync();
			_carregado = true;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível carregar editoras/autores: {ex.Message}", "OK");
		}
	}

	private async void OnSalvarClicked(object? sender, EventArgs e)
	{
		var nome = NomeEntry.Text?.Trim() ?? string.Empty;

		if (nome.Length == 0)
		{
			await DisplayAlert("Campo obrigatório", "Informe o título do livro.", "OK");
			NomeEntry.Focus();
			return;
		}

		if (!Validacao.TentarLerAno(AnoEntry.Text, out var ano))
		{
			await DisplayAlert("Campo inválido",
				$"Informe um ano de publicação válido (de 1 a {DateTime.Now.Year + 1}).",
				"OK");
			AnoEntry.Focus();
			return;
		}

		if (!Validacao.TentarLerIsbn(IsbnEntry.Text, out var isbn))
		{
			await DisplayAlert("Campo inválido",
				"Informe o ISBN usando somente números (até 14 dígitos).",
				"OK");
			IsbnEntry.Focus();
			return;
		}

		if (EditoraPicker.SelectedItem is not Editora editora)
		{
			await DisplayAlert("Campo obrigatório", "Selecione a editora.", "OK");
			return;
		}

		if (AutorPicker.SelectedItem is not Autor autor)
		{
			await DisplayAlert("Campo obrigatório", "Selecione o autor.", "OK");
			return;
		}

		var novo = new Livro
		{
			Nome = nome,
			AnoPublicacao = ano,
			Isbn = isbn,
			EditoraId = editora.Id,
			AutorId = autor.Id,
			Observacoes = ObservacoesEditor.Text?.Trim() ?? string.Empty
		};

		try
		{
			await _livros.InserirAsync(novo);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível gravar: {ex.Message}", "OK");
			return;
		}

		await DisplayAlert("Sucesso", "Livro gravado com sucesso!", "OK");
		await Navigation.PopAsync();
	}

	private async void OnVoltarClicked(object? sender, EventArgs e)
		=> await Navigation.PopAsync();
}
