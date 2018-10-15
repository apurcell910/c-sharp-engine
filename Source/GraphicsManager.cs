using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine
{
    public class GraphicsManager
    {
        private readonly Game game;
        private readonly Platform platform;

        internal GraphicsManager(Game game, Platform platform)
        {
            this.game = game;
            this.platform = platform;
        }
    }
}
