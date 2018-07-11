using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LINQ_XML
{
    class Program
    {
        private static Model5 db = new Model5();
        static void Main(string[] args)
        {
            //Task1();
            //Task2();
            //Task4();
            //Task6();
            Task7();
        }
        static void Task1()
        {
           // a.Все зоны/ участки которые принадлежат PavilionId = 1, при этом каждая зоны должна находиться в отдельном XML файле с наименованием PavilionId.
           DirectoryInfo dir = new DirectoryInfo("Pavilion");
            if (!dir.Exists)
                Directory.CreateDirectory("Pavilion");
            foreach (Area area in db.Area.Where(o=>o.PavilionId == 1))
            {
                XElement xe = new XElement("Area",
                    new XElement("Name",
                        new XAttribute("AreaID", area.AreaId),
                    area.Name),

                    new XElement("FullName", area.FullName),
                    new XElement("IP", area.IP)
                    );
                xe.Save("Pavilion/PavilionId_"+area.AreaId+".xml");
            }
        }
        static void Task2()
        {
            // b.Используя Directory класс, создать папки с название зон / участков, в каждую папку выгрузить XML файл на основе данных их таблицы.
            DirectoryInfo dir = new DirectoryInfo("Task2");
            if (!dir.Exists)
                Directory.CreateDirectory("Task2");
            foreach (Area area in db.Area)
            {
                Directory.CreateDirectory("Task2/" + area.Name);
                XElement xe = new XElement("Area",
                    new XElement("Name",
                        new XAttribute("AreaID", area.AreaId),
                    area.Name),

                    new XElement("FullName", area.FullName),
                    new XElement("IP", area.IP)
                    );
                string path = string.Format("{0}/{1}/{2}.xml", "Task2", area.Name, area.AreaId);
                xe.Save(path); 
            }
        }
        static  void Task4()
        {
            // c.Выгрузить XML файл только тех участков, которые не имеют дочерних элементов (ParentId = 0);
            // d.Выгрузить из таблицы Timer, данные только для зон у которых есть IP адрес, при этом в XML файл должны войти следующие поля: UserId, Area Name (name из Талицы Area), DateStart
          XElement xe = new XElement("Timers");
            foreach (Timer timer in db.Timer.Where(o=> !string.IsNullOrEmpty(o.Area.IP) && o.DateFinish != null))
            {
                XElement te =
                    new XElement("Timer",
                                    new XAttribute("TimerID", timer.TimerId),
                                 new XAttribute("UserID", timer.UserId),
                                 new XAttribute("DateStart", timer.DateStart),
                                 new XElement("AreaStart", timer.Area.Name)
                                 );
                xe.Add(te);        
            }
            xe.Save("TimerInfo.xml");
        }

        static void Task6()
        {
            //g.Выгрузить данные с таблицы Area в переменную, на основе данных в этой переменной создать XML файл имеющим Xmlns = «area», а также namespace - http://logbook.itstep.org/
            XNamespace nameSpace = "http://logbook.itstep.org/";

            XElement xe = new XElement("Areas");

            foreach (Area area in db.Area)
            {
                XElement x0 = new XElement(nameSpace + "Area",
                    new XElement(nameSpace + "Name", area.Name),
                    new XElement(nameSpace + "FullName", area.FullName),
                    new XElement(nameSpace + "IP", area.IP));
                xe.Add(x0);
            }
            xe.Save("Task6.xml");
        }

        static void Task7()
        {
            // a.Вывести на экран поля UserId, AreaId, DocumentId из XML файла пункта F
            XDocument xd = XDocument.Load("TimerInfo.xml");

            foreach (XElement timer in xd.Element("Timers").Elements("Timer"))
            {
                Console.WriteLine(timer.Attribute("UserID").Value);
            }

        }
    }
}
