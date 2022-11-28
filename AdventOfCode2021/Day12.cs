using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day12
	{
		public static int Run()
		{
			//var data = GetTestData(); // 10, 36
			//var data = GetTestData2(); // 19, 103
			//var data = GetTestData3(); // 226, 3509
			var data = GetData();

			//var result = RunPart1(data); // 3463
			var result = RunPart2(data); // 91533 (this took like 5 hours... whoops)

			return result;
		}

		private static int RunPart2(List<Connection> data)
		{
			// Get all paths that lead from start to end
			List<Path> paths = new List<Path>();

			// Get the starting point and its connections
			var startingPoint = data.Find(c => c.StartingPoint.IsStart).StartingPoint;
			var startingConnections = data.Where(c => c.StartingPoint.Equals(startingPoint));

			// Add a path for each connection from the starting point
			foreach (var connection in startingConnections)
			{
				paths.Add(new Path(connection));
			}

			// Loop over each path until all paths have reached the end
			while (paths.Any(p => !p.IsCompleted))
			{
				// Get a path to work on next
				var path = paths.Find(p => !p.IsCompleted);
				Console.WriteLine($"Checking path: {path}");

				// Get all connections to this path
				var connections = GetConnectionsMayVisitOneSmallCaveTwice(path, data);

				var newPaths = path.Branch(connections);
				paths.AddRange(newPaths);
				paths.Remove(path);
			}

			return paths.Count;
		}

		private static int RunPart1(List<Connection> data)
		{
			// Get all paths that lead from start to end
			List<Path> paths = new List<Path>();

			// Get the starting point and its connections
			var startingPoint = data.Find(c => c.StartingPoint.IsStart).StartingPoint;
			var startingConnections = data.Where(c => c.StartingPoint.Equals(startingPoint));

			// Add a path for each connection from the starting point
			foreach (var connection in startingConnections)
			{
				paths.Add(new Path(connection));
			}

			// Loop over each path until all paths have reached the end
			while (paths.Any(p => !p.IsCompleted))
			{
				// Get a path to work on next
				var path = paths.Find(p => !p.IsCompleted);

				// Get all connections to this path
				var connections = GetConnections(path, data);

				var newPaths = path.Branch(connections);
				paths.AddRange(newPaths);
				paths.Remove(path);
			}

			return paths.Count;
		}

		private static List<Connection> GetConnections(Path path, List<Connection> data)
		{
			// First, get all connections where one of the points is the path's ending point
			var candidates = data.Where(c =>
				(c.StartingPoint.Equals(path.EndingPoint) || c.EndingPoint.Equals(path.EndingPoint))
				// Exclude starting point
				&& !c.StartingPoint.IsStart);

			// Next, flip the connection points so the starting point is the path's ending point
			var flipped = candidates.Select(c => new Connection(path.EndingPoint.Name, c.EndingPoint.Equals(path.EndingPoint) ? c.StartingPoint.Name : c.EndingPoint.Name));

			// Finally, return only connections where the ending point is a small cave we haven't visited yet, or not a small cave
			return flipped.Where(c =>
				!(c.EndingPoint.IsSmallCave && path.ContainsPoint(c.EndingPoint))
			).ToList();
		}

		private static List<Connection> GetConnectionsMayVisitOneSmallCaveTwice(Path path, List<Connection> data)
		{
			// First, get all connections where one of the points is the path's ending point
			var candidates = data.Where(c =>
				(c.StartingPoint.Equals(path.EndingPoint) || c.EndingPoint.Equals(path.EndingPoint))
				// Exclude starting point
				&& !c.StartingPoint.IsStart);

			// Next, flip the connection points so the starting point is the path's ending point
			var flipped = candidates.Select(c => new Connection(path.EndingPoint.Name, c.EndingPoint.Equals(path.EndingPoint) ? c.StartingPoint.Name : c.EndingPoint.Name));

			// Finally, return only connections where the ending point is a small cave we haven't visited yet, or not a small cave
			// Actually, we can visit one small cave once per path
			return flipped.Where(c =>
				!(c.EndingPoint.IsSmallCave && path.ContainsPoint(c.EndingPoint))
				|| !path.HasVisitedSmallCaveTwice
			).ToList();
		}

		private static List<Connection> GetConnections(ConnectingPoint startingPoint, List<Connection> data)
		{
			return data.Where(c => c.StartingPoint.Equals(startingPoint)).ToList();
		}

		private static List<Connection> GetTestData()
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

		private static List<Connection> GetTestData2()
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

		private static List<Connection> GetTestData3()
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

		private static List<Connection> GetData()
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

		private static List<Connection> ParseData(string data)
		{
			var rows = data.Split("\r\n");

			var connections = new List<Connection>();

			foreach (string row in rows)
			{
				var points = row.Split("-");
				string startingPoint = points[0];
				string endingPoint = points[1];

				var connection = new Connection(startingPoint, endingPoint);
				connections.Add(connection);
			}

			return connections;
		}
	}

	public class Path
	{
		public List<Connection> Connections { get; }

		public ConnectingPoint EndingPoint => Connections[Connections.Count - 1].EndingPoint;
		public bool IsCompleted => Connections.Any(c => c.EndingPoint.IsEnd);
		public bool HasVisitedSmallCaveTwice { get; set; }

		public Path(Connection connection)
		{
			Connections = new List<Connection>()
			{
				connection
			};
		}

		public Path(List<Connection> connections)
		{
			Connections = new List<Connection>(connections);
		}

		// Create a new path from the existing path plus the new connection
		private Path(Path existingPath, Connection newConnection)
		{
			Connections = new List<Connection>(existingPath.Connections)
			{
				newConnection
			};

			// If the same small cave has been visited more than once,
			// then mark this path as having visited a small cave twice
			var smallCavesVisited = Connections.Where(c => c.EndingPoint.IsSmallCave).Select(c => c.EndingPoint);
			HasVisitedSmallCaveTwice = smallCavesVisited.Count() > smallCavesVisited.Distinct().Count();
		}

		public List<Path> Branch(List<Connection> connections)
		{
			return connections.Select(c => new Path(this, c)).ToList();
		}

		// Returns true if this path contains any connections that include the given point
		public bool ContainsPoint(ConnectingPoint point)
		{
			return Connections.Any(c => c.StartingPoint.Equals(point) || c.EndingPoint.Equals(point));
		}

		public override string ToString()
		{
			return string.Join(",", Connections);
		}
	}

	public class Connection
	{
		public ConnectingPoint StartingPoint { get; private set; }
		public ConnectingPoint EndingPoint { get; private set; }

		public Connection(string startingPointName, string endingPointName)
		{
			if (endingPointName == "start" || startingPointName == "end")
			{
				// Switch "start" to be the starting point or "end" to be the ending point
				string temp = startingPointName;
				startingPointName = endingPointName;
				endingPointName = temp;
			}
			StartingPoint = new ConnectingPoint(startingPointName);
			EndingPoint = new ConnectingPoint(endingPointName);
		}

		public override string ToString()
		{
			return $"{StartingPoint}-{EndingPoint}";
		}

		public override bool Equals(object obj)
		{
			Connection other = (Connection)obj;
			return (StartingPoint.Equals(other.StartingPoint) && EndingPoint.Equals(other.EndingPoint))
				|| (StartingPoint.Equals(other.EndingPoint) && EndingPoint.Equals(other.StartingPoint));
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(StartingPoint, EndingPoint);
		}
	}

	public class ConnectingPoint
	{
		public string Name { get; private set; }
		public bool IsLargeCave => !IsStart && !IsEnd && char.IsUpper(Name[0]);
		public bool IsSmallCave => !IsStart && !IsEnd && !IsLargeCave;
		public bool IsStart => Name == "start";
		public bool IsEnd => Name == "end";

		public ConnectingPoint(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			ConnectingPoint other = (ConnectingPoint)obj;
			return Name == other.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name);
		}
	}
}
