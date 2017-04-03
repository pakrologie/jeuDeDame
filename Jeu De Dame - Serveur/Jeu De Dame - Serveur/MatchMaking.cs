using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu_De_Dame___Serveur
{
	class MatchMaking
	{
		static List<Client> Attente = new List<Client>();
		public static void InscriptionMatch(Client MonClient)
		{
			Console.WriteLine("Inscription de " + MonClient.info_main.pseudo);
			if (MonClient.info_game.isplaying || DejaInscrit(MonClient))
			{
				return;
			}

			Attente.Add(MonClient);

			if (Attente.Count % 2 == 0 && Attente.Count != 0)
			{
				Console.WriteLine("2 joueurs vont êtes mis en relation ...");
				int IndexJoueur1 = ClientManager.byPseudo(Attente[0].info_main.pseudo);
				int IndexJoueur2 = ClientManager.byPseudo(Attente[1].info_main.pseudo);
				if (IndexJoueur1 == -1 || IndexJoueur2 == -1)
				{
					return;
				}
				
				MatchTrouver(IndexJoueur1, IndexJoueur2);
				Attente.RemoveAt(0);
				Attente.RemoveAt(0);
			}
		}

		public static void MatchTrouver(int IndexJoueur1, int IndexJoueur2)
		{
			ClientManager.ListClient[IndexJoueur1].info_game.opponent = ClientManager.ListClient[IndexJoueur2].info_main.pseudo;
			ClientManager.ListClient[IndexJoueur2].info_game.opponent = ClientManager.ListClient[IndexJoueur1].info_main.pseudo;

			ClientManager.ListClient[IndexJoueur1].info_main.playerTop = false;
			ClientManager.ListClient[IndexJoueur2].info_main.playerTop = true;

			ClientManager.ListClient[IndexJoueur1].info_game.tour = true;
			ClientManager.ListClient[IndexJoueur2].info_game.tour = false;

			Match.startGame(IndexJoueur1, IndexJoueur2);
		}

		public static void SupprimerListeAttente(string pseudo)
		{
			for (int i = 0; i < Attente.Count; i++)
			{
				if (Attente[i].info_main.pseudo == pseudo)
				{
					Attente.RemoveAt(i);
					return;
				}
			}
		}

		public static bool DejaInscrit(Client MonClient)
		{
			for (int i = 0; i < Attente.Count; i++)
			{
				if (Attente[i].info_main.pseudo == MonClient.info_main.pseudo)
				{
					return true;
				}
			}
			return false;
		}
	}
}
