
namespace AlbelliInterceptor
{
    internal class FilePrisoner : IDisposable
    {
        public static FilePrisoner Capture(string filename)
        {
            return new FilePrisoner(filename);
        }

        public string FullName { get; }
        private readonly FileStream? _stream;

        private FilePrisoner(string filename)
        {
            FullName = filename;
            try
            {
                // This is not really needed...
                _stream = File.OpenRead(filename);
            }
            catch
            {

            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
