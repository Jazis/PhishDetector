using System;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;


namespace ConsoleApp1
{
    class Program
    {
        public static IWebDriver driver;
        public static Thread thread0;
        public static string target = "<none>";
        public static string domain = "";
        public static string ip = "";
        public static List<string> all_urls = new List<string>();
        public static List<string> crawled_dirs = new List<string>();
        public static List<string> good_urls = new List<string>();
        public static List<string> post_pages = new List<string>();
        public static List<string> Index_of_urls = new List<string>();
        public static List<string> index_files = new List<string>();
        public static List<string> texts = new List<string>();
        public static List<string> possible_original = new List<string>();

        static void Main(string[] args)
        {
            banner();
        }
        public static void site_set()
        {
            Console.Write("===Site here==>");
            target = Console.ReadLine();
        }
        public static void directory_by_ready_links()
        {
            for (int i = 0; i < all_urls.Count; i++)
            {
                string[] link = all_urls[i].Split('/');
                foreach (string elem in link)
                {
                    if (elem.Contains('.')) { }
                    else
                    {
                        if (crawled_dirs.Contains(elem)) { }
                        else { crawled_dirs.Add(elem); }
                    }
                }
            }
            crawled_dirs.Add("logs");
            crawled_dirs.Add("log");
            crawled_dirs.Add("uploads");
            crawled_dirs.Add("upload");
        }

        public static void banner()
        {
            Console.Clear();
            Console.WriteLine(

            "\n\t██╗░░██╗███████╗██╗░░░░░██╗░░░░░░█████╗░\n" +
            "\t██║░░██║██╔════╝██║░░░░░██║░░░░░██╔══██╗\n" +
            "\t███████║█████╗░░██║░░░░░██║░░░░░██║░░██║\n" +
            "\t██╔══██║██╔══╝░░██║░░░░░██║░░░░░██║░░██║\n" +
            "\t██║░░██║███████╗███████╗███████╗╚█████╔╝\n" +
            "\t╚═╝░░╚═╝╚══════╝╚══════╝╚══════╝░╚════╝░\n"
            );
            File.Delete("output_infos.txt");
            Console.WriteLine(
                $"\t[1]Set target - https://example.com/\n" +
                $"\t[2]Start scan\n"
            );
            while (true)
            {
                Console.Write("===Action==>");
                string action = Console.ReadLine();
                if (action == "1")
                {
                    site_set();
                }
                if (action == "2")
                {
                    main_func();
                }
            }
        }

        public static void mysearch_searching()
        {
            Console.WriteLine($"[+] Mysearch Start");
            foreach (string elem0 in texts)
            {
                string elem = elem0.Replace("\t", string.Empty).Replace("\n", string.Empty);
                string[] newline = elem.Split('>');
                for (int io = 0; io < newline.Length; io++)
                {
                    int jum = 0;
                    foreach (char ch in newline[io])
                    {
                        if (ch == ' ')
                        {
                            jum++;
                            if (jum == 5)
                            {
                                try
                                {
                                    Console.WriteLine($"[+] Trying - {elem}");
                                    driver.Navigate().GoToUrl($"https://search.mysearch.com/web?q={elem}&tpr=10&page=1");
                                    string[] vs = driver.PageSource.Split('\"');
                                    for (int i = 0; i < vs.Length; i++)
                                    {
                                        if (vs[i].Contains("algo-display-url"))
                                        {
                                            string link = vs[i + 1].Replace("\n", string.Empty).Split('<')[0];
                                            possible_original.Add(link.Replace(">", string.Empty));
                                            Console.WriteLine($"[+] Possible original - {link.Replace(">", string.Empty)}");
                                            using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                            {
                                                writer.Write($"[POSSIBLE] {link.Replace(">", string.Empty)}\n");
                                            }
                                        }
                                    }
                                    Console.WriteLine($"[+] Possible original - {possible_original.Count}");
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            driver.Quit();

        }

        public static void possible_sites_check()
        {
            List<string> sites = new List<string>();
            foreach (string elem in possible_original)
            {
                if (sites.Contains(elem.Split('/')[2])) { }
                else { sites.Add(elem.Split('/')[2]); }
            }
        }

        public static void fuzzer()
        {
            driver = new PhantomJSDriver();
            foreach (string elem in post_pages)
            {
                List<string> textBoxes = new List<string>();
                List<string> btns = new List<string>();
                driver.Navigate().GoToUrl(elem);
                string[] vs = driver.PageSource.Split(' ');
                for (int i =0; i< vs.Length; i++)
                {
                    if (vs[i].Contains("type=\"text\""))
                    {
                        for (int j =0; j<10; i++)
                        {
                            try
                            {
                                if (vs[i - 5 + j].Contains("name="))
                                {
                                    string st = vs[i - 5 + j].Split('=')[1].Replace("\"", string.Empty);
                                    textBoxes.Add(st);
                                    Console.WriteLine($"[+] Textbox founded - {st}");
                                    driver.FindElement(By.Name(st)).Click();
                                    driver.FindElement(By.Name(st)).SendKeys("lololol");
                                }
                            }
                            catch { }
                        }
                    }
                    if (vs[i].Contains("<button type="))
                    {
                        for (int j = 0; j < 5; i++)
                        {
                            try
                            {
                                if (vs[i + j].Contains("id="))
                                {
                                    string st = vs[i + j].Split('=')[1].Replace("\"", "");
                                    btns.Add(st);
                                    Console.WriteLine($"[+] Button founded - {st}");
                                    if (st == "login" && st == "submit") { driver.FindElement(By.Name(st)).Click(); }
                                    
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            driver.Quit();
        }

        public static void main_func()
        {
            Console.WriteLine("[+] Scan started");
            domain = target.Split('/')[0] + "//" + target.Split('/')[2] + '/';
            Console.WriteLine($"[+] Domain - {domain}");
            try { ip = Dns.GetHostByName(target.Split('/')[2]).AddressList[0].ToString(); } catch { }
            using (StreamWriter writer = File.AppendText("output_infos.txt"))
            {
                writer.Write($"[Domain] {domain}\n");
                writer.Write($"[Ip] {ip}\n");
            }
            Console.WriteLine($"[+] IP - {ip}");
            target_crawl();
            Console.WriteLine($"[+] Crawler (by page) end | Pages - {all_urls.Count}");
            directory_by_ready_links();
            Console.WriteLine($"[+] Directory taking end | Directories - {crawled_dirs.Count}");
            crawled_directories();
            //Thread thread1 = new Thread(parallele);
            //thread1.Start();
            driver = new PhantomJSDriver();
            Console.WriteLine($"[texts] {texts.Count}");
            mysearch_searching();
            possible_sites_check();
            fuzzer();
            Console.WriteLine($"[+] Directory crawling end | 200 code urls - {good_urls.Count}");
            if (Index_of_urls.Count != 0)
            {
                Console.WriteLine($"[+] Index of/ urls - {Index_of_urls.Count}");
            }
            if (index_files.Count != 0)
            {
                Console.WriteLine($"[+] Founded files in Index of / - {index_files.Count}");
            }
            if (post_pages.Count != 0)
            {
                Console.WriteLine($"[+] With POST action - {post_pages.Count}");
            }
        }

        public static void files_find(string[] pageSource)
        {
            for (int i = 0; i < pageSource.Length; i++)
            {
                if (pageSource[i].Contains("<img src="))
                {
                    index_files.Add(pageSource[i + 5]);
                    using (StreamWriter writer = File.AppendText("output_infos.txt"))
                    {
                        writer.Write($"[FILE] {pageSource[i + 5]}\n");
                    }
                }
            }
        }

        public static void crawled_directories()
        {
            for (int i = 0; i < crawled_dirs.Count; i++)
            {
                string url = target + crawled_dirs[i] + '/';
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url.ToString());
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(stream);
                    string[] vs = streamReader.ReadToEnd().Split("\"");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine($"[200] {url}");
                        good_urls.Add(url);
                        using (StreamWriter writer = File.AppendText("output_infos.txt"))
                        {
                            writer.Write($"[200] {url}\n");
                        }
                        foreach (string elem in vs)
                        {
                            if (elem.Contains("Index of /"))
                            {
                                Console.WriteLine($"[200-index] {url}");
                                Index_of_urls.Add(url);
                                using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                {
                                    writer.Write($"[200-index] {url}\n");
                                }
                                files_find(vs);
                            }
                            if (elem.Contains("<form method="))
                            {
                                Console.WriteLine($"[200-POST] {url}");
                                post_pages.Add(url);
                                using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                {
                                    writer.Write($"[200-post] {url}\n");
                                }
                            }
                            if (elem.Contains(" "))
                            {
                                int jum = 0;
                                foreach (char ch in elem)
                                {
                                    if (ch == ' ')
                                    {
                                        jum++;
                                        if (jum == 5 & elem.Contains("</p>"))
                                        {

                                            if (texts.Contains(elem)) { }
                                            else
                                            {
                                                texts.Add(elem.Split("<p>")[1].Split("</p>")[0]);
                                                using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                                {
                                                    writer.Write($"[TEXTS] {elem.Split("<p>")[1].Split("</p>")[0]}\n");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else { }
                }
                catch { }
            }
        }
        public static void target_crawl()
        {
            if (target == null)
            {
                Console.WriteLine("\n=================\nPlease, choose target\n=================\n");
                banner();
            }
            else
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(target.ToString());
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.193 Safari/537.36";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(stream);
                    string[] vs = streamReader.ReadToEnd().Split('\"');
                    for (int i = 0; i < vs.Length; i++)
                    {
                        try
                        {
                            if (vs[i].Contains("http") & vs[i].Contains("://"))
                            {
                                if (all_urls.Contains(target + vs[i])) { }
                                else
                                {
                                    all_urls.Add(target + vs[i]);
                                    using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                    {
                                        writer.Write($"[link]{target + vs[i]}\n");
                                    }
                                }
                            }
                            if (vs[i].Contains("href=") & !vs[i + 1].Contains("://"))
                            {
                                if (all_urls.Contains(target + vs[i])) { }
                                else
                                {
                                    all_urls.Add(target + vs[i + 1]);
                                    using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                    {
                                        writer.Write($"[link]{target + vs[i + 1]}\n");
                                    }
                                }
                            }
                            if (vs[i].Contains("scr=") & !vs[i + 1].Contains("://"))
                            {
                                if (all_urls.Contains(target + vs[i])) { }
                                else
                                {
                                    all_urls.Add(target + vs[i + 1]);
                                    using (StreamWriter writer = File.AppendText("output_infos.txt"))
                                    {
                                        writer.Write($"[link]{target + vs[i + 1]}\n");
                                    }
                                }
                            }
                        }
                        catch { }

                    }
                }
                catch { Console.WriteLine("[-] Connection Error"); Thread.Sleep(2000); }
            }
        }

    }
}
