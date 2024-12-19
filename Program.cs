using System.Data;
using Microsoft.Data.SqlClient;

namespace EmployeeManager;

class Program
{
	private static async Task Main(string[] args)
	{
		DbConnection db = new DbConnection();

		SqlConnection? tryConn = await db.Connect();

		if (tryConn == null)
		{
			return;
		}

		SqlConnection connection = tryConn;

		bool active = true;

		while (active)
		{
			ConsolePanel.ShowFirstMenu();

			string answer = Console.ReadLine() ?? "";

			switch (answer)
			{
				case "1":
					db.ShowData(connection);
					break;
				case "2":
					Dictionary<string, string> r = ConsolePanel.ShowInsertMenu();

					DateTime hireDate = DateTime.Now;
					double salary = 0.0;
					try
					{
						hireDate = DateTime.Parse(r["hireDate"]);
						salary = double.Parse(r["salary"]);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}

					db.InsertData(
						connection,
						r["firstName"],
						r["lastName"],
						hireDate,
						salary,
						r["department"]
					);
					break;
				case "3":
					break;
				case "4":
					var id = ConsolePanel.AskQuestion("Veuillez saisir l'ID: ", "-1");
					int parsedId;
					if (id == "-1") break;
					try
					{
						parsedId = int.Parse(id);
					}
					catch (Exception e)
					{
						Console.WriteLine("L'ID ne fonctionne pas !");
						break;
					}

					db.DeleteData(connection, parsedId);
					break;
				case "5":
					active = false;
					break;
				default:
					break;
			}
		}
	}
}