using MonoGame.Framework.Utilities;

namespace GameLibrary.Models
{
    public static class Platforms
    {
        /// <summary>
        /// Returns the enum of the supported platform
        /// </summary>
        /// <returns>Monogame platform from the Monogame Framework Utilities namespace</returns>
        public static MonoGamePlatform DeterminePlatform()
        {
            MonoGamePlatform currentPlatform = PlatformInfo.MonoGamePlatform;

            return currentPlatform;
        }
    }
}
