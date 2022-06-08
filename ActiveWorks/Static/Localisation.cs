using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows.Forms;

namespace ActiveWorks.Static
{
    public static class Localisation
    {

        public static IEnumerable<Control> All(this Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                foreach (Control grandChild in control.Controls.All())
                    yield return grandChild;

                yield return control;
            }
        }

        public static void ApplyResourceToControl(ResourceManager res, Control control)
        {


            foreach (Control c in control.Controls)
            {
                if (c is ToolStrip)
                {
                    ApplyResourceToToolStrip(res, (ToolStrip)c);
                }
                else if (c is ObjectListView)
                {
                    ApplyResourceToObjectListView(res, (ObjectListView)c);
                }
                else if (c is TreeListView)
                {
                    ApplyResourceToTreeListView(res, (TreeListView)c);
                }
                else
                {
                    ApplyResourceToControl(res, c);
                }



            }



            var text = res.GetString(control.Text);
            //Debug.WriteLine($"control name: {control.Name}, text: {control.Text}, text = {text}");
            control.Text = text ?? control.Text;
        }

        private static void ApplyResourceToTreeListView(ResourceManager res, TreeListView treeListView)
        {
            foreach (OLVColumn column in treeListView.AllColumns)
            {
                column.Text = res.GetString(column.Text) ?? column.Text;
            }
        }

        private static void ApplyResourceToObjectListView(ResourceManager res, ObjectListView objectListView)
        {
            foreach (OLVColumn column in objectListView.AllColumns)
            {
                column.Text = res.GetString(column.Text) ?? column.Text;
            }
        }

        private static void ApplyResourceToToolStrip(ResourceManager res, ToolStrip toolStrip)
        {
            foreach (var item in toolStrip.Items)
            {
                if (item is ToolStripLabel)
                {
                    ((ToolStripLabel)item).Text = res.GetString(((ToolStripLabel)item).Text) ??
                                                   ((ToolStripLabel)item).Text;
                }
                else if (item is ToolStripButton)
                {
                    ((ToolStripButton)item).Text = res.GetString(((ToolStripButton)item).Text) ??
                                                   ((ToolStripButton)item).Text;
                }
                else if (item is ToolStripTextBox)
                {
                    ((ToolStripTextBox)item).ToolTipText = res.GetString(((ToolStripTextBox)item).ToolTipText) ??
                                                   ((ToolStripTextBox)item).ToolTipText;
                }
                else if (item is ToolStripSplitButton)
                {
                    ((ToolStripSplitButton)item).Text = res.GetString(((ToolStripSplitButton)item).Text) ??
                                                         ((ToolStripSplitButton)item).Text;

                    // ApplyResourceToToolStripSplitButton(res,(ToolStripSplitButton) item);

                }
                else if (item is ToolStripSeparator)
                { }
                else if (item is ToolStripProgressBar)
                { }
                else if (item is ToolStripDropDownItem)
                {
                    ((ToolStripDropDownItem)item).Text = res.GetString(((ToolStripDropDownItem)item).Text) ??
                                                        ((ToolStripDropDownItem)item).Text;
                }
                else if (item is ToolStripComboBox)
                {

                }
                else
                {
                    throw new Exception(item.ToString());
                }
            }
        }

        //private static void ApplyResourceToToolStripSplitButton(ResourceManager res, ToolStripSplitButton item)
        //{
        //    foreach (var items in item.DropDownItems)
        //    {
        //        if (items is ToolStripMenuItem)
        //        {
        //            ((ToolStripMenuItem)items).Text = res.GetString(((ToolStripMenuItem)items).Text) ??
        //                                                  ((ToolStripMenuItem)items).Text;
        //        }
        //        else
        //        {
        //            throw new Exception(items.ToString());
        //        }

        //    }
        //}


        public static IEnumerable<Control> GetOffsprings(this Control @this)
        {
            foreach (Control child in @this.Controls)
            {
                yield return child;
                foreach (var offspring in GetOffsprings(child))
                    yield return offspring;
            }
        }
        public static IEnumerable<ToolStripItem> GetSubItems(this ToolStrip @this)
        {
            foreach (ToolStripItem child in @this.Items)
            {
                yield return child;
                foreach (var offspring in child.GetSubItems())
                    yield return offspring;
            }
        }

        public static IEnumerable<ToolStripItem> GetSubItems(this ToolStripItem @this)
        {
            if (!(@this is ToolStripDropDownItem))
                yield break;

            foreach (ToolStripItem child in ((ToolStripDropDownItem)@this).DropDownItems)
            {
                yield return child;
                foreach (var offspring in child.GetSubItems())
                    yield return offspring;
            }
        }
    }


}
