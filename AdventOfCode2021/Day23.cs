using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day23
	{
		public static int Run()
		{
			bool isTest = false;
			bool isPart2 = true;
			var data = isTest ? GetTestData(isPart2) : GetData(isPart2);

			// Part 1: test: 12521, 10607
			// Part 2: test: 44169, 59071
			var result = isPart2 ? RunPart2(data) : RunPart1(data);

			//  40753 too low


			return result;
		}

		private static int RunPart2(Burrow initialBurrow)
		{
//			string part2Data =
//@"#D#C#B#A#
//#D#B#A#C#";

			return RunPart1(initialBurrow);
		}

		private static int RunPart1(Burrow initialBurrow)
		{
			//initialBurrow.Print();

			// Here's the plan: just run every possible move we can.
			// Many plans will run out of moves quickly, and can then be discarded.
			// Only keep plans where the amphipods end up in their homes.
			// Then find the plan with the least energy.

			int leastEnergyExpended = int.MaxValue;

			var burrows = new List<Burrow>() { initialBurrow };

			while (burrows.Count > 0)
			{
				var burrow = burrows.First();

				burrow.Print();

				// If this burrow is done (all the amphipods are in their respective homes),
				// check its energy usage, remove it from the list, and move on to the next burrow.
				if (burrow.IsDone)
				{
					leastEnergyExpended = Math.Min(leastEnergyExpended, burrow.EnergyExpended);
					burrows.Remove(burrow);
					continue;
				}

				// Get all positions with pods
				var podPositions = burrow.Positions.Where(p => !p.IsEmpty && !p.Pod.IsHome);

				// TODO Check whether this burrow is still viable.
				// If any hallway pods are blocking each other, then we can't continue.

				//var potentialNewBurrows = new List<Burrow>();

				// For each position, get all of its pod's potential moves and add a new burrow
				// Consider only hallway pods first
				bool isHallwayPodMovedToRoom = false;
				foreach (var podPosition in podPositions)
				{
					if (podPosition.IsHallway)
					{
						// The only position this pod can move to is its room,
						// and only if its room is empty of other pod types
						//var roomPositions = burrow.Positions.Where(p => p.IsRoom && p.Type == podPosition.Pod.Type);

						//// If any rooms contain a pod of another type, then this pod is blocked from moving in
						//if (roomPositions.Any(p => !p.IsEmpty && p.Pod.Type != podPosition.Pod.Type))
						//{
						//	continue;
						//}

						//int roomX = roomPositions.First().X;

						//// Make sure there are no pods between this pod and the room. We already checked the room
						//// positions, so just check horizontally down the hallway.
						//bool isBlocked = false;
						//if (roomX > podPosition.X)
						//{
						//	for (int x = podPosition.X + 1; x < roomX; x++)
						//	{
						//		//if (!burrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
						//		if (!burrow.GetPosition(x, 1).IsEmpty)
						//		{
						//			isBlocked = true;
						//			break;
						//		}
						//	}
						//}
						//else
						//{
						//	for (int x = podPosition.X - 1; x > roomX; x--)
						//	{
						//		//if (!burrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
						//		if (!burrow.GetPosition(x, 1).IsEmpty)
						//		{
						//			isBlocked = true;
						//			break;
						//		}
						//	}
						//}
						//if (isBlocked)
						//{
						//	continue;
						//}

						//// This pod is not blocked from moving into the room. Move it to the lowest available position.
						//var lowestY = roomPositions.Where(p => p.IsEmpty).Max(p => p.Y);

						//// Move the pod into its room
						//var nextBurrow = burrow.Clone();
						//nextBurrow.MovePod(podPosition.X, podPosition.Y, roomX, lowestY);
						//burrows.Add(nextBurrow);

						//// Disregard other potential moves
						//break;

						var nextBurrow = AttemptToMoveHallwayPodToRoom(burrow, podPosition);

						if (nextBurrow != null)
						{
							burrows.Add(nextBurrow);
							isHallwayPodMovedToRoom = true;

							// Disregard other potential moves
							break;
						}
					}
				}

				if (!isHallwayPodMovedToRoom)
				{
					var allPotentialNewBurrows = new List<Burrow>();
					isHallwayPodMovedToRoom = false;

					// If we weren't able to move a hallway pod to a room, then consider the pods that are still in rooms
					foreach (var podPosition in podPositions)
					{
						if (!podPosition.IsHallway)
						{
							//// first check whether any pods can move directly from their current room to their same type room
							//var roomPositions = burrow.Positions.Where(p => p.IsRoom && p.Type == podPosition.Pod.Type);

							//bool roomIsOpen = true;

							//// If any rooms contain a pod of another type, then this pod is blocked from moving in
							//if (roomPositions.Any(p => !p.IsEmpty && p.Pod.Type != podPosition.Pod.Type))
							//{
							//	roomIsOpen = false;
							//}

							//if (roomIsOpen)
							//{
							//	// Now check whether there are any pods blocking this pod
							//	int roomX = roomPositions.First().X;


							//}


							var movedPodType = podPosition.Pod.Type;
							var potentialNewBurrows = new List<Burrow>();

							// Move this pod out into each open position in the hallway, except for just outside rooms
							// (Don't worry about moving it from the hallway to its room at this time)
							foreach (var nextPosition in burrow.Positions.Where(p => p.IsAvailableHallway))
							{
								// Make sure there are no pods between this pod and the hallway position
								// First, check above the current pod
								bool isBlocked = false;
								for (int y = podPosition.Y - 1; y >= 1; y--)
								{
									//if (!burrow.Positions.First(p => p.X == podPosition.X && p.Y == y).IsEmpty)
									if (!burrow.GetPosition(podPosition.X, y).IsEmpty)
									{
										isBlocked = true;
										break;
									}
								}
								if (isBlocked)
								{
									// If we're blocked going up, then there's no where to go. Break
									break;
								}

								// Now check horizontally down the hallway
								if (nextPosition.X > podPosition.X)
								{
									for (int x = podPosition.X + 1; x < nextPosition.X; x++)
									{
										//if (!burrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
										if (!burrow.GetPosition(x, 1).IsEmpty)
										{
											isBlocked = true;
											break;
										}
									}
								}
								else
								{
									for (int x = podPosition.X - 1; x > nextPosition.X; x--)
									{
										//if (!burrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
										if (!burrow.GetPosition(x, 1).IsEmpty)
										{
											isBlocked = true;
											break;
										}
									}
								}
								if (isBlocked)
								{
									continue;
								}

								// Whew, the pod is not blocked. Move it!
								var nextBurrow = burrow.Clone();
								nextBurrow.MovePod(podPosition.X, podPosition.Y, nextPosition.X, nextPosition.Y);
								//burrows.Add(nextBurrow);
								potentialNewBurrows.Add(nextBurrow);
							}

							// Now check whether this same pod can move from the hallway to its room.
							// If yes, keep the burrow with the lowest expended energy, and discard the rest.

							// Now check whether any of these new burrows have a hallway pod that can move to its room.
							// If yes, then keep only that burrow and discard the rest.
							Burrow roomToRoomBurrow = null;
							foreach (var potentialNewBurrow in potentialNewBurrows)
							{
								var hallwayPodPositions = potentialNewBurrow.Positions.Where(p => !p.IsEmpty && p.IsHallway && p.Pod.Type == movedPodType);

								foreach (var hallwayPodPosition in hallwayPodPositions)
								{
									var potentialRoomToRoomBurrow = AttemptToMoveHallwayPodToRoom(potentialNewBurrow, hallwayPodPosition);

									// If we were successful in moving this pod from one room to another
									if (potentialRoomToRoomBurrow != null)
									{
										isHallwayPodMovedToRoom = true;

										if (roomToRoomBurrow == null)
										{
											roomToRoomBurrow = potentialRoomToRoomBurrow;
										}
										else if (potentialRoomToRoomBurrow.EnergyExpended < roomToRoomBurrow.EnergyExpended)
										{
											// Keep the next burrow with the least expended energy
											roomToRoomBurrow = potentialRoomToRoomBurrow;
										}

										//burrows.Add(nextBurrow);
										//isHallwayPodMovedToRoom = true;

										// Disregard other potential moves
										//break;

										
									}

									// The only position this pod can move to is its room,
									// and only if its room is empty of other pod types
									//var potentialNewBurrowRoomPositions = potentialNewBurrow.Positions.Where(p => p.IsRoom && p.Type == hallwayPodPosition.Pod.Type);

									//// If any rooms contain a pod of another type, then this pod is blocked from moving in
									//if (potentialNewBurrowRoomPositions.Any(p => !p.IsEmpty && p.Pod.Type != hallwayPodPosition.Pod.Type))
									//{
									//	continue;
									//}

									//int roomX = potentialNewBurrowRoomPositions.First().X;

									//// Make sure there are no pods between this pod and the room. We already checked the room
									//// positions, so just check horizontally down the hallway.
									//bool isBlocked = false;
									//if (roomX > hallwayPodPosition.X)
									//{
									//	for (int x = hallwayPodPosition.X + 1; x < roomX; x++)
									//	{
									//		//if (!potentialNewBurrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
									//		if (!potentialNewBurrow.GetPosition(x, 1).IsEmpty)
									//		{
									//			isBlocked = true;
									//			break;
									//		}
									//	}
									//}
									//else
									//{
									//	for (int x = hallwayPodPosition.X - 1; x > roomX; x--)
									//	{
									//		//if (!potentialNewBurrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
									//		if (!potentialNewBurrow.GetPosition(x, 1).IsEmpty)
									//		{
									//			isBlocked = true;
									//			break;
									//		}
									//	}
									//}
									//if (isBlocked)
									//{
									//	continue;
									//}

									//// This pod is not blocked from moving into the room. Move it to the lowest available position.
									//var lowestY = potentialNewBurrowRoomPositions.Where(p => p.IsEmpty).Max(p => p.Y);

									//// Move the pod into its room
									//var nextBurrow = potentialNewBurrow.Clone();
									//nextBurrow.MovePod(hallwayPodPosition.X, hallwayPodPosition.Y, roomX, lowestY);
									//burrows.Add(nextBurrow);

									//// Disregard other potential moves
									//break;
								}

								// We only care about one room-to-room move this time around
								//if (isHallwayPodMovedToRoom)
								//{
								//	break;
								//}
							}

							// If we couldn't move any pods directly from one room to the next,
							// then we want to consider all potential moves of pods to the hallway
							//if (!isHallwayPodMovedToRoom)
							if (roomToRoomBurrow == null)
							{
								allPotentialNewBurrows.AddRange(potentialNewBurrows);
							}
							else
							{
								burrows.Add(roomToRoomBurrow);
								break;
							}
						}
					}

					// If we couldn't move a pod directly from one room to another,
					// then add all potential burrows to our list of burrows to examine
					if (!isHallwayPodMovedToRoom)
					{
						burrows.AddRange(allPotentialNewBurrows);
					}
				}

				// Remove this burrow, as only its successors will be examined from here on out
				burrows.Remove(burrow);
			}


			return leastEnergyExpended;
		}

		private static Burrow AttemptToMoveHallwayPodToRoom(Burrow burrow, BurrowPosition podPosition)
		{
			// The only position this pod can move to is its room,
			// and only if its room is empty of other pod types
			var roomPositions = burrow.Positions.Where(p => p.IsRoom && p.Type == podPosition.Pod.Type);

			// If any rooms contain a pod of another type, then this pod is blocked from moving in
			if (roomPositions.Any(p => !p.IsEmpty && p.Pod.Type != podPosition.Pod.Type))
			{
				return null;
			}

			int roomX = roomPositions.First().X;

			// Make sure there are no pods between this pod and the room. We already checked the room
			// positions, so just check horizontally down the hallway.
			bool isBlocked = false;
			if (roomX > podPosition.X)
			{
				for (int x = podPosition.X + 1; x < roomX; x++)
				{
					//if (!burrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
					if (!burrow.GetPosition(x, 1).IsEmpty)
					{
						isBlocked = true;
						break;
					}
				}
			}
			else
			{
				for (int x = podPosition.X - 1; x > roomX; x--)
				{
					//if (!burrow.Positions.First(p => p.X == x && p.Y == 1).IsEmpty)
					if (!burrow.GetPosition(x, 1).IsEmpty)
					{
						isBlocked = true;
						break;
					}
				}
			}
			if (isBlocked)
			{
				return null;
			}

			// This pod is not blocked from moving into the room. Move it to the lowest available position.
			var lowestY = roomPositions.Where(p => p.IsEmpty).Max(p => p.Y);

			// Move the pod into its room
			var nextBurrow = burrow.Clone();
			nextBurrow.MovePod(podPosition.X, podPosition.Y, roomX, lowestY);
			//burrows.Add(nextBurrow);

			// Disregard other potential moves
			//break;

			return nextBurrow;
		}

		//private static bool IsViable(Burrow burrow)
		//{
		//	var hallwayPodPositions = burrow.Positions.Where(p => !p.IsEmpty && p.IsHallway).OrderBy(p => p.X);


		//}

		private static Burrow GetTestData(bool isPart2)
		{
			string data =
@"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";

			if (isPart2)
			{
				data =
@"#############
#...........#
###B#C#B#D###
  #D#C#B#A#
  #D#B#A#C#
  #A#D#C#A#
  #########";
			}

			return ParseData(data);
		}

		private static Burrow GetData(bool isPart2)
		{
			string data =
@"#############
#...........#
###B#B#D#D###
  #C#A#A#C#
  #########";

			if (isPart2)
			{
				data =
@"#############
#...........#
###B#B#D#D###
  #D#C#B#A#
  #D#B#A#C#
  #C#A#A#C#
  #########";
			}

			return ParseData(data);
		}

		private static Burrow ParseData(string data)
		{
			var burrow = new Burrow();

			var rows = data.Split("\r\n");

			int y = 1;

			foreach (string row in rows)
			{
				if (row.Contains("A") || row.Contains("B") || row.Contains("C") || row.Contains("D"))
				{
					y++;

					for (int x = 3; x <= 9; x += 2)
					{
						var pod = row[x] switch
						{
							'A' => new Amphipod(AmphipodType.A, 1),
							'B' => new Amphipod(AmphipodType.B, 10),
							'C' => new Amphipod(AmphipodType.C, 100),
							'D' => new Amphipod(AmphipodType.D, 1000),
							_ => throw new Exception(),
						};

						var homeType = x switch
						{
							3 => AmphipodType.A,
							5 => AmphipodType.B,
							7 => AmphipodType.C,
							9 => AmphipodType.D,
							_ => throw new Exception(),
						};

						burrow.Positions.Add(new BurrowPosition(x, y, pod, homeType));
					}
				}
			}

			// If any pods start off in the correct spots, mark them as being home
			Burrow.MaxY = burrow.Positions.Max(p => p.Y);
			for (int x = 3; x <= 9; x += 2)
			{
				// Start at the bottom and work our way up
				for (y = Burrow.MaxY; y > 1; y--)
				{
					var position = burrow.GetPosition(x, y);
					if (position.Type == position.Pod.Type)
					{
						position.Pod.IsHome = true;
					}
					else
					{
						// No need to check the pod above, as it will need to move regardless.
						// Move on to the next room.
						break;
					}
				}
			}

			return burrow;
		}
	}

	public class Burrow
	{
		public static int MaxY;

		public List<BurrowPosition> Positions { get; private set; }
		public int EnergyExpended { get; set; }

		public Burrow()
		{
			Positions = new List<BurrowPosition>();
			for (int x = 1; x <= 11; x++)
			{
				Positions.Add(new BurrowPosition(x, 1));
			}
		}

		public bool IsDone => Positions.Where(p => !p.IsEmpty).All(p => p.Pod.IsHome);

		public Burrow Clone()
		{
			var newBurrow = new Burrow
			{
				EnergyExpended = EnergyExpended
			};
			newBurrow.Positions = new List<BurrowPosition>();

			foreach (var position in Positions)
			{
				newBurrow.Positions.Add(position.Clone());
			}

			return newBurrow;
		}

		public void MovePod(int oldX, int oldY, int newX, int newY)
		{
			//var oldPosition = Positions.First(p => p.X == oldX && p.Y == oldY);
			var oldPosition = GetPosition(oldX, oldY);
			//var newPosition = Positions.First(p => p.X == newX && p.Y == newY);
			var newPosition = GetPosition(newX, newY);

			newPosition.Pod = oldPosition.Pod;
			oldPosition.Pod = null;

			// Return the amount of energy used by the pod to move to the new position
			EnergyExpended += (Math.Abs(newX - oldX) + Math.Abs(newY - oldY)) * newPosition.Pod.EnergyPerStep;

			// If the pod is now in its room, mark it as being home so we don't try to move it anymore
			if (newPosition.IsRoom && newPosition.Type == newPosition.Pod.Type)
			{
				newPosition.Pod.IsHome = true;
			}
		}

		public BurrowPosition GetPosition(int x, int y)
		{
			return Positions.First(p => p.X == x && p.Y == y);
		}

		public void Print()
		{
			Console.WriteLine("#############");

			// Hallway: #...........#
			Console.Write("#");
			for (int x = 1; x <= 11; x++)
			{
				//var position = Positions.First(p => p.X == x && p.Y == 1);
				var position = GetPosition(x, 1);
				Console.Write(position.Pod?.Type.ToString() ?? ".");
			}
			Console.WriteLine("#");

			// Top rooms: ###.#.#.#.###
			Console.Write("###");
			for (int x = 3; x <= 9; x += 2)
			{
				//var position = Positions.First(p => p.X == x && p.Y == 2);
				var position = GetPosition(x, 2);
				Console.Write((position.Pod?.Type.ToString() ?? ".") + "#");
			}
			Console.WriteLine("##");

			// Rooms after top room: #.#.#.#.#
			for (int y = 3; y <= MaxY; y++)
			{
				Console.Write("  #");
				for (int x = 3; x <= 9; x += 2)
				{
					//var position = Positions.First(p => p.X == x && p.Y == 3);
					var position = GetPosition(x, y);
					Console.Write((position.Pod?.Type.ToString() ?? ".") + "#");
				}
				Console.WriteLine();
			}

			Console.WriteLine("  #########\r\n\r\n");
		}
	}

	public class BurrowPosition
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool IsHallway => Y == 1;
		public bool IsRoom => !IsHallway;
		public AmphipodType? Type { get; private set; }
		public Amphipod Pod { get; set; }
		public bool IsEmpty => Pod == null;
		public bool IsAvailableHallway => IsHallway && X != 3 && X != 5 && X != 7 && X != 9 && IsEmpty;
		public bool IsPodHome => !IsEmpty && Type == Pod.Type;

		public BurrowPosition(int x, int y)
		{
			X = x;
			Y = y;
		}

		public BurrowPosition(int x, int y, Amphipod pod, AmphipodType type) : this(x, y)
		{
			Pod = pod;
			Type = type;
		}

		public BurrowPosition Clone()
		{
			return new BurrowPosition(X, Y, Pod?.Clone(), Type.GetValueOrDefault());
		}

		public override string ToString()
		{
			return $"[{X},{Y}] => {Pod?.Type.ToString() ?? "."}";
		}
	}

	public class Amphipod
	{
		public AmphipodType Type { get; private set; }
		public int EnergyPerStep { get; private set; }
		public bool IsHome { get; set; }

		public Amphipod(AmphipodType type, int energyPerStep)
		{
			Type = type;
			EnergyPerStep = energyPerStep;
		}

		public Amphipod Clone()
		{
			return new Amphipod(Type, EnergyPerStep)
			{
				IsHome = IsHome
			};
		}
	}

	public enum AmphipodType
	{
		A, B, C, D
	}
}
