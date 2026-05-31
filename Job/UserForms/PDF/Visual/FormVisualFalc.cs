using Interfaces;
using Interfaces.Licensing;
using JobSpace.Licensing;
using JobSpace.Models.ScreenPrimitives;
using JobSpace.Static;
using JobSpace.Static.Pdf.Create.Falc;
using JobSpace.Static.Pdf.Visual.Falc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.Visual
{
    public partial class FormVisualFalc : Form
    {
        public FalcSchemaParams SchemaParams { get; private set; }
        Control[] _parts;
        NumericUpDown[] _deltas;
        Label[] _labels;
        FileInfo _fsi;
        private Pen _greenLinePen = new Pen(Color.Green, 0.6f);
        private Pen _whiteLinePen = new Pen(Color.White, 1f);
        decimal[] partsDelta;
        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();
        decimal page_w = 0;
        decimal page_h = 0;

        public FormVisualFalc(IFileSystemInfoExt fsi)
        {
            _fsi = new FileInfo(fsi.FileInfo.FullName);
            InitializeComponent();


            uc_PreviewBrowserFile1.SetFunc_GetScreenPrimitives(GetPrimitives);

            nud_width.Value = fsi.Format.Width;
            page_h = fsi.Format.Height;

            _parts = new Control[] { gb_p1, gb_p2, gb_p3, gb_p4, gb_p5, gb_p6, gb_p7, gb_p8, gb_p9, gb_p10 };
            _deltas = new NumericUpDown[] {numericUpDown1, numericUpDown2, numericUpDown3, numericUpDown4, numericUpDown5,
                numericUpDown6, numericUpDown7, numericUpDown8, numericUpDown9, numericUpDown10 };
            _labels = new Label[] { label1, label2, label3, label4, label5,
                label6, label7, label8, label9, label10 };

            cb_cnt_falc.SelectedIndex = 0;

        }

        private List<IScreenPrimitive> GetPrimitives(int arg)
        {
            _primitives.Clear();
            Recalc();
            return _primitives;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 1; i < _parts.Length; i++)
            {
                _parts[i].Enabled = (i <= cb_cnt_falc.SelectedIndex + 1);
            }
            Redraw();
        }

        void Redraw()
        {
            uc_PreviewBrowserFile1.Redraw();
        }


        private void Recalc()
        {
            if (_deltas == null) return;

            int cntPart = cb_cnt_falc.SelectedIndex + 2;
            decimal width = nud_width.Value;

            partsDelta = new decimal[cntPart];
            decimal delta = 0;
            for (int i = 0; i < cntPart; i++)
            {
                delta += _deltas[i].Value;
                partsDelta[i] = delta;
            }

            delta = partsDelta.Sum();
            decimal partWidth = (width + delta) / cntPart;
            for (int i = 0; i < cntPart; i++)
            {
                partsDelta[i] = Math.Round(partWidth - partsDelta[i], 2);
                _labels[i].Text = partsDelta[i].ToString();
            }

            Draw();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void FormVisualFalc_Load(object sender, EventArgs e)
        {
            uc_PreviewBrowserFile1.Show(_fsi.ToFileSystemInfoExt());
        }

        void Draw()
        {
            if (partsDelta == null) return;

            _primitives = new List<IScreenPrimitive>();

            float x = 0;

            decimal[] widths = partsDelta;

            if (cb_mirrored_parts.Checked && uc_PreviewBrowserFile1.GetCurrentPageIdx() % 2 == 0)
            {
                widths = partsDelta.Reverse().ToArray();
            }

            for (int i = 0; i < widths.Length - 1; i++)
            {
                x += (float)(widths[i]);

                _primitives.Add(new ScreenLine(_whiteLinePen, x, 0, x, (float)page_h));
                _primitives.Add(new ScreenLine(_greenLinePen, x, 0, x, (float)page_h));
            }

            DrawDimensions(widths);
        }

        private void DrawDimensions(decimal[] widths)
        {
            float x = 0;
            float y = 5;

            for (int i = 0; i <= widths.Length - 1; i++)
            {
                var x_ofs = x + (float)(widths[i] / 2);

                ScreenText screenText = new ScreenText()
                {
                    Text = (widths[i]).ToString(),
                    Location = new PointF(x_ofs, y)
                };

                _primitives.Add(screenText);
                x += (float)(widths[i]);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Redraw();
        }


        private void nud_width_ValueChanged(object sender, EventArgs e)
        {
            page_w = nud_width.Value;
            Redraw();
        }


        private void btn_load_schema_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Falc Schema|*.falcschema";
                ofd.RestoreDirectory = false;
                ofd.InitialDirectory = Path.GetDirectoryName(_fsi.FullName);
                ofd.FileName = Path.Combine(Path.GetDirectoryName(_fsi.FullName), Path.GetFileNameWithoutExtension(_fsi.FullName) + ".falcschema");
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // зчитати json файл
                    var strJson = File.ReadAllText(ofd.FileName);
                    var param = System.Text.Json.JsonSerializer.Deserialize<FalcSchemaParams>(strJson);
                    cb_mirrored_parts.Checked = param.Mirrored;
                    
                    var rawPartsWidth = param.RawPartsWidth ?? param.PartsWidth ?? Array.Empty<decimal>();
                    int falcCnt = param.FalcCnt > 0 ? param.FalcCnt : Math.Max(1, rawPartsWidth.Length - 1);
                    cb_cnt_falc.SelectedIndex = Math.Max(0, Math.Min(cb_cnt_falc.Items.Count - 1, falcCnt - 1));

                    for (int i = 0; i < _deltas.Length && i < rawPartsWidth.Length; i++)
                    {
                        _deltas[i].Value = rawPartsWidth[i];
                    }
                    Redraw();
                }
            }
        }
        [RequiresFeature(LicenseFeature.ExportPdf)]
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!LicenseUiGate.RequireFor(this, GetType(), nameof(btn_ok_Click)))
            {
                return;
            }

            SchemaParams = new FalcSchemaParams()
            {
                Mirrored = cb_mirrored_parts.Checked,
                PartsWidth = partsDelta,
                CreateFileAndSchema = cb_create_file_and_schema.Checked,
                CreateSchema = cb_create_schema.Checked,
                LineLen = (double)numLen.Value,
                LineDistance = (double) numDistanse.Value,
                Color = uc_PdfColorSelector1.MarkColor
                
            };

            if (cb_save_schema.Checked)
            {
                PrepareForSave();
                SaveSchema();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void PrepareForSave()
        {
            SchemaParams.FalcCnt = cb_cnt_falc.SelectedIndex+1;
            SchemaParams.RawPartsWidth = new decimal[_deltas.Length];
            for (int i = 0; i < _deltas.Length; i++)
            {
                SchemaParams.RawPartsWidth[i] = _deltas[i].Value;
            }
        }

        [RequiresFeature(LicenseFeature.ThreeDPreview)]
        private void btn_3d_Click(object sender, EventArgs e)
        {
            if (!LicenseUiGate.RequireFor(this, GetType(), nameof(btn_3d_Click)))
            {
                return;
            }


            try
            {
                Cursor = Cursors.WaitCursor;

                Recalc();

                if (partsDelta == null || partsDelta.Length < 2)
                {
                    MessageBox.Show("Не вдалося розрахувати частини для 3D.", "3D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Falc3DHtmlExporter.ExportAndOpen(
                    _fsi.FullName,
                    nud_width.Value,
                    page_h,
                    partsDelta.ToArray(),
                    cb_mirrored_parts.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "3D", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SaveSchema()
        {
            string targetfile = Path.Combine(Path.GetDirectoryName(_fsi.FullName), $"{Path.GetFileNameWithoutExtension(_fsi.FullName)}.falcschema");
            var strJson = System.Text.Json.JsonSerializer.Serialize<FalcSchemaParams>(SchemaParams, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(targetfile, strJson);
        }
    }
}
