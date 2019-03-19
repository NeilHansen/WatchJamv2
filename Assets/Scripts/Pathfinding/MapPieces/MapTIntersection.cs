using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPieceUtility;

public class MapTIntersection : AbstractPiece
{
    MapTIntersection()
    {
        type = PieceType.TIntersection;
        localCenter = Vector3.zero;
        AddOpening(Vector3.forward);
        AddOpening(Vector3.back);
        AddOpening(Vector3.left);
        materialIndex = 1;
    }

    protected override IEnumerator TraverseLight()
    {
        if (flashLight)
            goto SkipTraverse;

        List<int> lightTexturePattern = new List<int>();

        if (pathfindingPrevious)
        {
            Vector3 fromDirection = pathfindingPrevious.transform.position - transform.position;
            int inDirection = DirectionTowardsIncomingVector(-fromDirection);
            switch (inDirection)
            {
                case 0:
                    lightTexturePattern.Add(0);
                    break;
                case 1:
                    lightTexturePattern.Add(2);
                    break;
                case 2:
                    lightTexturePattern.Add(3);
                    break;
            }
        }

        lightTexturePattern.Add(1);

        Vector3 nextDirection = pathfindingNext.transform.position - transform.position;
        int exitDirection = DirectionTowardsIncomingVector(-nextDirection);
        if (exitDirection > -1)
        {
            switch (exitDirection)
            {
                case 0:
                    lightTexturePattern.Add(0);
                    break;
                case 1:
                    lightTexturePattern.Add(2);
                    break;
                case 2:
                    lightTexturePattern.Add(3);
                    break;
            }
        }

        foreach (int index in lightTexturePattern)
        {
            mpb.SetTexture("_EmissionMap", invertLighting ? inverseTranverseTex[index] : tranverseTex[index]);
            lightRenderer.SetPropertyBlock(mpb);
            yield return new WaitForSeconds(traverseInterval);
        }

        mpb.SetTexture("_EmissionMap", invertLighting ? darkTex : fullLitTex);
        lightRenderer.SetPropertyBlock(mpb);

        SkipTraverse:
        yield return null;
        //Start traverse light to next map piece
        pathfindingNext.StartLightTraverse();
    }
}
