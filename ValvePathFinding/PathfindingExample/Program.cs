using System;

namespace PathfindingExample
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PathfindingExampleGame game = new PathfindingExampleGame())
            {
                game.Run();
            }
        }
    }
#endif
}

