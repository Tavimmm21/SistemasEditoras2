using System.Globalization;

namespace SistemasEditoras.Helpers;

/// <summary>Leitura e validacao dos campos numericos do formulario de livros.</summary>
public static class Validacao
{
	/// <summary>Ano de publicacao: somente digitos, de 1 ate o ano que vem.</summary>
	public static bool TentarLerAno(string? texto, out int ano)
	{
		ano = 0;
		var limpo = texto?.Trim() ?? string.Empty;

		return limpo.Length is > 0 and <= 4
			&& int.TryParse(limpo, NumberStyles.None, CultureInfo.InvariantCulture, out ano)
			&& ano >= 1
			&& ano <= DateTime.Now.Year + 1;
	}

	/// <summary>ISBN: aceita hifens/espacos, somente digitos, ate 14 digitos (decimal(14)).</summary>
	public static bool TentarLerIsbn(string? texto, out decimal isbn)
	{
		isbn = 0;
		var limpo = (texto ?? string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);

		return limpo.Length is > 0 and <= 14
			&& decimal.TryParse(limpo, NumberStyles.None, CultureInfo.InvariantCulture, out isbn)
			&& isbn > 0;
	}
}
