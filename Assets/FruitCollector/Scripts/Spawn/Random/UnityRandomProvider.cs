using UnityEngine;
public sealed class UnityRandomProvider : IRandomProvider
{
    public int Range(int minInclusive, int maxExclusive)
    => Random.Range(minInclusive, maxExclusive);
}