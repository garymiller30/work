using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ExtensionMethods
{
    public static class Controls
    {
        /// <summary>
        /// Вызов делегата через control.Invoke, если это необходимо.
        /// </summary>
        /// <param name="control">Элемент управления</param>
        /// <param name="doit">Делегат с некоторым действием</param>
        public static void InvokeIfNeeded(this Control control, Action doit)
        {
            if (control.InvokeRequired)
                control.Invoke(doit);
            else
                doit();
        }
        /// <summary>
        /// Вызов делегата через control.Invoke, если это необходимо.
        /// </summary>
        /// <typeparam name="T">Тип параметра делегата</typeparam>
        /// <param name="control">Элемент управления</param>
        /// <param name="doit">Делегат с некоторым действием</param>
        /// <param name="arg">Аргумент делагата с действием</param>
        public static void InvokeIfNeeded<T>(this Control control, Action<T> doit, T arg)
        {
            if (control.InvokeRequired)
                control.Invoke(doit, arg);
            else
                doit(arg);
        }

        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
