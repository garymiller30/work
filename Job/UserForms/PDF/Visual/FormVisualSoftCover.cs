using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static.Pdf.Common;
using JobSpace.Static.Pdf.Visual.HardCover;
using JobSpace.Static.Pdf.Visual.SoftCover;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualSoftCover : Form
    {
        IFileSystemInfoExt _fileInfo;
        public SoftCoverParams CoverParams { get;set; }

        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();

        public FormVisualSoftCover(IFileSystemInfoExt f)
        {
            InitializeComponent();
            _fileInfo = f;
        }

        private void FormVisualSoftCover_Shown(object sender, EventArgs e)
        {
            if (_fileInfo != null)
            {
                uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);
                uc_PreviewBrowserFile1.Show(_fileInfo);

                ShowTotalCoverSize();
            }
        }

        private List<IScreenPrimitive> GetPrimitives(int pageNo)
        {
            _primitives.Clear();
            DrawSchema(pageNo);

            return _primitives;
        }

        private void ShowTotalCoverSize()
        {
            nud_total_width.Value = nud_root.Value + nud_left_klapan.Value + nud_right_klapan.Value + (nud_width.Value + nud_bleed.Value) * 2;
            nud_total_height.Value = nud_height.Value + (nud_bleed.Value * 2);
            Redraw();
        }

        void Redraw() => uc_PreviewBrowserFile1.Redraw();

        private void DrawSchema(int pageNo)
        {
            float bleed = (float)nud_bleed.Value;
            float width = (float)nud_width.Value;
            float height = (float)nud_height.Value;
            float left_klapan = (float)nud_left_klapan.Value;
            float right_klapan = (float)nud_right_klapan.Value;
            float root = (float)nud_root.Value;
            float totalW = (float)nud_total_width.Value;
            float totalH = (float)nud_total_height.Value;

            var pdfPageInfo = uc_PreviewBrowserFile1.GetPageInfo(pageNo);

            float x = ((float)pdfPageInfo.Trimbox.wMM() - totalW) / 2;
            float y = ((float)pdfPageInfo.Trimbox.hMM() - totalH) / 2;

            using (Pen pen = new Pen(Color.Red, 0.5f))
            {
                x += bleed;
                y += bleed;

                // обрізний розмір
                _primitives.Add(new ScreenRectangle(pen, x, y, left_klapan + right_klapan + width * 2 + root, height));

                if (left_klapan > 0)
                {
                    x += left_klapan;
                    // лівий клапан
                    _primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                }
                x += width;
                // ліва сторінка
                _primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                x += root;
                //корінець
                _primitives.Add(new ScreenLine(pen, x, y, x, y + height));


                x += width;

                // права сторінка
                _primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                if (right_klapan > 0)
                {
                    x += right_klapan;
                    // правий клапан
                    _primitives.Add(new ScreenLine(pen, x, y, x, y + height));

                }
            }
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            ShowTotalCoverSize();
        }

        private void btn_create_schema_Click(object sender, EventArgs e)
        {
            CoverParams = new SoftCoverParams
            {
                Bleed = (double)nud_bleed.Value,
                Width = (double)nud_width.Value,
                Height = (double)nud_height.Value,
                LeftKlapan = (double)nud_left_klapan.Value,
                RightKlapan = (double)nud_right_klapan.Value,
                Root = (double)nud_root.Value,
                FolderOutput = Path.GetDirectoryName(_fileInfo.FullName),
                Command = SoftCoverParams.CreateCommand.CreateSoftCover

            };
             DialogResult = DialogResult.OK;
             Close();
        }

        private void btn_apply_schema_Click(object sender, EventArgs e)
        {
            CoverParams = new SoftCoverParams
            {
                Bleed = (double)nud_bleed.Value,
                Width = (double)nud_width.Value,
                Height = (double)nud_height.Value,
                LeftKlapan = (double)nud_left_klapan.Value,
                RightKlapan = (double)nud_right_klapan.Value,
                Root = (double)nud_root.Value,
                FolderOutput = Path.GetDirectoryName(_fileInfo.FullName),
                Command = SoftCoverParams.CreateCommand.CreateSoftCoverWithFile

            };
             DialogResult = DialogResult.OK;
             Close();
        }

        private void nud_Enter(object sender, EventArgs e)
        {
            // Select all text when entering numeric up-down
            NumericUpDown nud = sender as NumericUpDown;
            nud.Select(0, nud.Text.Length);
        }

        private void btn_save_schema_Click(object sender, EventArgs e)
        {
            using (Ookii.Dialogs.WinForms.VistaSaveFileDialog sfd = new Ookii.Dialogs.WinForms.VistaSaveFileDialog())
            {
                sfd.Filter = "Soft Cover Schema|*.scschema";
                sfd.DefaultExt = ".scschema";
                sfd.AddExtension = true;
                sfd.RestoreDirectory = false;
                sfd.InitialDirectory = Path.GetDirectoryName(_fileInfo.FullName);
                sfd.FileName = Path.Combine(Path.GetDirectoryName(_fileInfo.FullName), Path.GetFileNameWithoutExtension(_fileInfo.Name) + ".scschema");
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SoftCoverParams schema = new SoftCoverParams
                    {
                        Bleed = (double)nud_bleed.Value,
                        Width = (double)nud_width.Value,
                        Height = (double)nud_height.Value,
                        LeftKlapan = (double)nud_left_klapan.Value,
                        RightKlapan = (double)nud_right_klapan.Value,
                        Root = (double)nud_root.Value,
                    };
                    var strJson = System.Text.Json.JsonSerializer.Serialize<SoftCoverParams>(schema, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(sfd.FileName, strJson);

                }
            }
        }

        private void btn_load_schema_Click(object sender, EventArgs e)
        {
            using (Ookii.Dialogs.WinForms.VistaOpenFileDialog ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog())
            {
                ofd.Filter = "Hard Cover Schema|*.scschema";
                ofd.DefaultExt = ".scschema";
                ofd.AddExtension = true;
                ofd.InitialDirectory = Path.GetDirectoryName(_fileInfo.FileInfo.FullName);
                ofd.FileName = Path.Combine(Path.GetDirectoryName(_fileInfo.FullName), Path.GetFileNameWithoutExtension(_fileInfo.FileInfo.Name) + ".hcschema");
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // відкриваємо .json файл
                        var strJson = File.ReadAllText(ofd.FileName);
                        SoftCoverParams hcp = System.Text.Json.JsonSerializer.Deserialize<SoftCoverParams>(strJson);
                        nud_bleed.Value = (decimal)hcp.Bleed;
                        nud_width.Value = (decimal)hcp.Width;
                        nud_height.Value = (decimal)hcp.Height;
                        nud_left_klapan.Value = (decimal)hcp.LeftKlapan;
                        nud_right_klapan.Value = (decimal)hcp.RightKlapan;
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
    }
}
