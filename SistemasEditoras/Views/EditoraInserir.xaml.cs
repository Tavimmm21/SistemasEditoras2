using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class EditoraInserir : ContentPage
{
	private readonly EditoraRepositorio _editoras;

	public EditoraInserir(EditoraRepositorio editoras)
	{
		InitializeComponent();
		_editoras = editoras;
	}

	private async void OnSalvarClicked(object? sender, EventArgs e)
	{
		var nome = NomeEntry.Text?.Trim() ?? string.Empty;
		var sigla = SiglaEntry.Text?.Trim() ?? string.Empty;

		// Validacao dos campos obrigatorios (bloqueia se invalido)
		if (nome.Length == 0)
		{
			await DisplayAlert("Campo obrigatório", "Informe o nome da editora.", "OK");
			NomeEntry.Focus();
			return;
		}

		if (sigla.Length == 0)
		{
			await DisplayAlert("Campo obrigatório", "Informe a sigla da editora.", "OK");
			SiglaEntry.Focus();
			return;
		}

		var nova = new Editora
		{
			Nome = nome,
			Sigla = sigla,
			Observacoes = ObservacoesEditor.Text?.Trim() ?? string.Empty
		};

		try
		{
			await _editoras.InserirAsync(nova);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível gravar: {ex.Message}", "OK");
			return;
		}

		await DisplayAlert("Sucesso", "Editora gravada com sucesso!", "OK");
		await Navigation.PopAsync();
	}

	private async void OnVoltarClicked(object? sender, EventArgs e)
		=> await Navigation.PopAsync();
}
