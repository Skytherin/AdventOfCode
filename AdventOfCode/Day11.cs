using System;
using System.Collections.Generic;
using System.IO;
using AdventOfCode2019.Utils;
using FluentAssertions;

namespace AdventOfCode2019
{
    public static class Day11
    {
        public static string Input => File.ReadAllText("Inputs/Day11.txt");

        public static void Step1()
        {
            var machine = new IntCodeMachine(Input.ToLongArray());
            var grid = new InfiniteGrid<Color>(Color.Black);

            var painted = new HashSet<Point>();
            var facing = new Vector(0, -1);

            machine.Drive(
                inputHandler: () => Convert.ToInt32(grid.CurrentColor),
                outputHandler: outputs =>
                {
                    if (outputs.Count != 2) throw new ApplicationException();
                    grid.CurrentColor = outputs[0] switch
                    {
                        0 => Color.Black,
                        1 => Color.White,
                        _ => throw new ApplicationException()
                    };
                    
                    painted.Add(grid.CurrentPosition);
                    facing = outputs[1] switch
                    {
                        0 => facing.TurnLeft(),
                        1 => facing.TurnRight(),
                        _ => throw new ApplicationException()
                    };
                    grid.Move(facing.dX, facing.dY);
                }
            );
            painted.Count.Should().Be(2088);
        }

        public static void Step2()
        {
            var machine = new IntCodeMachine(Input.ToLongArray());
            var grid = new InfiniteGrid<Color>(Color.Black);
            var facing = new Vector(0, -1);
            grid.CurrentColor = Color.White;

            machine.Drive(
                inputHandler: () => Convert.ToInt32(grid.CurrentColor),
                outputHandler: outputs =>
                {
                    if (outputs.Count != 2) throw new ApplicationException();
                    grid.CurrentColor = outputs[0] switch
                    {
                        0 => Color.Black,
                        1 => Color.White,
                        _ => throw new ApplicationException()
                    };
                    facing = outputs[1] switch
                    {
                        0 => facing.TurnLeft(),
                        1 => facing.TurnRight(),
                        _ => throw new ApplicationException()
                    };
                    grid.Move(facing.dX, facing.dY);
                }
            );

            grid.Rows().ForEach(row =>
            {
                row.ForEach(color => Console.Write(color == Color.Black ? " " : "*"));
                Console.WriteLine();
            });
        }
    }

    public enum Color
    {
        Black = 0,
        White = 1
    }

    public class InfiniteGrid<T>
    {
        // Position to value
        private Dictionary<Point, T> Grid { get; } = new Dictionary<Point, T>();

        public Point CurrentPosition { get; private set; } = new Point(0, 0);

        public InfiniteGrid(T defaultValue)
        {
            DefaultValue = defaultValue;
            Grid.Add(CurrentPosition, DefaultValue);
        }

        private T DefaultValue { get; }

        public T CurrentColor
        {
            get => Grid[CurrentPosition];
            set => Grid[CurrentPosition] = value;
        }

        public void Move(int dx, int dy)
        {
            var newPosition = new Point(CurrentPosition.X + dx, CurrentPosition.Y + dy);
            Grid[newPosition] = Grid.GetValueOrDefault(newPosition, DefaultValue);
            CurrentPosition = newPosition;
        }

        public List<List<T>> Rows()
        {
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;
            foreach (var key in Grid.Keys)
            {
                minX = Math.Min(minX, key.X);
                maxX = Math.Max(maxX, key.X);
                minY = Math.Min(minY, key.Y);
                maxY = Math.Max(maxY, key.Y);
            }

            var result = new List<List<T>>();

            for (var y = minY; y <= maxY; y++)
            {
                var row = new List<T>();
                for (var x = minX; x <= maxX; x++)
                {
                    row.Add(Grid.GetValueOrDefault(new Point(x, y), DefaultValue));
                }
                result.Add(row);
            }

            return result;
        }
    }
}
