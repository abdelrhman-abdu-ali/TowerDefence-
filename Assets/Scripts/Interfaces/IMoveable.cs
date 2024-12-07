using System;
using Unity.Entities;
using UnityEngine;

public interface IMoveable
{
    void Move(Vector3 destination);
    float MovementSpeed { get; }
}