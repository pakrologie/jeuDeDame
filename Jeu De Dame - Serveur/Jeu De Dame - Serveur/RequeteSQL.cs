using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Jeu_De_Dame___Serveur
{
	class RequeteSQL
	{
		static MySqlConnection connection;
		static string serveur;
		static string bdd;
		static  string utilisateur;
		static string passe;

		public static bool Initialisation()
		{
			serveur = "25.76.21.163";
			bdd = "jeu_de_dame";
			utilisateur = "pakrologie";
			passe = "noproblemo5";
			string connectionString;
			connectionString = "SERVER=" + serveur + ";" + "DATABASE=" + bdd + ";" + "UID=" + utilisateur + ";" + "PASSWORD=" + passe + ";";

			connection = new MySqlConnection(connectionString);

			if (Connexion())
			{
				Deconnexion();
				return true;
			}
			return false;
		}

		public static bool Connexion()
		{
			try
			{
				connection.Open();
				return true;
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						Console.WriteLine("Impossible de se connecter à la base de donnée");
						break;

					case 1045:
						Console.WriteLine("Utilisateur/Mot de passe incorrects :: Connexion Base de donnée");
						break;
				}
				return false;
			}
		}

		public static void Deconnexion()
		{
			try
			{
				connection.Close();
			}
			catch (MySqlException ex)
			{ }
		}

		public static void SetOnline(Client IsPlaying, bool isOnline)
		{
			string nomUtilisateur = IsPlaying.info_main.pseudo;
			string motDePasse = IsPlaying.info_main.passe;

			string requete = "UPDATE utilisateurs SET connecte='" + Convert.ToInt32(isOnline) + "' WHERE utilisateur='" + nomUtilisateur + "' And passe='" + motDePasse + "'";

			if (Connexion())
			{
				MySqlCommand cmd = new MySqlCommand(requete, connection);
				cmd.ExecuteNonQuery();

				Deconnexion();
			}
			else
				Console.WriteLine("Requête SQL pour l'utilisateur : " + nomUtilisateur + " Fonction : SetOnline() :: Failed");
		}

		public static void UpdateScore(Client IsPlaying, bool victoire)
		{
			string nomUtilisateur = IsPlaying.info_main.pseudo;
			string motDePasse = IsPlaying.info_main.passe;

			string requete = "SELECT victoire, defaite FROM utilisateurs WHERE utilisateur='" + nomUtilisateur + "' And passe='" + motDePasse + "'";

			if (Connexion())
			{
				MySqlCommand cmd = new MySqlCommand(requete, connection);
				MySqlDataReader dataReader = cmd.ExecuteReader();
				
				while (dataReader.Read())
				{
					int VictoireCount = Convert.ToInt32(dataReader["victoire"]);
					int DefaiteCount = Convert.ToInt32(dataReader["defaite"]);
					if (victoire)
					{
						requete = "UPDATE utilisateurs SET victoire='" + ++VictoireCount + "' WHERE utilisateur='" + nomUtilisateur + "' And passe='" + motDePasse + "'";
					}else
						requete = "UPDATE utilisateurs SET defaite='" + ++DefaiteCount + "' WHERE utilisateur='" + nomUtilisateur + "' And passe='" + motDePasse + "'";
					break;
				}
				dataReader.Close();

				cmd.CommandText = requete;
				cmd.ExecuteNonQuery();
				
				Deconnexion();
			}
			else
				Console.WriteLine("Requête SQL pour l'utilisateur : " + nomUtilisateur + " Fonction : UpdateScore() :: Failed");
		}
	}
}
