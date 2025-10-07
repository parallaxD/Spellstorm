[System.Serializable]
public class ElementTuple
{
    public ElementType elementType;
    public int count;

    public ElementTuple(ElementType type, int count)
    {
        this.elementType = type;
        this.count = count;
    }
}
