using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    /* Usage
     * 
     *   
     *      //Dictionary<int, int> dict = new Dictionary<int, int>();
            //dict.Add(1, 1);
            //byte[] input = SerializeUtility.SerializeAndCompress(dict);
            //Dictionary<int, int> dict2 = (Dictionary<int, int>)SerializeUtility.DeserializeAndDecompress(input);
     * 
     * */
    public static class SerializeUtility
    {
        public static byte[] Serialize(Object obj)
        {
            if (CommonUtilities.IsObjectEmpty(obj)) return null;
            byte[] bytes;
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                bytes = stream.ToArray();
            }
            return bytes;
        }

        public static byte[] SerializeAndCompress(Object obj)
        {
            return Compress(Serialize(obj));
        }

        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream stream = new MemoryStream())
            {
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress, true))
            {
                gzip.Write(raw, 0, raw.Length);
            }
            return stream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
        public static Object DeserializeAndDecompress(byte[] data)
        {         
            return Deserialize(Decompress(data));
        }

        public static Object Deserialize(byte[] data)
        {            
            if (CommonUtilities.IsBytesEmpty(data)) return null;

            Object obj = new Object();
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                obj = formatter.Deserialize(stream);
            }
            return obj;
        }

        //public static void WriteFile()
        //{
        //    FileStream fs = new FileStream(@"C:\temp\Test.txt", FileMode.Create);
        //    GZipStream compressor = new GZipStream(fs, CompressionMode.Compress);

        //    BinaryFormatter bf = new BinaryFormatter();
        //    Dictionary<int, int> dict = new Dictionary<int, int>();
        //    dict.Add(1, 1);
        //    bf.Serialize(compressor, dict);
        //    compressor.Close();
        //    fs.Close();
        //}

        //public static void LoadFile()
        //{
        //    try
        //    {
        //        FileStream fs = new FileStream(@"C:\temp\Test.txt", FileMode.Open);
        //        GZipStream decompressor = new GZipStream(fs, CompressionMode.Decompress);
        //        BinaryFormatter bf = new BinaryFormatter();
        //        Dictionary<int, int> dict = (Dictionary<int, int>)bf.Deserialize(decompressor);
        //        decompressor.Close();
        //        fs.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
