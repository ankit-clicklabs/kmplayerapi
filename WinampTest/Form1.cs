using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KMPlayerAPI;

namespace WinampTest
{
	public partial class Form1 : Form
	{
		KMPlayer win = new KMPlayer();
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			win.Seek(180000);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.Text = ((float)win.GetPos() / 1000f) + "s";
		}
	}
}
