using MoogleEngine;

namespace MoogleEngine.Clases
{
    //Guardar y Normalizar los textos
    class LoadText
    {
        static string Path = GetPath();
          
        //Obtener el path de los documentos
        static public string GetPath()
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Remove(path.LastIndexOf('M'));
            path += "/Content/";
            return path;
        }
        
        //Guardar el path
        static public string[] TextPath(string Path)
        {
            string[] TextPath = Directory.GetFiles(Path,"*.txt");

            return TextPath;
        }
        
        //Retirar signos no relevantes de los textos
        static public string[] JustText(string RawContent)
        {
            RawContent=RawContent.ToLower();
            string[] JustText = RawContent.Split(",.; /?><']}|-_=+)(*&^%$#!/*-+`~\n)".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return JustText;

        }

        //Guardar todos los textos en limpio
        static public string[] TextContent(string TextPath)
        {
            string Content = File.ReadAllText(TextPath);
            string[] AllContent = JustText(Content);
            return AllContent;
        }

        //Cuantas palabras tiene cada documento
        static public int Finaldoc()
        {
            int words = 0;
            Dictionary<string,int> Final = new Dictionary<string, int>();
            foreach(string paths in TextPath(Path))
            {
                foreach(string word in TextContent(paths))
                {
                    words++;                  
                }
                Final.Add(paths,words);
            }
            int result = Final.Values.Min();           
            return result;
        }

        //Crear una Tupla con Dos Diccionarios
        //Un diccionario es usado para guardar cuantas veces se repite cada palabra en cada documento
        //El otro diccionario es usado para guardar cuantas veces se repite cada palabra en general
        static public Tuple<Dictionary<string,Dictionary<string,double>>, Dictionary<string,double>> GeneralDocument()
        {
            Dictionary<string, string> Finalcont = new Dictionary<string, string>();
            Dictionary<string,Dictionary<string,double>> GeneralDoc = new Dictionary<string, Dictionary<string,double>>();
            Dictionary<string,double> GeneralWordCount = new Dictionary<string,double>();
            Tuple<Dictionary<string,Dictionary<string,double>>, Dictionary<string,double>> Chorizo = new Tuple<Dictionary<string, Dictionary<string,double>>, Dictionary<string,double>>(GeneralDoc,GeneralWordCount);


            foreach(string paths in TextPath(Path))
            {
                GeneralDoc.Add(paths,new Dictionary<string,double>());
                

                foreach(string word in TextContent(paths))
                {
                    if(GeneralDoc[paths].ContainsKey(word))
                    {
                        GeneralDoc[paths][word]++;
                    }
                    else
                    {
                        GeneralDoc[paths].Add(word,1);

                        if(GeneralWordCount.ContainsKey(word))
                        {
                            GeneralWordCount[word]++;
                        }               
                        else
                        {
                            GeneralWordCount.Add(word,1);
                        }

                    }                                         
                }
            }
            return Chorizo;
        }
    }    
}

