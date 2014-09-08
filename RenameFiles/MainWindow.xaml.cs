using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using swf = System.Windows.Forms;

namespace RenameFiles
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
        }
        string folderpath="";
        List<string> oldfilename = new List<string>( );
        private void btnOpenDir_Click( object sender, RoutedEventArgs e )
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog( );
            if (fbd.ShowDialog( ) != swf.DialogResult.OK)
            {
                return;
            }
            if(fbd.SelectedPath==null)return;
            List<string> filepaths = new List<string>( );
            folderpath = fbd.SelectedPath.ToString( );
            if (folderpath != "")
            {
                lbxNames.Items.Clear( );
                oldfilename.Clear( );
                filepaths = Directory.GetFiles(folderpath, "*.*").ToList();
                foreach (string file in filepaths)
                {

                    int nidx = file.LastIndexOf('\\')+ 1;
                    string name=file.Substring(nidx);
                    oldfilename.Add(name );
                    lbxNames.Items.Add(name);              
                }
                folderpath += "\\";
                gbDic.Header = gbDic.Header + " | 当前文件夹：" + folderpath;
            }
       }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            cbdelchar.IsChecked = true;
            cbfist.IsChecked = true;
            cblast.IsChecked = true;
            tbnewname.IsEnabled = false;
        }
       
        private void btnRename_Click( object sender, RoutedEventArgs e )
        {
            if (folderpath == "") { swf.MessageBox.Show("请先选择文件夹！", "警告", MessageBoxButtons.OK); return; }
            List<string> newnameList = new List<string>( );
            try
            {
                if (folderpath == "") return;
                string strfist = "", strlast = "", strdelchar = "", strnewname = "";
                if (cbfist.IsChecked == true)
                {
                    if (tbadfist.Text != null) strfist = tbadfist.Text.ToString( );
                }
                if (cblast.IsChecked == true)
                {
                    if (tbadlast.Text != null) strlast = tbadlast.Text.ToString( );
                }
                if (cbdelchar.IsChecked == true)
                {
                    if (tbdelchar.Text != null) strdelchar = tbdelchar.Text.ToString( );
                }
                if (cbnewname.IsChecked == true)
                {
                    if (tbnewname.Text != null) strnewname = tbnewname.Text.ToString( );
                }
                lbxShow.Items.Add("");
                lbxShow.Items.Add("----------------------------------------------------------------------");
                lbxShow.Items.Add("开始重命名…………");
                lbxShow.Items.Add("----------------------------------------------------------------------");
                //取所有文件总数                
                long count = oldfilename.Count;
                if (count > 0)
                {
                    int a = count.ToString( ).Length;
                    count = 1;
                    for (int i = 0; i < a; i++) { count *= 10; }
                }
                else
                {
                    lbxShow.Items.Add("文件夹无文件！");
                    lbxShow.Items.Add("----------------------------------------------------------------------");
                    return;
                }
                foreach (string oldname in oldfilename)
                {
                    int nlido = oldname.LastIndexOf('.');
                    if (nlido == -1) nlido = oldname.Length;
                    string newname = "";
                    if (strnewname != "")
                    {
                        newname = strnewname + count + oldname.Substring(nlido);
                        count++;
                    }
                    else
                    {
                        string tmpname = oldname.Substring(0, nlido), strlinechar = "\r\n";
                        string[ ] split = strdelchar.Split(strlinechar.ToCharArray( ));
                        //删除原文件名中的个别单词
                        foreach (string str in split)
                        {
                            if (str != "") tmpname = tmpname.Replace(str, "");
                        }
                        newname = strfist + tmpname + strlast + oldname.Substring(nlido);
                    }
                    newnameList.Add(newname);
                    File.Move(folderpath + oldname, folderpath + newname);
                    lbxShow.Items.Add(" 成功 将文件\"" + oldname + "\"重命名为：\"" + newname+"\"");
                }
                lbxShow.Items.Add("----------------------------------------------------------------------");
                lbxShow.Items.Add( "共 "+oldfilename.Count+" 个文件  全部重命名成功！");
                lbxShow.Items.Add("----------------------------------------------------------------------");
                oldfilename.Clear( );
                oldfilename = newnameList;
                lbxNames.Items.Clear( );
                foreach (string name in oldfilename)
                {
                    lbxNames.Items.Add(name);
                }
            }
            catch
            {
                lbxShow.Items.Clear( );
                lbxShow.Items.Add("部分重命名失败！");
            }
            finally
            { 
                lbxShow.SelectedItem = lbxShow.Items[lbxShow.Items.Count-1]; 
            }
        }

        private void cbfist_Checked( object sender, RoutedEventArgs e )
        {
            tbadfist.IsEnabled = true;
        }
        private void cbfist_Unchecked( object sender, RoutedEventArgs e )
        {
            tbadfist.IsEnabled = false;
        }

        private void cblast_Checked( object sender, RoutedEventArgs e )
        {
            tbadlast.IsEnabled = true;
        }
        private void cblast_Unchecked( object sender, RoutedEventArgs e )
        {
            tbadlast.IsEnabled = false;
        }

        private void cbdelchar_Checked( object sender, RoutedEventArgs e )
        {
            tbdelchar.IsEnabled = true;
        }
        private void cbdelchar_Unchecked( object sender, RoutedEventArgs e )
        {
            tbdelchar.IsEnabled = false;
        }

        private void cbnewname_Unchecked( object sender, RoutedEventArgs e )
        {
            tbnewname.IsEnabled = false;
            cbdelchar.IsEnabled = true;
            cblast.IsEnabled = true;
            cbfist.IsEnabled = true;
        }
        private void cbnewname_Checked( object sender, RoutedEventArgs e )
        {
            tbnewname.IsEnabled = true;
            cbfist.IsChecked = false;
            cblast.IsChecked = false;
            cbdelchar.IsChecked = false;
            cbdelchar.IsEnabled = false;
            cblast.IsEnabled = false;
            cbfist.IsEnabled = false;
        }

        private void btnReadMe_Click( object sender, RoutedEventArgs e )
        {
            ReadMe rm = new ReadMe( );
            rm.Show( );
        }

        private void btnQuit_Click( object sender, RoutedEventArgs e )
        {
            this.Close( );
        }


    }
}
