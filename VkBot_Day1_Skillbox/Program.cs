using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VkBot_Day1_Skillbox
{
    class Program
    {
        static string path = "http://ksergey.ru/file/homework.xml";
        static Dictionary<string, int> map = new Dictionary<string, int>();
        static List<XElement> arr;
        static string file;
        static void Main(string[] args){
            try{
                Console.WriteLine("Процесс пошёл, подождите минутку...");
                getFile();

                arr = XDocument.Parse(file).Descendants("WEATHER").Descendants("REPORT").Descendants("TOWN").ToList();

                consider();

                createFile();

                Console.WriteLine("Хороший конец");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Файл не найден");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка работы программы"+ex);
            }
            Console.ReadKey();
        }
        static void getFile() {
            System.Net.WebClient wc = new System.Net.WebClient();
            file = wc.DownloadString(path);
        }
        static void consider() {
            for (int i = 0; i < arr.Count; i++)
            {
                string temp = arr[i].Element("TEMPERATURE").Attribute("value").Value;

                if (map.ContainsKey(temp)) map[temp]++;
                else map.Add(temp, 1);
            }
        }
        static void createFile() {
            XElement doc = new XElement("DB");
            XElement head = new XElement("Head");
            foreach (var temp in map)
            {
                XElement temper = new XElement("Temperature");
                XAttribute thermometer = new XAttribute("Thermometer", temp.Key);
                XAttribute count = new XAttribute("Count", temp.Value);
                temper.Add(thermometer);
                temper.Add(count);
                head.Add(temper);
            }
            doc.Add(head);
            doc.Save("DB.xml");
        }
    }
}
