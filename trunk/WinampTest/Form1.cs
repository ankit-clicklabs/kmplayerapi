using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinampAPI;

namespace WinampTest
{
	public partial class Form1 : Form
	{
		Winamp win = new Winamp();
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.Text = ((float)win.GetPos() / 1000f) + "s";
		}
	}
}
