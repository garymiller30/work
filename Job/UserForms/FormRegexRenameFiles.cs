using BrightIdeasSoftware;
using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms
{
    public partial class FormRegexRenameFiles : Form
    {
        List<IFileSystemInfoExt> _original_files;
        List<PreviewItem> _preview_items = new List<PreviewItem>();

        public FormRegexRenameFiles(List<IFileSystemInfoExt> files)
        {
            InitializeComponent();
            _original_files = files;

            foreach (var file in _original_files)
            {
                _preview_items.Add(new PreviewItem(file.FileInfo.FullName));
            }
            olv_find.OwnerDraw = true;
            olv_find.AddObjects(_preview_items);
        }

        class PreviewItem
        {
            private string _raw;
            public string Folder { get; set; }
            public string OriginalName { get; set; }
            public string Extension { get;set;}
            public string NewName { get; set; }
            public List<string> Variants { get; set; } = new List<string>();

            public PreviewItem(string file)
            {
                _raw = file;
                Folder = System.IO.Path.GetDirectoryName(file);
                OriginalName = System.IO.Path.GetFileNameWithoutExtension(file);
                Extension = System.IO.Path.GetExtension(file);
            }

            public void CreateVariants(string text)
            {
                Variants = Regex.Matches(OriginalName, text)
                    .Cast<Match>()
                    .Select(x => x.Value)
                    .Distinct()
                    .ToList();
            }

            public void Parse(string text)
            {
                
            }
        }

        private void tb_find_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_find.Text))
            {
                olv_find.DefaultRenderer = null;
                return;
            }
            TextMatchFilter filter = TextMatchFilter.Regex(olv_find, tb_find.Text);

            olv_find.DefaultRenderer = new HighlightTextRenderer(filter);
            olv_find.Refresh();
           
            CreateVariants(tb_find.Text);
        }

        private void CreateVariants(string text)
        {
            _preview_items.ForEach(x=>x.CreateVariants(text));
            _preview_items.ForEach(x => x.Parse(tb_replace.Text));

        }

        private void tb_replace_TextChanged(object sender, EventArgs e)
        {
            _preview_items.ForEach(x => x.Parse(tb_replace.Text));
        }

    }
}
