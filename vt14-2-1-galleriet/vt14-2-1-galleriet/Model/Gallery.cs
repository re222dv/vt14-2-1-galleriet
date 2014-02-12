using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace vt14_2_1_galleriet.Model {
    public class Gallery {
        private static readonly Regex ApprovedExtensions;
        public static readonly String PhysicalUploadedImagesPath;
        public static readonly String PhysicalImageThumbsPath;
        private static readonly Regex SanitizePath;

        static Gallery() {
            ApprovedExtensions = new Regex(@"^.*\.(gif|jpg|png)$", RegexOptions.IgnoreCase);

            PhysicalUploadedImagesPath = Path.Combine(
                AppDomain.CurrentDomain.GetData("APPBASE").ToString(),
                "Content/Images"
            );

            PhysicalImageThumbsPath = Path.Combine(
                PhysicalUploadedImagesPath,
                "Thumb"
            );

            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns all pictures in the pictures directory
        /// </summary>
        public static IEnumerable<Picture> GetPictures() {
            var di = new DirectoryInfo(PhysicalUploadedImagesPath);
            return (
                from fi in di.GetFiles()
                where ApprovedExtensions.IsMatch(fi.Name)
                select new Picture {
                    Name = fi.Name
                }
            );
        }

        /// <summary>
        /// Checks if the images exists
        /// </summary>
        private static bool ImageExists(String name) {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));
        }

        /// <summary>
        /// Returns a free filname
        /// </summary>
        private static String GetUniqueFileName(String fileName) {
            var fileNameParts = fileName.Split('.');

            for (var i = 1; ImageExists(fileName); i++) {
                var list = fileNameParts.ToList();
                list.Insert(fileNameParts.Length - 1, String.Format(" ({0}).", i));
                fileName = String.Join("", list);
            }

            return fileName;
        }

        /// <summary>
        /// Checks if the image is one of the supported filetypes
        /// </summary>
        private static bool IsValidImage(Image image) {
            if (image == null) {
                return false;
            }
            return image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid ||
                   image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid ||
                   image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid;
        }

        /// <summary>
        /// Saves the image and generates a thumbnail
        /// </summary>
        public static String SaveImage(Stream stream, String fileName) {
            SanitizePath.Replace(fileName, "");

            if (!ApprovedExtensions.IsMatch(fileName)) {
                throw new ArgumentOutOfRangeException();
            }

            fileName = GetUniqueFileName(fileName);

            var image = Image.FromStream(stream);
            if (!IsValidImage(image)) {
                throw new ArgumentException();
            }

            image.Save(Path.Combine(PhysicalUploadedImagesPath, fileName));

            var thumb = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            thumb.Save(Path.Combine(PhysicalImageThumbsPath, fileName));

            return fileName;
        }
    }
}