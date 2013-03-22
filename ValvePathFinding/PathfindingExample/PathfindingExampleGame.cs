using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PathfindingExample
{
    // Enumerate different types of tiles that exist in our world
    public enum TileType
    {
        Path, // Regular, walkable area of the world
        Wall, // Impassable wall
        Slow, // Passable, but will slow you down.
        OutOfWorld, // A special sentinel value that makes some code simpler.  We imagine that our world is an island in a sea of tiles of this kind.
    };

    // Class that does the work of gnerating a path
    class Pathfinder
    {
        // Owner
        PathfindingExampleGame game;

        // Starting and ending positions
        public Point start, goal;

        // The step-by-step path we should take.
        // (The whole purpose of our program is to fill in this list.)
        public List<Point> computedPath;

        Point CurrentLocation;

        // Constructor sets up pathfinder object with the given start and ending positions
        public Pathfinder(PathfindingExampleGame game, Point start, Point goal)
        {
            this.game = game;
            this.start = start;
            this.goal = goal;
            computedPath = new List<Point>();
            CurrentLocation = start;

            // Start the pathfinding algorithm.  Return iterator
            // object we can use to release the yields
            iteratorKludge = FindPath().GetEnumerator();
            iteratorKludge.MoveNext();
        }

        // Do the actual work of generating a path.
        //
        // Don't get distracted by the IEnumerable and yields in here.
        // That's just so that we can easily have our algorithm pause and
        // show what it's doing
        public IEnumerable<int> FindPath()
        {


            
            yield return 0;
        }

        public void Update()
        {
            if (game.IsTileWalkable(CurrentLocation.X, CurrentLocation.Y + 1) && !computedPath.Contains(new Point(CurrentLocation.X, CurrentLocation.Y + 1)))
            {
                computedPath.Add(new Point(CurrentLocation.X, CurrentLocation.Y + 1));
                CurrentLocation.Y++;
            }
            else if (game.IsTileWalkable(CurrentLocation.X + 1, CurrentLocation.Y) && !computedPath.Contains(new Point(CurrentLocation.X + 1, CurrentLocation.Y)))
            {
                computedPath.Add(new Point(CurrentLocation.X + 1, CurrentLocation.Y));
                CurrentLocation.X++;
            }
            else if (game.IsTileWalkable(CurrentLocation.X - 1, CurrentLocation.Y) && !computedPath.Contains(new Point(CurrentLocation.X - 1, CurrentLocation.Y)))
            {
                computedPath.Add(new Point(CurrentLocation.X - 1, CurrentLocation.Y));
                CurrentLocation.X--;
            }
            
            else if (game.IsTileWalkable(CurrentLocation.X, CurrentLocation.Y - 1) && !computedPath.Contains(new Point(CurrentLocation.X , CurrentLocation.Y - 1)))
            {
                computedPath.Add(new Point(CurrentLocation.X, CurrentLocation.Y - 1));
                CurrentLocation.Y--;
            }
        }


        // Show what we're doing
        public void DrawProgress()
        {
            // Until we figure out how the algorithm works, we cnanot write this!



        }

        // A kludge so that we easily can pause our algorithm and let the game loop
        // continue to run, without a bunch of messy state machine logic.  This is
        // an advanced technique and it's not important to understand it.
        IEnumerator<int> iteratorKludge;
        public void Advance()
        {
            iteratorKludge.MoveNext();
        }
    }

    // Main game class
    public class PathfindingExampleGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Pathfinder pathfinder;
        SpriteFont font;

        // Tile textures
        Texture2D outOfWorldTexture;
        Texture2D pathTexture;
        Texture2D slowTexture;
        Texture2D wallTexture;

        // Sprites
        Texture2D heroTexture;
        Texture2D goalTexture;

        // Some useful abstract textures
        Texture2D whiteTexture; // solid white
        Texture2D rectTexture; // white rectangle, transparent center.

        // Previous input state.  Used to detect rising edge of key press
        KeyboardState keyboardState;

        // For simplicity in this example, we'll just use a fixed-size world
        public static int kWorldSizeX = 12;
        public static int kWorldSizeY = 12;

        // Hardcode the size of a tile, in screen pixels.
        // (Actual artwork can be any resolution, since it will get scaled)
        static int kTileSizeX = 64;
        static int kTileSizeY = 64;

        // Store the contents of each grid cell in a 2D array
        TileType[,] tile = new TileType[kWorldSizeX, kWorldSizeY];

        public PathfindingExampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = kTileSizeX * kWorldSizeX;
            graphics.PreferredBackBufferWidth = kTileSizeY * kWorldSizeY;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            keyboardState = Keyboard.GetState();

            SetExampleWorld1();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load up textures
            pathTexture = Content.Load<Texture2D>("dw_grass");
            wallTexture = Content.Load<Texture2D>("dw_tree");
            outOfWorldTexture = Content.Load<Texture2D>("out_of_world_16x16");
            slowTexture = Content.Load<Texture2D>("dw_mountain");
            whiteTexture = Content.Load<Texture2D>("white_solid");
            rectTexture = Content.Load<Texture2D>("white_rect");
            heroTexture = Content.Load<Texture2D>("dw_hero");
            goalTexture = Content.Load<Texture2D>("dw_treasure");

            font = Content.Load<SpriteFont>( "MyFont" );
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Fetch current keyboard state
            KeyboardState newState = Keyboard.GetState();

            // Space key, step pathfinding algorithm
            if (newState.IsKeyDown(Keys.Space) && !keyboardState.IsKeyDown(Keys.Space))
                pathfinder.Update();

            // 1: reset world to example #1
            if (newState.IsKeyDown(Keys.D1) && !keyboardState.IsKeyDown(Keys.D1))
                SetExampleWorld1();

            // R: reset world to random maze
            if (newState.IsKeyDown(Keys.R) && !keyboardState.IsKeyDown(Keys.R))
                SetExampleWorldRandom();

            // Save state for next time
            keyboardState = newState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw the actual tiles
            DrawWorldTiles();

            // Draw a grid to make the tiles easier to see
            DrawWorldGrid();

            // Draw starting and ending positions
            DrawTextureAtTile(pathfinder.start.X, pathfinder.start.Y, heroTexture);
            DrawTextureAtTile(pathfinder.goal.X, pathfinder.goal.Y, goalTexture);

            // If we have a path, draw it.  Otherwise,
            // show our progress
            if (pathfinder.computedPath != null)
                DrawPath(pathfinder.computedPath, Color.Red);
            else
                pathfinder.DrawProgress();

            // Done, flush queued sprites.
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //
        // Basic tile world queries
        //

        // Fetch the contents of a particular tile
        public TileType GetTile(int x, int y)
        {

            // Check for coordinates out-of-bounds, and return sentinel if so
            if (x < 0 || x >= kWorldSizeX || y < 0 || y >= kWorldSizeY)
                return TileType.OutOfWorld;

            // Do array access
            return tile[x, y];
        }

        // Return true if we can walk on the tile at the given coords
        public bool IsTileWalkable(int x, int y)
        {
            // Because our set of known tile types is so small, we'll just use a hardcoded table lookup.
            switch (GetTile(x, y))
            {
                case TileType.OutOfWorld:
                case TileType.Wall:
                    return false;
                case TileType.Path:
                case TileType.Slow:
                    return true;
            }
            throw new Exception("Bogus tile type!");
        }

        //
        // Drawing
        //

        // Calculate bounds of tile
        Rectangle CalcTileRect(int x, int y)
        {
            return new Rectangle(x * kTileSizeX, y * kTileSizeY, kTileSizeX, kTileSizeY);
        }

        // Draw a texture in the given tile
        void DrawTextureAtTile(int x, int y, Texture2D texture)
        {
            spriteBatch.Draw(texture, CalcTileRect(x, y), Color.White);
        }

        // Draw a single world tile
        void DrawTile(int x, int y)
        {

            // Because our set of known tile types is so small, we'll just use a hardcoded table lookup.
            Texture2D texture;
            switch (GetTile(x, y))
            {
                case TileType.OutOfWorld: texture = outOfWorldTexture; break;
                case TileType.Path: texture = pathTexture; break;
                case TileType.Slow: texture = slowTexture; break;
                case TileType.Wall: texture = wallTexture; break;
                default: throw new Exception("Bogus tile type!");
            }
            DrawTextureAtTile(x, y, texture);
        }

        // Draw all of the tiles in the world
        void DrawWorldTiles()
        {
            for (int y = 0; y < kWorldSizeY; ++y)
                for (int x = 0; x < kWorldSizeX; ++x)
                    DrawTile(x, y);
        }

        // Draw the the grid rectangle for the specified tile in the specified color
        void DrawTileGrid(int x, int y, Color color)
        {
            spriteBatch.Draw(rectTexture, CalcTileRect(x, y), color);
        }

        // Draw a grid over the entire world
        void DrawWorldGrid(int alpha = 32)
        {
            Color c = new Color(0, 0, 0, alpha);
            for (int y = 0; y < kWorldSizeY; ++y)
                for (int x = 0; x < kWorldSizeX; ++x)
                    DrawTileGrid(x, y, c);
        }

        // Draw some text centered in the specified tile
        public void DrawTextInTile(int iTileX, int iTileY, string sText, Color color)
        {
            Vector2 sz = font.MeasureString(sText);
            Rectangle tileRect = CalcTileRect(iTileX, iTileY);
            Vector2 pos = new Vector2(tileRect.Center.X - sz.X * 0.5f, tileRect.Center.Y - sz.Y * 0.5f);
            spriteBatch.DrawString(font, sText, new Vector2(pos.X + 1, pos.Y + 1), Color.Black);
            spriteBatch.DrawString(font, sText, pos, color);
        }

        // Draw a path through through files
        void DrawPath(List<Point> tiles, Color color)
        {
            int n = tiles.Count();
            for (int i = 0; i < n; ++i)
                DrawTextInTile(tiles[i].X, tiles[i].Y, Convert.ToString(i), Color.Red);
        }
        //
        // World creation
        //

        // Clear the entire world to the specified value
        void SetAllTiles(TileType tileType = TileType.Path)
        {
            SetRangeOfTiles(0, 0, kWorldSizeX - 1, kWorldSizeY - 1, tileType);
        }

        // Set the tile type for a range of tiles
        void SetRangeOfTiles(int x0, int y0, int x1, int y1, TileType tileType)
        {
            for (int y = y0; y <= y1; ++y)
                for (int x = x0; x <= x1; ++x)
                    tile[x, y] = tileType;
        }

        // Setup the world to a example #1
        void SetExampleWorld1()
        {
            // A clear field
            SetAllTiles(TileType.Path);

            // Except with two horizontal obstacles to go around
            SetRangeOfTiles(0, kWorldSizeY / 3, kWorldSizeX * 2 / 3, kWorldSizeY / 3, TileType.Wall);
            SetRangeOfTiles(kWorldSizeX / 3, kWorldSizeY * 2 / 3, kWorldSizeX - 1, kWorldSizeY * 2 / 3, TileType.Wall);

            // Setup pathfinder to solve hardcoded start and end positions
            pathfinder = new Pathfinder(this, new Point(kWorldSizeX / 2, 0), new Point(kWorldSizeX / 2, kWorldSizeY - 1));
        }

        // Setup random world using a decent maze generator
        void SetExampleWorldRandom()
        {
            Random r = new Random();

            // Fill the world with wall tiles.  We'll carve out a walkable path from this.
            SetAllTiles(TileType.Wall);

            // We'll keep a list of possible places where we might branch off from.
            List<Point> possibleBranchTiles = new List<Point>();

            // Seed it with a random starting location
            possibleBranchTiles.Add(RandomTile(r));

            // X and Y displacement for the 4 cardinal directions
            int[] cardinalDX = { -1, 0, +1, 0 };
            int[] cardinalDY = { 0, +1, 0, -1 };

            // Run a decent number of iterations to try to add random additions to our path
            for (int i = kWorldSizeX * kWorldSizeY * 10; i >= 0; --i)
            {

                // Select a random location to try to branch out from
                int branchFromWalkable = r.Next(possibleBranchTiles.Count());
                Point branchFromPos = possibleBranchTiles[ branchFromWalkable ];

                // Select a random direction.  This neighbor is the candidate tile
                // that we might open up
                int dir = r.Next(4);
                int candidateX = branchFromPos.X + cardinalDX[dir];
                int candidateY = branchFromPos.Y + cardinalDY[dir];

                // Make sure there's a wall we could erase in that direction
                if ( GetTile( candidateX, candidateY ) != TileType.Wall )
                    continue;

                // Check if our candidate tile has any walkable neighbor, in
                // the direction that we are moving.  We'll check the 3x3
                // neighborhood centered at the candidate tile
                bool bWalkableNeighbor = false;
                for ( int dy = -1 ; dy <= 1 && !bWalkableNeighbor ; ++dy )
                {
                    if (dy == -cardinalDY[dir])
                        // headed back from where we came, a walkable neighbor here is OK
                        continue;
                    for ( int dx = -1 ; dx <= 1 && !bWalkableNeighbor ; ++dx )
                    {
                        if (dx == -cardinalDX[dir])
                            // headed back from where we came, a walkable neighbor here is OK
                            continue;

                        // Is the nighbor walkable?
                        int nx = candidateX + dx;
                        int ny = candidateY + dy;
                        if ( IsTileWalkable( nx, ny ) )
                            bWalkableNeighbor = true;
                    }
                }

                // If there's a walkable neighbor, don't branch in that direction,
                // because it would creates a blob of whitespace that makes the maze
                // uninteresting.  We want a long winding corridor.
                if ( !bWalkableNeighbor )
                {

                    // Most of the time, we won't branch off from that same point again.
                    // Sometimes, leave it behind so we can try to branch off from that
                    // location again
                    if (r.Next(100) > 30)
                        possibleBranchTiles.RemoveAt(branchFromWalkable);

                    // Set the chosen tile to walkable
                    tile[ candidateX, candidateY ] = TileType.Path;

                    // Remember the new tile we just opened up as a potential
                    // place to branch off from and continue the maze
                    possibleBranchTiles.Add( new Point( candidateX, candidateY ) );
                }
            }

            // Now choose random start and end positions
            SelectRandomStartAndEndGoal(r);
        }

        // Select random tile, using specified random sequence
        Point RandomTile(Random r)
        {
            return new Point(r.Next(kWorldSizeX), r.Next(kWorldSizeY));
        }

        // Return the squared distance between two points
        static int DistanceSq(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        // Select random start and ending positions, initialize pathfinder
        void SelectRandomStartAndEndGoal( Random r )
        {

            // Select the pair of starting and ending positions that are
            // farthest apart after a few tries.
            int best = -1;
            for (int tries = 0; tries < 5; ++tries)
            {

                // Chose a random valid starting position.
                Point start;
                do
                {
                    start = RandomTile(r);
                } while (!IsTileWalkable(start.X, start.Y));

                // Chose a different random valid ending position.
                Point goal;
                do
                {
                    goal = RandomTile(r);
                } while (!IsTileWalkable(goal.X, goal.Y) || start.Equals(goal));

                // Better than the one already chosen?
                int dSQ = DistanceSq(start, goal);
                if (dSQ > best)
                {

                    // Yep, setup pathfinder object to solve between
                    // these selected points
                    pathfinder = new Pathfinder(this, start, goal);
                    best = dSQ;
                }
            }
        }
    }
}
