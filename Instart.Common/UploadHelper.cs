﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instart.Common
{
    public class UploadHelper
    {
        static string s_imageDirectory = "/content/upload/images/";
        static string s_videoDirectory = "/content/upload/videos/";
        static string s_physicImageDirectory = System.Web.HttpContext.Current.Server.MapPath(s_imageDirectory);
        static string s_physicVideoDirectory = System.Web.HttpContext.Current.Server.MapPath(s_videoDirectory);
        static string[] s_imageExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        static string[] s_videoExtesions = new string[] { ".avi", ".rmvb", ".rm", ".asf", ".divx", ".mpg", ".mpeg", ".mpe", ".wmv", ".mp4", ".mkv", ".vob", ".ogg" };

        static UploadHelper()
        {
            InitDirectory();
        }

        static void InitDirectory()
        {
            if (!Directory.Exists(s_physicImageDirectory))
            {
                Directory.CreateDirectory(s_physicImageDirectory);
            }

            if (!Directory.Exists(s_physicVideoDirectory))
            {
                Directory.CreateDirectory(s_physicVideoDirectory);
            }
        }

        public static string Process(string fileName, Stream stream)
        {
            if (string.IsNullOrEmpty(fileName) || stream == null)
            {
                return string.Empty;
            }

            string ext = Path.GetExtension(fileName);
            string newFileName = string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
            string pathName = string.Empty;

            if (IsImage(ext))
            {
                pathName = Path.Combine(s_physicImageDirectory, newFileName);
            }
            else if (IsVideo(ext))
            {
                pathName = Path.Combine(s_physicVideoDirectory, newFileName);
            }

            if (string.IsNullOrEmpty(pathName))
            {
                return string.Empty;
            }

            using (var fs = new FileStream(pathName, FileMode.Create, FileAccess.ReadWrite))
            {
                int len = 1024, contentLen = 0;
                var buffer = new byte[len];

                while ((contentLen = stream.Read(buffer, 0, len)) != 0)
                {
                    fs.Write(buffer, 0, len);
                    fs.Flush();
                }
            }
            if (IsImage(ext))
            {
                return Path.Combine(s_imageDirectory, newFileName);
            }
            else if (IsVideo(ext))
            {
                return Path.Combine(s_videoDirectory, newFileName);
            }
            else
            {
                return Path.Combine(s_imageDirectory, newFileName);
            }
        }

        public static bool IsImage(string ext)
        {
            return s_imageExtensions.Contains(ext.ToLower());
        }

        public static bool IsVideo(string ext)
        {
            return s_videoExtesions.Contains(ext.ToLower());
        }
    }
}
