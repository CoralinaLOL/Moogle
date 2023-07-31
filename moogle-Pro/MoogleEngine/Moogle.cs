namespace MoogleEngine;

using System.Text.RegularExpressions;

public static class Moogle
{
    
    public static SearchResult Query(string query, Operations dataBase, Document[] documents){

    //Trabajando la query...
        (string[], List<string>) queryNormalize = QueryOperations.QueryNormalize(query, documents);
        double[] queryTF = QueryOperations.QueryTF(queryNormalize, dataBase);
        double[] cosineSimilarity = QueryOperations.CosineSimilarity(dataBase, QueryOperations.QueryTF_IDF(queryTF, dataBase));
        List<Tuple<double, int>> descendingOrder = QueryOperations.DescendingOrder(cosineSimilarity);

        
         SearchItem[] items = new SearchItem[descendingOrder.Count];

         if(query == string.Empty)
         {
            items = new SearchItem[1];
            items[0] = new SearchItem ("Por favor inserte una consulta","",0.0f);
         }
         

        string newquery = QueryOperations.SimilarSuggestion(queryNormalize.Item1, dataBase);

         for(int i = 0; i < descendingOrder.Count; i++){

            items[i] = new SearchItem(documents[descendingOrder[i].Item2].Name, QueryOperations.Snippet(queryNormalize.Item1,documents[descendingOrder[i].Item2].Words), Convert.ToSingle(cosineSimilarity[descendingOrder[i].Item2]));     
        }  
        return new SearchResult(items, newquery);
    }
}