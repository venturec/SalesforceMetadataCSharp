using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.log;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace SalesforceMetadata
{
    public partial class ExtractWebsites : Form
    {
        String urlsToRetrieveFile;
        String urlsToRetrieveFileTemp;
        String anchorTagsToExtract;
        //String retrievedUrlsfile;

        public ExtractWebsites()
        {
            InitializeComponent();
            populateFileNames();
        }

        private void btnGetPageLinks_Click(object sender, EventArgs e)
        {
            if (this.tbURL.Text == "") return;
            if (this.tbFileSaveLocation.Text == "") return;

            UtilityClass.partialUrl = "";
            UtilityClass.retrievedUrls = new HashSet<String>();
            UtilityClass.hrefsExtracted = new HashSet<String>();

            populateFileNames();

            StreamWriter swToRetrieve = new StreamWriter(urlsToRetrieveFile);
            swToRetrieve.Close();

            StreamWriter swAnchorTagsToExtract = new StreamWriter(anchorTagsToExtract);
            swAnchorTagsToExtract.Close();

            UtilityClass.retrievedUrls.Add(this.tbURL.Text);

            String[] splitUrl = this.tbURL.Text.Split('/');
            //splitUrl[1] = "//";

            for (Int32 i = 0; i < splitUrl.Length - 1; i++)
            {
                UtilityClass.partialUrl += splitUrl[i] + "/";
            }

            UtilityClass.partialUrl = UtilityClass.partialUrl.Substring(0, UtilityClass.partialUrl.Length - 1);

            String pageData = retrieveHTMLPage(this.tbURL.Text);
            extractAnchorTagsFromPage(pageData);
            anchorTagsToURL();
            clearAnchorTagsFile();

            // Now open the file URLsToRetrieve, read each line and update the AnchorTagsToExtract
            // The cmbLayers denotes how many loops to make from the web url entered into the text box
            for (Int32 i = 0; i < Convert.ToInt32(cmbLayers.Text); i++)
            {
                writeToLogFile("btnGetPageLinks_Click For Loop", i.ToString());

                StreamReader sr = new StreamReader(urlsToRetrieveFile);
                while (sr.EndOfStream == false)
                {
                    String url = sr.ReadLine();

                    writeToLogFile("btnGetPageLinks_Click For Loop", url);
                    writeToLogFile("btnGetPageLinks_Click For Loop", UtilityClass.retrievedUrls.Contains(url).ToString());

                    if (!UtilityClass.retrievedUrls.Contains(url))
                    {
                        pageData = retrieveHTMLPage(url);
                        if (pageData != "")
                        {
                            extractAnchorTagsFromPage(pageData);
                            UtilityClass.retrievedUrls.Add(url);
                        }
                    }
                }

                sr.Close();

                // At the end of this process, move all URLs in urlsToRetrieveFile to retrievedUrlsfile
                if (i < Convert.ToInt32(cmbLayers.Text))
                {
                    anchorTagsToURL();
                    clearAnchorTagsFile();
                }
            }

            MessageBox.Show("Extraction Complete");
        }


        public String retrieveHTMLPage(String strUrl)
        {

            String data = "";

            //if (this.cbLoadToBrowser.CheckState == CheckState.Unchecked)
            //{
            //    WebRequest request = WebRequest.Create(strUrl);
            //    WebResponse response = request.GetResponse();
            //    Stream responseStream = response.GetResponseStream();

            //    using (StreamReader sr = new StreamReader(responseStream))
            //    {
            //        data = sr.ReadToEnd();
            //    }
            //}
            //else
            //{

            // Get the domain from strUrl and compare to what you have in the UtilityClass.partialUrl.
            // If they are the same, then proceed to extract the page data.

            WebRequest request = WebRequest.Create(strUrl);
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                using (StreamReader sr = new StreamReader(responseStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                // TODO: Add some error handling here
            }

            //}

            return data;
        }


        public void extractAnchorTagsFromPage(String pageData)
        {
            StreamWriter swAnchorTagsToExtract = new StreamWriter(anchorTagsToExtract, true);
            foreach (String tag in extractAnchorTags(pageData))
            {
                swAnchorTagsToExtract.WriteLine(tag);
            }

            swAnchorTagsToExtract.Close();
        }


        public List<String> extractAnchorTags(String pageData)
        {
            List<String> anchorTagsList = new List<String>();

            List<Char> htmlCharacterData = pageData.ToList();
            String anchorTag = "";
            Boolean insideAnchorTag = false;
            for (Int32 i = 0; i < htmlCharacterData.Count; i++)
            {
                if (htmlCharacterData[i] == '<' 
                    && htmlCharacterData[i + 1] == 'a'
                    && htmlCharacterData[i + 2] == ' ')
                {
                    insideAnchorTag = true;
                }

                if (insideAnchorTag)
                {
                    anchorTag = anchorTag + htmlCharacterData[i].ToString();
                }

                if (htmlCharacterData[i] == '>' && insideAnchorTag == true)
                {
                    insideAnchorTag = false;
                    anchorTagsList.Add(anchorTag);
                    anchorTag = "";
                }
            }

            return anchorTagsList;
        }

        public MatchCollection extractAllHTMLTags(String pageData)
        {
            Regex regex = new Regex("(</?([^>/]*)/?>)");
            //Regex regex = new Regex(@"<a>\s*(.+?)\s*</a>");
            MatchCollection matches = regex.Matches(pageData);

            return matches;
        }

        public void anchorTagsToURL()
        {
            StreamReader srAnchorTags = new StreamReader(anchorTagsToExtract);

            Int32 numberOfLines = 0;
            while (srAnchorTags.EndOfStream == false)
            {
                srAnchorTags.ReadLine();
                numberOfLines++;
            }

            srAnchorTags.Close();

            if (numberOfLines > 0)
            {
                // Why did I do this? 
                // UtilityClass.hrefsExtracted.Clear();

                srAnchorTags = new StreamReader(anchorTagsToExtract);
                StreamWriter swUrlsToRetrieve = new StreamWriter(urlsToRetrieveFile, true);

                while (srAnchorTags.EndOfStream == false)
                {
                    // extract the href= tag
                    String[] urlSplit = srAnchorTags.ReadLine().Split(' ');

                    foreach (String us in urlSplit)
                    {
                        if (us.StartsWith("href="))
                        {
                            Boolean inUrlString = false;
                            inUrlString = false;
                            String subUrl = "";
                            foreach (Char c in us.ToCharArray())
                            {
                                if (c == '\'' && inUrlString == false)
                                {
                                    inUrlString = true;
                                }
                                else if (c == '\'' && inUrlString == true)
                                {
                                    inUrlString = false;
                                }
                                else if (c == '\"' && inUrlString == false)
                                {
                                    inUrlString = true;
                                }
                                else if (c == '\"' && inUrlString == true)
                                {
                                    inUrlString = false;
                                }
                                else if (inUrlString == true)
                                {
                                    subUrl += c;
                                }
                            }

                            // if url ends with # then skip
                            // if url is the same as the url in tbUrl, then skip
                            // if file contains the Url, then skip
                            // you'll need to keep the url in a hashset to keep the urls unique
                            if (subUrl != "#")
                            {
                                String hrefLink = "";

                                if (subUrl.StartsWith("http"))
                                {
                                    hrefLink = subUrl;
                                }
                                else if (subUrl.StartsWith("//"))
                                {
                                    hrefLink = "http:" + subUrl;
                                }
                                else if (subUrl.StartsWith("/"))
                                {
                                    hrefLink = UtilityClass.partialUrl + subUrl;
                                }
                                else if (subUrl.StartsWith("../"))
                                {
                                    // Knock off the .. at the beginning of the subUrl
                                    while (subUrl.StartsWith("../"))
                                    {
                                        subUrl = subUrl.Substring(3, subUrl.Length - 3);
                                    }

                                    hrefLink = UtilityClass.partialUrl + "/" + subUrl;
                                }
                                else if (subUrl.StartsWith("./"))
                                {
                                    while (subUrl.StartsWith("./"))
                                    {
                                        subUrl = subUrl.Substring(2, subUrl.Length - 2);
                                    }

                                    hrefLink = UtilityClass.partialUrl + "/" + subUrl;
                                }
                                else
                                {
                                    hrefLink = UtilityClass.partialUrl + "/" + subUrl;
                                }

                                writeToLogFile("anchorTagsToURL", hrefLink);
                                writeToLogFile("btnGetPageLinks_Click For Loop", UtilityClass.hrefsExtracted.Contains(hrefLink).ToString());

                                // Check if the tool is going to traverse URLs outside of the domain 
                                // like Facebook, Twitter and other links
                                if (!UtilityClass.hrefsExtracted.Contains(hrefLink)
                                    && this.cbStayInSameDomain.CheckState == CheckState.Checked
                                    && hrefLink.StartsWith(UtilityClass.partialUrl))
                                {
                                    swUrlsToRetrieve.WriteLine(hrefLink);
                                    UtilityClass.hrefsExtracted.Add(hrefLink);
                                }
                                else if(!UtilityClass.hrefsExtracted.Contains(hrefLink)
                                        && this.cbStayInSameDomain.CheckState == CheckState.Unchecked)
                                {
                                    swUrlsToRetrieve.WriteLine(hrefLink);
                                    UtilityClass.hrefsExtracted.Add(hrefLink);
                                }
                            }
                        }
                    }
                }

                // Close the swToRetrieve
                srAnchorTags.Close();

                // Close the writing to swToRetrieveTemp
                swUrlsToRetrieve.Close();

            }
        }


        private void tbFileSaveLocation_DoubleClick(object sender, EventArgs e)
        {
            this.tbFileSaveLocation.Text = UtilityClass.folderBrowserSelectPath("Save URLs To...", true, FolderEnum.SaveTo);
        }


        private void clearAnchorTagsFile()
        {
            StreamWriter swAnchorTagsToExtract = new StreamWriter(anchorTagsToExtract);
            swAnchorTagsToExtract.Close();
        }


        private void btnRetrieveWebsites_Click(object sender, EventArgs e)
        {
            if (this.tbFileSaveLocation.Text == "") return;

            populateFileNames();

            if (!Directory.Exists(this.tbFileSaveLocation.Text + "\\SavedPages\\"))
            {
                Directory.CreateDirectory(this.tbFileSaveLocation.Text + "\\SavedPages\\");
            }


            // If the application has an error and stops halfway through, we don't want to rewrite everything
            if (!File.Exists(this.urlsToRetrieveFile))
            {
                StreamWriter urlsToRetrieveWriter = new StreamWriter(this.urlsToRetrieveFile);
                urlsToRetrieveWriter.WriteLine(this.tbURL.Text);
                urlsToRetrieveWriter.Close();
            }
            else
            {
                // Open up the file, see if the URL exists in the list, and if not write it
                StreamReader urlsToRetrieveReader = new StreamReader(this.urlsToRetrieveFile);
                String urls = urlsToRetrieveReader.ReadToEnd();
                urlsToRetrieveReader.Close();

                if (!urls.Contains(this.tbURL.Text))
                {
                    StreamWriter urlsToRetrieveWriter = new StreamWriter(this.urlsToRetrieveFile, true);
                    urlsToRetrieveWriter.WriteLine(this.tbURL.Text);
                    urlsToRetrieveWriter.Close();
                }
            }

            StreamReader sr = new StreamReader(urlsToRetrieveFile);
            while (sr.EndOfStream == false)
            {
                String url = sr.ReadLine();
                String pageData = retrieveHTMLPage(url);

                if (url.EndsWith("/"))
                {
                    url = url.Substring(0, url.Length - 1);
                }

                String[] splitUrl = url.Split('/');


                // Build the file name to save
                // Knock off the last "_"
                // Add an .html if it does not exist
                //String fileName = splitUrl[splitUrl.Length - 1] + ".html";
                String fileName = "";
                for (Int32 i = 0; i < splitUrl.Length; i++)
                {
                    if (i > 2)
                    {
                        fileName = fileName + splitUrl[i] + "_";
                    }
                }

                if (fileName.Length != 0)
                {
                    // Knock off the last "_" on the file name before adding the .html
                    fileName = fileName.Substring(0, fileName.Length - 1);

                    if (!fileName.EndsWith(".html"))
                    {
                        fileName = fileName + ".html";
                    }

                    if (fileName.Contains("#"))
                    {
                        splitUrl = fileName.Split('#');
                        fileName = splitUrl[0];
                    }

                    fileName = fileName.Replace(";", "_");
                    fileName = fileName.Replace("-", "_");
                    fileName = fileName.Replace("?", "_");
                    fileName = fileName.Replace("+", "_");
                    fileName = fileName.Replace("&", "_");
                    fileName = fileName.Replace("=", "_");

                }
                else
                {
                    fileName = "mainpage.html";
                }

                // Write the pageData to a folder/file
                try
                {
                    StreamWriter sw = new StreamWriter(this.tbFileSaveLocation.Text + "\\SavedPages\\" + fileName);
                    sw.Write(pageData);
                    sw.Close();
                }
                catch (Exception writeExc)
                {
                    writeToLogFile("Write HTML File error", fileName);
                }
            }

            sr.Close();

            MessageBox.Show("Web Page Download Complete");
        }

        private void populateFileNames()
        {
            this.urlsToRetrieveFile = this.tbFileSaveLocation.Text + "\\" + "URLsToRetrieve.txt";
            this.urlsToRetrieveFileTemp = this.tbFileSaveLocation.Text + "\\" + "URLsToRetrieveTemp.txt";
            this.anchorTagsToExtract = this.tbFileSaveLocation.Text + "\\" + "AnchorTagsToExtract.txt";
        }


        private void btnHTMLToText_Click(object sender, EventArgs e)
        {
            foreach (String fileName in Directory.GetFiles(this.tbFileSaveLocation.Text + "\\SavedPages\\"))
            {
                StreamReader sr = new StreamReader(fileName);
                List<String> anchorTagList = extractAnchorTags(sr.ReadToEnd());

            }
        }


        private void writeToLogFile(String method, String value)
        {
            StreamWriter sw = new StreamWriter(this.tbFileSaveLocation.Text + "\\LogFile.txt", true);

            sw.WriteLine(method + '\t' + value);

            sw.Close();
        }


        private void btnPDFToText_Click(object sender, EventArgs e)
        {
            if (this.tbPDFFileLocation.Text == "") return;

            PdfReader reader = new PdfReader(this.tbPDFFileLocation.Text);

            String[] parsedFilePath = this.tbPDFFileLocation.Text.Split('\\');
            String[] parsedFileName = parsedFilePath[parsedFilePath.Length - 1].Split('.');

            String originationFilePath = "";
            for (Int32 i = 0; i < parsedFilePath.Length - 1; i++)
            {
                originationFilePath = originationFilePath + parsedFilePath[i] + "\\";
            }

            StreamWriter sw;
            if (this.tbFileSaveLocation.Text == "")
            {
                sw = new StreamWriter(originationFilePath + parsedFileName[0] + ".txt");
            }
            else
            {
                sw = new StreamWriter(this.tbFileSaveLocation.Text + "\\" + parsedFileName[0] + ".txt");
            }

            if (cmbIncludeTextPos.Text == "Yes")
            {
                iTextSharp.text.Utilities.includeTextPositionsInFile = true;
                sw.WriteLine("CharSpaceWidth" + "\t" + "StartLocation" + "\t" + "EndLocation" + "\t" + "DistParallelStart" + "\t" + "DistParallelEnd" + "\t" + "DistPerpendicular" + "\t" + "TextValue");
            }
            else
            {
                iTextSharp.text.Utilities.includeTextPositionsInFile = false;
            }


            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                ITextExtractionStrategy its = new LocationTextExtractionStrategy();

                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);
                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                sw.WriteLine(s);
            }

            sw.Close();
            reader.Close();

            MessageBox.Show("PDF Extraction Complete");
        }

        private void tbPDFFileLocation_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.tbPDFFileLocation.Text = ofd.FileName;
            }
        }
    }
}
