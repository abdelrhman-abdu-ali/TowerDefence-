using System;
using Unity.Entities;
using UnityEngine;

public interface IShootable
{
    void Shoot(Vector3 targetPosition);
    float FireRate { get; }
    float Range { get; }
}
