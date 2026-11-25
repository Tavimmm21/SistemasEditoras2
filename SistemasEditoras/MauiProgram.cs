using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SistemasEditoras.Dados;
using SistemasEditoras.Views;

namespace SistemasEditoras;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>();

		// Camada de dados: um unico banco/conexao para o app inteiro.
		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<EditoraRepositorio>();
		builder.Services.AddSingleton<AutorRepositorio>();
		builder.Services.AddSingleton<LivroRepositorio>();

		// So as paginas que ficam registradas no Shell (o Shell resolve via DI).
		// Inserir/Alterar sao abertas com Navigation.PushAsync.
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<EditoraPrincipal>();
		builder.Services.AddTransient<AutorPrincipal>();
		builder.Services.AddTransient<LivroPrincipal>();
		builder.Services.AddTransient<DadosDesenvolvedor>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
