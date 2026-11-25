namespace SistemasEditoras.Views;

public partial class DadosDesenvolvedor : ContentPage
{
	// ===================================================================
	// >>> ALTERE AQUI: preencha com os seus dados antes de gravar o video <<<
	// ===================================================================
	private const string NomeDesenvolvedor = "SEU NOME COMPLETO";
	private const string RaDesenvolvedor = "SEU RA";
	private const string DataDaAvaliacao = "DD/MM/AAAA";

	public DadosDesenvolvedor()
	{
		InitializeComponent();

		AppLabel.Text = AppInfo.Current.Name;
		NomeLabel.Text = $"Desenvolvedor: {NomeDesenvolvedor}";
		RaLabel.Text = $"RA: {RaDesenvolvedor}";
		DataAvaliacaoLabel.Text = $"Data da avaliação: {DataDaAvaliacao}";
	}

	private async void OnHomeClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("//MainPage");
}
