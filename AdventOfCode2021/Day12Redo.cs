using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode2021
{
	public class Day12Redo
	{
		public static int Run()
		{
			//var data = GetTestData(); // 10, 36
			//var data = GetTestData2(); // 19, 103
			//var data = GetTestData3(); // 226, 3509
			var data = GetData();

			var part1Result = RunPart1(data); // 3463
			Console.WriteLine("Part 1 result: " + part1Result);

			var part2Result = RunPart2(data); // 91533
			Console.WriteLine("Part 2 result: " + part2Result); // Down to 8 minutes

			return part1Result;
		}

		private static int RunPart2(Dictionary<Cave, List<Cave>> caveMap)
		{
			return RunPart1(caveMap, true);
		}

		private static int RunPart1(Dictionary<Cave, List<Cave>> caveMap, bool mayVisitOneSmallCaveTwice = false)
		{
			// Get all paths that lead from start to end
			List<CavePath> paths = new List<CavePath>();

			// Get the starting point and add it as our first path
			var startingCave = caveMap.Keys.ToList().Find(c => c.IsStart);
			paths.Add(new CavePath(startingCave));

			// Loop over each path until all paths have reached the end
			while (true)
			{
				// Get a path to work on next
				var path = paths.Find(p => !p.IsCompleted);

				if (path == null)
				{
					// All paths have reached the end (or been discarded). We are done here
					break;
				}

				// Get all caves connected to the cave at the end of this path
				var nextCaves = GetNextCaves(path, caveMap[path.EndingCave], mayVisitOneSmallCaveTwice);

				var newPaths = path.Branch(nextCaves);
				paths.AddRange(newPaths);
				paths.Remove(path);
			}

			return paths.Count;
		}

		private static List<Cave> GetNextCaves(CavePath path, List<Cave> potentialNextCaves, bool mayVisitOneSmallCaveTwice = false)
		{
			// Filter out small caves that have already been visited
			return potentialNextCaves.Where(cave =>
				!(cave.IsSmallCave && path.ContainsCave(cave))
				|| (mayVisitOneSmallCaveTwice && !path.HasVisitedOneSmallCaveTwice)
			).ToList();
		}

		private static Dictionary<Cave, List<Cave>> GetTestData()
		{
			string data =
@"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

			return ParseData(data);
		}

		private static Dictionary<Cave, List<Cave>> GetTestData2()
		{
			string data =
@"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

			return ParseData(data);
		}

		private static Dictionary<Cave, List<Cave>> GetTestData3()
		{
			string data =
@"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";

			return ParseData(data);
		}

		private static Dictionary<Cave, List<Cave>> GetData()
		{
			string data =
@"RT-start
bp-sq
em-bp
end-em
to-MW
to-VK
RT-bp
start-MW
to-hr
sq-AR
RT-hr
bp-to
hr-VK
st-VK
sq-end
MW-sq
to-RT
em-er
bp-hr
MW-em
st-bp
to-start
em-st
st-end
VK-sq
hr-st";

			return ParseData(data);
		}

		private static Dictionary<Cave, List<Cave>> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			// Maps a cave to each connected cave
			var caveMap = new Dictionary<Cave, List<Cave>>();
			var caves = new List<Cave>(); // Keep track of caves so we don't have duplicate objects

			foreach (string row in rows)
			{
				List<Cave> connectedCaves = new List<Cave>();

				foreach (string caveName in row.Split("-"))
				{
					var cave = caves.Find(c => c.Name == caveName);

					if (cave == null)
					{
						cave = new Cave(caveName);
						caves.Add(cave);
					}

					// Add this cave to the map, but exclude the end cave
					if (!caveMap.ContainsKey(cave) && !cave.IsEnd)
					{
						caveMap.Add(cave, new List<Cave>());
					}

					connectedCaves.Add(cave);
				}

				// Map the caves to each other, but exclude the starting cave from other caves' connected caves list
				var firstCave = connectedCaves[0];
				var secondCave = connectedCaves[1];
				if (!firstCave.IsEnd && !secondCave.IsStart)
				{
					caveMap[firstCave].Add(secondCave);
				}
				if (!firstCave.IsStart && !secondCave.IsEnd)
				{
					caveMap[secondCave].Add(firstCave);
				}
			}

			return caveMap;
		}
	}

	public class CavePath
	{
		public List<Cave> Caves { get; }
		public Cave EndingCave { get; }
		public bool IsCompleted { get; }
		public bool HasVisitedOneSmallCaveTwice { get; set; }

		public CavePath(Cave startingCave)
		{
			Caves = new List<Cave>()
			{
				startingCave
			};

			EndingCave = startingCave;
		}

		// Create a new path from the existing path plus the next cave
		private CavePath(CavePath existingPath, Cave nextCave)
		{
			// If the same small cave has been visited more than once,
			// then mark this path as having visited a small cave twice
			HasVisitedOneSmallCaveTwice = existingPath.HasVisitedOneSmallCaveTwice
				|| (nextCave.IsSmallCave && existingPath.Caves.Contains(nextCave));

			Caves = new List<Cave>(existingPath.Caves)
			{
				nextCave
			};

			EndingCave = nextCave;

			if (nextCave.IsEnd)
			{
				IsCompleted = true;
			}
		}

		// Branch into a new path for each new cave
		public List<CavePath> Branch(List<Cave> nextCaves)
		{
			return nextCaves.Select(cave => new CavePath(this, cave)).ToList();
		}

		// Returns true if this path includes the given cave
		public bool ContainsCave(Cave cave)
		{
			return Caves.Any(c => c == cave);
		}

		public override string ToString()
		{
			return string.Join(",", Caves);
		}
	}

	public class Cave
	{
		public string Name { get; private set; }
		public bool IsLargeCave => !IsStart && !IsEnd && char.IsUpper(Name[0]);
		public bool IsSmallCave => !IsStart && !IsEnd && !IsLargeCave;
		public bool IsStart => Name == "start";
		public bool IsEnd => Name == "end";

		public Cave(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
