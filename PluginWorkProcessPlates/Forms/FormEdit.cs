using System;
using System.Linq;
using System.Windows.Forms;
using Interfaces;

namespace PluginWorkProcessPlates.Forms
{
    public partial class FormEdit : Form
    {
        private readonly PlateProcess _process;
        private PlateSettings _settings;
        private IUserProfile _profile;

        private FormEdit()
        {
            InitializeComponent();
            Init_cms_komplects();
            DialogResult = DialogResult.Cancel;
        }

        private void Init_cms_komplects()
        {
            int maxCnt = 20;
            for (int i = 1; i <= maxCnt; i++)
            {
                var menu_item = new ToolStripMenuItem(i.ToString());
                menu_item.Click += (s, e) =>
                {
                    if (int.TryParse(((ToolStripMenuItem)s).Text, out int cnt))
                    {
                        numericUpDownKomplekt.Value = cnt;
                    }
                };
                cms_komplects.Items.Add(menu_item);
            }
        }

        public FormEdit(PlateProcess process) : this()
        {
            _process = process;
            Bind();

        }

        public FormEdit(PlateProcess process, IUserProfile profile) : this(process)
        {
            _profile = profile;
            _settings = profile.Plugins.LoadSettings<PlateSettings>();

            var items = _settings.Formats?.ToArray();
            if (items != null && items.Length >0)
            {
                comboBoxFormats.Items.AddRange(items);
                comboBoxFormats.SelectedIndex= 0;
            }
        }

        private void Bind()
        {
            numericUpDownWidth.Value = _process.PlateFormat.Width;
            numericUpDownHeight.Value = _process.PlateFormat.Height;
            numericUpDownCount.Value = _process.CountPlates;
            numericUpDownPrice.Value = _process.PriceForPlate;

            objectListViewPays.AddObjects(_process.Pays.ToArray());

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            UnBind();
            if (_process.PlateFormat.Width == 0 || _process.PlateFormat.Height == 0 || _process.CountPlates == 0)
            {
                MessageBox.Show("Вкажіть коректні параметри форми та кількість!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveFormat();

            Close();
        }

        private void SaveFormat()
        {
            if (_profile != null)
            {
                var format = new Format() { Width = numericUpDownWidth.Value, Height = numericUpDownHeight.Value };
                if (format.Width == 0 || format.Height == 0) return;

                if (!_settings.Formats.Contains(format))
                {
                    _settings.Formats.Add(format);
                    _profile.Plugins.SaveSettings(_settings);
                }
            }

        }

        private void UnBind()
        {
            _process.PlateFormat.Width = numericUpDownWidth.Value;
            _process.PlateFormat.Height = numericUpDownHeight.Value;
            _process.CountPlates = (int)numericUpDownCount.Value * (int)numericUpDownKomplekt.Value;
            _process.PriceForPlate = (int)numericUpDownPrice.Value;
            _process.Pays.Clear();
            if (objectListViewPays.GetItemCount() > 0)
                _process.Pays.AddRange(objectListViewPays.Objects.Cast<Pay>());
        }

        private void додатиПлатіжToolStripMenuItem_Click(object sender, EventArgs e)
        {

            objectListViewPays.AddObject(new Pay());
        }

        private void видалитиПлатіжToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectListViewPays.RemoveObjects(objectListViewPays.SelectedObjects);
        }

        private void objectListViewPays_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Column == olvColumnSum)
            {
                if (e.RowObject is Pay pay)
                {
                    var res = decimal.TryParse(e.NewValue.ToString(), out decimal result);
                    if (res) pay.Sum = result;
                }
            }
        }

        private void numericUpDownWidth_Enter(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).Select(0, ((NumericUpDown)sender).Text.Length);
        }

        private void comboBoxFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFormats.SelectedItem is Format format)
            {
                numericUpDownWidth.Value = format.Width;
                numericUpDownHeight.Value = format.Height;
            }
        }

        private void buttonPlus1_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 1;
        }

        private void buttonPlus2_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 2;
        }

        private void buttonPlus3_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 3;
        }

        private void buttonPlus4_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 8;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 16;
        }

        private void buttonPlus5_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 5;
        }

        private void buttonPlus6_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 6;
        }

        private void buttonPlus7_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 7;
        }

        private void buttonPlus9_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 9;
        }

        private void buttonPlus10_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 10;
        }

        private void buttonPlus11_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 11;
        }

        private void buttonPlus12_Click(object sender, EventArgs e)
        {
            numericUpDownCount.Value += 12;
        }
    }
}
