using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day21
	{
		public static long Run()
		{
			bool isTest = true;
			//var result = RunPart1(isTest); // test: 739785, 798147
			//var result = RunPart1Attempt2(isTest); // test: 739785, 798147
			var result = RunPart2(isTest);

			return result;
		}

		private static long RunPart2(bool isTest)
		{
			var player1 = new Player(isTest ? 4 : 7);
			var player2 = new Player(isTest ? 8 : 5);

			var originalGame = new Game(player1, player2, 21);
			originalGame.CurrentPlayer = player1;
			var games = new List<Game> { originalGame };

			long player1WinCount = 0;
			long player2WinCount = 0;

			while (games.Any())
			{
				// Get any game that's still in the list
				var game = games.First();

				// If the game is over, increment the win count and remove the game from the list
				if (game.IsOver)
				{
					if (game.Player1Won)
					{
						player1WinCount++;
					}
					else
					{
						player2WinCount++;
					}
				}
				else
				{
					// This game is not over. Take another turn, which results in several new games to keep track of
					var newGames = game.TakeDiracTurn();
					games.AddRange(newGames);
				}

				// Remove this game from the list, as it's either over or been replaced by new games
				games.Remove(game);
			}

			// Count up the number of wins for each player
			//foreach (var game in games)
			//{
			//	if (game.Player1Won)
			//	{
			//		player1WinCount++;
			//	}
			//	else
			//	{
			//		player2WinCount++;
			//	}
			//}

			return Math.Max(player1WinCount, player2WinCount);
		}

		private static int RunPart1(bool isTest)
		{
			var player1 = new Player(isTest ? 4 : 7);
			var player2 = new Player(isTest ? 8 : 5);

			int die = 0;
			int rollCount = 0;

			var currentPlayer = player1;

			int winningScore = 1000;
			while (player1.Score < winningScore && player2.Score < winningScore)
			{
				// The number of spaces the player will move
				int moveCount = 0;

				// Roll the deterministic die 3 times to find the number of spaces the current player will move
				for (int i = 0; i < 3; i++)
				{
					// Roll the die - it always rolls one higher than its previous roll
					die++;
					rollCount++;

					// This is a 100-sided die - after 100, it goes back to 1
					if (die == 101)
					{
						die = 1;
					}

					moveCount += die;
				}

				// Move the current player the number of spaces
				currentPlayer.Position = ((currentPlayer.Position - 1 + moveCount) % 10) + 1;

				// Increase the current player's score by their current position
				currentPlayer.Score += currentPlayer.Position;

				// Switch who the current player is for next round
				currentPlayer = currentPlayer == player1 ? player2 : player1;
			}

			var loser = player1.Score >= 1000 ? player2 : player1;

			return loser.Score * rollCount;
		}

		private static int RunPart1Attempt2(bool isTest)
		{
			var player1 = new Player(isTest ? 4 : 7);
			var player2 = new Player(isTest ? 8 : 5);

			var game = new Game(player1, player2, 1000);
			var currentPlayer = player1;

			while (!game.IsOver)
			{
				// The number of spaces the player will move
				int moveCount = 0;

				// Roll the deterministic die 3 times to find the number of spaces the current player will move
				for (int i = 0; i < 3; i++)
				{
					moveCount += game.RollDeterministicDie();
				}

				// Move the current player the number of spaces
				currentPlayer.Position = ((currentPlayer.Position - 1 + moveCount) % 10) + 1;

				// Increase the current player's score by their current position
				currentPlayer.Score += currentPlayer.Position;

				// Switch who the current player is for next round
				currentPlayer = currentPlayer == player1 ? player2 : player1;
			}

			var loser = player1.Score >= player2.Score ? player2 : player1;

			return loser.Score * game.RollCount;
		}
	}

	public class Game
	{
		public Player Player1 { get; private set; }
		public Player Player2 { get; private set; }
		public int PointsToWin { get; private set; }
		private int _deterministicDie;
		public int RollCount { get; private set; }
		public Player CurrentPlayer { get; set; }

		public bool IsOver => Player1.Score >= PointsToWin || Player2.Score >= PointsToWin;
		public bool Player1Won => IsOver && Player1.Score > Player2.Score;

		public Game(Player player1, Player player2, int pointsToWin)
		{
			Player1 = player1;
			Player2 = player2;
			PointsToWin = pointsToWin;
		}

		public Game Clone()
		{
			var clone = new Game(Player1.Clone(), Player2.Clone(), PointsToWin);
			clone.CurrentPlayer = Player1 == CurrentPlayer ? clone.Player1 : clone.Player2;
			return clone;
		}

		public int RollDeterministicDie()
		{
			// Roll the die - it always rolls one higher than its previous roll
			_deterministicDie++;
			RollCount++;

			// This is a 100-sided die - after 100, it goes back to 1
			if (_deterministicDie == 101)
			{
				_deterministicDie = 1;
			}

			return _deterministicDie;
		}

		public List<Game> RollDiracDie()
		{
			var games = new List<Game>();
			
			for (int i = 1; i <= 3; i++)
			{
				var clone = Clone();

				// Move the current player the number of spaces rolled
				clone.CurrentPlayer.Position = ((clone.CurrentPlayer.Position - 1 + i) % 10) + 1;

				games.Add(clone);
			}

			return games;
		}

		public List<Game> TakeDiracTurn()
		{
			var games = new List<Game> { this };

			// Each turn is 3 rolls
			for (int i = 0; i < 3; i++)
			{
				var newGames = new List<Game>();

				// Roll the die for each game in the list, keeping track of all the new games
				foreach (var game in games)
				{
					newGames.AddRange(game.RollDiracDie());
				}

				// Replace the list of games with the new games
				games.Clear();
				games.AddRange(newGames);
			}

			// After rolling the dice, for each game: update the score for the current player and change turns
			foreach (var game in games)
			{
				// Increase the current player's score by their current position
				game.CurrentPlayer.Score += game.CurrentPlayer.Position;

				// Switch who the current player is for next round
				game.ChangeTurns();
			}

			return games;
		}

		/// <summary>
		/// Sets the current player to the other player.
		/// </summary>
		public void ChangeTurns()
		{
			CurrentPlayer = Player1 == CurrentPlayer ? Player2 : Player1;
		}
	}

	public class Player
	{
		public int Score { get; set; }
		public int Position { get; set; }

		public Player(int startingPosition)
		{
			Position = startingPosition;
		}

		public Player Clone()
		{
			return new Player(Position)
			{
				Score = Score
			};
		}
	}
}
