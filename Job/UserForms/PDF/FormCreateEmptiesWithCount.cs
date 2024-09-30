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
    }
}
