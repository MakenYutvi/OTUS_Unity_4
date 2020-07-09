using UnityEngine;

namespace Core
{
    public interface IModel
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
    }
}
