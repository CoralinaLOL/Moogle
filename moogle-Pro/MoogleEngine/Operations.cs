namespace MoogleEngine;

public class Operations{

    /* En esta clase es implementado el modelo de espacio vectorial en el cual cada palabra es representada 
    como un vector, donde la componente i-esima es su valor de Tf-Idf para el documento j-esimo */

    public List<double[]> tf {get; private set;}
    public Dictionary<string,int> vocabulary {get; private set;}
    public double[] idf {get; private set;}
    public double[,] tf_idf {get; private set;}

    /* Objeto creado con el fin de pre procesar la base de datos encontrada en la carpeta "Content"
    y almacenar en propiedades(que luego serán utilizadas) el valor de TF, IDF y TF-IDF de cada palabra en cada 
    documento, además de que tiene una propiedad "vocabulary" que no es más que un diccionario con el cual se asocia a
    cada palabra encontrada en el corpus de documentos con una columna en la matriz del modelo vectorial.  */

    public Operations (Document[] documents){

        this.tf = new List<double[]>();
        this.vocabulary = new Dictionary<string,int>();
        TF(documents);

        this.idf = new double[this.tf.Count];
        IDF(this.tf);
        
        this.tf_idf = new double[this.tf[0].Length, this.idf.Length];
        TF_IDF(); 
    }



 public void TF(Document[] documents){
    int currentRow = 0;

    for (int i = 0; i < documents.Length; i++){
        foreach(string word in documents[i].Words){
             if (this.vocabulary.ContainsKey(word))
             {
                this.tf[this.vocabulary[word]][i]++;
             }
             else { 
                this.vocabulary.Add(word, currentRow); 
                this.tf.Add(new double[documents.Length]);
                this.tf[this.vocabulary[word]][i]++;
                currentRow++;
            }
        }
    }
    for(int j = 0; j < this.tf.Count; j++){
        for (int i = 0; i < documents.Length; i++){
            this.tf[j][i] /= documents[i].Words.Length;
        }
    }
 }



 public void IDF(List<double[]> tf){

    for(int j = 0; j < this.tf.Count; j++){
        for(int i = 0; i < this.tf[j].Length; i++){
            if(this.tf[j][i] != 0){
                this.idf[j]++;
            }
        }
        this.idf[j] = Math.Log10((1+this.tf[j].Length)/this.idf[j]);
    }
 }



    public void TF_IDF(){

        for(int i = 0; i < this.tf_idf.GetLength(0); i++){
            for(int j = 0; j < this.tf_idf.GetLength(1); j++){
                this.tf_idf[i,j] = this.tf[j][i] * this.idf[j];
            }
        }
    }

}