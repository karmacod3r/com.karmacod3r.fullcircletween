namespace FullCircleTween.Core.Interfaces
{
    public interface ISerializableValue
    {
        string Serialize();
        object Deserialize(string value);
    }
}