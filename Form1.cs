using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int opponentsSpeed = 3;
        int score =0;
        Random randNumb = new Random();

        List<PictureBox> opponentsList = new List<PictureBox>();
        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
               progressBar1.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                GameTimer.Stop();
            }

            txtAmmo.Text = "Боеприпасы: " + ammo;
            txtScore.Text = "Счет: " + score;

            if (goLeft == true && pictureBox1.Left > 0)
            {
                pictureBox1.Left -= speed;
            }
            if (goRight == true && pictureBox1.Left + pictureBox1.Width < this.ClientSize.Width)
            {
                pictureBox1.Left += speed;
            }
            if (goUp == true && pictureBox1.Top > 45)
            {
                pictureBox1.Top -= speed;
            }
            if (goDown == true && pictureBox1.Top + pictureBox1.Height < this.ClientSize.Height)
            {
                pictureBox1.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (pictureBox1.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;

                    }
                }


                if (x is PictureBox && (string)x.Tag == "opponents")
                {

                    if (pictureBox1.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }


                    if (x.Left > pictureBox1.Left)
                    {
                        x.Left -= opponentsSpeed;
                        ((PictureBox)x).Image = Properties.Resources.opponent_Left;
                    }
                    if (x.Left < pictureBox1.Left)
                    {
                        x.Left += opponentsSpeed;
                        ((PictureBox)x).Image = Properties.Resources.opponent_right;
                    }
                    if (x.Top > pictureBox1.Top)
                    {
                        x.Top -= opponentsSpeed;
                        ((PictureBox)x).Image = Properties.Resources.opponent_normal1;
                    }
                    if (x.Top < pictureBox1.Top)
                    {
                        x.Top += opponentsSpeed;
                        ((PictureBox)x).Image = Properties.Resources.opponent_Botton;
                    }

                }



                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "projectile" && x is PictureBox && (string)x.Tag == "opponents")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            opponentsList.Remove(((PictureBox)x));
                            MakeOpponents();
                        }
                    }
                }


            }


        }

        private void KeyIsDouwn(object sender, KeyEventArgs e)
        {
            if (gameOver == true)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                facing = "left";
                pictureBox1.Image = Properties.Resources.Tank_left;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                pictureBox1.Image = Properties.Resources.Tank_right;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                pictureBox1.Image = Properties.Resources.Tank_normal1;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                pictureBox1.Image = Properties.Resources.Tank_Botton;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            
            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootProjectile(facing);


                if (ammo < 1)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void ShootProjectile(string direction)
        {
            Projectile shootProjectile = new Projectile();
            shootProjectile.direction = direction;
            shootProjectile.projectileLeft = pictureBox1.Left + (pictureBox1.Width / 2);
            shootProjectile.projectileTop = pictureBox1.Top + (pictureBox1.Height / 2);
            shootProjectile.MakeProjectile(this);
        }

        private void MakeOpponents()
        {
            PictureBox opponents = new PictureBox();
            opponents.Tag = "opponents";
            opponents.Image = Properties.Resources.opponent_Botton;
            opponents.Left = randNumb.Next(0, 900);
            opponents.Top = randNumb.Next(0, 800);
            opponents.SizeMode = PictureBoxSizeMode.AutoSize;
            opponentsList.Add(opponents);
            this.Controls.Add(opponents);
            pictureBox1.BringToFront();
        }

        private void DropAmmo()
        {

            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.BBB;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNumb.Next(60, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNumb.Next(10, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            pictureBox1.BringToFront();
        }

        private void RestartGame()
        {
            pictureBox1.Image = Properties.Resources.Tank_normal1;

            foreach (PictureBox i in opponentsList)
            {
                this.Controls.Remove(i);
            }

            opponentsList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeOpponents();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 10;

            GameTimer.Start();
        }
    }
}
