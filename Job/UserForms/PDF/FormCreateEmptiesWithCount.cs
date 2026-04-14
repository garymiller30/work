using JobSpace.Static.Pdf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public sealed partial class FormCreateEmptiesWithCount : Form
    {

        public List<EmptyTemplate> PdfTemplates { get; } = new List<EmptyTemplate>();

        public FormCreateEmptiesWithCount()
        {
            InitializeComponent();

            ToolTip tt = new ToolTip();
            tt.SetToolTip(bnt_import, "Таблиця має бути в csv форматі. Кількість колонок - 4, послідовність як в таблиці");

            DialogResult = DialogResult.Cancel;
            objectListView1.AddObjects(PdfTemplates);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddTemplate();

        }

        private void AddTemplate()
        {
            var template = new EmptyTemplate()
            {
                Width = (double)nW.Value,
                Height = (double)nH.Value,
                Count = (int)nCount.Value,
                Multiplier = (int)nMul.Value
            };

            if (template.IsValidated())
            {
                PdfTemplates.Add(template);
                objectListView1.AddObject(template);
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void nW_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void objectListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedItems();
            }
        }

        private void RemoveSelectedItems()
        {
            if (objectListView1.SelectedObjects.Count > 0)
            {
                var delList = objectListView1.SelectedObjects.Cast<EmptyTemplate>().ToList();

                foreach (var item in delList)
                {

                    PdfTemplates.Remove(item);

                }
                objectListView1.RemoveObjects(delList);

            }
        }

        private void bnt_import_Click(object sender, EventArgs e)
        {
            // імпортуємо шаблони з csv файлу. Використовуємо діалог з Ookii.Dialogs.WinForms для вибору файлу
            using (var ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog())
            {
                ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lines = System.IO.File.ReadAllLines(ofd.FileName);
                        // Очікуємо, що кожен рядок має формат: width,height,count,multiplier
                        //пропускаємо заголовок
                        for (int i = 1; i < lines.Length; i++)
                        {
                            var line = lines[i].Trim();
                            var parts = line.Split(',');
                            if (parts.Length >= 4)
                            {
                                if (double.TryParse(parts[0], out double w) &&
                                    double.TryParse(parts[1], out double h) &&
                                    int.TryParse(parts[2], out int count) &&
                                    int.TryParse(parts[3], out int mul))
                                {
                                    var template = new EmptyTemplate()
                                    {
                                        Width = w,
                                        Height = h,
                                        Count = count,
                                        Multiplier = mul
                                    };
                                    if (template.IsValidated())
                                    {
                                        PdfTemplates.Add(template);
                                        objectListView1.AddObject(template);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при імпорті: " + ex.Message);
                    }
                }

            }
        }
    }
}