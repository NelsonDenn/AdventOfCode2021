using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day11
	{
		public static int Run()
		{
			//var data = GetTestData();
			var data = GetData();

			//var result = RunPart1(data); // 1702
			var result = RunPart2(data); // 251

			return result;
		}

		// Get the step when all octopuses flash simultaneously
		private static int RunPart2(OctopusField octopusField)
		{
			int step = 1;

			// Run until all octopuses flash simultaneously, i.e. the flash count == the number of octopuses
			while (true)
			{
				int flashCount = octopusField.IncreaseEnergyLevels();

				if (flashCount == octopusField.Octopuses.Length)
				{
					break;
				}

				step++;
			}

			return step;
		}

		private static int RunPart1(OctopusField octopusField)
		{
			int totalFlashCount = 0;

			// Run for 100 steps
			for (int i = 0; i < 100; i++)
			{
				totalFlashCount += octopusField.IncreaseEnergyLevels();
			}

			return totalFlashCount;
		}

		private static OctopusField GetTestData()
		{
			string data =
@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

			return ParseData(data);
		}

		private static OctopusField GetData()
		{
			string data =
@"8577245547
1654333653
5365633785
1333243226
4272385165
5688328432
3175634254
6775142227
6152721415
2678227325";

			return ParseData(data);
		}

		private static OctopusField ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var octopusfield = new OctopusField(rows[0].Length, rows.Length);

			for (int y = 0; y < rows.Length; y++)
			{
				var row = rows[y].ToCharArray();

				for (int x = 0; x < row.Length; x++)
				{
					int energyLevel = row[x] - '0'; // Convert to int
					octopusfield.AddOctopus(x, y, energyLevel);
				}
			}

			return octopusfield;
		}
	}

	public class OctopusField
	{
		public Octopus[,] Octopuses { get; }
		public int Width { get; }
		public int Height { get; }

		private int MaxX => Width - 1;
		private int MaxY => Height - 1;

		public OctopusField(int width, int height)
		{
			Width = width;
			Height = height;
			Octopuses = new Octopus[width, height];
		}

		public void AddOctopus(int x, int y, int energyLevel)
		{
			Octopuses[x, y] = new Octopus(energyLevel);
		}

		public int IncreaseEnergyLevels()
		{
			int totalFlashCount = 0;

			// Increase the energy levels of all octopuses by 1
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					Octopuses[x, y].EnergyLevel++;
				}
			}

			// Now check if any octopuses energy levels are greater than 9
			// If so, that octopus flashes, resetting its energy level to 0,
			// and increasing all surrounding (including diagonal) octopuses'
			// energy levels by 1.
			// Note: Octopuses who have already flashed do not gain any additional energy.
			int flashCount;
			do
			{
				flashCount = GetFlashCount();
				totalFlashCount += flashCount;
			} while (flashCount > 0);

			// Reset the HasFlashed for each octopus
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					Octopuses[x, y].HasFlashed = false;
				}
			}

			return totalFlashCount;
		}

		private int GetFlashCount()
		{
			int flashCount = 0;

			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					var octopus = Octopuses[x, y];
					if (octopus.EnergyLevel > 9)
					{
						// This octopus flashes. Increase surrounding octopuses energy levels.
						for (int nearbyX = x - 1; nearbyX <= x + 1; nearbyX++)
						{
							for (int nearbyY = y - 1; nearbyY <= y + 1; nearbyY++)
							{
								// Make sure this point is actually within the plot. Exclude the current octopus
								if (nearbyX >= 0 && nearbyX <= MaxX && nearbyY >= 0 && nearbyY <= MaxY && !(nearbyX == x && nearbyY == y))
								{
									var nearbyOctopus = Octopuses[nearbyX, nearbyY];

									// Skip this octopus if it has already flashed. Its energy level should remain at 0.
									if (nearbyOctopus.HasFlashed)
									{
										continue;
									}

									nearbyOctopus.EnergyLevel++;
								}
							}
						}

						// Mark this octopus as having flashed for this step and reset its energy level to 0
						octopus.Flash();

						// Increase our flash counter for this check
						flashCount++;
					}
				}
			}

			return flashCount;
		}
	}

	public class Octopus
	{
		public int EnergyLevel { get; set; }
		public bool HasFlashed { get; set; }

		public Octopus(int energyLevel)
		{
			EnergyLevel = energyLevel;
		}

		// Resets the energy level to 0 and indicates it has flashed this step
		public void Flash()
		{
			EnergyLevel = 0;
			HasFlashed = true;
		}

		public override string ToString()
		{
			return EnergyLevel.ToString();
		}
	}

}
