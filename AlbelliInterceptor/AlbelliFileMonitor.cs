using System.Diagnostics;

namespace AlbelliInterceptor
{
    internal class AlbelliFileMonitor : IDisposable
    {
        private readonly FileSystemWatcher _watcher;
        private readonly List<FilePrisoner> _prisoners = new List<FilePrisoner>();
        private readonly bool _killAlbelliSoftwareBeforeCopying;

        public AlbelliFileMonitor(bool killAlbelliSoftwareBeforeCopying)
        {
            _killAlbelliSoftwareBeforeCopying = killAlbelliSoftwareBeforeCopying;
            
            var watchFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp";
            LogMessage($"Running in folder: {watchFolder}");
            _watcher = new FileSystemWatcher(watchFolder);
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                             | NotifyFilters.CreationTime
                                             | NotifyFilters.DirectoryName
                                             | NotifyFilters.FileName
                                             | NotifyFilters.LastAccess
                                             | NotifyFilters.LastWrite
                                             | NotifyFilters.Security
                                             | NotifyFilters.Size;

            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
            _watcher.Error += OnError;

            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            LogMessage($"Error, {e.GetException()}");
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            //LogMessage($"Rename: {e.OldName} -> {e.Name}");
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.Name.ToLower().EndsWith(".alb") ||
                e.Name.ToLower().EndsWith(".zip") ||
                e.Name.ToLower().EndsWith(".app"))
            {
                LogMessage($"Deleted: {e.Name}");
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.Name.ToLower().EndsWith("order.app"))
            {
                LogMessage($"File captured: {e.Name}");
                _prisoners.Add(FilePrisoner.Capture(e.FullPath));
            }
            if (e.Name.ToLower().EndsWith(".zip") && (_prisoners.Count > 0))
            {
                LogMessage($"Albelli software created the zip file, copying PDF file(s)...");
                CopyCapturedFiles();
                ReleasePrisoners();
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            //LogMessage($"Changed: {e.Name}");
        }

        private void LogMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }

        public void Dispose()
        {
            ReleasePrisoners();
        }

        private void CopyCapturedFiles()
        {
            if (_killAlbelliSoftwareBeforeCopying)
            {
                KillAlbelliSoftware();
            }
            foreach (var file in _prisoners)
            {
                try
                {
                    var dekstopFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Photobook.pdf";
                    int i = 1;
                    while (File.Exists(dekstopFile))
                    {
                        dekstopFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\Photobook_{i}.pdf";
                    }
                    LogMessage($"Copy file {file.FullName} -> {dekstopFile}");
                    File.Copy(file.FullName, dekstopFile);
                }
                catch (Exception ex)
                {
                    LogMessage("Error copying file");
                }
            }
            ReleasePrisoners();
        }

        private void KillAlbelliSoftware()
        {
            foreach (Process process in Process.GetProcessesByName("apc"))
            {
                process.Kill();
            }
        }

        private void ReleasePrisoners()
        {
            foreach (var file in _prisoners)
            {
                file.Dispose();
            }
        }
    }
}
