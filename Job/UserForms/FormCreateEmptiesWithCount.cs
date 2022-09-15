using Job.Models;
using PDFManipulate.Shema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Job.UserForms
{
    public sealed partial class FormCreateEmptiesWithCount : Form
    {
        public List<EmptyTemplate> PdfTemplates { get; } = new List<EmptyTemplate>();
        
        public FormCreateEmptiesWithCount()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var template = AddTemplate();
            if (template != null)
            {
                objectListView1.AddObject(template);
            }
        }

        private EmptyTemplate AddTemplate()
        {
            var template = new EmptyTemplate() { 
                Width = (double)nW.Value,
                Height = (double)nH.Value,
                Count = (int) nCount.Value,
                Multiplier = (int) nMul.Value
            };

            if (template.IsValidated())
            {
                PdfTemplates.Add(template);
                return template;
            }
            return null;
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
    }
}
