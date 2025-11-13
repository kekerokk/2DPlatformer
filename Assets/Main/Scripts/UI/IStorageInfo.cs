using System;
public interface IStorageInfo {
    public int money { get; }

    public event Action MoneyChanged;
}
