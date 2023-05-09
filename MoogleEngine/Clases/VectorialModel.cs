using MoogleEngine;

namespace MoogleEngine.Clases
{
    //Implementacion del Modelo Vectorial
    class VectorialModel
    {
        //Calculo del IDF
        static public Dictionary<string,double> IDF(Dictionary<string,double> GeneralWordCount , double TotalDoc)
        {
            Dictionary<string,double> IDFs = new Dictionary<string, double>();
            foreach(string word in GeneralWordCount.Keys)
            {           
                IDFs.Add(word,Math.Log10(TotalDoc/GeneralWordCount[word]));
            }

            return IDFs;
        }

        //Calculo del TF
        static public Dictionary<string,Dictionary<string,double>> TF(Dictionary<string,Dictionary<string,double>> GeneralDoc, Dictionary<string,double> WordText)
        {
            Dictionary<string,Dictionary<string,double>> TFs = new Dictionary<string, Dictionary<string, double>>();

            foreach(string word in GeneralDoc.Keys)
            {
                double MaxWordFrec = GeneralDoc[word].Values.Max();

                TFs[word]=new Dictionary<string, double>();
                foreach (string word2 in WordText.Keys)
                {
                    if(GeneralDoc[word].ContainsKey(word2))
                    {
                        TFs[word][word2]=GeneralDoc[word][word2]/MaxWordFrec;
                    }
                    else
                    {
                        TFs[word][word2]=0;
                    }                    
                }
            } 
            return TFs;

        }

        //Calculo de los pesos de los documentos
        static public Dictionary<string,Dictionary<string,double>> Weights(Dictionary<string,double> IDFs , Dictionary<string,Dictionary<string,double>> TFs)
        {
            Dictionary<string,Dictionary<string,double>> Weights = new Dictionary<string, Dictionary<string, double>>();
            foreach(string document in TFs.Keys)
            {
                Dictionary<string,double> WordWeight = new Dictionary<string, double>();
                foreach(string word in TFs[document].Keys)
                {
                    WordWeight.Add(word,TFs[document][word]*IDFs[word]);
                }
                Weights.Add(document,WordWeight);
            }
            return Weights;
        }

        //Peso de la query
        static public Dictionary<string,double> queryweight (Dictionary<string, double> IDFs, string[] query)
        {
            Dictionary<string,double> queryweight = new Dictionary<string, double>();
            Dictionary<string,double> TFquery = new Dictionary<string, double>();
            
            foreach(string word in query)
            {
                if(TFquery.ContainsKey(word))
                {
                    TFquery[word]++;
                    
                }
                else
                {
                    TFquery[word]=1;
                }
            }

            double a = 0.5;
            double MaxValueQuery = TFquery.Values.Max();
            foreach(string word in query)
            {
                queryweight[word]=((a+(1-a)*(TFquery[word]/MaxValueQuery))*IDFs[word]);
            }
            return queryweight;
        }
        
        //Asignacion del score a cada documento
        static public Dictionary<string,double> score(string[] query, Dictionary<string,double> IDFs, Dictionary<string,Dictionary<string,double>> GeneralDic, Dictionary<string,Dictionary<string,double>> Weights)        
        {           
            Dictionary<string,double> queryweights = queryweight(IDFs, query);

            Dictionary<string,double> scores = new Dictionary<string, double>();

            foreach(string document in GeneralDic.Keys)
            {
                scores[document]=0;               
                double numeratorsumvalue = 0; 
                double denominator1 = 0;
                double denominator2 = 0;
                foreach(string word2 in GeneralDic[document].Keys)
                {
                    denominator1 += (Math.Pow(Weights[document][word2],2));
                }

                foreach(string word in query)
                {
                    denominator2 += Math.Pow(queryweights[word],2);    
                    
                    if(Weights[document].ContainsKey(word))
                    {
                        numeratorsumvalue += (Weights[document][word]*queryweights[word]);
                    }
                }

                scores[document] = numeratorsumvalue/((Math.Sqrt(denominator1))*(Math.Sqrt(denominator2)));
                    
            }          
            return scores;

        }

        //Busqueda de la sugerencia
        static public string[] Sugestion(Dictionary<string, double> GeneralWordCount, string[] Query, Dictionary<string, Dictionary<string, double>> GeneralDic)
        {
            string[] sugquery = new string[Query.Length];
            int i = 0;
            string tempWord = "";

            foreach (string word in Query)
            {
                int minLevenshtein = int.MaxValue;

                if (!GeneralWordCount.ContainsKey(word))
                {
                    foreach (string word2 in GeneralWordCount.Keys)
                    {
                        int levenshteinDist = StringDistance(word, word2);
                        if (minLevenshtein > levenshteinDist)
                        {
                            minLevenshtein = StringDistance(word, word2);
                            tempWord = word2;
                            sugquery[i] = word2;

                        }
                    }
                }
                else
                {
                    sugquery[i] = word;
                }
                i++;
            }
            MoogleEngine.Moogle.sugestion = string.Join(" ", sugquery);
            return sugquery;
        }

        //Distancia de Levenshtein
        static public int StringDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; d[i, 0] = i++)
            {

            }
            for (int j = 0; j <= m; d[0, j] = j++)
            {

            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j -1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];

        }            
    }
}



