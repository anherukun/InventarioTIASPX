using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;

namespace InventarioTIASPX.Application
{
    public class ApplicationManager
    {
        private static int BUFFER_SIZE = 64 * 1024;
        public static string Base64Encode(string value)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        }
        public static string Base64Decode(string value)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(value));
        }
        /// <summary>
        /// Comprime un arreglo de bytes con <see cref="GZipStream"/>
        /// </summary>
        /// <param name="data">Arreglo de bytes</param>
        /// <returns>Arreglo de bytes</returns>
        public static byte[] Compress(byte[] data)
        {
            if (data != null)
            {
                using (var stream = new MemoryStream())
                {
                    using (var buffer = new BufferedStream(new GZipStream(stream, CompressionMode.Compress), BUFFER_SIZE))
                    {
                        buffer.Write(data, 0, data.Length);
                    }

                    return stream.ToArray();
                }
            }
            else
                throw new NullReferenceException("No se puede comprimir un objeto nulo");
        }
        /// <summary>
        /// Descomprime un arreglo de bytes con <see cref="GZipStream"/>
        /// </summary>
        /// <param name="data">Arreglo de bytes</param>
        /// <returns>Arreglo de bytes</returns>
        public static byte[] Decompress(byte[] data)
        {
            if (data != null)
            {
                using (var compressedstream = new MemoryStream(data))
                {
                    using (var decompressedstream = new MemoryStream())
                    {
                        using (var buffer = new BufferedStream(new GZipStream(compressedstream, CompressionMode.Decompress), BUFFER_SIZE))
                        {
                            buffer.CopyTo(decompressedstream);
                        }

                        return decompressedstream.ToArray();
                    }
                }
            }
            else
                throw new NullReferenceException("No se puede comprimir un objeto nulo");
        }
        public static byte[] ToBytes(string s) => Encoding.Unicode.GetBytes(s);

        public static string GenerateGUID => Guid.NewGuid().ToString().ToUpper();
    }
}