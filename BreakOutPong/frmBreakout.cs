/* Name: Miles Eastwood
 * Date: May 2014
 * Project: Unit 12/13 Form Application Game
 * 
 * Description: A simple breakout pong game, in which the player controls a paddle at the bottom of the screen
 *                  and has to a ball from touching the bottom of the screen. The ball will eventually 'breakout'
 *                      and touch the top of the screen, in which case the player wins the game.
 */

/*IDEAS FOR IMPROVEMENT:
 * XXXDoneXXXDifferent Bricks  - Bricks with an individual 'hit count' that goes down each time it is hit, and the brick only dissapears once it reaches zero. 
 *                      Could be colour coded
 * Levels - Turn the InitializeBrick function into InitializeLevelOne. Then make InitializeLevelTwo, Three, etc. 
 *              The other levels would draw different patterns of bricks.
 * XXXDoneXXXGame Interface - Have the game actually playing in a rectangle, and have a smaller rectangle on the form above or below the game. 
 *                      Could show various things such as:
 *      XXXDoneXXXLives - An integer that starts at three and goes down once each time the ball hits the bottom of screen. 
 *                  Game restarts the current level. Could be graphically shown by little balls in underbar
 *      XXXDoneXXXScore - Could be as simple as a static number of points per brick, or more for stronger bricks. Could be done in a number of ways
 * PowerUps - Could be done as a random event, or like difficulty mod, by # of bricks hit (every 10 bricks drop powerup). Random number could choose which
 *              powerup to create (multiple balls, wider paddle, slow ball speed, 'press space to shoot'). Powerups would fall from last block broken and if
 *                  they intersect with paddle, function becomes true for a certain number of timer ticks (e.g 300 ticks for 10 seconds)
 * High Score - May or may not be useful depending on how meaningful the scoring system is. At game over messagebox player could type in name and view top 3.
 * XXXDoneXXXBall Angling - At the moment, the ball starts at a 45 degree angle from paddle (5 up and 5 right or left per tick). As the difficulty increases,
 *                  the vertical velocity increases so the angle changes, but it would be cool if the ball relfected at a different angle depending on the 
 *                      place on the paddle that the ball hits. Can be done by making a number of invisible rectangles that move with the paddle and are only
 *                          used for collision. 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace BreakOutPong
{
    public partial class frmBreakout : Form
    {
        //create new rectangle for paddle
        int paddleStartX = 120;
        int paddleStartY = 430;
        Rectangle paddle = new Rectangle(120, 430, 60, 10);
        //create 5 rectangles that will sit within the rectangle and be used for collissions(different angles off different rectangles)
        Rectangle farLeftPaddle = new Rectangle(120, 430, 10, 10);
        Rectangle secondLeftPaddle = new Rectangle(130, 430, 10, 10);
        Rectangle middlePaddle = new Rectangle(140, 430, 20, 10);
        Rectangle secondRightPaddle = new Rectangle(160, 430, 10, 10);
        Rectangle farRightPaddle = new Rectangle(170, 430, 10, 10);

        //create new rectangle for ball
        int ballStartX = 140;
        int ballStartY = 410;
        Rectangle ball = new Rectangle(140, 410, 20, 20);

        //create new random class, to set initial x direction to left or right
        Random randy = new Random();

        //create integer for horizontal motion of paddle
        int paddleHorizontal = 0;

        //create integers for horizontal and vertical motion of ball
        int ballHorizontal = 0;
        int ballVertical = 0;
        int ballVelocity = 0;
        double ballAngle = 0;

        //create bool variable that says whether game has started or not
        bool gameActive = false;
        bool gameLost = false;
        bool gameWon = false;

        //bool variable to check to see if a key is currently being used, to account for if left and right keys are pressed at the same time
        bool keyPressed = false;

        //Create a list to draw the blocks to
        List<Brick> brickList = new List<Brick>();

        //Create empty rectangle that replaces rectangles that have been hit
        Rectangle emptyRectangle = new Rectangle(0, 0, 0, 0);

        //This variable will go up every time a brick is hit, and will be used to modify difficulty
        int bricksHit = 0;

        //This rectangle provides the black background for the data that will be displayed at the bottom of the screen
        Rectangle BottomScreen = new Rectangle(0, 460, 300, 140);

        //Integer for keeping track of lives
        int lifeCount = 3;

        int levelCount = 1;

        //This function is called in the timer, and redraws the bricks as taken from the bricklist
        void DrawBricks(PaintEventArgs e)
        {
            for (int i = 0; i <= (brickList.Count() - 1); i++)
            {
                brickList[i].paintBrick(e);
            }
        }


        //This function is only called at the beggining of the game or when game is restarted, and populates the bricklist
        void InitializeLevelOne()
        {
            int brickX = 10;
            int brickY = 5;
            //First for loop is for number of rows of bricks (in this case 6)
            for (int i = 0; i <= 5; i++)
            {
                //Second for loop is for number of bricks in each row (in this case 9)
                for (int j = 0; j <= 8; j++)
                {
                    Brick brick = new Brick(brickX, brickY, 20, 10, 1);
                    brickList.Add(brick);
                    brickX += 30;
                }
                brickX = 10;
                brickY += 20;
            }
        }

        void InitializeLevelTwo()
        {

        }
        //This function resets the position of the ball and paddle, redraws all of the bricks, and requires the player to press space to begin again
        void InitializeGame()
        {
            gameLost = false;
            gameWon = false;
            gameActive = false;
            this.tmr_Animation.Enabled = true;
            ball.X = ballStartX;
            ball.Y = ballStartY;
            ResetPaddles();
            ballHorizontal = 0;
            ballVertical = 0;
            ballVelocity = 0;
            ballAngle = 0;
            bricksHit = 0;
            lifeCount = 3;
            levelCount = 1;
            brickList.Clear();
            if (levelCount == 1)
                InitializeLevelOne();
        }

        //This function is very similar to initialize game, but keeps bricks as they are and decreases life count
        void LifeLost()
        {
            lifeCount -= 1;
            gameLost = false;
            gameWon = false;
            gameActive = false;
            this.tmr_Animation.Enabled = true;
            ball.X = ballStartX;
            ball.Y = ballStartY;
            ResetPaddles();
            ballHorizontal = 0;
            ballVertical = 0;
            ballVelocity = 0;
            ballAngle = 0;
        }

        void ResetPaddles()
        {
            paddle.X = paddleStartX;
            paddle.Y = paddleStartY;
            farLeftPaddle.X = 120;
            farLeftPaddle.Y = 430;
            secondLeftPaddle.X = 130;
            secondLeftPaddle.Y = 430;
            middlePaddle.X = 140;
            middlePaddle.Y = 430;
            secondRightPaddle.X = 160;
            secondRightPaddle.Y = 430;
            farRightPaddle.X = 170;
            farRightPaddle.Y = 430;
        }

        //THIS IS NEW FOR FINAL PROJECT
        void VelocityCalc(ref double ballAngle, ref int ballVelocity, ref int ballHorizontal, ref int ballVertical)
        {
            int posNeg;
            if (ballHorizontal > 0)
                posNeg = 1;
            else
                posNeg = -1;

            ballVertical = (int)Math.Round(ballVelocity * Math.Sin(ballAngle));
            ballHorizontal = (int)Math.Round(ballVelocity * Math.Cos(ballAngle));
            ballHorizontal *= posNeg;
        }

        public frmBreakout()
        {
            InitializeComponent();
        }

        private void frmBreakout_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //draw paddle
            g.FillRectangle(Brushes.Black, paddle);

            //draw ball
            g.FillEllipse(Brushes.Orange, ball);

            //draw bricks
            DrawBricks(e);

            //Draw bottom of screenfor info NEW FOR FINAL PROJECT
            g.FillRectangle(Brushes.Black, BottomScreen);

            //Display score (for now this is just number of bricks hit)
            Font scoreFont = new Font("Arial", 13);
            string scoreDisplay = "Score: " + bricksHit.ToString();
            g.DrawString(scoreDisplay, scoreFont, Brushes.White, 5, 470);

            //Display current ball speed (this relates to difficulty)
            string ballSpeedDisplay = "Ball Speed: " + Math.Abs(ballVelocity);
            g.DrawString(ballSpeedDisplay, scoreFont, Brushes.White, 5, 490);

            //Display current lives in the form of symbols
            int symbolX = 260;
            int symbolY = 535;
            for (int i = 0; i < lifeCount; i++)
            {
                g.FillEllipse(Brushes.Orange, new Rectangle(symbolX, symbolY, 20, 20));
                symbolX -= 30;
            }

            //Display some basic instructions before the game starts
            if (gameActive == false)
            {
                Font drawFont = new Font("Arial", 13);
                e.Graphics.DrawString("Press Space to Start", drawFont, Brushes.Black, 60, 150);
                e.Graphics.DrawString("Try to BreakOut\u2122 to the top!", drawFont, Brushes.Black, 40, 170);
            }

            //This is a bit of a relic of early dev, is now replaced by the message box
            /*if (gameLost == true)
            {
                //Create new font to display on screen and display that the game is over
                Font drawFont = new Font("Arial", 16);
                e.Graphics.DrawString("Game Over", drawFont, Brushes.Red, 80, 150);
            }*/
        }

        private void frmBreakout_KeyDown(object sender, KeyEventArgs e)
        {
            //Pressing the space key launches the ball in a random x direction and a set y speed
            if (gameActive == false)
            {
                if (e.KeyCode == Keys.Space)
                {
                    gameActive = true;

                    ballAngle = (45 * Math.PI) / 180;
                    ballVelocity = 7;

                    VelocityCalc(ref ballAngle, ref ballVelocity, ref ballHorizontal, ref ballVertical);
                    ballVertical *= -1;

                    //Use random number to deside whether the ball starts moving to left or right
                    if (randy.Next(1, 3) == 1)
                        ballHorizontal *= -1;
                }
            }

            //Paddle cannot move until spacebar has been pressed and game has been started
            if (gameActive == true)
            {
                //Sends the paddle left
                if (e.KeyCode == Keys.Left && keyPressed == false)
                {
                    keyPressed = true;
                    paddleHorizontal = -8;
                }
                //Sends the paddle right
                if (e.KeyCode == Keys.Right && keyPressed == false)
                {
                    keyPressed = true;
                    paddleHorizontal = 8;
                }
            }
        }

        private void tmr_Animation_Tick(object sender, EventArgs e)
        {
            //Moves paddle to left IF it is NOT at leftbound 
            if (paddle.X > 0 && paddleHorizontal < 0)
            {
                paddle.X += paddleHorizontal;
                farLeftPaddle.X += paddleHorizontal;
                secondLeftPaddle.X += paddleHorizontal;
                middlePaddle.X += paddleHorizontal;
                secondRightPaddle.X += paddleHorizontal;
                farRightPaddle.X += paddleHorizontal;
            }

            //Moves paddle to right IF it is NOT at rightbound
            if (paddle.X + paddle.Width < 285 && paddleHorizontal > 0)
            {
                paddle.X += paddleHorizontal;
                farLeftPaddle.X += paddleHorizontal;
                secondLeftPaddle.X += paddleHorizontal;
                middlePaddle.X += paddleHorizontal;
                secondRightPaddle.X += paddleHorizontal;
                farRightPaddle.X += paddleHorizontal;
            }

            //Reverses X direction if ball reaches left or right bound
            if (ball.X < 0 || ball.X + ball.Width > 285)
                ballHorizontal = -ballHorizontal;

            //Stops ball moving if ball reaches top of screen and ends game
            if (ball.Y < 0)
            {
                ballVertical = 0;
                ballHorizontal = 0;
                levelCount += 1;
                gameActive = false;
                if (levelCount > 3)
                {
                    gameWon = true;
                }
            }

            //Checks for intersection between ball and paddle, and allows ball to bounce off

            if (ball.IntersectsWith(farLeftPaddle) || ball.IntersectsWith(farRightPaddle))
            {
                ballAngle = 30 * Math.PI / 180;
                VelocityCalc(ref ballAngle, ref ballVelocity, ref ballHorizontal, ref ballVertical);
                ballVertical = -ballVertical;
            }
            else if (ball.IntersectsWith(secondLeftPaddle) || ball.IntersectsWith(secondRightPaddle))
            {
                ballAngle = 45 * Math.PI / 180;
                VelocityCalc(ref ballAngle, ref ballVelocity, ref ballHorizontal, ref ballVertical);
                ballVertical = -ballVertical;
            }
            else if (ball.IntersectsWith(middlePaddle))
            {
                ballAngle = 60 * Math.PI / 180;
                VelocityCalc(ref ballAngle, ref ballVelocity, ref ballHorizontal, ref ballVertical);
                ballVertical = -ballVertical;
            }

            //Checks if ball intersection for every brick
            for (int i = 0; i <= (brickList.Count() - 1); i++)
            {
                if (ball.IntersectsWith(brickList[i].getBrick()))
                {
                    brickList[i].brickHit();
                    ballVertical = Math.Abs(ballVertical);
                    bricksHit++;
                    if (bricksHit % 5 == 0)
                    {
                        ballVelocity += 1;
                        VelocityCalc(ref ballAngle, ref ballVelocity, ref ballHorizontal, ref ballVertical);
                    }
                }

            }

            //Ends the game if ball reaches bottom of screen
            if (ball.Y + ball.Height > 460)
            {
                if (lifeCount > 0)
                {
                    LifeLost();
                }
                else
                {
                    gameLost = true;
                    gameActive = false;
                    ballHorizontal = 0;
                    ballVertical = 0;
                }
            }

            //Check to see if player wants to play again
            if (gameWon == true || gameLost == true)
            {
                this.tmr_Animation.Enabled = false;
                string message;
                string caption = "Play Again?";
                if (gameWon == true) message = "You Won! Play again?";
                else message = "You Lost... Play Again?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                //Creates message box asking if player wants to play again
                result = MessageBox.Show(message, caption, buttons);

                //Resets game if player presses yet
                if (result == DialogResult.Yes)
                {
                    InitializeGame();
                }
                //Closes game if player presses no
                if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }

            //Updates postion of ball
            ball.X += ballHorizontal;
            ball.Y += ballVertical;

            //Repaints the form to animate
            Refresh();
        }

        private void frmBreakout_KeyUp(object sender, KeyEventArgs e)
        {
            //Stops the paddle moving left
            if (e.KeyCode == Keys.Left)
            {
                keyPressed = false;
                paddleHorizontal = 0;
            }
            //Stops the paddle moving left
            if (e.KeyCode == Keys.Right)
            {
                keyPressed = false;
                paddleHorizontal = 0;
            }
        }

        private void frmBreakout_Load(object sender, EventArgs e)
        {
            InitializeGame();
        }

        class Brick
        {
            int hitCount = 0;
            Rectangle brick = new Rectangle();

            public Brick(int brickX, int brickY, int brickWidth, int brickHeight, int strength)
            {
                hitCount = strength;
                brick.X = brickX;
                brick.Y = brickY;
                brick.Width = brickWidth;
                brick.Height = brickHeight;
            }

            public void paintBrick(PaintEventArgs e)
            {
                SolidBrush brickBrush = new SolidBrush(Color.Red);

                if (hitCount == 2)
                    brickBrush.Color = Color.Green;
                if (hitCount == 1)
                    brickBrush.Color = Color.Blue;

                e.Graphics.FillRectangle(brickBrush, brick);
            }

            public void brickHit()
            {
                hitCount -= 1;

                if (hitCount <= 0)
                {
                    brick.X = 0;
                    brick.Y = 0;
                    brick.Width = 0;
                    brick.Height = 0;
                }
            }

            public Rectangle getBrick()
            {
                return brick;
            }

        }


    }
}
