using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyossUpgradeScheduler.Services;
namespace MyossUpgradeScheduler
{
    public partial class SchedulerForm : Form
    {
        public SchedulerForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            ReminderService Obj = new ReminderService();
            String Rem1 = Obj.Reminder1EmailService();
            String Rem2 = Obj.Reminder2EmailService();
            Application.Exit();
            }
            catch(Exception ex)
            {
               
            }
        }
    }
}
