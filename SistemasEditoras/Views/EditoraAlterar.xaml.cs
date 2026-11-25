using SistemasEditoras.Dados;
using SistemasEditoras.Models;

namespace SistemasEditoras.Views;

public partial class EditoraAlterar : ContentPage
{
	private readonly EditoraRepositorio _editoras;
	private readonly Editora _editora;

	public EditoraAlterar(EditoraRepositorio editoras, Editora editora)
	{
		InitializeComponent();
		_editoras = editoras;
		_editora = editora;

		// Pre-carrega os dados do item
		IdEntry.Text = editora.Id.ToString();
		NomeEntry.Text = editora.Nome;
		SiglaEntry.Text = editora.Sigla;
		ObservacoesEditor.Text = editora.Observacoes;
	}

	private async void OnSalvarClicked(object? sender, EventArgs e)
	{
		var nome = NomeEntry.Text?.Trim() ?? string.Empty;
		var sigla = SiglaEntry.Text?.Trim() ?? string.Empty;

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

		_editora.Nome = nome;
		_editora.Sigla = sigla;
		_editora.Observacoes = ObservacoesEditor.Text?.Trim() ?? string.Empty;

		try
		{
			await _editoras.AlterarAsync(_editora);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Não foi possível gravar: {ex.Message}", "OK");
			return;
		}

		await DisplayAlert("Sucesso", "Editora alterada com sucesso!", "OK");
		await Navigation.PopAsync();
	}

	private async void OnVoltarClicked(object? sender, EventArgs e)
		=> await Navigation.PopAsync();
}
