using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    public static ParticleEffectsManager instance;

    [SerializeField] private ParticleSystem breakingParticles; 


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    public void PlayBreakingEffect(Vector3 position, Color fruitColor)
    {
        if (breakingParticles != null)
        {
           
            ParticleSystem breakingInstance = Instantiate(breakingParticles, position, Quaternion.identity);

            var main = breakingInstance.main;
            main.startColor = fruitColor;

            Destroy(breakingInstance.gameObject, main.duration + main.startLifetime.constantMax);
        }
    }
}