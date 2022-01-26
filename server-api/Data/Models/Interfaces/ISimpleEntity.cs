using System;

namespace server_api.Data.Models.Interfaces
{
    public interface ISimpleEntity<T> where T:IEquatable<T>
    {
        T Id {get;set;}
        string Name {get;}
    }
}
