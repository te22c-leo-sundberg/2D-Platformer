using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject sebbeJumpscare;
    [SerializeField] float sebbeJumpscareTimer;
    [SerializeField] int sebbeJumpscareChance;
    [SerializeField] int sebbeMoveCounter;
    [SerializeField] int sebbeDifficulty;
    [SerializeField] int sebbeRequiredMoves;
    [SerializeField] float sebbeJumpscareCooldown;
    [SerializeField] float sebbeJumpscareTime;
    float originalJumpscareTime;
    float originalJumpscareCooldown;
    float originalJumpscareTimer;
    ulong runtime;
    void Start()
    {
        originalJumpscareTime = sebbeJumpscareTime;
        originalJumpscareTimer = sebbeJumpscareTimer;
        originalJumpscareCooldown = sebbeJumpscareCooldown;
    }

    void Update()
    {
        sebbeJumpscareCooldown -= Time.deltaTime;
        if (sebbeJumpscareCooldown <= 0)
        {
            sebbeJumpscareTimer -= Time.deltaTime;
            if (sebbeJumpscareTimer <= 0)
            {
                sebbeJumpscareChance = Random.Range(1, 21);
                if (sebbeJumpscareChance <= sebbeDifficulty)
                {
                    sebbeMoveCounter++;
                }
                sebbeJumpscareTimer = originalJumpscareTimer;
            }
            if (sebbeMoveCounter >= sebbeRequiredMoves)
            {
                sebbeJumpscare.SetActive(true);
                sebbeJumpscareTime -= Time.deltaTime;
            }
            if (sebbeJumpscareTime <= 0)
            {
                sebbeJumpscareTime = originalJumpscareTime;
                sebbeJumpscareCooldown = Random.Range(originalJumpscareCooldown - 20, originalJumpscareCooldown + 20);
                sebbeMoveCounter = 0;
                sebbeJumpscare.SetActive(false);
            }
        }
    }
}