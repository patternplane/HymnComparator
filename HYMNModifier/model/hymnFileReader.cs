using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMNModifier.model
{
    public class hymnFileReader
    {
        // 파일 입출력시 구분자
        private const string SEPARATOR = "∂";
        private const string HYMN_SEPARATOR = "∫";

        public Hymn[] readFile(string filePath)
        {
            try
            {
                System.IO.FileStream f = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
                System.IO.StreamReader r = new System.IO.StreamReader(f);

                string rawdata = r.ReadToEnd();

                r.Close();
                f.Close();

                return getHymnList(rawdata);
            }
            catch
            {
                return null;
            }
        }

        private Hymn[] getHymnList(string fileData)
        {
            List<Hymn> PrimitiveHymnList = new List<Hymn>(645);

            string rawData = fileData;

            string[] songs = rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < songs.Length; i++)
                PrimitiveHymnList.Add(
                    new Hymn() { contents = songs[i].Split(new string[] { HYMN_SEPARATOR }, StringSplitOptions.None) }
                    );

            return PrimitiveHymnList.ToArray();
        }
    }
}
