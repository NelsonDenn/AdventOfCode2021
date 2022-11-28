using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day21Redo
	{
		public static long Run()
		{
			bool isTest = false;
			//var result = RunPart1(isTest); // test: 739785, 798147
			//var result = RunPart1Attempt2(isTest); // test: 739785, 798147
			var result = RunPart2(isTest); // test: 444356092776315, 809953813657517

			return result;
		}

		private static long RunPart2(bool isTest)
		{
			var originalGame = new Game2
			{
				Player1Position = isTest ? 4 : 7,
				Player2Position = isTest ? 8 : 5
			};

			var positionIncreases = GetPerTurnPositionIncreases();

			// Map of games to count of games with the same values
			var games = new Dictionary<Game2, long>();
			games.Add(originalGame, 1);

			long player1WinCount = 0;
			long player2WinCount = 0;
			bool player1sTurn = true;

			while (games.Any())
			{
				var newGames = new Dictionary<Game2, long>();

				// Increase the scores for each game
				foreach (var gameToGameCount in games)
				{
					var game = gameToGameCount.Key;
					long gameCount = gameToGameCount.Value;

					foreach (var positionIncrease in positionIncreases)
					{
						// Find the update position and score for whomever's turn it is
						int currentPosition = player1sTurn ? game.Player1Position : game.Player2Position;
						int currentScore = player1sTurn ? game.Player1Score : game.Player2Score;

						int newPosition = ((currentPosition - 1 + positionIncrease.Key) % 10) + 1;
						int newScore = currentScore + newPosition;

						// The number of new games with the new setup (Player1Position/Player1Score/Player2Position/Player2Score)
						long newGameCount = gameCount * positionIncrease.Value;

						// If the new score is at least 21, the current player wins
						// Update that player's win count, and continue on (we no longer need to keep track of the game)
						if (newScore >= 21)
						{
							if (player1sTurn)
							{
								player1WinCount += newGameCount;
							}
							else
							{
								player2WinCount += newGameCount;
							}

							continue;
						}

						// Set up the new key
						var newGame = new Game2
						{
							Player1Position = player1sTurn ? newPosition : game.Player1Position,
							Player1Score = player1sTurn ? newScore : game.Player1Score,
							Player2Position = player1sTurn ? game.Player2Position : newPosition,
							Player2Score = player1sTurn ? game.Player2Score : newScore
						};
						
						// For each game with the current setup (key), increase the current player's
						// position and score, then add new games times the count of the current position increase
						// with the new game setup
						if (!newGames.ContainsKey(newGame))
						{
							newGames.Add(newGame, newGameCount);
						}
						else
						{
							newGames[newGame] += newGameCount;
						}
					}
				}

				// Replace our current games map with the new games map
				games = newGames;

				// Change turns
				player1sTurn = !player1sTurn;
			}

			return Math.Max(player1WinCount, player2WinCount);
		}

		// Each turn, the universe splits 27 times, and the player's position increases by these amounts
		// Map of position increases to count of times that position increase occurs each turn
		private static Dictionary<int, int> GetPerTurnPositionIncreases()
		{
			var positionIncreases = new Dictionary<int, int>();

			for (int firstTurnDieRoll = 1; firstTurnDieRoll <= 3; firstTurnDieRoll++)
			{
				for (int secondTurnDieRoll = 1; secondTurnDieRoll <= 3; secondTurnDieRoll++)
				{
					for (int thirdTurnDieRoll = 1; thirdTurnDieRoll <= 3; thirdTurnDieRoll++)
					{
						int scoreIncrease = firstTurnDieRoll + secondTurnDieRoll + thirdTurnDieRoll;
						if (!positionIncreases.ContainsKey(scoreIncrease))
						{
							positionIncreases.Add(scoreIncrease, 1);
						}
						else
						{
							positionIncreases[scoreIncrease] += 1;
						}
					}
				}
			}

			return positionIncreases;
		}
	}

	public class Game2
	{
		public int Player1Position { get; set; }
		public int Player1Score { get; set; }
		public int Player2Position { get; set; }
		public int Player2Score { get; set; }

		public override bool Equals(object obj)
		{
			Game2 other = (Game2)obj;
			return Player1Position == other.Player1Position
				&& Player1Score == other.Player1Score
				&& Player2Position == other.Player2Position
				&& Player2Score == other.Player2Score;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Player1Position, Player1Score, Player2Position, Player2Score);
		}

		public override string ToString()
		{
			return $"{Player1Position}-{Player1Score}-{Player2Position}-{Player2Score}";
		}
	}
}
