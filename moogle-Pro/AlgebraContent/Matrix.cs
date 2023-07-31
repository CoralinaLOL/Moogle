namespace AlgebraContent;
using System;


public class Matrix{

    public int rows { get; private set; }
    public int cols { get; private set; }

    public Matrix(int rows, int cols){

    this.rows = rows;
    this.cols = cols;
    Matrix = new double [rows,cols];   }

    public Matrix MatrixAddition(Matrix a, Matrix b){
        
        if(a.rows==b.rows && a.cols==b.cols){
            Matrix result = new Matrix [a.rows,a.cols];
            for(int i=0; i<a.rows; i++){
                for(int j=0; j<a.cols; j++){
                    result[i,j]=a[i,j]+b[i,j];
                }
            }
            return result;
        }
        else {throw new ArgumentException("La operación suma no está definida para matrices con diferentes dimensiones");}
    }

    public Matrix MatrixMultiplication(Matrix a, Matrix b){
        
        if(a.cols == b.rows){
            Matrix result = new Matrix[a.rows, b.cols];
            for(int i = 0; i<a.rows; i++){
                for(int j = 0; j<b.cols; j++){
                    for( int k = 0; k<a.cols; k++){
                        result[i,j] += a[i,k]*b[k,j];
                    }
                }
            }
            return result;
        }
        else{ throw new ArgumentException("La operación de multiplicación entre dos matrices sólo está definida cuando una matriz tiene la misma cantidad de columnas que de filas la otra");}
    } 

    public Matrix MultiplicationForNumber(Matrix a, int num){
        Matrix result = new Matrix[a.rows,a.cols];
        for(int i =0; i< a.rows; i++){
            for(int j = 0; j< a.cols; j++){
                result [i,j] = a[i,j]* num;
            }
        }
        return result;
    }

    public Matrix MultiplicationForVector(Matrix a, Vector v){
            if (a.cols == v.size) {
                Matrix result = new Matrix[a.rows,v.size];

            for (int i = 0; i < a.rows; i++){
                for (int j = 0; j < v.size; j++){
                    result[i,j] = a[i] * v[j];
                }
            }
            return result;
        }
        else{ throw new ArgumentException("La operación de una matriz y un vector sólo está definida para matrices con la misma cantidad de filas que de columnas el vector");}
    }

    public Matrix MatrixSubtraction(Matrix a, Matrix b){
        if(a.rows==b.rows && a.cols==b.cols){
            return MatrixAddition(a, MultiplicationForNumber(b, -1));
        }
        else {throw new ArgumentException("La operación diferencia no está definida para matrices con diferentes dimensiones");}
    }

    public Matrix MatrixTranspose(Matrix a){
        Matrix result = new Matrix [a.rows, a.cols];
        for(int i = 0; i< a.rows; i++){
            for(int j = 0; j< a.cols; j++){
                result[j,i]= a[i,j];
            }
        }
        return result;
    }
}
