using System.Data;
using Microsoft.Data.SqlClient;

namespace EmployeeManager;

class Program
{
	private static async Task Main(string[] args)
	{
		// Creer la chaine de connexion avec les infos de login
		var builder = new SqlConnectionStringBuilder
		{
			DataSource = "localhost",
			UserID = "sa",
			Password = "root",
			InitialCatalog = "ExerciceDB",
			IntegratedSecurity = true,
			TrustServerCertificate = true
		};

		// Créer la connexion grâce à la chaine de connexion
		var connection = new SqlConnection(builder.ConnectionString);
		try
		{
			// Essayer de se connecter à la db 
			await connection.OpenAsync();
		}
		catch (Exception e)
		{
			// si il y a une exception nous affichons le message d'erreur
			Console.WriteLine(e.Message);
		}

		// Si l'état de la connexion n'est pas ouvert
		if (connection.State != ConnectionState.Open)
		{
			// nous affichons "Connection error" puis nous sortons du programme
			Console.WriteLine("Connection error");
			return;
		}

		foreach (var firstName in GetAllFirstName(connection))
		{
			Console.WriteLine($"Prénom: {firstName}");
		}

		// Nous préparons la requete SQL
		String query = """
		               INSERT into Employees (FirstName, LastName, HireDate, Salary, Department)
		               values ('Antonin', 'Do Souto', '17/12/2024', 4, 'IT');
		               """;

		SqlCommand command = new SqlCommand(query, connection);

		int nbsOfAffRows = command.ExecuteNonQuery();

		Console.WriteLine($"Nombre de lignes affectés : {nbsOfAffRows}");
	}

	static List<String> GetAllFirstName(SqlConnection connection)
	{
		// On créer un tableau de chaine de charactères
		List<string> firstNames = new List<string>();

		// Nous préparons la requete SQL
		String query = "Select * from Employees";

		// Nous créons la requete
		SqlCommand command = new SqlCommand(query, connection);

		// Nous executons la requête
		SqlDataReader reader = command.ExecuteReader();

		// Tant qu'on peux lire une donnée
		while (reader.Read())
		{
			// Nous lisons la donnée et nous selectons la colonne FirstName
			// Nous rajoutons la valeur dans le tableau de prénoms
			firstNames.Add(reader["FirstName"].ToString() ?? "?");
		}

		reader.Close();

		return firstNames;
	}
}