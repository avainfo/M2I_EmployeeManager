using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManager;

public class ConsolePanel
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

	public static void ShowModificationPanel(SqlConnection con, DbConnection db)
	{
		string idStr = AskQuestion("Veuillez rentrer l'ID: ");
		int id;

		try
		{
			id = int.Parse(idStr);
		}
		catch (Exception e)
		{
			return;
		}

		// results = "1, 3,      5, a,ae, 5,"
		string results = AskQuestion(
			"Selectionnez les valeurs que vous souhaitez modifier: " +
			"\n  (1) First Name" +
			"\n  (2) Last Name" +
			"\n  (3) Hire Date" +
			"\n  (4) Salary" +
			"\n  (5) Department" +
			"\n(Veuillez séparer les réponses grâce à une virugle !)" +
			"\nSaisir votre choix: "
		);

		// results = "1,3,5,a,ae,5,"
		results = results.Replace(" ", "");

		// selectedOptions = ["1", "3", "5", "a", "ae", "5", ""] (Tableau de string)
		List<string> selectedOptions = results.Split(",").ToList();

		// [1, 3, 5]
		HashSet<int> filteredOptions = new HashSet<int>();

		foreach (string option in selectedOptions)
		{
			if (option.IsNullOrEmpty() || !char.IsDigit(option, 0)) continue;

			int x = int.Parse(option[0].ToString());
			if (x is > 0 and < 6)
			{
				filteredOptions.Add(x);
			}
		}
		
		List<string> options =
		[
			"le prénom",
			"le nom de famille ",
			"la date d'embauche",
			"le salaire",
			"le département",
		];

		List<string> selectedValues = [];
		
		foreach (int option in filteredOptions)
		{
			selectedValues.Add(AskQuestion($"Veuillez rentrer {options[option - 1]}: "));
		}

		db.UpdateData(con, id, filteredOptions.ToArray(), selectedValues.ToArray());
	}
}