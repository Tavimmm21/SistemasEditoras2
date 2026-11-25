using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class AutorAlterar : ContentPage
{
	private readonly AutorRepositorio _autores;
	private readonly Autor _autor;

	public AutorAlterar(AutorRepositorio autores, Autor autor)
	{
		InitializeComponent();
		_autores = autores;
		_autor = autor;

		// Pre-carrega os dados do item
		IdEntry.Text = autor.Id.ToString();
		NomeEntry.Text = autor.Nome;
		PseudonimoEntry.Text = autor.Pseudonimo;
		ObservacoesEditor.Text = autor.Observacoes;
	}

	private async void OnSalvarClicked(object? sender, EventArgs e)
	{
		var nome = NomeEntry.Text?.Trim() ?? string.Empty;
		var pseudonimo = PseudonimoEntry.Text?.Trim() ?? string.Empty;

		if (nome.Length == 0)
		{
			await DisplayAlert("Campo obrigatório", "Informe o nome do autor.", "OK");
			NomeEntry.Focus();
			return;
		}

		if (pseudonimo.Length == 0)
		{
			await DisplayAlert("Campo obrigatório", "Informe o pseudônimo do autor.", "OK");
			PseudonimoEntry.Focus();
			return;
		}

		_autor.Nome = nome;
		_autor.Pseudonimo = pseudonimo;
		_autor.Observacoes = ObservacoesEditor.Text?.Trim() ?? string.Empty;

		try
		{
			await _autores.AlterarAsync(_autor);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível gravar: {ex.Message}", "OK");
			return;
		}

		await DisplayAlert("Sucesso", "Autor alterado com sucesso!", "OK");
		await Navigation.PopAsync();
	}

	private async void OnVoltarClicked(object? sender, EventArgs e)
		=> await Navigation.PopAsync();
}
