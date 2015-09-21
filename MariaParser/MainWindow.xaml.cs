using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using MariaParser.Models;

namespace MariaParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IList<FileInfo> lstf;
        IList<ItemModel> HtmlFiles;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Console.Write(ex.InnerException);
            }
            

            using (var s = new WofraContext())
            {
                var sf = s.Authors.ToList();
            }

            HtmlFiles = new List<ItemModel>();

            var AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dir = new System.IO.DirectoryInfo(AppPath + @"\Resources\");
            lstf = dir.GetFiles("*.htm", SearchOption.TopDirectoryOnly);

       
         
            //.Where(f => f.Name.ToLower().EndsWith(".htm")).ToList();
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            //an to xreiasteis
            ////////StreamReader sr = lstf[0].OpenText();
            ////////string s = "";
            ////////while ((s = sr.ReadLine()) != null)
            ////////{
            ////////    Console.WriteLine(s);
            ////////}
            ////////sr.Close();
            using (var db = new WofraContext())
            {

               HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
               

                // There are various options, set as needed
                htmlDoc.OptionFixNestedTags = true;


                for (int i = 0; i < lstf.Count; i++)
                {

                    // filePath is a path to a file containing the html
                    htmlDoc.Load(lstf[i].FullName);

                    var im = new ItemModel() { Id = i + 1, Html = htmlDoc.DocumentNode.OuterHtml, Info = lstf[i].FullName };
                    HtmlFiles.Add(im);
                    db.ItemModels.Add(im);
                    db.SaveChanges();
                    //List<DocumentNode> nodes = new List<DocumentNode>();
                    //HtmlNodeCollection Results = htmlDoc.DocumentNode.SelectNodes("//ul[@class='docMain']");

                    //List<HtmlNode> Results = htmlDoc.DocumentNode.SelectNodes("//ul[@class='docMain']").ToList(); 
                    //Descendants("ul").Where(tr => tr.GetAttributeValue("class", "").Contains("docMain")).ToList();
                    HtmlNodeCollection Results = htmlDoc.DocumentNode.SelectNodes("//ul[@class='documentListData docMain ScopusResultsListRowColor'] | //ul[@class='documentListData docMain bg4']");

                   // var pubs = HtmlFiles[i].Publications;

                    int id = 0;
                    foreach (var item in Results)
                    {

                        var p = new Publication() { Id = id, html = item.OuterHtml };



                        //var PubRes = item.SelectSingleNode("//li[@class='dataCol2']").Descendants("a").Single();
                        //var PubRes1 = item.SelectSingleNode("//li[@class='dataCol2']").Descendants().Where(tag => tag.Name.ToLower() == "a").First();
                        var PubRes = item.ChildNodes.Where(ch => ch.HasAttributes && ch.Attributes["class"].Value == "dataCol2").First().Descendants().Where(tag => tag.Name.ToLower() == "a").First();

                        p.title = PubRes.InnerHtml;
                        p.link = PubRes.GetAttributeValue("href", string.Empty);

                        int aid = 0;

                        var AuthorRes = item.ChildNodes.Where(ch => ch.HasAttributes && ch.Attributes["class"].Value == "dataCol3").First().Descendants("a");
                        p.AuthorsString = AuthorRes.FirstOrDefault().OuterHtml;

                        foreach (var auth in AuthorRes)
                        {

                            var a = new Authors() { Id = aid, link = auth.GetAttributeValue("href", string.Empty), Priority=aid };

                            a.Name = auth.InnerHtml;
                            p.Authors.Add(a);
                            db.Authors.Add(a);
                            db.SaveChanges();
                            aid++;
                        }

                        pubs.Add(p);
                        db.Publications.Add(p);
                        db.SaveChanges();
                        id++;
                    }

                }

                //private void Button_Click_1(object sender, RoutedEventArgs e)
                //{
                //    using (var db = new WofraContext())
                //    {
                //        // Create and save a new Blog
                //        //Console.Write("Enter a name for a new Auth: ");
                //        //var name = Console.ReadLine();

                //        var auth = new Authors { Name = "maria" };
                //        db.Authors.Add(auth);
                //        db.SaveChanges();


                //    }
                //}
            }
        }
    }
}
