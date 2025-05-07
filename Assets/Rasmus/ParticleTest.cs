using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] ParticleSystem ParticleSystem;

    public void TestParticle()
    {
        ParticleSystem.Play();
    }
}
