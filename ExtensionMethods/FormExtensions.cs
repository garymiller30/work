using System;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class FormExtensions
{
    public static Task<DialogResult> ShowDialogAsync(this Form form)
    {
        var tcs = new TaskCompletionSource<DialogResult>();

        void Handler(object sender, FormClosedEventArgs e)
        {
            form.FormClosed -= Handler;

            // якщо DialogResult не виставлений — вважаємо Cancel
            var result = form.DialogResult == DialogResult.None
                ? DialogResult.Cancel
                : form.DialogResult;

            tcs.TrySetResult(result);

            form.Dispose();
        }

        form.FormClosed += Handler;

        // показуємо немодально
        form.Show();

        return tcs.Task;
    }
}