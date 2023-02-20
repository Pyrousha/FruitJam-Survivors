using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] GameObject[] particleSystems;
    
    public void SpawnParticleSystem(ParticleSystemType type, Vector3 position, Quaternion rotation) {
        Instantiate(particleSystems[(int)type], position, rotation);
    }
}

public enum ParticleSystemType {
    Hit
}
