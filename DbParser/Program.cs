using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace DbParser
{
    class Program
    {
        static void Main(string[] args)
        {
            void WriteScript(string text)
            {
                using (StreamWriter stream = new StreamWriter(@"D:\DBScript.txt", true, System.Text.Encoding.Default))
                {
                    stream.WriteLine(text);
                }
            }

            string DBaseName = Path.GetRandomFileName(); //генерация случайного названия базы данных
            DBaseName = DBaseName.Replace(".", "");
            string script = "Create database ";
            script += DBaseName;
            script += ";";
            WriteScript(script);

            Assembly assembly = Assembly.LoadFile(@"C:\Users\home\source\repos\reflectionPractical\reflectionPractical\bin\Debug\reflectionPractical.exe");
            List<Entity> DbaseEntity = new List<Entity>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name != "Program")
                {
                    string name = type.Name;
                    Entity entity = new Entity(name);
                    foreach (var memberInfo in type.GetMembers())
                    {
                        if (memberInfo is PropertyInfo)
                        {
                            entity.PropertyName.Add(memberInfo.Name.ToString() + " ");
                            var property = memberInfo as PropertyInfo;
                            if (property.PropertyType.ToString() == "System.String")
                                entity.PropertyType.Add("NVARCHAR(MAX), ");
                            else if (property.PropertyType.ToString() == "System.Int32")
                                entity.PropertyType.Add("INT, ");
                            else if (property.PropertyType.ToString() == "System.Double")
                                entity.PropertyType.Add("FLOAT");
                            else if (property.PropertyType.ToString() == "System.Guid")
                                entity.PropertyType.Add("UNIQUEIDENTIFIER, ");
                            else if (property.PropertyType.ToString() == "System.DateTime")
                                entity.PropertyType.Add("DATE, ");
                            else if (property.PropertyType.ToString() == "System.Boolean")
                                entity.PropertyType.Add("BIT, ");
                            else if (property.PropertyType.ToString() == "System.Decimal")
                                entity.PropertyType.Add("MONEY, ");
                        }
                    }
                    DbaseEntity.Add(entity);
                }
            }
            foreach (var entity in DbaseEntity)
            {
                int i = 0;
                script = "create table ";
                script += entity.Name;
                script += "\n(\n";
                foreach (var propertyName in entity.PropertyName)
                {
                    if(i == entity.PropertyName.Count - 1)
                    {
                        entity.PropertyType[i] = entity.PropertyType[i].Substring(0, entity.PropertyType[i].Length - 2); // изначально типы данных полей класса 
                    }                                                                                                    //добавляются с запятой, но перед добавлением  
                    script += entity.PropertyName[i];                                                                    //последнего поля в скрипт этим методом я удаляю запятую
                    script += entity.PropertyType[i];
                    script += "\n";
                    i++;
                }
                script += ");";
                WriteScript(script);
            }
            Console.ReadKey();
        }
    }
}
