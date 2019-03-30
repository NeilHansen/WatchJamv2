using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapStraight : AbstractPiece
{
    MapStraight()
    {
        type = PieceType.Straight;
        localCenter = Vector3.zero;
        AddOpening(Vector3.forward);
        AddOpening(Vector3.back);
        materialIndex = 0;
    }

    protected override IEnumerator PulseLight()
    {
        //If tile is flashing or part of room, skip current tile
        if (flashLight || roomIndex != -1)
            goto SkipTraverse;

        List<int> lightTexturePattern = new List<int>();

        //Add sequence of patterns with outgoing direction
        Vector3 nextDirection = pathfindingNext.transform.position - transform.position;
        int exitDirection = DirectionTowardsIncomingVector(nextDirection);
        if (exitDirection > -1)
        {
            switch (exitDirection)
            {
                case 0:
                    lightTexturePattern.Add(0);
                    lightTexturePattern.Add(1);
                    lightTexturePattern.Add(2);
                    break;
                case 1:
                    lightTexturePattern.Add(2);
                    lightTexturePattern.Add(1);
                    lightTexturePattern.Add(0);
                    break;
            }
        }

        //Loop through all emissive texture patterns in list
        foreach (int index in lightTexturePattern)
        {
            mpb.SetTexture("_EmissionMap", invertLighting ? inverseTranverseTex[index] : tranverseTex[index]);
            lightRenderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(pulseInterval);
        }

        ResetLight();

        SkipTraverse:
        yield return null;
        //Start traverse light to next map piece
        if (pathfindingNext != null)
            pathfindingNext.StartLightPulse();
    }
}
