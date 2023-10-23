using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace LotR_2ool
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapOpen();
        }

        int[] VariableArray;
        int[] OffsetArray;

        string ExecutablePath;


        private void GetData()
        {
            OpenFileDialog FileOpen = new OpenFileDialog();
            MessageBox.Show("Please select your Lords2 Executable (Lords2.exe)");

            if (!(FileOpen.ShowDialog() == DialogResult.OK))
            {
                return;
            }
            if (!File.Exists(FileOpen.FileName))
            {
                return;
            }



            byte[] Data = File.ReadAllBytes(FileOpen.FileName);
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(Data, 0, Data.Length);
            List<string> Output = new List<string>();


            binaryReader.BaseStream.Position = 0x0D2BB8;

            for (int Pass = 0; Pass < 198; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + "_ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());                
            }

            binaryReader.BaseStream.Position = 0x0D36C4;

            for (int Pass = 0; Pass < 51; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0D37E0;

            for (int Pass = 0; Pass < 16; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0D4508;

            for (int Pass = 0; Pass < 50; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0D6974;

            for (int Pass = 0; Pass < 585; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0D9DEC;

            for (int Pass = 0; Pass < 16; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0DA270;

            for (int Pass = 0; Pass < 68; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0DA3DC;

            for (int Pass = 0; Pass < 44; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            binaryReader.BaseStream.Position = 0x0DC8F0;

            for (int Pass = 0; Pass < 119; Pass++)
            {
                Output.Add(binaryReader.BaseStream.Position.ToString("x") + " _ " + "N/A" + " _" + binaryReader.ReadInt32().ToString());
            }

            SaveFileDialog FileSave = new SaveFileDialog();
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(FileSave.FileName, Output.ToArray());
            }
        }
        


        public void MapOpen()
        {
            string PluginPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "LotR2ool.plugin");
            if (!File.Exists(PluginPath))
            {
                MessageBox.Show("Missing 'LotR2ool.plugin' file, please repair installation or run from the program directory.");
                return;
            }

            OpenFileDialog FileOpen = new OpenFileDialog();
            MessageBox.Show("Please select your Lords2 Executable (Lords2.exe)");

            if (!(FileOpen.ShowDialog() == DialogResult.OK))
            {
                return;
            }
            if (!File.Exists(FileOpen.FileName))
            {
                return;
            }

            ExecutablePath = FileOpen.FileName;

            byte[] Data = File.ReadAllBytes(ExecutablePath);
            MemoryStream memoryStream = new MemoryStream();
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            memoryStream.Write(Data, 0, Data.Length);
            memoryStream.Position = 0;

            string[] PluginData = File.ReadAllLines(PluginPath);

            int PropCount = PluginData.Length;
            VariableArray = new int[PropCount];
            OffsetArray = new int[PropCount];
            PropListBox.Items.Clear();
            int ParseInt = 0;
            for (int ThisProp = 0; ThisProp < PropCount; ThisProp++)
            {
                string[] Split = PluginData[ThisProp].Split('_');

                if ( (Split.Length < 2) || (!int.TryParse(Split[0], System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ParseInt )) )
                {
                    PropListBox.Items.Add("Void");
                    VariableArray[ThisProp] = -1;
                    continue;
                }
                PropListBox.Items.Add(Split[1]);
                OffsetArray[ThisProp] = ParseInt;
                binaryReader.BaseStream.Position = OffsetArray[ThisProp];
                VariableArray[ThisProp] = binaryReader.ReadInt32();

            }


        }

        private void createOffsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void PropListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VariableArray.Length > PropListBox.SelectedIndex)
            {
                VariableBox.Text = VariableArray[PropListBox.SelectedIndex].ToString();
            }
        }

        private void VariableBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void VariableBox_KeyUp(object sender, KeyEventArgs e)
        {
            int ParseInt;
            if (PropListBox.SelectedIndex == -1)
            {
                return;
            }
            if (PropListBox.SelectedIndex > VariableArray.Length)
            {
                return;
            }


            if (int.TryParse(VariableBox.Text, out ParseInt))
            {
                VariableArray[PropListBox.SelectedIndex] = ParseInt;
                return;
            }

            
            
            
        }


        public void SaveData()
        {
            MessageBox.Show("Please save your new executable");
            SaveFileDialog FileSave = new SaveFileDialog();
            if (!(FileSave.ShowDialog()== DialogResult.OK))
            {
                return;
            }

            byte[] FileData = File.ReadAllBytes(ExecutablePath);

            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            memoryStream.Write(FileData, 0, FileData.Length);
            

            for (int ThisData = 0; ThisData < VariableArray.Length; ThisData++)
            {
                binaryWriter.BaseStream.Position = OffsetArray[ThisData];
                binaryWriter.Write(Convert.ToInt32(VariableArray[ThisData]));
            }

            File.WriteAllBytes(FileSave.FileName, memoryStream.ToArray());

        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ABWindow = new About();
            ABWindow.Show();
        }
    }
}
