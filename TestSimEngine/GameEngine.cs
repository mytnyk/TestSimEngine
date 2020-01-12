using System;
using System.Diagnostics;

namespace TestSimEngine
{
    public class GameEngine
    {
        public const int N = 10000;
        public readonly Obj[] Objects = new Obj[N];

        private readonly Stopwatch _gameTimer = new Stopwatch();

        public void Create()
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                //Objects[i] = new Obj();
                Objects[i].Init();
            }

            var indices = new int[Objects.Length];

            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }
            // sort by x
            Array.Sort(indices, (l, r) => 
            {
                return Math.Sign(Objects[l].X - Objects[r].X);
            });
            Objects[indices[0]].LeftAdjacent = -1;
            for (int i = 0; i < indices.Length - 1; i++)
            {
                var objId = indices[i];
                var nextObjId = indices[i + 1];
                Objects[objId].RightAdjacent = nextObjId;
                Objects[nextObjId].LeftAdjacent = objId;
            }
            Objects[indices[indices.Length - 1]].RightAdjacent = -1;
            // sort by y
            Array.Sort(indices, (t, b) =>
            {
                return Math.Sign(Objects[t].Y - Objects[b].Y);
            });
            Objects[indices[0]].TopAdjacent = -1;
            for (int i = 0; i < indices.Length - 1; i++)
            {
                var objId = indices[i];
                var nextObjId = indices[i + 1];
                Objects[objId].BottomAdjacent = nextObjId;
                Objects[nextObjId].TopAdjacent = objId;
            }
            Objects[indices[indices.Length - 1]].BottomAdjacent = -1;

            // start timer?
            _gameTimer.Start();
        }

        //private TimeSpan _previousGameTick = TimeSpan.Zero;
        //public int TicksPerSecond { get; set; } = 0;
        //public double CalculationTimeInMs { get; set; } = 0.0;
        public void Tick(double elapsedSeconds)
        {
            //TicksPerSecond++;
            //var elapsed = _gameTimer.Elapsed;
            //var elapsedInMs = (elapsed - _previousGameTick).TotalSeconds;
            //_previousGameTick = elapsed;
            for (int i = 0; i < Objects.Length; i++)
            {
                Objects[i].X += elapsedSeconds * Objects[i].VelocityX;
                // update position by X:
                var l = Objects[i].LeftAdjacent;
                var r = Objects[i].RightAdjacent;
                while (l != -1 && (Objects[l].X > Objects[i].X))
                {
                    r = l;
                    l = Objects[l].LeftAdjacent;
                }
                while (r != -1 && (Objects[r].X < Objects[i].X))
                {
                    l = r;
                    r = Objects[r].RightAdjacent;
                }
                if (Objects[i].LeftAdjacent != l)
                { // relative position has changed
                    //1.collapse old segment
                    var la = Objects[i].LeftAdjacent;
                    var ra = Objects[i].RightAdjacent;
                    if (la != -1)
                        Objects[la].RightAdjacent = ra;
                    if (ra != -1)
                        Objects[ra].LeftAdjacent = la;
                    //2.assign new segment
                    Objects[i].LeftAdjacent = l;
                    Objects[i].RightAdjacent = r;
                    //3.update segment
                    if (l != -1)
                        Objects[l].RightAdjacent = i;
                    if (r != -1)
                        Objects[r].LeftAdjacent = i;
                }

                Objects[i].Y += elapsedSeconds * Objects[i].VelocityY;

                // update position by Y:
                var t = Objects[i].TopAdjacent;
                var b = Objects[i].BottomAdjacent;
                while (t != -1 && (Objects[t].Y > Objects[i].Y))
                {
                    b = t;
                    t = Objects[t].TopAdjacent;
                }
                while (b != -1 && (Objects[b].Y < Objects[i].Y))
                {
                    t = b;
                    b = Objects[b].BottomAdjacent;
                }
                if (Objects[i].TopAdjacent != t)
                { // relative position has changed
                    //1.collapse old segment
                    var ta = Objects[i].TopAdjacent;
                    var ba = Objects[i].BottomAdjacent;
                    if (ta != -1)
                        Objects[ta].BottomAdjacent = ba;
                    if (ba != -1)
                        Objects[ba].TopAdjacent = ta;
                    //2.assign new segment
                    Objects[i].TopAdjacent = t;
                    Objects[i].BottomAdjacent = b;
                    //3.update segment
                    if (t != -1)
                        Objects[t].BottomAdjacent = i;
                    if (b != -1)
                        Objects[b].TopAdjacent = i;
                }
            }
            //CalculationTimeInMs = (_gameTimer.Elapsed - elapsed).TotalMilliseconds;
        }
    }
}
