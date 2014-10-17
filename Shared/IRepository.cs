using System;
using System.Collections.Generic;

public interface IRepository<TDataClass>
{
    TDataClass CreateEmpty();
    void Query(Action<IEnumerable<TDataClass>> action);
    void Find(string name, Action<TDataClass> complete);
    void Save(TDataClass data);
    void Delete(TDataClass data);
}