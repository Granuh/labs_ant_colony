using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntColony
{
    public partial class Form1 : Form
    {
        AntBase AntBase;        // Ук-ль на класс базу

        public Form1()
        {
            AntBase = new AntBase();
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            AntBase.Update();       // Обновляем базу

            textBoxAntsNumber.Text = AntBase.getAntsNum().ToString();
            textBoxWarriors.Text = AntBase.getNumWarriors().ToString();
            textBoxScouts.Text = AntBase.getNumScouts().ToString();
            textBoxBuilders.Text = AntBase.getNumBuilders().ToString();
            textBoxFooders.Text = AntBase.getNumFooders().ToString();
            textBoxFood.Text = AntBase.getFood().ToString();
            textBoxGrowthRate.Text = AntBase.getGrowthRate().ToString();
            textBoxCapacity.Text = AntBase.getCapacity().ToString();

            panel1.Invalidate();    // Вызвать перерисовку
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
             AntBase.Draw(e.Graphics);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = 100 - (int)(numericUpDown1.Value - 1) * 10;
        }
    }
}
