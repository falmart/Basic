using MoogleEngine.Clases;
namespace MoogleEngine;


public static class Moogle
{
    public static string sugestion = "";
    public static string queri = "";  
    public static int result = 0;
    public static Dictionary<string, string[]> AllContent = new Dictionary<string, string[]>();
    public static Dictionary<string, Dictionary<string, double>> GeneralDic = new Dictionary<string, Dictionary<string, double>>();
    public static Dictionary<string, double> GeneralWordCount = new Dictionary<string, double>();
    public static Dictionary<string, double> IDFs = new Dictionary<string, double>();
    public static Dictionary<string, Dictionary<string, double>> TFs = new Dictionary<string, Dictionary<string, double>>();
    public static Dictionary<string, Dictionary<string, double>> Weight = new Dictionary<string, Dictionary<string, double>>();
    public static Dictionary<string, double> scores = new Dictionary<string, double>();
    public static Dictionary<string, double> queryweight = new Dictionary<string, double>();
    
    //Llamado desde Moogle Server para cargar los datos durante la carga del proyecto
    public static void Call()
    {
        Tuple<Dictionary<string, Dictionary<string, double>>, Dictionary<string, double>> theTuple = LoadText.GeneralDocument();
        result = LoadText.Finaldoc();
        GeneralDic = theTuple.Item1;
        GeneralWordCount = theTuple.Item2;
        IDFs = VectorialModel.IDF(GeneralWordCount, GeneralDic.Keys.Count());
        TFs = VectorialModel.TF(GeneralDic, IDFs);
        Weight = VectorialModel.Weights(IDFs, TFs);
    }

    //Mostrar el snippet
    public static string Snipet(string path)
    {       
        string readingSnipet = System.IO.File.ReadAllText(path);              
        LoadText.JustText(readingSnipet);
        int rnvalue = result/20;        
        return readingSnipet.Substring(0,rnvalue);
    }


    //Metodo final, se implementa la sugerencia y el snippet y se organizan los documentos por score 
    public static SearchResult Query(string query) 
    {
        queri=query;
        string[] query2 = LoadText.JustText(query);
        query2=VectorialModel.Sugestion(GeneralWordCount, query2, GeneralDic);
        scores=VectorialModel.score(query2, IDFs, GeneralDic, Weight);

        SearchItem[] items = new SearchItem[scores.Keys.Count()];

        List<Tuple<double,string>> results = new List<Tuple<double,string>>();

        foreach(string document in scores.Keys)
        {
            Tuple<double,string> xvalue = new Tuple<double,string>(scores[document], document);
            results.Add(xvalue);
        }

        results.Sort();
        results.Reverse();
        int Count = 0;

        for(int i =0;i<results.Count();i++)
        {
            string trueneutral = results[i].Item2;
            string neutral = results[i].Item2.Substring(40);
            neutral=neutral.Substring(0,neutral.Length-3);
            double xcore = results[i].Item1;
            items[Count]=new SearchItem(neutral, Snipet(trueneutral), (float)xcore);
            Count++;
        }
        
        return new SearchResult(items, sugestion);
    }

}
