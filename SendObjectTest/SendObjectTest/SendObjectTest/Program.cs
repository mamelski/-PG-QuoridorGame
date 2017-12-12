namespace SendObjectTest
{
    using System;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            var test = new Test();
            test.SendBoardEvent();
            test.GetBoardEvent();
            Console.ReadLine();
        }
    }
}
