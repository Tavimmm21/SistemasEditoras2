using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class AutorInserir : ContentPage
{
	private readonly AutorRepositorio _autores;

	public AutorInserir(AutorRepositorio autores)
	{
		InitializeComponent();
		_autores = autores;
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

		var novo = new Autor
		{
			Nome = nome,
			Pseudonimo = pseudonimo,
			Observacoes = ObservacoesEditor.Text?.Trim() ?? string.Empty
		};

		try
		{
			await _autores.InserirAsync(novo);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível gravar: {ex.Message}", "OK");
			return;
		}

		await DisplayAlert("Sucesso", "Autor gravado com sucesso!", "OK");
		await Navigation.PopAsync();
	}

	private async void OnVoltarClicked(object? sender, EventArgs e)
		=> await Navigation.PopAsync();
}
