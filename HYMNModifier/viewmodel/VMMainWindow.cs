using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HYMNModifier.viewmodel
{
    public class VMMainWindow : VM
    {

        public string path1 { get; set; } = "";
        public string path2 { get; set; } = "";

        public string _text1 = "";
        public string text1 { get { return _text1; } set { _text1 = value; text1Changed(); } }
        public string _text2 = "";
        public string text2 { get { return _text2; } set { _text2 = value; text2Changed(); } }
        public string differentText { get; set; } = "";

        public BindingList<int> hymns { get; private set; } = new BindingList<int>();

        public ICommand CDoCompare { get; }
        public ICommand CSelectionChanged { get; }
        public ICommand CRecheck { get; }
        public ICommand CReGenerate { get; }
        public ICommand CSave { get; }

        // ==================================================

        private int currentSelection = -1;

        private model.Hymn[] data1;
        private model.Hymn[] data2;

        // ==================================================

        public VMMainWindow()
        {
            CDoCompare = new RelayCommand(obj => compare());
            CSelectionChanged = new RelayCommand(obj => select((int)obj));
            CRecheck = new RelayCommand(obj => recompare());
            CReGenerate = new RelayCommand(obj => genFile());
            CSave = new RelayCommand(obj => save());
        }

        // ==================================================

        private void save()
        {
            model.HymnsManager man = new model.HymnsManager();
            man.saveToFile(data1 ,System.IO.Path.GetFileName(path1));
            man.saveToFile(data2, System.IO.Path.GetFileName(path2));
        }

        private string[] separator(string text)
        {
            string[] verses = text.Trim().Replace("\r\n", "\n").Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < verses.Length; i++)
                verses[i] = verses[i].Substring(3).Trim();
            return verses;
        }

        private void text1Changed()
        {
            data1[currentSelection - 1].contents = separator(text1);
            recompare();
        }

        private void text2Changed()
        {
            data2[currentSelection - 1].contents = separator(text2);
            recompare();
        }

        private void showDiff(string text1, string text2)
        {
            int a = 0;
            int b = 0;

            while (true)
            {
                while (a < text1.Length && (char.IsWhiteSpace(text1[a]) || text1[a] == '-'))
                    a++;
                while (b < text2.Length && (char.IsWhiteSpace(text2[b]) || text2[b] == '-'))
                    b++;

                if (a >= text1.Length && b >= text2.Length)
                {
                    differentText = "★★★동일★★★";
                    OnPropertyChanged(nameof(differentText));
                    break;
                }
                else if (a >= text1.Length) 
                {
                    differentText = text1;
                    OnPropertyChanged(nameof(differentText));
                    break;
                }
                else if (b >= text2.Length)
                {
                    differentText = text2;
                    OnPropertyChanged(nameof(differentText));
                    break;
                }
                else if (text1[a] != text2[b])
                {
                    differentText = text1.Substring(0, a);
                    OnPropertyChanged(nameof(differentText));
                    break;
                }
                else
                {
                    a++;
                    b++;
                }
            }
        }

        private void genFile()
        {
            model.UnstandardFileReader r = new model.UnstandardFileReader();
            model.HymnsManager man = new model.HymnsManager();

            man.saveToFile(r.case1(@".\1. 찬송가-가사TXT-"), "자료1.txt");
            man.saveToFile(r.case2(@".\2. 새찬송가 전곡 가사.txt"), "자료2.txt");
            man.saveToFile(r.case2(@".\3. 찬송가 가사 1~645장.txt"), "자료3.txt");
        }

        private void compare()
        {
            try
            {
                model.hymnFileReader hr = new model.hymnFileReader();
                data1 = hr.readFile(path1);
                data2 = hr.readFile(path2);
            }
            catch
            {
                System.Windows.MessageBox.Show("파일 읽기 실패");
                return;
            }

            if (data1 == null || data2 == null)
                return;

            hymns.Clear();
            for (int i = 0; i < 645; i++)
            {
                if (data1[i].contents.Length != data2[i].contents.Length)
                    hymns.Add(i + 1);
                else
                {
                    StringBuilder str1 = new StringBuilder();
                    StringBuilder str2 = new StringBuilder();
                    for (int j = 0; j < data1[i].contents.Length; j++)
                    {
                        foreach (char c in data1[i].contents[j])
                            if (!char.IsWhiteSpace(c) && c != '-')
                                str1.Append(c);
                        foreach (char c in data2[i].contents[j])
                            if (!char.IsWhiteSpace(c) && c != '-')
                                str2.Append(c);

                        if (str1.ToString().CompareTo(str2.ToString()) != 0)
                        {
                            hymns.Add(i + 1);
                            break;
                        }
                    }
                }
            }
        }

        private void select(int n)
        {
            currentSelection = n;

            string content = "";
            for (int i = 0; i < data1[n - 1].contents.Length; i++)
            {
                content += (i + 1) + "절\n";
                content += data1[n - 1].contents[i] + "\n\n";
            }
            _text1 = content;
            OnPropertyChanged(nameof(text1));

            content = "";
            for (int i = 0; i < data2[n - 1].contents.Length; i++)
            {
                content += (i + 1) + "절\n";
                content += data2[n - 1].contents[i] + "\n\n";
            }
            _text2 = content;
            OnPropertyChanged(nameof(text2));

            showDiff(text1, text2);
        }

        private void recompare()
        {
            StringBuilder str1 = new StringBuilder();
            StringBuilder str2 = new StringBuilder();
            foreach (char c in text1)
                if (!char.IsWhiteSpace(c) && c != '-')
                    str1.Append(c);
            foreach (char c in text2)
                if (!char.IsWhiteSpace(c) && c != '-')
                    str2.Append(c);
            /*
            if (str1.ToString().CompareTo(str2.ToString()) != 0)
            {
                System.Windows.MessageBox.Show("아직 다릅니다!");
            }
            else
            {
                System.Windows.MessageBox.Show("같아요 ★★★★★★★★");
            }*/
            showDiff(text1, text2);
        }
    }
}
