using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteGallery
{
    internal class Log(string path) : IDisposable
    {
        public string LogPath => path;

        internal readonly Lazy<StreamWriter> LazyWriter = new(() =>
        {
            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

            return new StreamWriter(File.Open(path, new FileStreamOptions()
            {
                Mode = FileMode.OpenOrCreate,
                Access = FileAccess.ReadWrite,
                Options = FileOptions.WriteThrough
            }));
        });

        internal StreamWriter Writer => this.LazyWriter.Value;

        public void WriteLine(string message)
        {
            Writer.WriteLine(message);
            Writer.Flush();
        }

        public void Dispose() => Writer.Dispose();
    }
}
