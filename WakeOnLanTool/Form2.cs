using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WakeOnLanTool
{
    public partial class Form2 : Form
    {
        public List<string> RefMacs;
        private ComboBox remoteCombo;


        public Form2()
        {
            InitializeComponent();
        }

        public void setRefMacs(ref List<string> RefList, ref ComboBox refcbox)
        {
            RefMacs = RefList;
            remoteCombo = refcbox;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (RefMacs != null)
            {     
                RefMacs.Add(textBox1.Text);
                remoteCombo.Items.Add(textBox1.Text);
            }
            this.Hide();
        }
    }
}
