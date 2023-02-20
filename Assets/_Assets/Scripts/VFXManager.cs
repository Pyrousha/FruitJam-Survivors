using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    [Header("Particle systems")]
    [SerializeField] GameObject[] particleSystems;

    [Header("Animation clip particles")]
    [SerializeField] private GameObject template;
    [SerializeField] private AnimationClip[] anims;
    [Header("Misc")] 
    [SerializeField] private GameObject textFlyout;
    
    public void SpawnParticleSystem(ParticleSystemType type, Vector3 position, Quaternion rotation) {
        Instantiate(particleSystems[(int)type], position, rotation);
    }

    public GameObject SpawnParticleSystem(ParticleSystemType type, Transform parent) {
        return Instantiate(particleSystems[(int)type], parent);
    }

    public void CreateVFX(ParticleType type, Vector3 pos, bool flip) {
        GameObject particle = Instantiate(template, pos, Quaternion.identity);

        Animator animator = particle.GetComponent<Animator>();
        AnimatorOverrideController animController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animController["particle"] = anims[(int)type];
        animator.runtimeAnimatorController = animController;

        particle.GetComponent<DestroyAfterDelay>().SetLifetime(anims[(int)type].length);

        particle.GetComponent<SpriteRenderer>().flipX = flip;
    }

    public void CreateTextFlyout(string text, Vector3 position) {
        Instantiate(textFlyout, position, Quaternion.identity).GetComponent<TextFlyoutController>().Init(text);
    }

}

public enum ParticleSystemType {
    Hit, Dust_Burst, Dust_Trail
}

public enum ParticleType {
    Dust_Small, Dust_Large, Dust_Splash
}
