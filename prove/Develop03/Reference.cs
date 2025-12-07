class Reference
{
    private string referenceText;

    public Reference(string referenceText)
    {
        this.referenceText = referenceText;
    }

    public override string ToString()
    {
        return referenceText;
    }
}