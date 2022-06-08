using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Job.Static
{
    public class SysImageListHelper
    {
        private SysImageListHelper()
        {
        }

        private ImageList.ImageCollection SmallImageCollection
        {
            get
            {
                if (_listView != null)
                    return _listView.SmallImageList.Images;
                if (_treeView != null)
                    return _treeView.ImageList.Images;
                return null;
            }
        }

        protected ImageList.ImageCollection LargeImageCollection => _listView?.LargeImageList.Images;

        private ImageList SmallImageList
        {
            get
            {
                if (_listView != null)
                    return _listView.SmallImageList;
                return _treeView?.ImageList;
            }
        }

        private ImageList LargeImageList
        {
            get
            {
                if (_listView != null)
                    return _listView.LargeImageList;
                return null;
            }
        }


        /// <summary>
        /// Create a SysImageListHelper that will fetch images for the given tree control
        /// </summary>
        /// <param name="treeView">The tree view that will use the images</param>
        public SysImageListHelper(TreeView treeView)
        {
            if (treeView.ImageList == null)
            {
                treeView.ImageList = new ImageList { ImageSize = new Size(16, 16) };
            }
            _treeView = treeView;
        }

        private readonly TreeView _treeView;

        /// <summary>
        /// Create a SysImageListHelper that will fetch images for the given listview control.
        /// </summary>
        /// <param name="listView">The listview that will use the images</param>
        /// <remarks>Listviews manage two image lists, but each item can only have one image index.
        /// This means that the image for an item must occur at the same index in the two lists. 
        /// SysImageListHelper instances handle this requirement. However, if the listview already
        /// has image lists installed, they <b>must</b> be of the same length.</remarks>
        public SysImageListHelper(ListView listView)
        {
            if (listView.SmallImageList == null)
            {
                listView.SmallImageList = new ImageList
                {
                    ColorDepth = ColorDepth.Depth32Bit,
                    ImageSize = new Size(16, 16)
                };
            }

            if (listView.LargeImageList == null)
            {
                listView.LargeImageList = new ImageList
                {
                    ColorDepth = ColorDepth.Depth32Bit,
                    ImageSize = new Size(32, 32)
                };
            }


            _listView = listView;
        }

        private readonly ListView _listView;

        /// <summary>
        /// Return the index of the image that has the Shell Icon for the given file/directory.
        /// </summary>
        /// <param name="path">The full path to the file/directory</param>
        /// <returns>The index of the image or -1 if something goes wrong.</returns>
        public int GetImageIndex(string path)
        {
            if (Directory.Exists(path))
                path = Environment.SystemDirectory; // optimization! give all directories the same image
            else if (Path.HasExtension(path))
                path = Path.GetExtension(path);

            if (SmallImageCollection.ContainsKey(path))
                return SmallImageCollection.IndexOfKey(path);

            try
            {
                AddImageToCollection(path, SmallImageList, ShellUtilities.GetFileIcon(path, true, true));
                AddImageToCollection(path, LargeImageList, ShellUtilities.GetFileIcon(path, false, true));
            }
            catch (ArgumentNullException)
            {
                return -1;
            }

            return SmallImageCollection.IndexOfKey(path);
        }

        private void AddImageToCollection(string key, ImageList imageList, Icon image)
        {
            if (imageList == null)
                return;

            if (imageList.ImageSize == image.Size)
            {
                imageList.Images.Add(key, image);
                return;
            }

            using (var imageAsBitmap = image.ToBitmap())
            {
                var bm = new Bitmap(imageList.ImageSize.Width, imageList.ImageSize.Height);
                var g = Graphics.FromImage(bm);
                g.Clear(imageList.TransparentColor);
                var size = imageAsBitmap.Size;
                var x = Math.Max(0, (bm.Size.Width - size.Width) / 2);
                var y = Math.Max(0, (bm.Size.Height - size.Height) / 2);
                g.DrawImage(imageAsBitmap, x, y, size.Width, size.Height);
                imageList.Images.Add(key, bm);
            }
        }
    }
}
