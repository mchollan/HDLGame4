using System;
using Foundation;
using GameLibrary;
using UIKit;

namespace iOSGame
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static MainGame _game;

        internal static void RunGame()
        {
            _game = new MainGame();
            _game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, typeof(Program));
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}

