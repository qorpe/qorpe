namespace Qorpe.Infrastructure.Data.Lite;

public sealed class PropertyComparer<T>(string propertyName, bool ascending) : IComparer<T>
{
    public int Compare(T x, T y)
    {
        var property = typeof(T).GetProperty(propertyName) 
            ?? throw new ArgumentException($"Property {propertyName} not found on type {typeof(T)}");

        var xValue = property.GetValue(x);
        var yValue = property.GetValue(y);

        int comparison = Comparer<object>.Default.Compare(xValue, yValue);
        return ascending ? comparison : -comparison;
    }
}
