using Microsoft.IdentityModel.Tokens;

namespace EmployeeManager;

public class ConsoleView
{
	public static void ShowFirstMenu()
	{
		Console.Write("Veuillez choisir une action:" +
		              "\n  (1) Affichez toutes les employé(e)s" +
		              "\n  (2) Ajouter un(e) employé(e)" +
		              "\n  (3) Modifier un(e) employé(e)" +
		              "\n  (4) Supprimer un(e) employé(e)" +
		              "\n  (5) Quitter" +
		              "\n\nVotre choix: "
		);
	}

	public static Dictionary<string, string> ShowInsertMenu()
	{
		Dictionary<string, string> values = new Dictionary<string, string>
		{
			{ "firstName", AskQuestion("Veuillez rentré le prénom: ") },
			{ "lastName", AskQuestion("Veuillez rentré le nom de famille: ") },
			{ "hireDate", AskQuestion("Veuillez rentré la date d'embauche: ", "1970/01/01") },
			{ "salary", AskQuestion("Veuillez rentré le salaire: ", "0") },
			{ "department", AskQuestion("Veuillez rentré le secteur: ") }
		};

		return values;
	}

	public static string AskQuestion(string title, string defaultValue = "")
	{
		Console.Write(title);
		var result = Console.ReadLine();
		return (result.IsNullOrEmpty()) ? defaultValue : result!;
	}
}