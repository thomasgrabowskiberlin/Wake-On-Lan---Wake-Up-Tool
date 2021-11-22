using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WakeOnLanLibrary;


namespace WakeOnLanTool
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        public Form2 ChildForm;
        private WakeOnLan WOL;
        byte[] NIC = { 0xD8, 0xBB, 0xC1, 0x0C, 0x11, 0xDC }; // GamingPC D8:BB:C1:0C:11:DC
        //byte[] NIC = { 0x00, 0x1F, 0x16, 0x15, 0x7C, 0x68 }; // X200s 00:1F:16:15:7C:68
        private List<String> Macs;

        public Form1()
        {
            InitializeComponent();
            ChildForm = new Form2();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            Macs = new List<String>();
            WOL = new WakeOnLan();
            Macs.Add("D8:BB:C1:0C:11:DC"); // example Mac 1
            Macs.Add("00:1F:16:15:7C:68"); // example Mac 2
            foreach (var item in Macs)
                comboBox1.Items.Add(item);
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PhysicalAddress mac;
            string stAdr;
            try
            {
                stAdr = comboBox1.GetItemText(comboBox1.Items[comboBox1.SelectedIndex]).ToString().Replace(":", string.Empty);
                mac = PhysicalAddress.Parse(stAdr);
                NIC = mac.GetAddressBytes();
            }
            catch
            {
                MessageBox.Show("MAC address format error");
                return;
            }
            finally
            {

            }

            WOL.setBroadCastIP("255.255.255.255");
            WOL.setNIC(NIC);
            WOL.SendMagicBytes();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string line;
            openFileDialog.FileName = "*.lst";
            openFileDialog.ShowDialog();
            try
            {
                TextReader texR = new StreamReader(openFileDialog.FileName);
                Macs.Clear();
                comboBox1.Items.Clear();
                line = "Start";
                while (line != null)
                {
                    line = texR.ReadLine();
                    if (line != null)
                    {
                        Macs.Add(line);
                        comboBox1.Items.Add(line);
                    }
                }
                comboBox1.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show("Error", "Load File Error");
            }
        }

        private void saveListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextWriter texW;
            saveFileDialog.Filter = "List of mac adresses| *.lst";
            saveFileDialog.FileName = "MacAdresses.lst";
            saveFileDialog.ShowDialog();

            texW = new StreamWriter(saveFileDialog.FileName);
            foreach (string item in Macs)
                texW.WriteLine(item);
            texW.Close();
        }

        private void clearListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.ResetText();
            Macs.Clear();
        }

        private void removeItemFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string item;
            item = comboBox1.SelectedItem.ToString();
            if (item != null)
            {
                comboBox1.Items.Remove(comboBox1.SelectedItem);
                Macs.Remove(item);
            }
            try
            {
                comboBox1.SelectedIndex = 0;
            }
            catch
            {
                comboBox1.ResetText();
            }
        }

        private void addNewItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm.setRefMacs(ref Macs, ref comboBox1);
            ChildForm.Show();
        }
    }
}
