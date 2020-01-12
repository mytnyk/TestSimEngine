using System;

namespace TestSimEngine
{
    public struct Obj
    {
        private static int NextId = 0;
        private static readonly Random rX = new Random(10);
        private static readonly Random rY = new Random(20);
        public int Id { get; private set; }
        public string Name { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        // so far velocity per second??
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public int LeftAdjacent { get; set; }
        public int RightAdjacent { get; set; }
        public int TopAdjacent { get; set; }
        public int BottomAdjacent { get; set; }

        public void Init()
        {
            Id = NextId++;
            Name = $"{Id}";
            X = rX.NextDouble();
            VelocityX = (rX.NextDouble() - 0.5) * 0.1;
            Y = rY.NextDouble();
            VelocityY = (rY.NextDouble() - 0.5) * 0.1;
        }

    }
}
