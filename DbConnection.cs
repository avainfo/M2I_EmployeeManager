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

	public void UpdateData(SqlConnection con, int id, int[] selectedOptions, string[] selectedValues)
	{
		//selectedOptions = [1, 3, 5]
		//selectedValues = ["Antonin", "2024/12/20", "IT"]
		List<string> options =
		[
			"FirstName",
			"LastName",
			"HireDate",
			"Salary",
			"Department",
		];

		// ["FirstName = @1", "HireDate = @2", "Department = @3"]
		List<string> chosenOptions = [];

		int i = 1;
		foreach (int sOption in selectedOptions)
		{
			chosenOptions.Add($"{options[sOption - 1]} = @{i++}");
		}

		// "FirstName = @1, HireDate = @2, Department = @3"
		string formattedOptions = string.Join(", ", chosenOptions);


		//UPDATE dbo.Employees SET FirstName = @1, HireDate = @2, Department = @3 WHERE EmployeeID = 52
		string query = $"UPDATE dbo.Employees SET {formattedOptions} WHERE EmployeeID = @EID";
		SqlCommand command = new SqlCommand(query, con);

		/*
		 * 1, "Antonin"
		 * 2, "2024/12/20"
		 * 3, "IT"
		 */
		i = 1;
		foreach (string selectedValue in selectedValues)
		{
			command.Parameters.AddWithValue((i++).ToString(), selectedValue);
		}
		command.Parameters.AddWithValue("EID", id);

		int success = command.ExecuteNonQuery();
		Console.WriteLine((success >= 1) ? "Employé modifié !" : $"Erreur: id({id}) n'existe pas");
	}
}