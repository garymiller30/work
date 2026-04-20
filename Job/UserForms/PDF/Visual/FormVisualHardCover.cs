using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.HardCover;
using JobSpace.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualHardCover : Form
    {
        IFileSystemInfoExt _fileInfo;
        public HardCoverParams CoverParams { get; set; }
        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();
        public FormVisualHardCover(IFileSystemInfoExt f)
        {
            InitializeComponent();

            _fileInfo = f;

            uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);

        }

        private List<IScreenPrimitive> GetPrimitives(int pageNo)
        {
            _primitives.Clear();

            DrawSchema(pageNo);

            return _primitives;
        }

        void Redraw() => uc_PreviewBrowserFile1.Redraw();

        private void CalcSchemaAuto()
        {
            var pdfPageInfo = uc_PreviewBrowserFile1.GetCurrentPageInfo();

            nud_width.Value = ((decimal)pdfPageInfo.Trimbox.wMM() - (nud_zagyn.Value + nud_rastav.Value) * 2 - nud_root.Value) / 2;
            nud_height.Value = (decimal)pdfPageInfo.Trimbox.hMM() - (nud_zagyn.Value * 2);
        }
        private void nud_width_ValueChanged(object sender, EventArgs e)
        {
            ShowTotalCoverSize();
        }

        private void ShowTotalCoverSize()
        {
            nud_total_width.Value = nud_root.Value + (nud_width.Value + nud_rastav.Value + nud_zagyn.Value) * 2;
            nud_total_height.Value = nud_height.Value + (nud_zagyn.Value * 2);
            Redraw();

        }

        private void DrawSchema(int pageNo)
        {
            // Загини
            float zagyn = (float)nud_zagyn.Value;
            float width = (float)nud_width.Value;
            float height = (float)nud_height.Value;
            float rastav = (float)nud_rastav.Value;
            float root = (float)nud_root.Value;
            float totalW = (float)nud_total_width.Value;
            float totalH = (float)nud_total_height.Value;

            var pdfPageInfo = uc_PreviewBrowserFile1.GetPageInfo(pageNo);

            float x = ((float)pdfPageInfo.Trimbox.wMM() - totalW) / 2;
            float y = ((float)pdfPageInfo.Trimbox.hMM() - totalH) / 2;



            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                // загальний розмір
                _primitives.Add(new ScreenRectangle(pen, x, y, totalW, totalH));
                // лівий загин
                _primitives.Add(new ScreenLine(pen, x + zagyn, y, x + zagyn, y + totalH));
                // правий загин
                _primitives.Add(new ScreenLine(pen, x + totalW - zagyn, y, x + totalW - zagyn, y + totalH));
                // верхній загин
                _primitives.Add(new ScreenLine(pen, x, y + zagyn, x + totalW, y + zagyn));
                // нижній загин
                _primitives.Add(new ScreenLine(pen, x, y + totalH - zagyn, x + totalW, y + totalH - zagyn));
                // ліва сторінка
                _primitives.Add(new ScreenLine(pen, x + zagyn + width, y + zagyn, x + zagyn + width, y + totalH - zagyn));
                // растав
                _primitives.Add(new ScreenLine(pen, x + zagyn + width + rastav, y + zagyn, x + zagyn + width + rastav, y + totalH - zagyn));
                // корінець
                _primitives.Add(new ScreenLine(pen, x + zagyn + width + rastav + root, y + zagyn, x + zagyn + width + rastav + root, y + totalH - zagyn));
                // права сторінка
                _primitives.Add(new ScreenLine(pen, x + zagyn + width + rastav + root + rastav, y + zagyn, x + zagyn + width + rastav + root + rastav, y + totalH - zagyn));
            }
        }

        private void FormVisualHardCover_Shown(object sender, EventArgs e)
        {
            uc_PreviewBrowserFile1.Show(_fileInfo);
            CalcSchemaAuto();
            ShowTotalCoverSize();
        }
        HardCoverParams CreateParameters()
        {
            return new HardCoverParams
            {
                CreateBack = cb_create_back.Checked,
                BackAnglesCut = cb_angles_cut.Checked,
                CreateFilePlusSchema = cb_create_file_plus_chema.Checked,
                CreateSchema = cb_create_schema.Checked,
                SaveSchema = cb_save_schema.Checked,
                Height = (double)nud_height.Value,
                Width = (double)nud_width.Value,
                Zagyn = (double)nud_zagyn.Value,
                Rastav = (double)nud_rastav.Value,
                Root = (double)nud_root.Value,
                FolderOutput = Path.GetDirectoryName(_fileInfo.FileInfo.FullName)
            };
        }
        void SaveSchema(HardCoverParams coverParams)
        {
            var targetFile = Path.Combine(Path.GetDirectoryName(_fileInfo.FullName), $"{Path.GetFileNameWithoutExtension(_fileInfo.FullName)}.hcschema");
            var strJson = System.Text.Json.JsonSerializer.Serialize<HardCoverParams>(coverParams, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(targetFile, strJson);
        }
        private void btn_load_schema_Click(object sender, EventArgs e)
        {
            using (Ookii.Dialogs.WinForms.VistaOpenFileDialog ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog())
            {
                ofd.Filter = "Hard Cover Schema|*.hcschema";
                ofd.DefaultExt = ".hcschema";
                ofd.AddExtension = true;
                ofd.InitialDirectory = Path.GetDirectoryName(_fileInfo.FileInfo.FullName);
                ofd.FileName = Path.Combine(Path.GetDirectoryName(_fileInfo.FullName), Path.GetFileNameWithoutExtension(_fileInfo.FileInfo.Name) + ".hcschema");
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // відкриваємо .json файл
                        var strJson = File.ReadAllText(ofd.FileName);
                        HardCoverParams hcp = System.Text.Json.JsonSerializer.Deserialize<HardCoverParams>(strJson);
                        nud_height.Value = (decimal)hcp.Height;
                        nud_width.Value = (decimal)hcp.Width;
                        nud_zagyn.Value = (decimal)hcp.Zagyn;
                        nud_rastav.Value = (decimal)hcp.Rastav;
                        nud_root.Value = (decimal)hcp.Root;
                        ShowTotalCoverSize();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Error load schema: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void nud_Enter(object sender, EventArgs e)
        {
            // віділиняємо все в NumericUpDown при фокусі
            NumericUpDown nud = sender as NumericUpDown;
            nud.Select(0, nud.Text.Length);

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            CoverParams = CreateParameters();
            if (CoverParams.SaveSchema)
            {
                SaveSchema(CoverParams);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
