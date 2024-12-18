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

		ShowData(connection);
		int success = InsertData(connection, "Goulwen", "Delaunay", DateTime.Now, 1200.0, "IT");
		if (success >= 1)
		{
			Console.WriteLine("Donnée(s) insérée(s) !");
			ShowData(connection);
		}
		else
		{
			await Console.Error.WriteLineAsync("Erreur sur l'insertion de données !");
		}
	}

	private static void ShowData(SqlConnection connection)
	{
		const string query = "SELECT * FROM Employees";

		using SqlCommand command = new SqlCommand(query, connection);
		using SqlDataReader reader = command.ExecuteReader();

		while (reader.Read())
		{
			Console.WriteLine(
				$"Salaire de {reader["FirstName"]} {reader["LastName"]}[{reader["Department"]}]: {reader["Salary"]}$"
			);
		}

		reader.Close();
	}

	private static int InsertData(SqlConnection con, string firstName, string lastName, DateTime hireDate,
		double salary, string department)
	{
		string query = """
		                INSERT INTO Employees(FirstName, LastName, HireDate, Salary, Department)
		                VALUES (@Firstname, @LastName, @HireDate, @Salary, @Department)
		                """;
		SqlCommand command = new SqlCommand(query, con);

		command.Parameters.AddWithValue("FirstName", firstName);
		command.Parameters.AddWithValue("LastName", lastName);
		command.Parameters.AddWithValue("HireDate", hireDate);
		command.Parameters.AddWithValue("Salary", salary);
		command.Parameters.AddWithValue("Department", department);

		int success = command.ExecuteNonQuery();
		return success;
	}
}