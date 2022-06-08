namespace ActiveWorks
{
    /*
        public partial class FormColorProof : Form
        {

            public Profile UserProfile { get; set; }

            public FormColorProof()
            {
                InitializeComponent();

                olvColumn_Cost.AspectGetter= delegate(object r)
                {
                    if (numericUpDown_Width.Value != 0 && numericUpDown_Height.Value != 0)
                    {
                        var etalon = ((numericUpDown_Width.Value/1000)*(numericUpDown_Height.Value/1000));
                        var calc = ((((ColorProof) r).Width/1000)*(((ColorProof) r).Height/1000)*numericUpDown_Cost.Value);

                        return (calc/etalon).ToString("N02");
                    }
                    return 0;
                };

                //= r => {
                //    (((((ColorProof)r).Width / 1000) * (((ColorProof)r).Height / 1000) * numericUpDown_Cost.Value)
                //    / ((numericUpDown_Width.Value / 1000) * (numericUpDown_Height.Value / 1000))).ToString("N02")}
                //    ;

                olvColumn_Customer.AspectGetter = r => ((ColorProof) r).Customer;
                objectListView_Forms.FormatRow += objectListView_Forms_FormatRow;

                objectListView_Forms.AddObjects(UserProfile.Proofs.Get());
            }

            void objectListView_Forms_FormatRow(object sender, FormatRowEventArgs e)
            {
                e.Item.BackColor = ((ColorProof) e.Model).Payed ? Color.Aquamarine : Color.Bisque;
            }

            private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
            {
                objectListView_Forms.AddObject(UserProfile.Proofs.Add());
            }

            private void objectListView_Forms_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
            {
                if (e.Column == olvColumn_Customer)
                {
                    var cb = new ComboBox
                    {
                        Bounds = e.CellBounds,
                        Font = ((ObjectListView)sender).Font,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        DataSource = UserProfile.Customers.ToList(),
                        DisplayMember = "Name"
                    };
                    e.Control = cb;
                }
            }

            private void objectListView_Forms_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
            {
                if (e.Column == olvColumn_Width)
                {
                    decimal d;
                    var b = decimal.TryParse(e.NewValue.ToString(), out d);
                    if (b)
                        ((ColorProof) e.RowObject).Width = d;
                }
                else if (e.Column == olvColumn_Height)
                {
                    decimal d;
                    var b = decimal.TryParse(e.NewValue.ToString(), out d);
                    if (b)
                        ((ColorProof)e.RowObject).Height = d;
                }
                else if (e.Column == olvColumn_Customer)
                {
                    var box = e.Control as ComboBox;
                    if (box != null)
                    {
                        if (box.SelectedItem != null)
                        {
                            ((ColorProof)e.RowObject).Customer = ((Job.Customer)box.SelectedItem).Name;
                            objectListView_Forms.RefreshObject(e.RowObject);
                        }
                    }
                }
                else if (e.Column == olvColumn_Date)
                {
                    ((ColorProof)e.RowObject).Date = (DateTime)e.NewValue;
                }
                else if (e.Column == olvColumn_Payed)
                {
                    ((ColorProof)e.RowObject).Payed = (bool)e.NewValue;

                }
                else if (e.Column == olvColumn_Description)
                {
                    ((ColorProof)e.RowObject).Description = e.NewValue.ToString();
                }

                UserProfile.Proofs.Update((ColorProof)e.RowObject);
            }

            private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
            {
                var cp = objectListView_Forms.SelectedObject as ColorProof;
                if (cp != null)
                {
                    UserProfile.Proofs.Remove(cp);
                    objectListView_Forms.RemoveObject(cp);
                }
            }

            private void numericUpDown_Width_ValueChanged(object sender, EventArgs e)
            {
                objectListView_Forms.RefreshObjects(objectListView_Forms.Objects.Cast<ColorProof>().ToArray());
            }

            private void numericUpDown_Height_ValueChanged(object sender, EventArgs e)
            {
                objectListView_Forms.RefreshObjects(objectListView_Forms.Objects.Cast<ColorProof>().ToArray());
            }

            private void numericUpDown_Cost_ValueChanged(object sender, EventArgs e)
            {
                objectListView_Forms.RefreshObjects(objectListView_Forms.Objects.Cast<ColorProof>().ToArray());
            }
        }
    */
}
