using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Path_Comp))]
public class ParticlePathFlow : MonoBehaviour
{

    public bool isPathUpdating = false;
    public bool hasRandomStartingPoints = false;


    [Range(0.0f, 5.0f)]
    public float pathWidth = 0.0f;


    private ParticleSystem.Particle[] _particle_array;
    private ParticleSystem _particle_system;
    private Path_Comp _path_comp;

    private int _numParticles;


#if UNITY_EDITOR
    void Reset()
    {
        Start();
    }
#endif

    void Start()
    {

        _path_comp = GetComponent<Path_Comp>();
        _particle_system = GetComponent<ParticleSystem>();
        _particle_array = new ParticleSystem.Particle[_particle_system.main.maxParticles];

    }


    void LateUpdate()
    {

        if (_particle_array == null)
        {

            Start();
            _path_comp.Update_Path();

        }
        else if (isPathUpdating)
        {

            _path_comp.Update_Path();

        }



        _numParticles = _particle_system.GetParticles(_particle_array);

        if (_numParticles > 0)
        {

            for (int i = 0; i < _numParticles; i++)
            {

                ParticleSystem.Particle obj = _particle_array[i];

                // This made it based on the particle lifetime
                //					float normalizedLifetime = (1.0f - obj.remainingLifetime / obj.startLifetime);
                //
                //					if(hasRandomStartingPoints){
                //						normalizedLifetime += Get_Value_From_Random_Seed_0t1(obj.randomSeed, 100.0f);
                //						normalizedLifetime = normalizedLifetime % 1.0f;
                //					}
                //
                //					Path_Point axis = _path_comp.GetPathPoint(_path_comp.TotalDistance * normalizedLifetime);

                // This made it based on the paritcle speed
                float dist = (obj.startLifetime - obj.remainingLifetime) * obj.velocity.magnitude;
                if (hasRandomStartingPoints)
                    dist += Get_Value_From_Random_Seed_0t1(obj.randomSeed, 100.0f) * _path_comp.TotalDistance;
                dist = dist % _path_comp.TotalDistance;

                Path_Point axis = _path_comp.GetPathPoint(dist);

                Vector2 offset = Vector2.zero;
                if (pathWidth > 0)
                {
                    offset = Math_Functions.AngleToVector2D(obj.randomSeed % 360.0f);
                    offset *= Get_Value_From_Random_Seed_0t1(obj.randomSeed, 150.0f) * pathWidth;
                }

                _particle_array[i].position = axis.point +
                    (axis.right * offset.x) +
                    (axis.up * offset.y);

            }

            _particle_system.SetParticles(_particle_array, _numParticles);

        }


    }

    private float Get_Value_From_Random_Seed_0t1(float seed, float converter)
    {
        return (seed % converter) / converter;
    }

}