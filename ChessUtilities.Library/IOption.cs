namespace ChessUtilities.Library;

public interface IOption<T> {}

public class Some<T> : IOption<T>
{
    T _value;

    public Some(T value)
    {
        _value = value;
    }
    T Value() => _value;
}

public class None<T> : IOption<T> {}
