using Microsoft.Data.SqlClient;

namespace EmployeeManager;

public class DbConnection
{
	public async Task<SqlConnection?> Connect()
	{
		var builder = new SqlConnectionStringBuilder
		{
			DataSource = "localhost",
			UserID = "sa",
			Password = "root",
			InitialCatalog = "ExerciceDB",
			IntegratedSecurity = true,
			TrustServerCertificate = true
		};

		var connection = new SqlConnection(builder.ConnectionString);
		try
		{
			await connection.OpenAsync();
			return connection;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			Console.WriteLine("Connection error");
			return null;
		}
	}


	public void ShowData(SqlConnection? connection)
	{
		const string query = "SELECT * FROM dbo.Employees";

		using SqlCommand command = new SqlCommand(query, connection);
		using SqlDataReader reader = command.ExecuteReader();

		while (reader.Read())
		{
			Console.WriteLine(
				$"Salaire de {reader["FirstName"]} {reader["LastName"]}[{reader["Department"]}]: {reader["Salary"]}$"
			);
		}

		string? rdl = null;

		Console.WriteLine(rdl ?? "");

		reader.Close();
	}

	public void InsertData(SqlConnection con, string firstName, string lastName, DateTime hireDate, double salary, string department)
	{
		const string query = """
		                     INSERT INTO dbo.Employees(FirstName, LastName, HireDate, Salary, Department)
		                     VALUES (@Firstname, @LastName, @HireDate, @Salary, @Department)
		                     """;
		SqlCommand command = new SqlCommand(query, con);

		command.Parameters.AddWithValue("FirstName", firstName);
		command.Parameters.AddWithValue("LastName", lastName);
		command.Parameters.AddWithValue("HireDate", hireDate);
		command.Parameters.AddWithValue("Salary", salary);
		command.Parameters.AddWithValue("Department", department);

		int success = command.ExecuteNonQuery();
		Console.WriteLine((success >= 1) ? "Employé ajouté !" : "Erreur dans l'ajout");
	}

	public void DeleteData(SqlConnection con, int id)
	{
		const string query = "DELETE from dbo.Employees where EmployeeID = @EmployeeID";
		SqlCommand command = new SqlCommand(query, con);

		command.Parameters.AddWithValue("EmployeeID", id);

		int success = command.ExecuteNonQuery();
		Console.WriteLine((success >= 1) ? "Employé supprimé !" : $"Erreur: id({id}) n'existe pas");
	}
}