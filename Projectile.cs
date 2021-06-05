using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Projectile
    {
        public string direction;
        public int projectileLeft;
        public int projectileTop;

        private int speed = 20;
        private PictureBox projectile = new PictureBox();
        private Timer projectileTimer = new Timer();

        public void MakeProjectile(Form form)
        {
            projectile.BackColor = Color.Yellow;
            projectile.Size = new Size(10, 10);
            projectile.Tag = "projectile";
            projectile.Left = projectileLeft;
            projectile.Top = projectileTop;
            projectile.BringToFront();

            form.Controls.Add(projectile);
            projectileTimer.Interval = speed;
            projectileTimer.Tick += new EventHandler(ProjectileTimerEvent);
            projectileTimer.Start();
        }

        private void ProjectileTimerEvent(object sender, EventArgs e)
        {
            if (direction == "left")
            {
                projectile.Left -= speed;
            }

            if (direction == "right")
            {
                projectile.Left += speed;
            }

            if (direction == "up")
            {
                projectile.Top -= speed;
            }

            if (direction == "down")
            {
                projectile.Top += speed;
            }

            if (projectile.Left < 10 || projectile.Left > 900 || projectile.Top < 10 || projectile.Top > 800)
            {
                projectileTimer.Stop();
                projectileTimer.Dispose();
                projectile.Dispose();
                projectileTimer = null;
                projectile = null;
            }
        }
    }
}
