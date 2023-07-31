namespace MoogleEngine;

using System;
using System.Text.RegularExpressions;

public class QueryOperations{


/* Método que se encarga de almacenar en un array de objetos Document, el nombre, la dirección y el contenido
ya normalizado de cada documento txt encontrado en la carpeta "Content". */

    public static Document[] LoadFiles(){
    List<Document> documents = new List<Document>();
    
    string[] files = Directory.GetFiles(Path.Combine("..","Content"));                // Obteniendo un array con todos los documentos txt

    foreach(string file in files){
        string fileName = Path.GetFileName(file);                                    //Guardando en un string el nombre de cada documento
        string[] content = Normalize(File.ReadAllText(file).Split(' '));            // Guardando en un array el contenido de cada documento ya normalizado y separado por palabras  
        documents.Add(new Document(fileName, file, content));           
    }

    return documents.ToArray();
 }


/* Método que recibe un array de string con el contenido de un texto y elimina cada símbolo extraño que detecte,
dejando solamente las letras(en minúscula y sin tilde) y los números.
Devuelve un array de string con el contenido normalizado. */

 public static string[] Normalize(string[] content){
    List<string> normalizedContent = new List<string>();

    for(int i = 0; i < content.Length; i++){
        string lowercase = content[i].ToLower();                                                                    // Poniendo las palabras en minúsculas
        string clean = Regex.Replace(lowercase, @"[^a-z0-9\s]", "");                                               // Eliminando los símbolos extraños, dejando sólo las letras y los números   
        clean = clean.Replace('á','a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('ú', 'u');   // Eliminando las tildes
        if(clean != ""){
            normalizedContent.Add(clean);                           
        }
        
    }

    return normalizedContent.ToArray();
 }


/* Método que recibe un string y un array de Document, y devuelve una tupla conformada por el string(query) normalizado
y una lista de string que contiene las palabras en las cuales aparece el símbolo "$". */

    public static (string[], List<string>) QueryNormalize(string query, Document[] documents){   
       (string[], List<string>) symbols = Symbols(query.Split(' '), documents);
       string[] content = symbols.Item1 = Normalize(symbols.Item1);
       
        return (content, symbols.Item2);
    }


/* Método que recibe una tupla(la misma tupla devuelta por el método anterior) y un objeto Operations y devuelve
un array de double que contiene el valor de TF de cada palabra del primer Item de la tupla.  */

    public static double[] QueryTF((string[], List<string>) queryNormalize, Operations dataBase){
        double[] queryTf = new double[dataBase.vocabulary.Count];
        
        int totalWords = queryNormalize.Item1.Length;

        foreach (string word in queryNormalize.Item1){
            if (dataBase.vocabulary.ContainsKey(word))
            { 
                if (queryNormalize.Item2.Contains(word))
            {
                queryTf[dataBase.vocabulary[word]] = int.MaxValue;
            }
                else { queryTf[dataBase.vocabulary[word]]++; }
            }
        }
        for (int i = 0; i < totalWords; i++){
            
            queryTf[i] /= totalWords;
        }
            
        return queryTf;
    }


/* Método que recibe un array de double(el calculado anteriormente) y un objeto Operations, y se encarga de multiplicar
el valor de TF de cada palabra con el valor de IDF respectivo(obtenido atravez de la propiedad "idf" del objeto Operations).
Devuelve un array de double con el valor TF-IDF de cada palabra de la query. */


    public static double[] QueryTF_IDF(double[] queryTF, Operations dataBase){
        double[] queryTF_IDF = new double[dataBase.idf.Length];

        for (int i = 0; i < queryTF_IDF.Length; i++){
            queryTF_IDF[i] = queryTF[i] * dataBase.idf[i];
        }

        return queryTF_IDF;
    }


/* Método que recibe un objeto Operations y un array double(con los valores de TF-IDF previamente calculados) y auxiliandose
de otros dos métodos calcula la similitud entre dos vectores/documentos.
Devuelve un array de double con las medidas de similitud entre el array de TF-IDF de la query y la matriz de TF-IDF de la
base de datos.  */

    public static double[] CosineSimilarity(Operations dataBase, double[] queryTF_IDF){
        double scalarProduct = 0;
        double[] cosineSimilarity = new double[dataBase.tf_idf.GetLength(0)];
        double[] norms = Norms(dataBase);
        double queryNorm = QueryNorm(queryTF_IDF);

        for (int i = 0; i < dataBase.tf_idf.GetLength(0); i++){
            for (int j = 0; j < dataBase.tf_idf.GetLength(1); j++){
                scalarProduct += queryTF_IDF[j] * dataBase.tf_idf[i,j];
            }       
            cosineSimilarity[i] = scalarProduct / (norms[i] * queryNorm);
            scalarProduct = 0;
        }
        return cosineSimilarity;
    }



    public static double[] Norms(Operations dataBase){
        double[] norms = new double[dataBase.tf_idf.GetLength(0)];
        for (int i = 0; i < norms.Length; i++){
            for (int j = 0; j < dataBase.tf_idf.GetLength(1); j++){
                norms[i] += dataBase.tf_idf[i,j] * dataBase.tf_idf[i,j];
            }
            norms[i] = Math.Sqrt(norms[i]);
        }

        return norms;
    }

    public static double QueryNorm(double[] queryTF){
        double norm = 0;
        for (int j = 0; j < queryTF.Length; j++){
            norm += queryTF[j] * queryTF[j];
        }

        return Math.Sqrt(norm);
    }


/* Método que recibe un array de double(el calculado por el método anterior) y devuelve una lista de tuplas de double e int,
en las que utilizando el algoritmo "Burbuja" organiza de forma descendiente(según el valor de TF-IDF) cada documento que
corresponda con la query realizada(osea, que su valor de TF-IDF sea mayor que 0). */

    public static List<Tuple<double, int>> DescendingOrder (double[] cosineSimilarity){
        List<Tuple<double, int>> descendingOrder = new List<Tuple<double, int>>();

        for (int i = 0; i < cosineSimilarity.Length; i++){
            if (cosineSimilarity[i] > 0)
            {
                descendingOrder.Add(new Tuple<double, int>(cosineSimilarity[i], i));
            }
        }

        for (int i = 0; i < descendingOrder.Count; i++){
            for (int j = i+1; j < descendingOrder.Count; j++){
                if (descendingOrder[i].Item1 < descendingOrder[j].Item1){
                    Tuple<double, int> aux = descendingOrder[i];
                    descendingOrder[i] = descendingOrder[j];
                    descendingOrder[j] = aux;
                }
            }
        }

        return descendingOrder;
    }


/* Método que recibe dos array de string y devuelve un string que será un fragmento de texto que corresponda al documento
donde aparece alguna de las palabras de la query. */

    public static string Snippet(string[] queryNormalize, string[] document){
        string snippet = "";
        string document1 = string.Join(" ",document);

        foreach (string word in queryNormalize){

            var match = Regex.Match(document1, word);
            int index = match.Index;
               
            snippet = $"{document1.Substring(Math.Max(0, index - 80), Math.Min(160, (document1.Length - 2)-index))} ";
                
        }
        return snippet;
    }


/* Método que recibe un array de string y un array de Document.
Retorna una tupla conformada por un array de string(el mismo recibido) y una lista de string que contiene las palabras
encontradas en la query que tienen en alguna parte de su cuerpo el símbolo "$". */    

    public static (string[], List<string>) Symbols(string[] content, Document[] documents){
        List<string> moneyWords = new List<string>();

        foreach (string word in content){
            
            if ( word.Contains("$")){
                moneyWords.Add(Regex.Replace(word, @"[^a-z0-9\s]", "").ToLower());
            }
        }
            return (content, moneyWords);
    }


/* Método basado en el algoritmo de Levenshtein, el cual calcula la distancia de edición(menor número posible
de operaciones de inserción, eliminación o sustitución necesarias para transformar un string en otro) entre dos strings.
Este algoritmo utiliza una matriz(dp) de memorización para almacenar los resultados previamente calculados y evitar
así la repetición innecesaria de cálculos.
Retorna un int con la distancia anteriormente explicada.*/ 

    public static int LevenshteinDistance(string query, string word){
        int[,] dp = new int[query.Length + 1, word.Length + 1];
        
        for(int i = 0; i <= query.Length; i++){
            for( int j = 0; j <= word.Length; j++){

                if (i == 0) { dp[i,j] = j; }

                else if (j == 0) { dp[i,j] = i; }

                else if (query[i - 1] == word[j - 1]) 
                {
                    dp[i,j] = dp[i - 1, j - 1];
                }
                
                else { dp[i,j] = 1 + Math.Min(Math.Min(dp[i,j - 1], dp[i - 1,j]), dp[i - 1,j - 1]); }

            }
        }    

        return dp[query.Length,word.Length]; 

    }


    /* Método que recibe un array de string y un objeto Operations. El método se encarga de calcular la menor distancia 
    entre cada palabra de la query(array de string) y cada palabra existente en la propiedad vocabulary de Operations;
    devolviendo así cuales serían las palabras que existen realmente en nuestra base de datos que podrían sustituir en la
    query a alguna mal escrita o no encontrada.*/ 

     public static string SimilarSuggestion( string[] queryNormalize, Operations dataBase){
         string[] sim = new string [queryNormalize.Length];
         int minDistance = int.MaxValue;

         for (int i = 0; i < queryNormalize.Length; i++){
             if (!dataBase.vocabulary.ContainsKey("queryNormalize[i]")){

                foreach (string word in dataBase.vocabulary.Keys){

                    int ld = LevenshteinDistance(queryNormalize[i], word);

                    if (ld < minDistance)
                    {
                     minDistance = ld;
                     sim[i] = word;
                    }
                }
            minDistance = int.MaxValue;
            }
            else { sim[i] = queryNormalize[i]; }
        }   
         string similar = string.Join(" ", sim);
         return similar;
     }

    

}