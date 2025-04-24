using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace GroceryApp
{
	class DbConnection
	{
		private static NpgsqlConnection Connection = new("Host=localhost; Database=Grocery; Username=postgres; Password=1234");

		public static void Initialize()
		{
			Connection.Open();
		}

		public static NpgsqlCommand Command(string cmdString)
		{
			return new NpgsqlCommand(cmdString, Connection);
		}

		public static bool UserExists(string login, string password)
		{
			try
			{
				using var v_cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM grocery_user WHERE user_name = '{login}' AND user_password = '{password}'", Connection);
				using var v_reader = v_cmd.ExecuteReader();

				v_reader.Read();

				return v_reader.GetInt32(0) > 0;
			}
			catch { }

			return false;
		}

		public static int CreateUser(string login, string password)
		{
			try
			{
				using var v_cmd = new NpgsqlCommand($"INSERT INTO grocery_user (user_name, user_password) VALUES ('{login}', '{password}') RETURNING id;", Connection);
				using var v_reader = v_cmd.ExecuteReader();

				if (v_reader.Read())
					return v_reader.GetInt32(0);
			}
			catch { }

			return -1;
		}

		public static int FindUser(string login, string password)
		{
			try
			{
				using var v_cmd = new NpgsqlCommand($"SELECT id FROM grocery_user WHERE user_name = '{login}' AND user_password = '{password}'", Connection);
				using var v_reader = v_cmd.ExecuteReader();

				if (v_reader.Read())
					return v_reader.GetInt32(0);
			}
			catch { }

			return -1;
		}

		public static int CheckAccount(string username, string password)
		{
			try
			{
				using var comm = new NpgsqlCommand("SELECT id FROM grocery_user WHERE user_name = @UserName AND user_password = @UserPass", Connection);
				comm.Parameters.AddWithValue("@UserName", username);
				comm.Parameters.AddWithValue("@UserPass", password);

				using var reader = comm.ExecuteReader();
				if (reader.Read())
					return reader.GetInt32(0);
			}
			catch { }

			return -1;
		}

		public static int CreateAccount(string Username, string Userpass)
		{
			try
			{
				using var comm = new NpgsqlCommand("INSERT INTO grocery_user (user_name, user_password) VALUES (@UserName, @UserPass) RETURNING id", Connection);
				comm.Parameters.AddWithValue("@UserName", Username);
				comm.Parameters.AddWithValue("@UserPass", Userpass);

				using var reader = comm.ExecuteReader();
				if (reader.Read())
					return reader.GetInt32(0);
			}
			catch { }

			return -1;
		}

		public static string GetAccountName(int accountId)
		{
			try
			{
				using var v_comm = new NpgsqlCommand("SELECT user_name FROM grocery_user WHERE id = @id", Connection);
				v_comm.Parameters.AddWithValue("@id", accountId);

				using var v_reader = v_comm.ExecuteReader();
				if (v_reader.Read())
					return v_reader.GetString(0);
			}
			catch { }

			return "ERROR";
		}

		/*
		public static int CreateOrGetUser(string login, string password)
		{
			int v_userId = FindUser(login, password);
			if (v_userId != -1) return v_userId;

			return CreateUser(login, password);
		}
		*/
	}
}
