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

namespace Stretched_Res_Editor_GUI
{
    public partial class Form1 : Form
    {
        const string pathLoc = "path.txt";
        string GameUserSettingsiniPath;
        string resx1 = "ResolutionSizeX=";
        string resx2 = "LastUserConfirmedResolutionSizeX=";
        string resx3 = "DesiredScreenWidth=";
        string resx4 = "LastUserConfirmedDesiredScreenWidth=";
        string resy1 = "ResolutionSizeY=";
        string resy2 = "LastUserConfirmedResolutionSizeY=";
        string resy3 = "DesiredScreenHeight=";
        string resy4 = "LastUserConfirmedDesiredScreenHeight=";
        public Form1()
        {
            InitializeComponent();
        }

        public void getResXandY()
        {
            try
            {
                if (GameUserSettingsiniPath != null)
                {
                    string n = File.ReadAllText(GameUserSettingsiniPath);
                    var result = n.Split('\n').Where(x => x.Contains(resx1)).Select(x => x.Replace(resx1, string.Empty)).FirstOrDefault().Trim();
                    string currentResolutionX = result.Substring(result.Length - 4);
                    curResX.Text = currentResolutionX;

                    var result2 = n.Split('\n').Where(x => x.Contains(resy1)).Select(x => x.Replace(resy1, string.Empty)).FirstOrDefault().Trim();
                    string currentResolutionY = result2.Substring(result2.Length - 4);
                    curResY.Text = currentResolutionY;
                }
            }
            catch
            {
                MessageBox.Show("There was an error reading your path, please make sure it is in the proper format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(pathLoc) && new FileInfo(pathLoc).Length != 0)
            {
                const string f = pathLoc;
                List<string> lines = new List<string>();
                using (StreamReader r = new StreamReader(f))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    r.Close();
                    r.Dispose();
                }
                filePath.Text = lines[0];
                GameUserSettingsiniPath = filePath.Text;

                getResXandY();
            }
            else
            {
                File.Create(pathLoc).Close();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (StreamWriter objWriter = new StreamWriter(pathLoc))
            {
                objWriter.Write(filePath.Text);
                objWriter.Dispose();
                objWriter.Close();
            }
            MessageBox.Show("Path Set. Please restart to get your current X and Y values added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(pathLoc))
            {
                MessageBox.Show("You have not selected a path to the GameUserSettings.ini file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (!String.IsNullOrEmpty(curResX.Text) && !String.IsNullOrEmpty(curResY.Text) && GameUserSettingsiniPath != null)
                {
                    string text = File.ReadAllText(GameUserSettingsiniPath);
                    text = text.Replace(resx1 + curResX.Text, resx1 + newResX.Text); // replace
                    text = text.Replace(resx2 + curResX.Text, resx2 + newResX.Text); // replace
                    text = text.Replace(resx3 + curResX.Text, resx3 + newResX.Text); // replace
                    text = text.Replace(resx4 + curResX.Text, resx4 + newResX.Text); // replace

                    text = text.Replace(resy1 + curResY.Text, resy1 + newResY.Text); // replace
                    text = text.Replace(resy2 + curResY.Text, resy2 + newResY.Text); // replace
                    text = text.Replace(resy3 + curResY.Text, resy3 + newResY.Text); // replace
                    text = text.Replace(resy4 + curResY.Text, resy4 + newResY.Text); // replace
                    File.WriteAllText(GameUserSettingsiniPath, text);
                    curResX.Text = newResX.Text;
                    curResY.Text = newResY.Text;
                    MessageBox.Show("Resolution Set!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter your current X and Y resolutions to continue, and/or make sure your path is set correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getResXandY();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("To find your GameUserSettings.ini file path:\n\nPress Windows Key + R to bring up Run.\nType in %localappdata% into the search bar and hit Enter.\nFind the FortniteGame folder and click on it.\nClick on the Saved folder.\nClick on the Config folder.\nClick on the WindowsClient folder.\nCopy the path by clicking the address bar at the top of Windows Explorer, and copying the path.\nMake sure you include " + "\\" + "GameUserSettings.ini at the end of the path.\n\nAn example would look like this:\n\nC:\\Users\\YOURUSERNAME\\AppData\\Local\\FortniteGame\\Saved\\Config\\WindowsClient\\GameUserSettings.ini");
        }
    }
}
