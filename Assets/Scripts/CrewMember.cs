using UnityEngine;


public class CrewMember : MonoBehaviour, SplintersTarget
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;

    private float _hp = 100f;

    public bool IsDead()
    {
        return _hp < 0;
    }

    public bool PotentiallyDie(Splinter splinter)
    {
        return (_hp - splinter.Damage) < 0;
    }

    public void Colored(Color color)
    {
        _meshRenderer.material.color = color;
    }

    public void HitSplinter(Splinter splinter)
    {
        _hp -= splinter.Damage;
    }
}
