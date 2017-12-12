using System;
using System.Collections.Generic;
using System.Text;

namespace Quoridor.Utils
{
    public static class RandomProvider
    {
        private static Random random = new Random();

        public static int GetRandomNumber()
        {
            LastNumber = random.Next();
            return LastNumber;
        }

        public static double GetRandomValue()
        {
            LastValue = random.NextDouble();
            return LastValue;
        }

        public static double LastValue { get; set; }

        public static int LastNumber { get; set; }
    }
}
