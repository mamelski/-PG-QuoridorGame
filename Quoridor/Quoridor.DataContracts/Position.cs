using System;
using System.Dynamic;
using System.Runtime.Serialization;

namespace Quoridor.DataContracts
{
    /// <summary>
    ///     The position.
    /// </summary>
    [DataContract]
    public class Position : IEquatable<Position>
    {
        public Position()
        {
        }

        public Position(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Gets or sets the x.
        /// </summary>
        [DataMember]
        public int X { get; set; }

        /// <summary>
        ///     Gets or sets the y.
        /// </summary>
        [DataMember]
        public int Y { get; set; }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !Equals(left, right);
        }

        public bool IsAdjacent(Position pos)
        {
            return (X == pos.X && Math.Abs(Y - pos.Y) == 2) || (Y == pos.Y && Math.Abs(X - pos.X) == 2);
        }


        public static Position operator +(Position p1, Position p2)
        {
            return new Position {X = p1.X + p2.X, Y = p1.Y + p2.Y};
        }


        public static Position operator -(Position p1, Position p2)
        {
            return new Position {X = p1.X - p2.X, Y = p1.Y - p2.Y};
        }

        public static Position operator -(Position p)
        {
            return new Position(-p.X, -p.Y);
        }
    }
}