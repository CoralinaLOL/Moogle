using System;


public class Vector{

    private double [] vector;
    public int size {get; private set;}

    public Vector (int size){
        
        this.vector = new double [size];
        this.size = size;
    }


    public Vector VectorAddition(Vector a, Vector b){
        if(a.size == b.size){
            Vector result = new Vector [a.size];
            for(int i =0; i<a.size; i++){
                result[i] = a[i] + b[i];
            }
            return result;
        }
        else{throw new ArgumentException("La operación suma no está definida para vectores de diferentes tamaños");}
    }


    public Vector VectorSubtraction(Vector a, Vector b){
        if(a.size == b.size){
            Vector result = new Vector [a.size];
            for(int i =0; i<a.size; i++){
                result[i] = a[i] - b[i];
            }
            return result;
        }
        else{throw new ArgumentException("La operación diferencia no está definida para vectores de diferentes tamaños");}
    }


    public Vector VectorMultiplication(Vector a, Vector b){
        if(a.size == b.size){
            Vector result = new Vector [a.size];
            for(int i =0; i<a.size; i++){
                result[i] = a[i] * b[i];
            }
            return result;
        }
        else{throw new ArgumentException("La operación multiplicación no está definida para vectores de diferentes tamaños");}
    }


    public Vector MultiplicationForNumber(Vector a, Vector b){
        Vector result = new Vector [a.size];
        for(int i =0; i<a.size; i++){
            result[i] = a[i] * b;
        } 
        return result;
    }


}