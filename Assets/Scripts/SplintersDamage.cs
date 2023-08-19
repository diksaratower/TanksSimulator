using System.Collections.Generic;
using UnityEngine;

public class SplintersDamage
{
    public List<Splinter> Splinters { get; } = new List<Splinter>();
    public List<CrewMember> KilledCrewMembers { get; } = new List<CrewMember>();
    public Vector3 StartPosition { get; }

    public SplintersDamage(List<Splinter> splinters, Vector3 position, List<CrewMember> killedCrewMembers)
    {
        Splinters = splinters;
        StartPosition = position;
        KilledCrewMembers = killedCrewMembers;
    }
}

public class Splinter
{
    public Vector3 HitPosition { get; }
    public float Damage { get; }

    public Splinter(Vector3 hitPosition)
    {
        HitPosition = hitPosition;
        Damage = 101;
    }

    public static bool CheckSplinterHit(Vector3 startPostion, Vector3 direction, Quaternion quaternion, float radius, out Splinter outSplinter, out List<CrewMember> killedCrewMembers)
    {
        killedCrewMembers = new List<CrewMember>();
        var ray = new Ray(startPostion, quaternion * direction);
        if (Physics.Raycast(ray, out RaycastHit hit, radius))
        {
            if (hit.collider.TryGetComponent<SplintersTarget>(out var splintersTarget))
            {
                var splinter = new Splinter(hit.point);

                if (splintersTarget is CrewMember)
                {
                    var crewMember = (splintersTarget as CrewMember);
                    if (crewMember.IsDead() == false && crewMember.PotentiallyDie(splinter))
                    {
                        killedCrewMembers.Add(crewMember);
                    }
                }
                splintersTarget.HitSplinter(splinter);
                outSplinter = splinter;
                return true;
            }
        }
        outSplinter = null;
        return false;
    }
}