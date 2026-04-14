using System;
using System.IO;
using System.Windows.Forms;

namespace SpeedDevelopingPlate
{
    public partial class Form1 : Form
    {
        const string StateFile = "state";
        public Form1()
        {
            

            InitializeComponent();

            PlateFormatsManager.OnAdd += PlateFormatsManager_OnAdd;
            PlateFormatsManager.OnChange += PlateFormatsManagerOnOnChange;
            PlateFormatsManager.Ondelete += PlateFormatsManager_Ondelete;

            objectListView1.AddObjects(PlateFormatsManager.GetPlateFormats);

            if (File.Exists(StateFile))
            {
                objectListView1.RestoreState(File.ReadAllBytes(StateFile));
            }

            Width = Properties.Settings.Default.WindowWidth;
            Height = Properties.Settings.Default.WindowHeight;

        }

        private void PlateFormatsManager_Ondelete(object sender, PlateFormat e)
        {
            objectListView1.RemoveObject(e);
        }

        private void PlateFormatsManagerOnOnChange(object sender, PlateFormat e)
        {
            objectListView1.RefreshObject(e);
        }

        private void PlateFormatsManager_OnAdd(object sender, PlateFormat e)
        {
            objectListView1.AddObject(e);
        }

        private void добавитьФорматПластиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FormAddPlateFormat())
            {
                f.ShowDialog();
            }
        }

        private void objectListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var o = objectListView1.SelectedObject;

            if (o != null)
            {
                using (var f = new FormEditSpeed(o))
                {
                    f.ShowDialog();
                }
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var o = objectListView1.SelectedObject;
            if (o != null)
            {
                PlateFormatsManager.Delete(o);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var os = objectListView1.SaveState();
            File.WriteAllBytes(StateFile,os);

            Properties.Settings.Default.WindowWidth = Width;
            Properties.Settings.Default.WindowHeight = Height;

            Properties.Settings.Default.Save();

        }

        private void objectListView1_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            
        }

        private void objectListView1_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            PlateFormatsManager.Save();
        }
    }
}
