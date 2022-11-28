using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day6
	{
		public static int Run()
		{
			var fish = GetFish();

			//var result = RunPart1(fish); // 379114
			var result = RunPart2(fish); // 1702631502303
			// Note: result of part 2 is a long
			return 0;
		}

		private static long RunPart2(List<int> fish)
		{
			Dictionary<int, long> fishCounts = new Dictionary<int, long>();

			// Populate all keys in the fishCounts dictionary
			// Each key represents the group of fish with <key> many days until they spawn another fish
			for (int i = 0; i < 9; i++)
			{
				fishCounts.Add(i, 0);
			}

			// Update counts of all fish to begin with
			for (int i = 0; i < fish.Count; i++)
			{
				fishCounts[fish[i]]++;
			}

			// How many fish would there be after 256 days?
			for (int i = 0; i < 256; i++)
			{
				long newFishCount = fishCounts[0];

				for (int j = 0; j < 8; j++)
				{
					fishCounts[j] = fishCounts[j + 1];
				}

				fishCounts[8] = newFishCount; // New fish spawn with 9 days to go
				fishCounts[6] += newFishCount; // These are the fish that just spawned a fish. Reset them to 7 days to go
			}

			return fishCounts.Sum(f => f.Value);
		}

		private static int RunPart1(List<int> fish)
		{
			for (int i = 0; i < 256; i++)
			{
				int newFishCount = 0;

				for (int j = 0; j < fish.Count; j++)
				{
					if (fish[j] == 0)
					{
						fish[j] = 6;
						newFishCount++;
					}
					else
					{
						fish[j]--;
					}
				}

				for (int j = 0; j < newFishCount; j++)
				{
					fish.Add(8);
				}
			}


			return fish.Count;
		}

		private static List<int> GetFish()
		{
			// Each fish is represented as its internal timer of days left until it spawns a new fish
			return new List<int>
			{
				1,4,3,3,1,3,1,1,1,2,1,1,1,4,4,1,5,5,3,1,3,5,2,1,5,2,4,1,4,5,4,1,5,1,5,5,1,1,1,4,1,5,1,1,1,1,1,4,1,2,5,1,4,1,2,1,1,5,1,1,1,1,4,1,5,1,1,2,1,4,5,1,2,1,2,2,1,1,1,1,1,5,5,3,1,1,1,1,1,4,2,4,1,2,1,4,2,3,1,4,5,3,3,2,1,1,5,4,1,1,1,2,1,1,5,4,5,1,3,1,1,1,1,1,1,2,1,3,1,2,1,1,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,4,5,1,3,1,4,4,2,3,4,1,1,1,5,1,1,1,4,1,5,4,3,1,5,1,1,1,1,1,5,4,1,1,1,4,3,1,3,3,1,3,2,1,1,3,1,1,4,5,1,1,1,1,1,3,1,4,1,3,1,5,4,5,1,1,5,1,1,4,1,1,1,3,1,1,4,2,3,1,1,1,1,2,4,1,1,1,1,1,2,3,1,5,5,1,4,1,1,1,1,3,3,1,4,1,2,1,3,1,1,1,3,2,2,1,5,1,1,3,2,1,1,5,1,1,1,1,1,1,1,1,1,1,2,5,1,1,1,1,3,1,1,1,1,1,1,1,1,5,5,1
			};
		}
	}
}
