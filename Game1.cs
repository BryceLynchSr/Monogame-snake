using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




namespace Ormen
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        MouseState mus = Mouse.GetState();
        MouseState gammalMus = Mouse.GetState();
        SpriteFont arial;

        Random slump = new Random();

        // 0 för meny, 1 för spel
        int scen = 0;

        // Menyvariabler
        Texture2D knappBild;
        Rectangle knappRect;
        string välkomstText = "Träffa ormen!";
        Vector2 välkomstPosition;

        // Spelvariabler
        List<Rectangle> ormar = new List<Rectangle>();
        Texture2D ormBild;
        int startAntalOrmar = 5;
        int updatesMellanNyaOrmar = 90;
        int updatesTillNästaOrm = 90;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>("ormspel");
            ormBild = Content.Load<Texture2D>("snake");
            knappBild = Content.Load<Texture2D>("button");
            knappRect = new Rectangle(400 - knappBild.Width / 2, 360, knappBild.Width, knappBild.Height);
            välkomstPosition = new Vector2(400 - arial.MeasureString(välkomstText).X / 2, 100);
        }

        protected override void Update(GameTime gameTime)
        {
            gammalMus = mus;
            mus = Mouse.GetState();

            switch (scen)
            {
                case 0:
                    UppdateraMeny();
                    break;
                case 1:
                    UppdateraSpel();
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            switch (scen)
            {
                case 0:
                    RitaMeny();
                    break;
                case 1:
                    RitaSpel();
                    break;
            }




            base.Draw(gameTime);
        }

        bool VänsterMusTryckt()
        {
            if (mus.LeftButton == ButtonState.Pressed && gammalMus.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void BytScen(int nyscen)
        {
            scen = nyscen;

            if (nyscen == 1)
            {
                ormar.Clear();
                for (int i = 0; i < startAntalOrmar; i++)
                {
                    LäggTillOrm();
                }
            }
            updatesTillNästaOrm = updatesMellanNyaOrmar;
        }

        void LäggTillOrm()
        {
            int nyOrmX = slump.Next(0, 800 - ormBild.Width);
            int nyOrmY = slump.Next(0, 480 - ormBild.Height);
            Rectangle nyOrmRect = new Rectangle(nyOrmX, nyOrmY, ormBild.Width, ormBild.Height);
            ormar.Add(nyOrmRect);
        }

        void UppdateraMeny()
        {
            if (VänsterMusTryckt() && knappRect.Contains(mus.Position))
            {
                BytScen(1);
            }
        }

        void UppdateraSpel()
        {
            updatesTillNästaOrm--;
            if (updatesTillNästaOrm <= 0)
            {
                LäggTillOrm();
                updatesTillNästaOrm = updatesMellanNyaOrmar;
            }

            if (VänsterMusTryckt())
            {
                for (int i = ormar.Count - 1; i >= 0; i--)
                {
                    if (ormar[i].Contains(mus.Position))
                    {
                        ormar.RemoveAt(i);
                        break;
                    }
                }
            }

            if (ormar.Count == 0)
            {
                BytScen(0);
            }
        }

        /// </summary>
        void RitaMeny()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(arial, välkomstText, välkomstPosition, Color.White);
            spriteBatch.Draw(knappBild, knappRect, Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Kod för att rita scenen Spel
        /// </summary>
        void RitaSpel()
        {
            GraphicsDevice.Clear(Color.Tomato);

            spriteBatch.Begin();
            foreach (Rectangle ormRect in ormar)
            {
                spriteBatch.Draw(ormBild, ormRect, Color.White);
            }
            spriteBatch.End();
        }
    }
}
