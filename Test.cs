using Microsoft.Data.SqlClient;

namespace EmployeeManager;

public class Test
{
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

	static void InsertEmployee(SqlConnection connection, string firstName, string lastName, string department,
		float salary, DateTime hireDate)
	{
		string insertQuery =
			"""
			INSERT INTO Employees (FirstName, LastName, HireDate, Salary, Department)
			VALUES (@FirstName, @LastName, @HireDate, @Department, CONVERT(DECIMAL, @Salary))
			""";
		using SqlCommand command = new SqlCommand(insertQuery, connection);
		command.Parameters.AddWithValue("@FirstName", firstName);
		command.Parameters.AddWithValue("@LastName", lastName);
		command.Parameters.AddWithValue("@Department", department);
		command.Parameters.AddWithValue("@Salary", (int)salary);
		command.Parameters.AddWithValue("@HireDate", hireDate);
		command.ExecuteNonQuery();
	}
}