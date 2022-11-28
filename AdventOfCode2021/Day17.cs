using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
	public class Day17
	{
		public static int Run()
		{
			//var targetArea = GetTestTargetArea();
			var targetArea = GetData();

			//var result = RunPart1(targetArea); // test: 45, 7750
			var result = RunPart2(targetArea); // test: 112, 4120

			return result;
		}

		private static int RunPart2(TargetArea targetArea)
		{
			int countOfProbesThatHitTargetArea = 0;

			for (int startingVelocityX = 1; startingVelocityX <= targetArea.MaxX; startingVelocityX++)
			{
				bool isNotViableX = false;

				for (int startingVelocityY = targetArea.MinY; startingVelocityY < 1000; startingVelocityY++)
				{
					if (isNotViableX)
					{
						break;
					}

					var probe = new Probe(startingVelocityX, startingVelocityY);

					while (true)
					{
						probe.Move();

						// If the probe is currently within the target area, store the max probe
						// height thus far and break
						if (probe.X >= targetArea.MinX && probe.X <= targetArea.MaxX &&
							probe.Y >= targetArea.MinY && probe.Y <= targetArea.MaxY)
						{
							countOfProbesThatHitTargetArea++;
							break;
						}

						// If the probe is not horizontally within the target area and it's not moving forward or backward, break
						if (probe.VelocityX == 0 && (probe.X < targetArea.MinX || probe.X > targetArea.MaxX))
						{
							// This x starting velocity won't work for any y staring velocity
							isNotViableX = true;
							break;
						}

						// If the probe has fallen below the target area, break
						if (probe.Y < targetArea.MinY)
						{
							break;
						}
					}
				}
			}

			return countOfProbesThatHitTargetArea;
		}

		private static int RunPart1(TargetArea targetArea)
		{
			int maxProbeHeight = 0;

			for (int startingVelocityX = 1; startingVelocityX <= targetArea.MaxX; startingVelocityX++)
			{
				bool isNotViableX = false;

				for (int startingVelocityY = 0; startingVelocityY < 1000; startingVelocityY++)
				{
					if (isNotViableX)
					{
						break;
					}

					var probe = new Probe(startingVelocityX, startingVelocityY);

					while (true)
					{
						probe.Move();

						// If the probe is currently within the target area, store the max probe
						// height thus far and break
						if (probe.X >= targetArea.MinX && probe.X <= targetArea.MaxX &&
							probe.Y >= targetArea.MinY && probe.Y <= targetArea.MaxY)
						{
							maxProbeHeight = Math.Max(maxProbeHeight, probe.MaxHeightReached);
							break;
						}

						// If the probe is not horizontally within the target area and it's not moving forward or backward, break
						if (probe.VelocityX == 0 && (probe.X < targetArea.MinX || probe.X > targetArea.MaxX))
						{
							// This x starting velocity won't work for any y staring velocity
							isNotViableX = true;
							break;
						}

						// If the probe has fallen below the target area, break
						if (probe.Y < targetArea.MinY)
						{
							break;
						}
					}
				}
			}

			return maxProbeHeight;
		}

		private static TargetArea GetTestTargetArea()
		{
			return new TargetArea(20, 30, -10, -5);
		}

		private static TargetArea GetData()
		{
			return new TargetArea(138, 184, -125, -71);
		}
	}

	public class Probe
	{
		public int VelocityX { get; private set; }
		private int _velocityY;
		public int X { get; private set; } = 0;
		public int Y { get; private set; } = 0;
		public int MaxHeightReached { get; private set; } = 0;

		public Probe(int velocityX, int velocityY)
		{
			VelocityX = velocityX;
			_velocityY = velocityY;
		}

		public void Move()
		{
			X += VelocityX;
			Y += _velocityY;

			// The probe's x velocity increases by 1 toward 0, and its y velocity decreases by 1
			_ = VelocityX > 0 ? VelocityX-- : VelocityX < 0 ? VelocityX++ : 0;
			_velocityY--;

			MaxHeightReached = Math.Max(MaxHeightReached, Y);
		}

		public override string ToString()
		{
			return $"({X}, {Y}) => ({VelocityX}, {_velocityY})";
		}
	}

	public class TargetArea
	{
		public int MinX { get; private set; }
		public int MaxX { get; private set; }
		public int MinY { get; private set; }
		public int MaxY { get; private set; }

		public TargetArea(int minX, int maxX, int minY, int maxY)
		{
			MinX = minX;
			MaxX = maxX;
			MinY = minY;
			MaxY = maxY;
		}
	}
}
