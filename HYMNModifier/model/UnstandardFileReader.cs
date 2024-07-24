using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HYMNModifier.model
{
    public class UnstandardFileReader
    {
        public Hymn[] case1(string dirPath)
        {
            Hymn[] hymns = new Hymn[645];

            DirectoryInfo d = new DirectoryInfo(dirPath);
            foreach (FileInfo f in d.GetFiles())
            {
                try
                {
                    FileStream file = new FileStream(f.FullName, FileMode.Open);
                    StreamReader reader = new StreamReader(file);

                    string rawData = reader.ReadToEnd();

                    reader.Close();
                    file.Close();

                    StringBuilder mr = new StringBuilder();
                    string[] ms = rawData.Trim().Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < ms.Length; i++)
                    {
                        StringBuilder temp = new StringBuilder();
                        foreach (string s in ms[i].Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            temp.Append(s).Append('\n');
                        }
                        temp.Remove(temp.Length - 1, 1);
                        ms[i] = temp.ToString();
                    }

                    List<string> contents = new List<string>();
                    StringBuilder verse = new StringBuilder();
                    string refrain = null;
                    foreach (string s in ms)
                    {
                        // 한 단 처리 부분

                        if (s[0] == '(')
                        {
                            contents.Add(s.Substring(3).Trim());
                        }
                        else if (s.StartsWith("후렴 :"))
                        {
                            refrain = s.Substring(4).Trim();
                        }
                        else
                        {
                            contents.Add(s);
                        }
                    }

                    if (refrain != null)
                    {
                        for (int i = 0; i < contents.Count; i++)
                        {
                            if (refrain.EndsWith("아멘") && i < contents.Count - 1)
                                contents[i] = contents[i] + "\n" + refrain.Substring(0, refrain.Length - 2);
                            else
                                contents[i] = contents[i] + "\n" + refrain;
                        }
                        refrain = null;
                    }

                    hymns[int.Parse(Path.GetFileNameWithoutExtension(f.Name)) - 1] = new Hymn() { contents = contents.ToArray() };
                }
                catch
                {

                }
            }

            return hymns;
        }

        public Hymn[] case2(string filePath)
        {
            Hymn[] hymns = new Hymn[645];

            FileStream file = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(file);

            string rawData = reader.ReadToEnd();

            reader.Close();
            file.Close();

            List<string> contents = new List<string>();
            StringBuilder verse = new StringBuilder();
            string refrain = null;
            int hymni = 1;
            Regex r = new Regex("[0-9]+\\.(\\([0-9]\\)|[0-9]절|[^0-9\\.])+");
            foreach (Match m in r.Matches(rawData))
            {
                // 매 장을 처리하는 부분

                StringBuilder mr = new StringBuilder();
                string[] ms = m.Value.Trim().Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (!ms[0].StartsWith(hymni.ToString()))
                {
                    throw new Exception("찬송가 순서가 맞지 않음!!");
                }

                for(int i = 0; i < ms.Length; i++)
                {
                    StringBuilder temp = new StringBuilder();
                    foreach (string s in ms[i].Split(new char[] { '\r','\n'}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!new Regex("^[0-9]+\\.").IsMatch(s))
                        {
                            temp.Append(s).Append('\n');
                        }
                    }
                    temp.Remove(temp.Length - 1, 1);
                    ms[i] = temp.ToString();
                }

                contents.Clear();
                int versei = 1;
                foreach (string s in ms)
                {
                    // 매 절을 처리하는 부분
                    if (s[0] == '(')
                    {
                        contents.Add(s.Substring(3).Trim());
                        versei++;
                    }
                    else if (s.StartsWith("후렴:"))
                    {
                        refrain = s.Substring(3).Trim();
                    }
                    else if (s.StartsWith("후렴"))
                    {
                        refrain = s.Substring(3).Trim();
                    }
                    else
                        contents.Add(s.Trim());
                }
                if (refrain != null)
                {
                    for (int i = 0; i < contents.Count; i++)
                    {
                        if (refrain.EndsWith("아멘") && i < contents.Count - 1)
                            contents[i] = contents[i] + "\n" + refrain.Substring(0, refrain.Length - 2);
                        else
                            contents[i] = contents[i] + "\n" + refrain;
                    }
                    refrain = null;
                }
                hymns[hymni++ - 1] = new Hymn() { contents = contents.ToArray()};
            }

            return hymns;
        }
    }
}
