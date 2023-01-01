/*
 * テキストボックスで入力させた後、元に戻らないようにしたい。
 * 最末尾に文字が入ってしまうことは
 * TextBox.SelectionStart プロパティ　https://msdn.microsoft.com/ja-jp/library/system.windows.controls.textbox.selectionstart(v=vs.95).aspx　
 * またはTextBoxBase- https://msdn.microsoft.com/ja-jp/library/system.windows.forms.textboxbase.selectionstart(v=vs.110).aspx
 * を利用して防げる。長さが発生する場合は別途判定する。 
 
 * このためメソッドを用意し、長さがある場合は両端にタグを挟む仕様とする。
 * DOBON.NET 正規表現の基本も参照。　http://dobon.net/vb/dotnet/string/regex.html
 * 
 */



using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

using System.Runtime.InteropServices; // DLL Import



namespace WindowsFormsApp2
{
    
    
    #region formclss&instance
    public partial class Form1 : Form
    {
        string tagwrite = "", codes, cdprpties = "";

        // プロパティによりコントロールを横断した値をつける.
        //　巧く使えないもの
        public string usedfilename { get; set; } = "default.html";
        public bool dttrim { get; set; } = false;
        public bool newfile { get; set; }

        // saveonclosing用のフラグ。
       public const int nostarted = 0;
       public const int newedit = 1;
       public const int fileloaded = 2;



        //　有意義なもの。
        public static string fpath;
        public static string uneditedtext;

        public static int saveonclosing;
        public static string puretext;

        public string enc { get; set; } = "shift_JIS";
        public static int crpos; //リッチテキストエディタを呼び出すときカクカクしないよう現在位置を記憶する。
        public string UdTextfunc { get; set;}
        public Point Udscr { get; set; }
        public int Udfuncpos { get; set; }
        

        private Point Scpos; // 現在位置のスクロールを保持するためにsendmessageに渡す。

        // tagにundoをかけるためのlist群
        // 以下、すべて「割り当てられません。常に規定値nullを使用します」と表示される。
        // http://mag.autumn.org/Content.modf?id=20050704134357
        // どうも仕様上、コンストラクタにlistを記述することが無理な可能性がある。
        // その都度動的に呼び出して配置しているケースがほとんどである。


        public Form1()
        {
            InitializeComponent();



            saveonclosing = nostarted;
            puretext = "";

            this.dttrim = false;
            this.usedfilename = "";
            // styles styles;
            // styles.htmlset = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\r\n<head>\r\n<title>\r\n\r\n</title>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n</html>";

            
            Scpos = new Point(0, 0);

            p.Click += new EventHandler(bt_Checked);
            b.Click += new EventHandler(bt_Checked);
            i.Click += new EventHandler(bt_Checked);
            u.Click += new EventHandler(bt_Checked);
            // s.Click += new EventHandler(bt_Checked);
            // tt.Click += new EventHandler(bt_Checked);
            link.Click += new EventHandler(bt_Checked);
            li.Click += new EventHandler(bt_Checked);

            br.Click += new EventHandler(bt_Checkedunlock);
            hr.Click += new EventHandler(bt_Checkedunlock);
            img.Click += new EventHandler(bt_Checkedunlock);
            blockquote.Click += new EventHandler(bt_Checked);
            ol.Click += new EventHandler(bt_Checked);
            ul.Click += new EventHandler(bt_Checked);
            fonttagput.Click += new EventHandler(bt_Checked);

            // htmlpreset.Click += new EventHandler(HTMLput);

            DTput.Click += new EventHandler(DandTput);
            dateputchange.Click += new EventHandler(Dtchanged);

            UTF8bt.Click += new EventHandler(UTF8bt_Click);
            shiftJISbt.Click += new EventHandler(shiftJISbt_Click);

            // textgroup.SelectedIndexChanged += new EventHandler(TG_select);

            timer1.Start();
            timer1.Tick += new EventHandler(DT_tick);

            textBox2.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.KeyDown += new KeyEventHandler(SKeysdown);

            /*
            List<int> Spos = new List<int>();// 最初の位置 pos
            List<int> RtnPos = new List<int>(); // 置き終わった（returnした）位置 pos
            List<int> movechr = new List<int>(); // tb.selectionlength。
            List<int> tag = new List<int>();// タグの長さ tag.length
            List<Point> Udscr = new List<Point>(); //戻るスクロールバーの位置 getscrollpos
            */
            // https://www.itlab51.com/?p=4257

            // ファンクションキーがnullでないか調べ、そうであった場合は設定しなおす。し直した場合はFKsavingをtrueに。
            // その場合、SettingをSaveする。


 
            
           

            Application.ApplicationExit += new EventHandler(AppExit);





        }


        #endregion
        #region syokisettei
        [STAThread]
        /* ツールストリップによるショートカット
         今後は下記のように改める。
         全てctrl付き：

         


             */
        private void SKeysdown(object sender, KeyEventArgs e)
        {

            // 0=<p></p>  1-6=<h1-6>
            if (e.KeyCode == Keys.D0 && e.Control == true)
            {
                textgroup.SelectedIndex = 0;
                p.PerformClick();
            }
            else if (e.KeyCode == Keys.D1 && e.Control == true)
            {
                textgroup.SelectedIndex = 1;
                p.PerformClick();
            }
            else if (e.KeyCode == Keys.D2 && e.Control == true)
            {
                textgroup.SelectedIndex = 2;
                p.PerformClick();
            }
            else if (e.KeyCode == Keys.D3 && e.Control == true)
            {
                textgroup.SelectedIndex = 3;
                p.PerformClick();
            }
            else if (e.KeyCode == Keys.D4 && e.Control == true)
            {
                textgroup.SelectedIndex = 4;
                p.PerformClick();
            }
            else if (e.KeyCode == Keys.D5 && e.Control == true)
            {
                textgroup.SelectedIndex = 5;
                p.PerformClick();
            }
            else if (e.KeyCode == Keys.D6 && e.Control == true)
            {
                textgroup.SelectedIndex = 6;
                p.PerformClick();

            }
            // k=<br> 
            else if (e.KeyCode == Keys.K && e.Control == true)
            { // line breaK。
                br.PerformClick();
            }
            // b=<b></b>
            else if (e.KeyCode == Keys.B && e.Control == true)
            {
                b.PerformClick();
            }
            // i=<i></i>
            else if (e.KeyCode == Keys.T && e.Control == true)
            { //ジャンプしてしまうので i>Tに。iTalic
                i.PerformClick();
            }
            // u=<u></u>
            else if (e.KeyCode == Keys.U && e.Control == true)
            {
                u.PerformClick();
            }
            // s=<s></s>
            /*
            else if (e.KeyCode == Keys.E && e.Control == true)
            { //保存と被る。Erasedでe。
                s.PerformClick();
            }
            */
            // l=<href> jumo
            else if (e.KeyCode == Keys.W && e.Control == true)
            {
                link.PerformClick();
            }
            // q=<blockquote></blockquote> blockQuote
            else if (e.KeyCode == Keys.Q && e.Control == true)
            {
                blockquote.PerformClick();
            }
            // m=<img> Picture
            else if (e.KeyCode == Keys.P && e.Control == true)
            {
                img.PerformClick();
            }
            // h=<hr>
            else if (e.KeyCode == Keys.H && e.Control == true)
            {
                hr.PerformClick();
            }
            else if (e.KeyCode == Keys.G && e.Control == true)
            {
                li.PerformClick();
            }
            else if (e.KeyCode == Keys.O && e.Control == true)
            {
                ol.PerformClick();
            }
            else if (e.KeyCode == Keys.N && e.Control == true)
            {
                ul.PerformClick();
            }
            else if (e.KeyCode == Keys.D && e.Control == true) 
            {

                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    textBox1.SelectionStart =
                     Tagputs(DateTime.Today.ToString("D"), textBox1.SelectionStart, ref textBox1);
                    textBox1.Focus();
                }
                else
                {
                    textBox1.SelectionStart =
                        Tagputs(DateTime.Today.ToString("d").ToString(), textBox1.SelectionStart, ref textBox1);
                    textBox1.Focus();
                }

            }
            else if (e.KeyCode == Keys.M && e.Control == true)
            {

                textBox1.SelectionStart =
                    Tagputs(DateTime.Now.ToString().Substring(11), textBox1.SelectionStart, ref textBox1);
                textBox1.Focus();

            }
            else if (e.KeyCode == Keys.F && e.Control == true)
            {
                Find.PerformClick();
            }

            #region
            else if (e.KeyCode == Keys.F1 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F1 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F2 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F2 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F3 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F3 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F4 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F4 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F5 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F5 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F6 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F6 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F7 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F7 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F8 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F8 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F9 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F9 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            else if (e.KeyCode == Keys.F10 && e.Shift == true)
            {

                if (textBox1.SelectionLength > 0)
                {
                    Properties.Settings.Default.F10 = textBox1.SelectedText;
                    textBox1.Focus();

                }


            }
            #endregion
        }

        private void AppExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }


        public void inputon()
        {

            textBox1.Visible = true;

            path.Visible = true;
            textBox2.Visible = true;

            SaveToolStripMenuItem.Visible = true;
            ReSaveToolStripMenuItem.Visible = true;
            dialogs.Visible = true;
            pageinfo.Visible = true;
            autotext.Visible = true;
        }
        #endregion
        #region files
        private void visibleinput()
        {
            toolStrip1.Visible = true;
            toolStrip2.Visible = true;
            toolStrip3.Visible = true;
            DTput.Visible = true;
            dateputchange.Visible = true;
            dateTimePicker1.Visible = true;
            groupBox1.Visible = true;


        }

        private void 新規作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool dobrowse = true;
            if (textBox1.Text != "" && textBox1.Visible == true)
            {
                DialogResult dr = MessageBox.Show("テキストは保存されません。続行しますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                {
                    dobrowse = false;
                }
            }
            if (dobrowse == true)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                //上部に表示する説明テキストを指定する
                fbd.Description = "保存先のフォルダを指定してください。";
                //ルートフォルダを指定する
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                //最初に選択するフォルダを指定する
                //RootFolder以下にあるフォルダである必要がある
                fbd.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //ユーザーが新しいフォルダを作成できるようにする
                //デフォルトでTrue
                fbd.ShowNewFolderButton = true;

                //ダイアログを表示する
                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    //選択されたフォルダを表示する
                    textBox2.Text = fbd.SelectedPath;
                    fpath = textBox2.Text;
                    Properties.Settings.Default.FileAddress = fpath;
                    Properties.Settings.Default.Save();
                    textBox1.Text = "";
                    inputon();

                    ReSaveToolStripMenuItem.Enabled = false;
                    shiftJISbt.Enabled = true;
                    UTF8bt.Enabled = true;
                    visibleinput();

                }
                puretext = "";
                saveonclosing = newedit;
            }

        }

        private void ReadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fdpath = fpath+"/X.X";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "default.html";
            ofd.Title = "開くファイルを選択してください";
            if (Path.GetExtension(fpath)==  String.Empty){
                ofd.InitialDirectory = System.IO.Path.GetDirectoryName(@fdpath);
            }
            else {
                ofd.InitialDirectory = System.IO.Path.GetDirectoryName(@fpath);
            }

                    
                    
            

            // ofd.InitialDirectory = System.IO.Path.GetDirectoryName(@fpath);
            ofd.Filter = "htmlファイル(*.htm,html) | *.htm;*.html|テキストファイル(*.txt)| *.txt";
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    /* 参考：https://dobon.net/vb/dotnet/string/getencodingobject.html
                   * defaultと同じ意味だが、将来の変更に合わせて取りあえず定数を設定。        

                     */
                    StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding(enc)); //GetEncoding("Shift_JIS"));
                    textBox1.Text = sr.ReadToEnd(); // メソッド グループ 'ReadToEnd' を非デリゲート型 'string' に変換することはできません。このメソッドを呼び出しますか? 
                    textBox2.Text = ofd.FileName;
                    this.usedfilename = ofd.FileName.Substring(ofd.FileName.LastIndexOf("/") + 1, ofd.FileName.Length);
                    sr.Close();
                    inputon();

                    fpath = textBox2.Text;
                    Properties.Settings.Default.FileAddress = fpath;
                    Properties.Settings.Default.Save();
                    shiftJISbt.Enabled = false;
                    UTF8bt.Enabled = false;

                    puretext = textBox1.Text;
                    saveonclosing = fileloaded;
                    visibleinput();
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }

        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* 注意：
             * 繰り返し々セーブ内容が焼き直せる。いちいち内容を初期化する必要がある。
             * https://dobon.net/vb/dotnet/file/writefile.html
             * streamwriter 宣言のブール数をFalseにすれば上書き対応
             * 
             */
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "default.html";
            sfd.Title = "保存先のファイルを選択してください";
            sfd.RestoreDirectory = true;
            sfd.Filter = "htmlファイル(*.htm,html) | *.html;*.htm|テキストファイル(*.txt)| *.txt";
            try
            {
                if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding(enc));

                sw.WriteLine(Trimmingtotxt(textBox1.Text));
                    textBox2.Text = sfd.FileName;
                sw.Close();

                fpath = textBox2.Text;
                    Properties.Settings.Default.FileAddress = fpath;
                    Properties.Settings.Default.Save();
                    ReSaveToolStripMenuItem.Enabled = true;
                    puretext = textBox1.Text;
                    saveonclosing = fileloaded;

            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void ReSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter swre = new StreamWriter(textBox2.Text, false, System.Text.Encoding.GetEncoding(enc));
            swre.Write(Trimmingtotxt(textBox1.Text));
            fpath = textBox2.Text;

            puretext = textBox1.Text;
            saveonclosing = fileloaded;

            swre.Close();
        }

        private String Trimmingtotxt(string Former)
        {
            string trimmed = Former;
            trimmed=trimmed.Replace("\r", "");
            trimmed = trimmed.Replace("\n", "\r\n");

            return trimmed;
        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Close();
        }
        #endregion

        #region Ccode
        private void UTF8bt_Click(object sender, EventArgs e)
        {
            if (shiftJISbt.Checked == true)
            {
                shiftJISbt.Checked = false;

                UTF8bt.Checked = true;
                enc = "utf-8";
            }

        }

        private void shiftJISbt_Click(object sender, EventArgs e)
        {
            if (UTF8bt.Checked == true)
            {
                UTF8bt.Checked = false;
                shiftJISbt.Checked = true;
                enc = "shift_jis";
            }
        }
        #endregion

        #region textfunction
        private int Tagputs(string tag, int POS, ref RichTextBox tb)
        {
             Win32Api.SendMessage(tb.Handle, 0x04DD, 0, ref Scpos); // EM_GETSCROLLPOS = 0x04DD;
            tb.Visible = false;
            int pos = POS;
            int movechr = tb.SelectionLength;
            
           

            StringBuilder edittext = new StringBuilder(tb.Text);
            edittext.Insert(pos, tag);
            string edited = edittext.ToString();
            textBox1.Text = edited;

            pos += movechr + tag.Length;
           

             Win32Api.SendMessage(tb.Handle, 0x04DE, 0, ref Scpos); //　EM_SETSCROLLPOS = 0x04DE;
            tb.Visible = true;
            return pos;

        }

        private int Getpos(int back, string str)
        {
            int backvalue = textBox1.SelectionStart;

            int selectPos = textBox1.SelectionStart;

            //カレットの位置までの行を数える
            int row = 1, startPos = 0;
            for (int endPos = 0;
                (endPos = str.IndexOf('\n', startPos)) < selectPos && endPos > -1;
                row++)
            {
                startPos = endPos + 1;
            }

            //列の計算
            int col = selectPos - startPos + 1;

            switch (back) // 1が列、2が行、ほかが一次元配列による位置
            {
                case 1:
                    backvalue = col;
                    break;
                case 2:
                    backvalue = row;
                    break;
                default:
                    break;


            }
            return backvalue;
        }
        private void Udfunc()
        {
            Udfuncpos = textBox1.SelectionStart;
            UdTextfunc = textBox1.Text;
            Win32Api.SendMessage(textBox1.Handle, 0x04DD, 0, ref Scpos); // EM_GETSCROLLPOS = 0x04DD;
            Udscr = Scpos;

        }



        #endregion

        #region powered by win32api

        class Win32Api
        {
            /* http://pinvoke.net/default.aspx/user32/SetScrollPos.html Setの方だが参考。VSの情報も元に捕捉すると…
             * IntPtr はハンドル固有の型。これを使って指定ハンドル部を指定。ハンドルは内部的にはポインタとなっている。
             * (System.Windows.)Orientation の箇所は本来intだが、原文でAPIでの水平・垂直等を示す定数を充てている。これをC#ネイティヴの表現に換える
             * 例：System.Windows.Forms.Orientation.Vertical　などと引用した側では返す必要がある。
             * 他、http://typea.info/tips/wiki.cgi?page=C%23+Win32+API+%A4%AA%A4%E8%A4%D3+DLL+%A4%CE%CD%F8%CD%D1 などを参照。
             * かなり古い関数であり、set/getscrollinfoを利用するよう勧められているが、簡潔に扱えることからこちらを当面利用。
             * http://www.atmarkit.co.jp/bbs/phpBB/viewtopic.php?topic=8750&forum=7
             * Getscroll()あるいは最初にスクロールポスで場所を得て、tagputs等の後基に戻す。
             * Rangeは起動時にスクロール行の最大幅を決める。65000行あまりまで設定できるので、最初に0から最大を設定してしまった方が良い。
             * 
             *現時点では無反応。
             * http://miku39.jp/blog/wp/?cat=20　こちらのSendMessageを使った方法に換える。
             */

            [DllImport("USER32.dll")]
            public static extern int SendMessage(IntPtr handle, Int32 Msg, Int32 wParam, ref Point lParam);
            public enum ScDir : int
            {
                SB_HORZ = 0,
                SB_VERT = 1,
                SB_CTL = 2,
                SB_BOTH = 3,
            };

        }
        #endregion

        #region btchecked
        private void bt_Checked(Object sender, EventArgs e)
        {
            ToolStripButton tmp = (ToolStripButton)sender;
            crpos = textBox1.SelectionStart;
            Udfunc();
            

            if (tmp.Name == "link") // link以外はnameを反映して閉じる
            {


                if (dialoglink.Checked == true)
                {
                    string relativeUrl =
                    OfdforPath("html(*.htm;html)|*.htm;*.html|txt(*.txt)|*.txt|すべてのファイル(*.*)|*.*", "リンクするファイルを選択してください", true);
                    if (relativeUrl != "")
                    {
                        relativeUrl=relativeUrl.Substring(relativeUrl.IndexOf('/') + 1);
                        crpos = Tagputs("<a href=\"" + relativeUrl + "\">", crpos, ref textBox1);
                        textBox1.SelectionStart = Tagputs("</a>", crpos, ref textBox1);
                    }

                    
                }

                else
                {
                    crpos = Tagputs("<a href=\"\">", crpos, ref textBox1);
                    textBox1.SelectionStart = Tagputs("</a>", crpos, ref textBox1);
                    

                }




                //　以上linkの場合終わり

            }
            else if (tmp.Name == "p")
            {
                switch (textgroup.Text)
                {
                    case "見出し1":
                        codes = "h1";

                        break;
                    case "見出し2":
                        codes = "h2";
                        break;
                    case "見出し3":
                        codes = "h3";
                        break;
                    case "見出し4":
                        codes = "h4";
                        break;
                    case "見出し5":
                        codes = "h5";
                        break;
                    case "見出し6":
                        codes = "h6";
                        break;
                    default:
                        codes = "p";
                        break;
                }
                switch (textgroup2.Text)
                {

                    case "左":
                        cdprpties += " align=\"left\"";
                        break;
                    case "中央":
                        cdprpties += " align=\"center\"";
                        break;
                    case "右":
                        cdprpties += " align=\"right\"";
                        break;
                    case "均等":
                        cdprpties += " align=\"justify\"";
                        break;

                    default:

                        break;
                }
                crpos = Tagputs("<" + codes + cdprpties + ">", crpos, ref textBox1);
                textBox1.SelectionStart = Tagputs("</" + codes + ">", crpos, ref textBox1);
                
                cdprpties = "";

            }
            else if (tmp.Name == "fonttagput")
            {
                tagwrite = "font";
                if (fontsizeCB.Text != "なし") { tagwrite += " size=\"" + fontsizeCB.Text + "\""; }
                if (fontcolorCB.Text == "選択" && ColorTranslator.ToHtml(fontcolorCB.ForeColor) != "WindowText")
                {
                    // 自由選択時は文字カラーを取得
                    tagwrite += " color=\"" + ColorTranslator.ToHtml(fontcolorCB.ForeColor) + "\"";
                }
                else if (fontcolorCB.Text != "なし" && fontcolorCB.Text != "選択")
                {
                    tagwrite += " color=\"" + fontcolorCB.Text + "\"";
                }

                if (tagwrite != "font")
                {
                    crpos = Tagputs("<" + tagwrite + ">", crpos, ref textBox1);
                    textBox1.SelectionStart = Tagputs("</font>", crpos, ref textBox1);

                }

            }
            else if (tmp.Name == "ul" || tmp.Name == "ol")
            { // link以外のタグ
                crpos = Tagputs("<" + tmp.Name + ">\n\n", crpos, ref textBox1);
                textBox1.SelectionStart = Tagputs("</" + tmp.Name + ">", crpos, ref textBox1);

            }
            else
            {
                crpos = Tagputs("<" + tmp.Name + ">", crpos, ref textBox1);
                textBox1.SelectionStart = Tagputs("</" + tmp.Name + ">", crpos, ref textBox1);
                
            }
            textBox1.Focus();







        }
        #endregion
        #region otertextput

        private void fontcolorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox box = (ToolStripComboBox)sender;

            if (box.Text == "選択")
            {
                ColorDialog cd = new ColorDialog();
                /* 基本16色を設定
                 * 
                black 	#000000 	 silver 	#c0c0c0 	 gray 	#808080 	 
  	            white 	#ffffff 	 maroon 	#800000 	 red 	#ff0000 	 
              	purple 	#800080 	 fuchsia 	#ff00ff 	 green 	#008000 	 
              	lime 	#00ff00 	 olive 	    #808000 	 yellow #ffff00 	 
              	navy 	#000080 	 blue 	    #0000ff 	 teal 	#008080 	 
             	aqua 	#00ffff 	
                 
                 */
                cd.CustomColors = new int[] {
                    0x000000,0xc0c0c0,0x808080,0xffffff,0x800000,0xff0000,0x800080,0xff00ff,
                    0x008000,0x00ff00,0x808000,0xffff00,0x000080,0x0000ff,0x008080,0x00ffff,
            };
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    //選択された色の取得

                    box.BackColor = cd.Color;
                }
            }
            else if (box.Text != "なし")
            {
                
                box.BackColor = Color.FromName(box.Text);
            }



        }
        private void bt_Checkedunlock(Object sender, EventArgs e)
        {

            ToolStripButton tmp = (ToolStripButton)sender;
            if (tmp.Name == "img")
            {
                if (dialogimage.Checked == true)
                {
                    string relativeUrl =
                        OfdforPath("jpg(*.jpg;jpeg)|*.jpg;*.jpeg|png(*.png)|*.png|bmp(*.bmp)|*.bmp|gif(*.gif)|*.gif|すべてのファイル(*.*)|*.*", "画像ファイルを選択してください", true);
                    if (relativeUrl != "") {

                        relativeUrl = relativeUrl.Substring(relativeUrl.IndexOf('/') + 1);
                        textBox1.SelectionStart = Tagputs("<img src=\"" + relativeUrl + "\">", textBox1.SelectionStart, ref textBox1);
                    }
                                     
                }
                else
                {
                    textBox1.SelectionStart = Tagputs("<img src=\"\">", textBox1.SelectionStart, ref textBox1);
                   

                }

            }
            else
            {
                textBox1.SelectionStart = Tagputs("<" + tmp.Name + ">", textBox1.SelectionStart, ref textBox1);
                // textBox1.Focus();

            }
             textBox1.Focus();
        }



        // 日付のイベント群
        private void Dtchanged(Object sender, EventArgs e)
        {
            switch (dateTimePicker1.Format)
            {
                case DateTimePickerFormat.Long:
                    dateTimePicker1.Format = DateTimePickerFormat.Short;

                    break;
                case DateTimePickerFormat.Short:
                    dateTimePicker1.Format = DateTimePickerFormat.Time;

                    break;
                case DateTimePickerFormat.Time:
                    dateTimePicker1.Format = DateTimePickerFormat.Long;

                    break;
            }

        }
        private void DandTput(Object sender, EventArgs e)
        {

            textBox1.SelectionStart = Tagputs(dateTimePicker1.Text, textBox1.SelectionStart, ref textBox1);
            textBox1.Focus();

            // textBox1.SelectionStart = pos + dateTimePicker1.Text.Length;
        }
        private void DT_tick(Object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }


        private string OfdforPath(string filter, string title, bool URL) 
            //相対パスを得る。boolは内部ファイルを設定する場合偽にする。
             /* 
              * Environment.CurrentDirectoryで判るのは、実行ファイルの位置。
              * 
              * 
              */
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.Title = title;
            ofd.RestoreDirectory = true;
            /*
             * この一文は場所を固定するので避けた。DOBON.NETによれば現在のディレクトリが省略時参照される。
             * ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            */
            Uri u1 = new Uri(textBox2.Text);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Uri u2 = new Uri(ofd.FileName);
                Uri relativeUri = u1.MakeRelativeUri(u2);
                string relativeUrl = relativeUri.ToString();
                return relativeUrl;

            }
            return "";
        }
        #endregion
        #region c&t&p&undo
        //　コピー・切り取り・貼り付け・元に戻す
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectionLength > 0)
            {
                textBox1.Copy();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectionLength > 0)
            {
                textBox1.Cut();
            }
        }
        
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Clipboard.GetText()!="")textBox1.Paste();
        }

        private void すべて選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        /* 
         以下f1ボタン処理
             */

        private void f1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F1);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F2);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F3);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F4);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F5);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F6);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F7);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F8);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F9);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }
        private void f10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string clips = Clipboard.GetText();
            Clipboard.SetText(Properties.Settings.Default.F10);
            textBox1.Paste();
            Clipboard.SetText(clips);
        }

        private void 編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();

        }

        private void テーブルの挿入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form4 table = new Form4();
            string results;


            if (table.ShowDialog(this) == DialogResult.OK)
            {
                /*
                 * 設定集。
                 */

                              

                results = "<table";

                if (table.tablealign != "なし")
                {
                    results += " align=\"" + table.tablealign+"\"";
                }
                if (table.colors[0] != Color.Empty)
                {
                    results += " bgcolor=\"" + ColorTranslator.ToHtml(table.colors[0]) + "\"";
                }
                if (table.width > 0)
                {
                    results += " width=\""+table.width;
                    if (table.inputbypx == false)
                    {
                        results += "%";
                    }
                }
                if (table.border > 0)
                {
                    results += " border=\"" + table.border+"\"";
                    if (table.colors[1]!= Color.Empty)
                    {
                        results += " bordercolor=\""+ColorTranslator.ToHtml(table.colors[1])+"\"";
                    }
                }
                if (table.padding > 0)
                {
                    results += " cellpadding=\""+table.padding+"\"";
                }
                if (table.spacing > 0)
                {
                    results += " cellspacing=\"" + table.spacing + "\"";
                }
                results += ">\n";

                // キャプション。変数は誤字だが無視
                if (table.caution.Length>0)
                {
                    results += "<caption";
                    if (table.ctbottom == true){
                        results += " align=\"bottom\"";
                    }
                    results += ">"+table.caution + "</caption>";
                }

                /* 
                 * 行および列の配置
                 */

                for (int y = 0; y < table.row; y++)
                {
                    results += "<tr>\n";
                    for(int x = 0; x < table.col; x++)
                    {
                        results += "<td>            </td>\n";
                    }
                    results += "</tr>\n\n";
                }
                results += "</table>\n";

                string clips = Clipboard.GetText();
                Clipboard.SetText(results);
                textBox1.Paste();
                Clipboard.SetText(clips);



                // Tagputs(results, 0, ref textBox1);
                table.Hide();


            }
            
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds fdoption=0 ; //None
            int spos = 0; //開始位置
            int epos = 0;
            bool found = false;


            if (findbox.Text != "") {

                /* 条件設定
                 * Findは
                 * https://docs.microsoft.com/ja-jp/dotnet/api/system.windows.forms.richtextbox.find?view=netframework-4.7.2#System_Windows_Forms_RichTextBox_Find_System_String_System_Int32_System_Int32_System_Windows_Forms_RichTextBoxFinds_
                 * RichTextBoxFinds は
                 * https://docs.microsoft.com/ja-jp/dotnet/api/system.windows.forms.richtextboxfinds?view=netframework-4.7.2
                 * 参照。
                 */

                //大小文字区別
                if (radioButton1.Checked == true)
                {
                    fdoption +=4; // MatchCase
                }

                
                if (FRcombo.Text == "すべて置換")
                {
                    DialogResult result = MessageBox.Show("全ての文字列を置き換えます。\r\nよろしいですか？", "警告",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if(result==DialogResult.OK)
                    {
                        textBox1.Text = textBox1.Text.Replace(findbox.Text, replacebox.Text);
                        MessageBox.Show( "置換が終了しました。", FRcombo.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }
                    

                }
                else // 検索と　置換
                {

                    switch (FRcombooption.Text)
                    {
                        case "下へ":
                            if (textBox1.SelectedText == findbox.Text)
                            {
                                textBox1.SelectionStart += textBox1.SelectionLength;
                            }
                            spos = textBox1.SelectionStart;
                            epos = textBox1.Text.Length;
                            break;


                        case "上へ":
                            spos = 0;
                            epos = textBox1.SelectionStart;
                            fdoption +=16; //Reverse.末尾から検索

                            break;
                    }

                    textBox1.Focus();
                    if (textBox1.Find(findbox.Text, spos,epos, fdoption) < 0)
                    {

                        MessageBox.Show(findbox.Text + "が見つかりません。", FRcombo.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        found = true;
                        textBox1.Select(textBox1.SelectionStart -replacebox.Text.Length*2, replacebox.Text.Length);
                    }
                    if (FRcombo.Text == "置換" && found==false)
                    {
                        string clips = Clipboard.GetText();
                        textBox1.Cut();
                        Clipboard.SetText(replacebox.Text);
                        textBox1.Paste();
                        textBox1.ClearUndo();

                        Clipboard.SetText(clips);

                        textBox1.Select(textBox1.SelectionStart- replacebox.Text.Length, replacebox.Text.Length);

                    }
                }


            }
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* 終了時に変更がある場合保存するか訊く。
             * 参考
             * https://dobon.net/vb/dotnet/form/unloadmode.html
             * 
             */

            if (saveonclosing > 0)
            {
                if (puretext != textBox1.Text)
                {
                    switch (MessageBox.Show("保存して終了しますか？", "確認",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk))
                    {
                        case DialogResult.Yes:
                            switch(saveonclosing)
                            {
                                case newedit:

                                    //saveと全く同じ
                                    SaveFileDialog sfd = new SaveFileDialog();
                                    sfd.FileName = "default.html";
                                    sfd.Title = "保存先のファイルを選択してください";
                                    sfd.RestoreDirectory = true;
                                    sfd.Filter = "htmlファイル(*.htm,html) | *.html;*.htm|テキストファイル(*.txt)| *.txt";
                                    try
                                    {
                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {
                                            StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding(enc));

                                            sw.WriteLine(Trimmingtotxt(textBox1.Text));
                                            textBox2.Text = sfd.FileName;
                                            sw.Close();

                                            fpath = textBox2.Text;
                                            Properties.Settings.Default.FileAddress = fpath;
                                            Properties.Settings.Default.Save();
                                            ReSaveToolStripMenuItem.Enabled = true;
                                            puretext = textBox1.Text;
                                            saveonclosing = fileloaded;

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }




                                    break;
                                case fileloaded:
                                    // Resaveと全く同じ
                                    StreamWriter swre = new StreamWriter(textBox2.Text, false, System.Text.Encoding.GetEncoding(enc));
                                    swre.Write(Trimmingtotxt(textBox1.Text));
                                    fpath = textBox2.Text;

                                    puretext = textBox1.Text;
                                    saveonclosing = fileloaded;

                                    swre.Close();


                                    break;
                            }
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                    }


                }
            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStrip1.Visible = false;
            toolStrip2.Visible = false;
            toolStrip3.Visible = false;
            DTput.Visible = false;
            dateputchange.Visible = false;
            dateTimePicker1.Visible = false;
            groupBox1.Visible = false;


            Properties.Settings.Default.Upgrade();
            if (Clipboard.GetText().Length==0) Clipboard.SetText(" ");
            if (Properties.Settings.Default.SearchUP) { FRcombooption.Text = "上へ"; } else { FRcombooption.Text = "下へ"; }
            if (Properties.Settings.Default.bigandsmall) { radioButton1.Checked = true; radioButton2.Checked = false; } 
            else { radioButton1.Checked = false; radioButton2.Checked = true; }
            FRcombo.Text = "検索";

            findbox.Text = Properties.Settings.Default.SearchWord;
            replacebox.Text = Properties.Settings.Default.ReplaceWord;


            bool FKsaving = false;
            
            if (Properties.Settings.Default.F1 == null)
            {
                Properties.Settings.Default.F1 = "<p> </p>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F2 == null)
            {
                Properties.Settings.Default.F2 = "<br>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F3 == null)
            {
                Properties.Settings.Default.F3 = "<b> </b>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F4 == null)
            {
                Properties.Settings.Default.F4 = "<u> </u>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F5 == null)
            {
                Properties.Settings.Default.F5 = "<i> </i>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F6 == null)
            {
                Properties.Settings.Default.F6 = "<a href=\"\"> </a>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F7 == null)
            {
                Properties.Settings.Default.F7 = "<img src=\"\">";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F8 == null)
            {
                Properties.Settings.Default.F8 = "<blockquote> </blockquote>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F9 == null)
            {
                Properties.Settings.Default.F9 = "<hr>";
                FKsaving = true;
            }
            if (Properties.Settings.Default.F10 == null)
            {
                Properties.Settings.Default.F10 =
                     "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n"
                    + "<head>\n"
                    + "<meta http - equiv = \"Content-StyleChanged-Type\" content = \"text/css\">\n"
                    + "<title>\n\n"
                    + "</title>\n"
                    + "</head>\n"
                    + "<body>\n\n\n"
                    + "</body>\n"
                    + "</html>";
                FKsaving = true;
            }
            if (FKsaving == true) Properties.Settings.Default.Save();
        }

        private void バージョン情報AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            infomation info = new infomation();
            info.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) Properties.Settings.Default.bigandsmall = true;
            Properties.Settings.Default.Save();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) Properties.Settings.Default.bigandsmall = false;
            Properties.Settings.Default.Save();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



        private void FRcombooption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox combo = (ToolStripComboBox)sender;
            if (combo.Text == "上へ") { Properties.Settings.Default.SearchUP = true; Properties.Settings.Default.Save(); }
            else if (combo.Text == "下へ") { Properties.Settings.Default.SearchUP = false; Properties.Settings.Default.Save(); }

        }

        private void FRcombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void findbox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SearchWord = findbox.Text;
            Properties.Settings.Default.Save();
        }

        private void replacebox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReplaceWord = replacebox.Text;
            Properties.Settings.Default.Save();
        }

        private void undoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
 
            if (textBox1.CanUndo==true)
            {

                textBox1.Undo();
                textBox1.ClearUndo();
                UdTextfunc = "";
            }
            else if (UdTextfunc!=""){
                textBox1.Text = UdTextfunc;
                Scpos = Udscr;
                Win32Api.SendMessage(textBox1.Handle, 0x04DE, 0, ref Scpos); //　EM_SETSCROLLPOS = 0x04DE;
                textBox1.SelectionStart = Udfuncpos;
                UdTextfunc = "";
            }
        }
        #endregion
        #region form2

        private void ページ設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fpath = textBox2.Text;
            Form2 PageCSS = new Form2();



            if (PageCSS.ShowDialog(this) == DialogResult.OK)
            {
                /*
                 * System.UriFormatException が発生しました
                 *   Message=無効な URI: URI の形式を決定できませんでした。
                 *   Source=<例外のソースを評価できません>
                 */


                
                string results;

                results = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">\n<head>\n<meta http-equiv=\"Content-StyleChanged-Type\" content=\"text/css\"";

                if (shiftJISbt.Checked == true) // 文字コード設定
                {
                    results += " charset=\"shift-jis\">\n";
                }
                else { results += " charset=\"UTF-8\">\n"; }

                results += "<STYLE type=\"text/css\">\nbody{\n";

                #region bgcolorset
                if (PageCSS.colors[1] != Color.Empty)
                {
                    results += "background-color:" + ColorTranslator.ToHtml(PageCSS.colors[1]) + ";\n";
                }
                if (PageCSS.imgpath != "") 
                {
                    results += "background-image:url(\"" +PageCSS.imgpath + "\");\n";
                }
                if (PageCSS.bgrepeat == "fixed")
                {
                    results += "background-attachment:fixed;\n";
                }
                else if (PageCSS.bgrepeat != "")
                {
                    results += "background-repeat:" + PageCSS.bgrepeat + ";\n";
                }
                #endregion
                #region font
                if (PageCSS.colors[2] != Color.Empty)
                {
                    results += "color:" + ColorTranslator.ToHtml(PageCSS.colors[2]) + ";\n";
                }
                #endregion

                #region texts
                if (PageCSS.texts[0] != -1)
                {
                    results += "font-size:" + PageCSS.texts[0];
                    if (PageCSS.inputbypx[0] == true)
                    {
                        results += "px";
                    }
                    else { results += "em"; }
                    results += ";\n";
                }
                if (PageCSS.texts[1] != -1)
                {
                    results += "line-height:" + PageCSS.texts[1];
                    if (PageCSS.inputbypx[0] == true)
                    {
                        results += "px";
                    }
                    else { results += "em"; }
                    results += ";\n";
                }
                #endregion

                #region margins
                // 

                if (PageCSS.margins[0] != -1 || PageCSS.margins[1] != -1)
                {
                    results += "margin:";
                    if (PageCSS.margins[0] == -1)
                    {
                        results += "0%";

                    }
                    else
                    {

                        results += PageCSS.margins[0];

                        if (PageCSS.inputbypx[1] == true)
                        {
                            results += "px";
                        }
                        else { results += "%"; }

                        results += ";\n";
                    }

                    if (PageCSS.margins[1] != -1)
                    {

                        results += PageCSS.margins[1];

                        if (PageCSS.inputbypx[1] == true)
                        {
                            results += "px";
                        }
                        else { results += "%"; }
                    }


                    results += ";\n";
                }

                #endregion
                results += "}";
                #region link color
                if (PageCSS.colors[1] != Color.Empty)
                {
                    results += "a:link{ color:" + ColorTranslator.ToHtml(PageCSS.colors[1]) + ";}\n";
                }
                if (PageCSS.colors[3] != Color.Empty)
                {
                    results += "a:visited{ color:" + ColorTranslator.ToHtml(PageCSS.colors[3]) + ";}\n";
                }

                #endregion
                results += "</style>\n";
                results += "<title>\n";
                #region title
                if (PageCSS.title != "") { results+=PageCSS.title+"\n";} else {
                    results += "\n"; }

                #endregion
                results += "</title>\n</head>\n<body>\n\n</body>\n</html>\n";

                string clips = Clipboard.GetText();
                Clipboard.SetText(results);
                textBox1.Paste();
                Clipboard.SetText(clips);

                // Tagputs(results,0, ref textBox1);
                PageCSS.Close();
            }

        }
        #endregion


    }
    
     




}






