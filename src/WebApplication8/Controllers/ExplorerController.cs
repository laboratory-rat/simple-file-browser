using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProofJob.Controllers
{
    [Route("api/[controller]")]
    public class ExplorerController : Controller
    {
        private readonly string _root;

        public ExplorerController(IHostingEnvironment hostEnvironment)
        {
            _root = hostEnvironment.ContentRootPath;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Get(_root);
        }

        [HttpGet("{path}")]
        public JsonResult Get(string path)
        {
            path = path.Replace("<", "\\");

            if (path.Length == 0)
                path = _root;

            Models.FileInfo currentInfo = new Models.FileInfo();

            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);

                currentInfo.Name = info.Name;
                currentInfo.Path = info.FullName;
                currentInfo.IsFolder = true;
                currentInfo.InnerDir = null;
                currentInfo.FilesSizes = new int[3] { 0, 0, 0 };

                List<Models.FileInfo> infoList = new List<Models.FileInfo>();

                if (info.Parent != null)
                    infoList.Add(new Models.FileInfo { Name = "..", Path = info.Parent.FullName.Replace("\\", "<"), IsFolder = true, InnerDir = null, FilesSizes = null});

                try
                {
                    foreach (DirectoryInfo innerInfo in info.GetDirectories())
                        infoList.Add(new Models.FileInfo { Name = innerInfo.Name, IsFolder = true, Path = innerInfo.FullName.Replace("\\", "<"), InnerDir = null, FilesSizes = null });

                    foreach (FileInfo innerFile in info.GetFiles())
                        infoList.Add(new Models.FileInfo { Name = innerFile.Name, IsFolder = false, Path = innerFile.FullName.Replace("\\", "<"), InnerDir = null, FilesSizes = null });

                    foreach (var allFiles in info.GetFiles("*", SearchOption.AllDirectories))
                    {
                        var size = allFiles.Length / 1000000;
                        if (size <= 10)
                            currentInfo.FilesSizes[0]++;
                        else if (size <= 50)
                            currentInfo.FilesSizes[1]++;
                        else if (size >= 100)
                            currentInfo.FilesSizes[2]++;
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    //Console.WriteLine("Exception: " + ex.Message);
                }

                currentInfo.InnerDir = infoList.ToArray();
            }

            return Json(currentInfo);
        }
    }
}