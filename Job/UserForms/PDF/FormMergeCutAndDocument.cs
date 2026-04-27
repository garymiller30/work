using Interfaces;
using JobSpace.Models.ScreenPrimitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF
{
    public partial class FormMergeCutAndDocument : Form
    {
        IEnumerable<IFileSystemInfoExt> _files;
        List<IFileSystemInfoExt> _targets;
        List<IScreenPrimitive> _primitives = new List<IScreenPrimitive>();
        public IFileSystemInfoExt CutFile => cb_cut_file.SelectedItem as IFileSystemInfoExt;
        public List<IFileSystemInfoExt> TargetFiles => _targets;

        public FormMergeCutAndDocument(IEnumerable<IFileSystemInfoExt> files)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            _files = files;

            cb_cut_file.Items.AddRange(_files.ToArray());
            cb_cut_file.DisplayMember = "Name";

            uc_FilePreviewControl1.SetFunc_GetScreenPrimitives(GetScreenPrimitives);
        }

        private List<IScreenPrimitive> GetScreenPrimitives(int page)
        {
            _primitives.Clear();

            var cutFile = cb_cut_file.SelectedItem as IFileSystemInfoExt;

            if (cutFile != null)
            {
                _primitives.Add(new ScreenPdf(cutFile,1));
            }
            return _primitives;
        }

        private void cb_cut_file_SelectedIndexChanged(object sender, EventArgs e)
        {
            _targets = _files.Where(x => x != cb_cut_file.SelectedItem).ToList();
            cb_target_file.Items.Clear();
            cb_target_file.Items.AddRange(_targets.ToArray());
            cb_target_file.DisplayMember = "Name";
            cb_target_file.SelectedIndex = 0;
        }

        private void cb_target_file_SelectedIndexChanged(object sender, EventArgs e)
        {
            var file = cb_target_file.SelectedItem as IFileSystemInfoExt;
            uc_FilePreviewControl1.Show(file);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
