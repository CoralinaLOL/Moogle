namespace MoogleEngine;

public class Document{
    public string Name {get; private set;}
    public string Path {get; private set;}
    public string[] Words {get; private set;}

    public Document(string Name, string Path, string[] Words){
        this.Name = Name;
        this.Path = Path;
        this.Words = Words;
    }
}
