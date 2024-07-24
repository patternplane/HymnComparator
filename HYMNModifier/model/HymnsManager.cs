using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HYMNModifier.model
{
    public class HymnsManager
    {
        // 파일 입출력시 구분자
        private const string SEPARATOR = "∂";
        private const string HYMN_SEPARATOR = "∫";
        
        public void saveToFile(Hymn[] hymns, string fileName = "result.txt")
        {
            string filePath = ".\\" + fileName;

            FileStream file = new FileStream(filePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(file);

            foreach (Hymn h in hymns)
            {
                for (int i = 0; i < h.contents.Length; i++)
                {
                    writer.Write(h.contents[i]);
                    if (i < h.contents.Length - 1)
                        writer.Write(HYMN_SEPARATOR);
                }
                writer.Write(SEPARATOR);
            }

            writer.Close();
            file.Close();
        }
    }
}
