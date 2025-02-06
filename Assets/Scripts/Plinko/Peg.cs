using UnityEngine;

public class Peg : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    public void PegAnimation()
    {
        _particleSystem.Play();
    }
}
