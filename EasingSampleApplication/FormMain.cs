using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasingSampleApplication
{
    public partial class FormMain : Form
    {
        private Timer timer;
        private Stopwatch easingTime;
        private int easingDuration;
        private double easingIndex;
        private MethodInfo methodInfo;

        public FormMain()
        {
            InitializeComponent();

            // fill combobox with easing function names
            var easingFunctionNames = Enum.GetNames(typeof(EasingFunction));
            foreach (var easingFunctionName in easingFunctionNames)
                comboBox1.Items.Add(easingFunctionName);
            comboBox1.SelectedIndex = 0;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            easingIndex = 0;

            easingDuration = (int)(numericUpDown1.Value * 1000);

            Type type = typeof(Easing);
            methodInfo = type.GetMethod(comboBox1.Text);

            timer = new Timer();
            timer.Interval = 1; 
            timer.Tick += Timer_Tick;
            timer.Start();

            easingTime = new Stopwatch();
            easingTime.Start();

            buttonPlay.Enabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            easingIndex = (double)easingTime.ElapsedMilliseconds / easingDuration;

            var result = methodInfo.Invoke(null, new object[] { easingIndex });
            pictureBox1.Left = 30 + (int)((double)result * (Width - pictureBox1.Size.Width - 60));

            if (easingIndex >= 1.0)
            {
                easingTime.Stop();
                easingTime = null;

                timer.Stop();
                buttonPlay.Enabled = true;
            }
        }
    }
}
