using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Interfaces;
using JobSpace.Static;
using Krypton.Toolkit;

namespace JobSpace.UserForms
{
    public partial class FormTrash : KryptonForm
    {
        private readonly IFileManager _fileManager;
        private SysImageListHelper _sysImageListHelper;

        public FormTrash(IFileManager fileManager)
        {
            InitializeComponent();
            _fileManager = fileManager;
            
            ConfigureObjectListView();
            LoadTrashItems();
        }

        private void ConfigureObjectListView()
        {
            _sysImageListHelper = new SysImageListHelper(objectListView1);

            // Configure name column with native Windows icons
            olvColumnName.ImageGetter = delegate (object rowObject)
            {
                var item = (TrashItem)rowObject;
                return _sysImageListHelper.GetImageIndex(item.FullPath);
            };

            // Formatting column values
            olvColumnType.AspectToStringConverter = delegate (object val)
            {
                return (bool)val ? "Папка" : "Файл";
            };

            olvColumnSize.AspectToStringConverter = delegate (object val)
            {
                long size = (long)val;
                if (size == 0) return "";
                if (size < 1024) return $"{size} B";
                if (size < 1024 * 1024) return $"{(size / 1024.0):F1} KB";
                return $"{(size / (1024.0 * 1024.0)):F1} MB";
            };

            olvColumnDeleted.AspectToStringFormat = "{0:dd.MM.yyyy HH:mm:ss}";

            // Grouping and sorting configuration
            objectListView1.ShowGroups = true;
            objectListView1.AlwaysGroupByColumn = olvColumnGroup;
        }

        private void LoadTrashItems()
        {
            var list = new List<TrashItem>();
            string tempPath = Path.Combine(_fileManager.Settings.RootFolder, UC.FileManager.TEMP_FOLDER);

            if (Directory.Exists(tempPath))
            {
                string[] dirs = Directory.GetDirectories(tempPath);
                foreach (string dir in dirs)
                {
                    string groupName = Path.GetFileName(dir);
                    DateTime deletedTime = Directory.GetCreationTime(dir);

                    // Add files
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        var fi = new FileInfo(file);
                        list.Add(new TrashItem
                        {
                            Name = fi.Name,
                            FullPath = file,
                            GroupName = groupName,
                            Size = fi.Length,
                            DeletedTime = deletedTime,
                            IsDirectory = false
                        });
                    }

                    // Add folders
                    string[] subdirs = Directory.GetDirectories(dir);
                    foreach (string subdir in subdirs)
                    {
                        var di = new DirectoryInfo(subdir);
                        list.Add(new TrashItem
                        {
                            Name = di.Name,
                            FullPath = subdir,
                            GroupName = groupName,
                            Size = 0, // Directory size is typically listed as 0 or empty in recycle list
                            DeletedTime = deletedTime,
                            IsDirectory = true
                        });
                    }
                }
            }

            objectListView1.SetObjects(list);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            var selected = objectListView1.SelectedObjects.Cast<TrashItem>().ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Оберіть елементи для відновлення.", "Відновлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int restoredCount = 0;
            foreach (var item in selected)
            {
                string targetPath = Path.Combine(_fileManager.Settings.RootFolder, item.Name);

                if (File.Exists(targetPath) || Directory.Exists(targetPath))
                {
                    var result = MessageBox.Show(
                        $"Файл або папка з ім'ям '{item.Name}' вже існує в папці замовлення. Замінити його?",
                        "Підтвердження заміни",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Cancel)
                        break;

                    if (result == DialogResult.No)
                        continue;

                    try
                    {
                        if (Directory.Exists(targetPath))
                            Directory.Delete(targetPath, true);
                        else if (File.Exists(targetPath))
                            File.Delete(targetPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не вдалося видалити існуючий елемент '{item.Name}': {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }

                try
                {
                    if (item.IsDirectory)
                    {
                        Directory.Move(item.FullPath, targetPath);
                    }
                    else
                    {
                        File.Move(item.FullPath, targetPath);
                    }
                    restoredCount++;

                    // Cleanup parent folder in temp if it became empty
                    string parentDir = Path.GetDirectoryName(item.FullPath);
                    if (Directory.Exists(parentDir) && !Directory.EnumerateFileSystemEntries(parentDir).Any())
                    {
                        Directory.Delete(parentDir);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не вдалося відновити '{item.Name}': {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (restoredCount > 0)
            {
                MessageBox.Show($"Успішно відновлено {restoredCount} елемент(ів).", "Відновлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTrashItems();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = objectListView1.SelectedObjects.Cast<TrashItem>().ToList();
            if (selected.Count == 0)
            {
                MessageBox.Show("Оберіть елементи для безповоротного видалення.", "Видалення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Ви дійсно бажаєте безповоротно видалити виділені елементи ({selected.Count} шт.)?",
                "Підтвердження видалення",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            int deletedCount = 0;
            foreach (var item in selected)
            {
                try
                {
                    if (item.IsDirectory)
                    {
                        Directory.Delete(item.FullPath, true);
                    }
                    else
                    {
                        File.Delete(item.FullPath);
                    }
                    deletedCount++;

                    // Cleanup parent folder in temp if it became empty
                    string parentDir = Path.GetDirectoryName(item.FullPath);
                    if (Directory.Exists(parentDir) && !Directory.EnumerateFileSystemEntries(parentDir).Any())
                    {
                        Directory.Delete(parentDir);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не вдалося видалити '{item.Name}': {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (deletedCount > 0)
            {
                MessageBox.Show($"Успішно видалено {deletedCount} елемент(ів).", "Видалено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTrashItems();
            }
        }

        private void btnEmpty_Click(object sender, EventArgs e)
        {
            string tempPath = Path.Combine(_fileManager.Settings.RootFolder, UC.FileManager.TEMP_FOLDER);

            if (!Directory.Exists(tempPath) || !Directory.EnumerateFileSystemEntries(tempPath).Any())
            {
                MessageBox.Show("Кошик вже порожній.", "Очищення кошика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                "Ви дійсно бажаєте повністю очистити кошик? Всі видалені файли будуть безповоротно втрачені.",
                "Очищення кошика",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Directory.Delete(tempPath, true);
                MessageBox.Show("Кошик успішно очищено.", "Очищення кошика", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTrashItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося очистити кошик: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTrashItems();
        }
    }

    public class TrashItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string GroupName { get; set; }
        public long Size { get; set; }
        public DateTime DeletedTime { get; set; }
        public bool IsDirectory { get; set; }
    }
}
