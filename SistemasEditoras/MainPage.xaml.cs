namespace SistemasEditoras;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnEditorasClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("//EditoraPrincipal");

	private async void OnAutoresClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("//AutorPrincipal");

	private async void OnLivrosClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("//LivroPrincipal");

	private async void OnDesenvolvedorClicked(object? sender, EventArgs e)
		=> await Shell.Current.GoToAsync("//DadosDesenvolvedor");
}
