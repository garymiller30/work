using JobSpace.Static.Pdf.Imposition;
using JobSpace.Static.Pdf.Imposition.Models;
using JobSpace.Static.Pdf.Imposition.Services.Impos.Processes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace JobSpace.UserForms.PDF.ImposItems
{
    public partial class ImposBindingControl : UserControl
    {
        GlobalImposParameters _imposParam;
        IBindControl curBindControl;

        private static readonly string[] BindTypeNames = {
            "розкидати на лист",
            "наскрізна нумерація",
            "один формат, різні тиражі",
            "розкласти розворотами"
            };

        private static readonly Func<IBindControl>[] BindControlFactories = {
            () => new BindingSimpleControl(),
            () => new BindingSimpleCutAndStackControl(),
            () => new BindingSimpleOneFormatDifferentCount(),
            () => new BindingSimpleSpreadControl()
        };

        public ImposBindingControl()
        {
            InitializeComponent();
            Disposed += ImposBindingControl_Disposed;
        }

        public void SetControlBindParameters(GlobalImposParameters imposParam)
        {
            if (_imposParam != null)
            {
                _imposParam.ControlsBind.PropertyChanged -= Parameters_PropertyChanged;
            }

            _imposParam = imposParam;
            _imposParam.ControlsBind.PropertyChanged += Parameters_PropertyChanged;
            
            cb_SelectBindType.Items.Clear();
            cb_SelectBindType.Items.AddRange(BindTypeNames);
            cb_SelectBindType.SelectedIndex = 0;
        }

        private void ImposBindingControl_Disposed(object sender, EventArgs e)
        {
            if (_imposParam != null)
            {
                _imposParam.ControlsBind.PropertyChanged -= Parameters_PropertyChanged;
            }

            DisposeCurrentBindControl();
        }

        private void Parameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Sheet" || _imposParam.ControlsBind.Sheet == null || _imposParam.ControlsBind.Sheet.TemplatePageContainer.TemplatePages.Count > 0) return;
            
            if (ModifierKeys != Keys.Alt)
            {
                Calc();
            }
        }

        private void cb_SelectBindType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_imposParam.ControlsBind == null) return;
            int idx = cb_SelectBindType.SelectedIndex;
            if (idx < 0 || idx >= BindControlFactories.Length) return;

            DisposeCurrentBindControl();
            curBindControl = BindControlFactories[idx]();
            curBindControl.SetControlBindParameters(_imposParam);

            ((UserControl)curBindControl).Dock = DockStyle.Fill;
            panelBindingControl.Controls.Add((UserControl)curBindControl);
        }

        private void DisposeCurrentBindControl()
        {
            var control = curBindControl as UserControl;
            curBindControl = null;

            if (control == null) return;

            panelBindingControl.Controls.Remove(control);
            control.Dispose();
        }

        public void RearangePages(List<PrintSheet> sheets, List<ImposRunPage> pages)
        {
            if (curBindControl == null) return;

            curBindControl.RearangePages(sheets,pages);
        }

        private void b_calc_Click(object sender, EventArgs e)
        {
            Calc();
        }

        private void Calc()
        {
            if (curBindControl == null) return;
            curBindControl.Calc();
        }

        public void CheckRunListPages(List<PrintSheet> printSheets, List<ImposRunPage> imposRunPages)
        {
            if (curBindControl == null) return;

            curBindControl.CheckRunListPages(printSheets, imposRunPages);
        }

        public void FixBackPageSizePosition(TemplatePage selectedPreviewPage)
        {
            ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, selectedPreviewPage);
        }

        internal void FixBackPageSizePosition(TemplatePageContainer templatePageContainer)
        {
            ProcessFixPageBackPosition.FixPosition(_imposParam.ControlsBind.Sheet, templatePageContainer);
        }

        public void RotateRight(TemplatePage sel_page)
        {

            throw new NotImplementedException();
        }
    }
}
